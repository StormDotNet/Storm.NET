// Storm.NET - Simple Topologically Ordered Reactive Model
// Copyright © 2019 Storm.NET. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace StormDotNet.Implementations
{
    using System;
    using System.Collections.Generic;

    internal abstract class StormBase<T> : StormContentBase<T>, IStorm<T>
    {
        private StormToken _currentToken;
        private StormVisitState _visitState;

        protected StormBase(IEqualityComparer<T>? comparer) : base(comparer)
        {
        }

        private event Action<StormToken, EStormVisitType>? OnVisitEvent;

        public event Action<StormToken, EStormVisitType>? OnVisit
        {
            add
            {
                OnVisitEvent += value;
                if (_visitState.IsInUpdate())
                {
                    value?.Invoke(_currentToken, EStormVisitType.EnterUpdate);
                }
            }
            remove => OnVisitEvent -= value;
        }

        public StormVisitState GetVisitState(out StormToken token)
        {
            token = _currentToken;
            return _visitState;
        }

        protected bool IsDescendant(IStormNode otherNode) => IsDescendantHelper.IsDescendant(_currentToken, this, OnVisitEvent, otherNode);

        protected void EnterLoopSearch(StormToken token)
        {
            if (!_visitState.CanEnterLoopSearch())
                throw new InvalidOperationException("Already in loop search");

            if (_visitState.IsInUpdate())
            {
                if (!_currentToken.Equals(token))
                    throw new InvalidOperationException("Unknown token");
            }
            else
            {
                _currentToken = token;
            }

            _visitState = _visitState.EnterLoopSearch();

            OnVisitEvent?.Invoke(token, EStormVisitType.EnterLoopSearch);
        }

        protected void LeaveLoopSearch(StormToken token)
        {
            if (!_visitState.CanLeaveLoopSearch())
                throw new InvalidOperationException("Not in loop search");

            if (!_currentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            if (!_visitState.IsInUpdate())
                _currentToken = default;

            _visitState = _visitState.LeaveLoopSearch();

            OnVisitEvent?.Invoke(token, EStormVisitType.LeaveLoopSearch);
        }

        protected void EnterUpdate(StormToken token)
        {
            if (!_visitState.CanEnterUpdate())
                throw new InvalidOperationException("Can't enter now");

            _currentToken = token;
            _visitState = _visitState.EnterUpdate();
            
            OnVisitEvent?.Invoke(token, EStormVisitType.EnterUpdate);
        }

        protected void LeaveUpdate(StormToken token, bool hasChanged)
        {
            if (!_visitState.CanLeaveUpdate())
                throw new InvalidOperationException("Can't leave now");

            if (!_currentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            _currentToken = default;
            _visitState = _visitState.LeaveUpdate();

            var visitType = hasChanged ? EStormVisitType.LeaveUpdateChanged : EStormVisitType.LeaveUpdateUnchanged;

            OnVisitEvent?.Invoke(token, visitType);
        }
    }
}