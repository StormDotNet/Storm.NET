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

    public abstract class StormBase<T> : StormContentBase<T>, IStorm<T>
    {
        private EState _visitState = EState.Idle;
        private bool _inLoopSearch;

        protected StormBase(IEqualityComparer<T>? comparer) : base(comparer)
        {
            CurrentToken = Storm.Token.Initial;
        }

        protected IStormToken CurrentToken { get; private set; }

        protected void Enter(IStormToken token)
        {
            if (CurrentToken != Storm.Token.Initial)
                throw new InvalidOperationException("Unexpected state");

            if (_visitState != EState.Idle || _inLoopSearch)
                throw new InvalidOperationException("Can't enter now");

            _visitState = EState.Entering;
            OnVisit?.Invoke(token, EStormVisitType.Enter);
            _visitState = EState.Entered;

            CurrentToken = token;
        }

        protected void EnterLoopSearch(IStormToken token)
        {
            if (CurrentToken != Storm.Token.Initial && CurrentToken != token)
                throw new InvalidOperationException("Unknown token");

            if (_inLoopSearch)
                throw new InvalidOperationException("Already in loop search");

            _inLoopSearch = true;
            OnVisit?.Invoke(token, EStormVisitType.EnterLoopSearch);
        }

        protected bool IsDescendant(IStormNode target)
        {
            if (target == this)
                return true;

            if (target is IStormSocket socket && socket.Target == this)
                return true;

            if (OnVisit == null)
                return false;

            var hasEntered = false;

            void TargetOnVisit(IStormToken enteredToken, EStormVisitType visitType)
            {
                hasEntered |= CurrentToken.Equals(enteredToken) && visitType == EStormVisitType.EnterLoopSearch;
            }

            target.OnVisit += TargetOnVisit;
            OnVisit.Invoke(CurrentToken, EStormVisitType.EnterLoopSearch);
            OnVisit.Invoke(CurrentToken, EStormVisitType.LeaveLoopSearch);
            target.OnVisit -= TargetOnVisit;

            return hasEntered;
        }

        private void Leave(IStormToken token, bool hasChanged)
        {
            _visitState = EState.Leaving;
            OnVisit?.Invoke(token, hasChanged ? EStormVisitType.LeaveChanged : EStormVisitType.LeaveUnchanged);
            _visitState = EState.Idle;
            CurrentToken = Storm.Token.Initial;
        }

        protected void LeaveLoopSearch(IStormToken token)
        {
            if (CurrentToken != Storm.Token.Initial && CurrentToken != token)
                throw new InvalidOperationException("Unknown token");

            if (!_inLoopSearch)
                throw new InvalidOperationException("Not in loop search");

            OnVisit?.Invoke(token, EStormVisitType.LeaveLoopSearch);
            _inLoopSearch = false;
        }

        protected void LeaveUnchanged(IStormToken token)
        {
            if (token != CurrentToken)
                throw new InvalidOperationException("Unknown token");

            if (_visitState != EState.Entered || _inLoopSearch)
                throw new InvalidOperationException("Can't leave now");

            Leave(token, false);
        }

        protected void LeaveWithError(IStormToken token, StormError error)
        {
            if (token != CurrentToken)
                throw new InvalidOperationException("Unknown token");

            if (_visitState != EState.Entered || _inLoopSearch)
                throw new InvalidOperationException("Can't leave now");

            var hasChanged = SetError(error);
            Leave(token, hasChanged);
        }

        protected void LeaveWithValue(IStormToken token, T value)
        {
            if (token != CurrentToken)
                throw new InvalidOperationException("Unknown token");

            if (_visitState != EState.Entered || _inLoopSearch)
                throw new InvalidOperationException("Can't leave now");

            var hasChanged = SetValue(value);
            Leave(token, hasChanged);
        }

        private event Action<IStormToken, EStormVisitType>? OnVisit;

        event Action<IStormToken, EStormVisitType>? IStormNode.OnVisit
        {
            add
            {
                OnVisit += value;
                if (_visitState == EState.Entered)
                {
                    value?.Invoke(CurrentToken, EStormVisitType.Enter);
                }
            }
            remove => OnVisit -= value;
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