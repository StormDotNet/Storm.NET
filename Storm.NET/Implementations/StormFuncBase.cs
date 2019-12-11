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

    internal abstract class StormFuncBase<T> : StormBase<T>
    {
        private readonly EStormSourceState[] _sourceStates;

        private int _enteredCount;
        private int _enteredLoopSearchCount;
        private bool _hasChanged;

        protected StormFuncBase(int length, IEqualityComparer<T>? comparer) : base(comparer)
        {
            _sourceStates = new EStormSourceState[length];
        }

        protected void OnVisit(int index, IStormToken token, EStormVisitType visitType)
        {
            switch (visitType)
            {
                case EStormVisitType.Enter:
                    OnEnter(index, token);
                    break;
                case EStormVisitType.LeaveChanged:
                    OnLeave(index, token, true);
                    break;
                case EStormVisitType.LeaveUnchanged:
                    OnLeave(index, token, false);
                    break;
                case EStormVisitType.EnterLoopSearch:
                    OnEnterLoopSearch(token);
                    break;
                case EStormVisitType.LeaveLoopSearch:
                    OnLeaveLoopSearch(token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(visitType), visitType, null);
            }
        }

        protected EStormFuncInputState GetState(int index)
        {
            return _sourceStates[index] switch
            {
                EStormSourceState.Idle => EStormFuncInputState.NotVisited,
                EStormSourceState.Enter => throw new InvalidOperationException("Token hasn't leave."),
                EStormSourceState.LeaveChanged => EStormFuncInputState.VisitedWithChange,
                EStormSourceState.LeaveUnchanged => EStormFuncInputState.VisitedWithoutChange,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected EStormSourceState GetSourceState(int index) => _sourceStates[index];

        protected abstract void Leave(IStormToken token);

        protected virtual void OnValidatedLeave(int index, bool hasChanged)
        {
        }

        private void OnEnter(int index, IStormToken token)
        {
            if (_enteredCount == 0)
            {
                Enter(token);
            }
            else if (!Equals(token, CurrentToken))
            {
                throw new InvalidOperationException("Unknown token");
            }

            _sourceStates[index] = _sourceStates[index] == EStormSourceState.Idle
                                       ? EStormSourceState.Enter
                                       : throw new InvalidOperationException("Can't enter now");
            _enteredCount++;
        }

        private void OnLeave(int index, IStormToken token, bool hasChanged)
        {
            if (token != CurrentToken)
                throw new InvalidOperationException("Unknown token");

            _sourceStates[index] = _sourceStates[index] == EStormSourceState.Enter
                                       ? hasChanged ? EStormSourceState.LeaveChanged : EStormSourceState.LeaveUnchanged
                                       : throw new InvalidOperationException("Can't leave here.");

            _enteredCount--;
            _hasChanged |= hasChanged;

            OnValidatedLeave(index, hasChanged);

            if (_enteredCount == 0)
            {
                if (_hasChanged)
                    Leave(CurrentToken);
                else
                    LeaveUnchanged(CurrentToken);

                _hasChanged = false;
                for (var i = 0; i < _sourceStates.Length; i++)
                    _sourceStates[i] = EStormSourceState.Idle;
            }
        }

        private void OnEnterLoopSearch(IStormToken token)
        {
            if (CurrentToken != Storm.Token.Initial && CurrentToken != token)
                throw new InvalidOperationException("Unknown token");

            if (_enteredLoopSearchCount == 0)
                EnterLoopSearch(token);

            _enteredLoopSearchCount++;
        }

        private void OnLeaveLoopSearch(IStormToken token)
        {
            if (CurrentToken != Storm.Token.Initial && CurrentToken != token)
                throw new InvalidOperationException("Unknown token");

            _enteredLoopSearchCount--;

            if (_enteredLoopSearchCount == 0)
                LeaveLoopSearch(token);
        }

        protected enum EStormSourceState
        {
            Idle = 0,
            Enter,
            LeaveChanged,
            LeaveUnchanged
        }
    }
}