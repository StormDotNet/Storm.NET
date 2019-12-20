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
        private EState _visitState = EState.Idle;
        private bool _inLoopSearch;
        private event Action<IStormToken, EStormVisitType>? OnVisit;

        protected StormBase(IEqualityComparer<T>? comparer) : base(comparer)
        {
        }

        event Action<IStormToken, EStormVisitType>? IStormNode.OnVisit
        {
            add
            {
                OnVisit += value;
                if (_visitState == EState.Entered)
                {
                    value?.Invoke(CurrentToken!, EStormVisitType.UpdateEnter);
                }
            }
            remove => OnVisit -= value;
        }

        protected IStormToken? CurrentToken { get; private set; }

        public bool TryGetEnteredToken(out IStormToken? token)
        {
            token = CurrentToken;
            return token != null;
        }

        protected bool IsDescendant(IStormNode node)
        {
            if (node == this)
                return true;

            if (node is IStormSocket<T> socket && socket.Target == this)
                return true;

            if (OnVisit == null)
                return false;

            var hasEntered = false;

            void TargetOnVisit(IStormToken enteredToken, EStormVisitType visitType)
            {
                if (enteredToken != CurrentToken)
                    throw new InvalidOperationException("Unknown token");
                hasEntered |= visitType == EStormVisitType.LoopSearchEnter;
            }

            node.OnVisit += TargetOnVisit;
            if (CurrentToken == null)
            {
                CurrentToken = Storm.Token.Create();
                OnVisit.Invoke(CurrentToken, EStormVisitType.LoopSearchEnter);
                OnVisit.Invoke(CurrentToken, EStormVisitType.LoopSearchLeave);
                CurrentToken = null;
            }
            else
            {
                OnVisit.Invoke(CurrentToken, EStormVisitType.LoopSearchEnter);
                OnVisit.Invoke(CurrentToken, EStormVisitType.LoopSearchLeave);
            }

            node.OnVisit -= TargetOnVisit;

            return hasEntered;
        }

        protected void RaiseLoopSearchEnter(IStormToken token)
        {
            if (CurrentToken != null && CurrentToken != token)
                throw new InvalidOperationException("Unknown token");

            if (_inLoopSearch)
                throw new InvalidOperationException("Already in loop search");

            _inLoopSearch = true;
            OnVisit?.Invoke(token, EStormVisitType.LoopSearchEnter);
        }

        protected void RaiseLoopSearchLeave(IStormToken token)
        {
            if (CurrentToken != null && CurrentToken != token)
                throw new InvalidOperationException("Unknown token");

            if (!_inLoopSearch)
                throw new InvalidOperationException("Not in loop search");

            OnVisit?.Invoke(token, EStormVisitType.LoopSearchLeave);
            _inLoopSearch = false;
        }

        protected void RaiseUpdateEnter(IStormToken token)
        {
            if (CurrentToken != null)
                throw new InvalidOperationException("Unexpected state");

            if (_visitState != EState.Idle || _inLoopSearch)
                throw new InvalidOperationException("Can't enter now");

            CurrentToken = token;

            _visitState = EState.Entering;
            try
            {
                OnVisit?.Invoke(token, EStormVisitType.UpdateEnter);
            }
            finally
            {
                _visitState = EState.Entered;
            }
        }

        protected void RaiseUpdateLeave(IStormToken token, bool hasChanged)
        {
            if (token != CurrentToken)
                throw new InvalidOperationException("Unknown token");

            if (_visitState != EState.Entered || _inLoopSearch)
                throw new InvalidOperationException("Can't leave now");

            _visitState = EState.Leaving;
            OnVisit?.Invoke(token, hasChanged ? EStormVisitType.UpdateLeaveChanged : EStormVisitType.UpdateLeaveUnchanged);
            _visitState = EState.Idle;
            CurrentToken = null;
        }

        private enum EState
        {
            Idle,
            Entering,
            Entered,
            Leaving
        }
    }
}