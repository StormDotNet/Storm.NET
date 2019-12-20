﻿// Storm.NET - Simple Topologically Ordered Reactive Model
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

        protected StormBase(IEqualityComparer<T>? comparer) : base(comparer)
        {
        }

        private event Action<StormToken, EStormVisitType>? OnVisitEvent;

        public event Action<StormToken, EStormVisitType>? OnVisit
        {
            add
            {
                OnVisitEvent += value;
                if (_visitState == EState.Entered)
                {
                    value?.Invoke(CurrentToken, EStormVisitType.UpdateEnter);
                }
            }
            remove => OnVisitEvent -= value;
        }

        protected StormToken CurrentToken { get; private set; }

        public bool TryGetEnteredToken(out StormToken token)
        {
            token = CurrentToken;
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
                hasEntered |= CurrentToken.Equals(enteredToken) && visitType == EStormVisitType.LoopSearchEnter;
            }

            node.OnVisit += TargetOnVisit;
            OnVisitEvent.Invoke(CurrentToken, EStormVisitType.LoopSearchEnter);
            OnVisitEvent.Invoke(CurrentToken, EStormVisitType.LoopSearchLeave);
            node.OnVisit -= TargetOnVisit;

            return hasEntered;
        }

        protected void RaiseLoopSearchEnter(StormToken token)
        {
            if (!CurrentToken.Equals(default) && !CurrentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            if (_inLoopSearch)
                throw new InvalidOperationException("Already in loop search");

            _inLoopSearch = true;
            OnVisitEvent?.Invoke(token, EStormVisitType.LoopSearchEnter);
        }

        protected void RaiseLoopSearchLeave(StormToken token)
        {
            if (!CurrentToken.Equals(default) && !CurrentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            if (!_inLoopSearch)
                throw new InvalidOperationException("Not in loop search");

            OnVisitEvent?.Invoke(token, EStormVisitType.LoopSearchLeave);
            _inLoopSearch = false;
        }

        protected void RaiseUpdateEnter(StormToken token)
        {
            if (!CurrentToken.Equals(default))
                throw new InvalidOperationException("Unexpected state");

            if (_visitState != EState.Idle || _inLoopSearch)
                throw new InvalidOperationException("Can't enter now");

            CurrentToken = token;

            _visitState = EState.Entering;
            try
            {
                OnVisitEvent?.Invoke(token, EStormVisitType.UpdateEnter);
            }
            finally
            {
                _visitState = EState.Entered;
            }
        }

        protected void RaiseUpdateLeave(StormToken token, bool hasChanged)
        {
            if (!CurrentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            if (_visitState != EState.Entered || _inLoopSearch)
                throw new InvalidOperationException("Can't leave now");

            _visitState = EState.Leaving;
            OnVisitEvent?.Invoke(token, hasChanged ? EStormVisitType.UpdateLeaveChanged : EStormVisitType.UpdateLeaveUnchanged);
            _visitState = EState.Idle;
            CurrentToken = default;
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