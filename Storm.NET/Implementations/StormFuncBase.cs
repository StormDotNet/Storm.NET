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

        protected void SourceOnVisit(int index, StormToken token, EStormVisitType visitType)
        {
            switch (visitType)
            {
                case EStormVisitType.UpdateEnter:
                    SourceOnUpdateEnter(index, token);
                    break;
                case EStormVisitType.UpdateLeaveChanged:
                    SourceOnUpdateLeave(index, token, true);
                    break;
                case EStormVisitType.UpdateLeaveUnchanged:
                    SourceOnUpdateLeave(index, token, false);
                    break;
                case EStormVisitType.LoopSearchEnter:
                    SourceOnLoopSearchEnter(token);
                    break;
                case EStormVisitType.LoopSearchLeave:
                    SourceOnLoopSearchLeave(token);
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

        protected abstract bool Update();

        protected virtual void SourceOnChanged(int index)
        {
        }

        private void SourceOnLoopSearchEnter(StormToken token)
        {
            if (!CurrentToken.Equals(default) && !CurrentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            if (_enteredLoopSearchCount == 0)
                RaiseLoopSearchEnter(token);

            _enteredLoopSearchCount++;
        }

        private void SourceOnLoopSearchLeave(StormToken token)
        {
            if (!CurrentToken.Equals(default) && !CurrentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            _enteredLoopSearchCount--;

            if (_enteredLoopSearchCount == 0)
                RaiseLoopSearchLeave(token);
        }

        private void SourceOnUpdateEnter(int index, StormToken token)
        {
            if (_enteredCount == 0)
            {
                RaiseUpdateEnter(token);
            }
            else if (!CurrentToken.Equals(token))
            {
                throw new InvalidOperationException("Unknown token");
            }

            _sourceStates[index] = _sourceStates[index] == EStormSourceState.Idle
                                       ? EStormSourceState.Enter
                                       : throw new InvalidOperationException("Can't enter now");
            _enteredCount++;
        }

        private void SourceOnUpdateLeave(int index, StormToken token, bool hasChanged)
        {
            if (!CurrentToken.Equals(token))
                throw new InvalidOperationException("Unknown token");

            _sourceStates[index] = _sourceStates[index] == EStormSourceState.Enter
                                       ? hasChanged ? EStormSourceState.LeaveChanged : EStormSourceState.LeaveUnchanged
                                       : throw new InvalidOperationException("Can't leave here.");

            _enteredCount--;
            _hasChanged |= hasChanged;

            if (hasChanged)
                SourceOnChanged(index);

            if (_enteredCount == 0)
            {
                if (_hasChanged)
                    _hasChanged = Update();

                RaiseUpdateLeave(token, _hasChanged);

                _hasChanged = false;
                for (var i = 0; i < _sourceStates.Length; i++)
                    _sourceStates[i] = EStormSourceState.Idle;
            }
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