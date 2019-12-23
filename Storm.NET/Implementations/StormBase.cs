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
        private bool _isInUpdate;
        private bool _isInLoopSearch;

        protected StormBase(IEqualityComparer<T>? comparer) : base(comparer)
        {
        }

        private event Action<StormToken, EStormVisitType>? OnVisitEvent;

        public event Action<StormToken, EStormVisitType>? OnVisit
        {
            add
            {
                OnVisitEvent += value;
                if (_isInUpdate)
                {
                    value?.Invoke(_currentToken, EStormVisitType.UpdateEnter);
                }
            }
            remove => OnVisitEvent -= value;
        }

        public bool TryGetEnteredToken(out StormToken token)
        {
            token = _currentToken;
            return !token.Equals(default);
        }

        protected bool IsDescendant(IStormNode node)
        {
            if (node == this)
                return true;

            if (node is IStormSocket<T> socket && socket.Target == this)
                return true;

            if (OnVisitEvent == null)
                return false;

            var hasEntered = false;

            void TargetOnVisit(StormToken enteredToken, EStormVisitType visitType)
            {
                hasEntered |= visitType == EStormVisitType.LoopSearchEnter;
            }

            node.OnVisit += TargetOnVisit;
            OnVisitEvent.Invoke(_currentToken, EStormVisitType.LoopSearchEnter);
            OnVisitEvent.Invoke(_currentToken, EStormVisitType.LoopSearchLeave);
            node.OnVisit -= TargetOnVisit;

            return hasEntered;
        }

        protected void RaiseLoopSearchEnter(StormToken token)
        {
            if (_isInLoopSearch)
                throw new InvalidOperationException("Already in loop search");

            if (_isInUpdate)
            {
                if (!_currentToken.Equals(token))
                    throw new InvalidOperationException("Unknown token");
            }
            else
            {
                _currentToken = token;
            }

            _isInLoopSearch = true;
            OnVisitEvent?.Invoke(token, EStormVisitType.LoopSearchEnter);
        }

        protected void RaiseLoopSearchLeave(StormToken token)
        {
            if (!_isInLoopSearch)
                throw new InvalidOperationException("Not in loop search");

            if (!_currentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            if (!_isInUpdate)
                _currentToken = default;

            OnVisitEvent?.Invoke(token, EStormVisitType.LoopSearchLeave);
            _isInLoopSearch = false;
        }

        protected void RaiseUpdateEnter(StormToken token)
        {
            if (_isInUpdate || _isInLoopSearch)
                throw new InvalidOperationException("Can't enter now");

            _currentToken = token;
            _isInUpdate = true;
            
            OnVisitEvent?.Invoke(token, EStormVisitType.UpdateEnter);
        }

        protected void RaiseUpdateLeave(StormToken token, bool hasChanged)
        {
            if (!_isInUpdate || _isInLoopSearch)
                throw new InvalidOperationException("Can't leave now");

            if (!_currentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            _currentToken = default;
            _isInUpdate = false;

            var visitType = hasChanged ? EStormVisitType.UpdateLeaveChanged : EStormVisitType.UpdateLeaveUnchanged;
            OnVisitEvent?.Invoke(token, visitType);
        }
    }
}