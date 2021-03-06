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

    internal abstract class StormFuncBase<T> : StormBase<T>
    {
        private readonly StormSourceState[] _sourceStates;

        private int _enteredCount;
        private int _enteredLoopSearchCount;
        private bool _hasChanged;

        protected StormFuncBase(int length, IEqualityComparer<T>? comparer) : base(comparer)
        {
            _sourceStates = new StormSourceState[length];
        }

        protected void SourceOnVisit(int index, StormToken token, EStormVisitType visitType)
        {
            if (token.Equals(default)) throw new ArgumentException("Default token not allowed", nameof(token));

            switch (visitType)
            {
                case EStormVisitType.EnterUpdate:
                    SourceOnEnterUpdate(index, token);
                    break;
                case EStormVisitType.LeaveUpdateChanged:
                    SourceOnLeaveUpdate(index, token, true);
                    break;
                case EStormVisitType.LeaveUpdateUnchanged:
                    SourceOnLeaveUpdate(index, token, false);
                    break;
                case EStormVisitType.EnterLoopSearch:
                    SourceOnEnterLoopSearch(index, token);
                    break;
                case EStormVisitType.LeaveLoopSearch:
                    SourceOnLeaveLoopSearch(index, token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(visitType), visitType, null);
            }
        }

        protected EStormFuncInputState GetState(int index) => _sourceStates[index].AsEStormFuncInputState();

        protected abstract bool Update();

        protected virtual void SourceOnLeaveUpdateChanged(int index)
        {
        }

        private void SourceOnEnterLoopSearch(int index, StormToken token)
        {
            var state = GetVisitState(out var enteredToken);

            if (state.HasToken() && !Equals(enteredToken, token))
                throw new InvalidOperationException("Unknown token");

            _sourceStates[index] = _sourceStates[index].EnterLoopSearch();
            _enteredLoopSearchCount++;

            if (_enteredLoopSearchCount == 1)
                EnterLoopSearch(token);
        }

        private void SourceOnLeaveLoopSearch(int index, StormToken token)
        {
            var state = GetVisitState(out var enteredToken);
            if (!state.CanLeaveLoopSearch())
                throw new InvalidOperationException("Can't leave now");

            if (!Equals(enteredToken, token))
                throw new InvalidOperationException("Unknown token");

            _sourceStates[index] = _sourceStates[index].LeaveLoopSearch();
            _enteredLoopSearchCount--;

            if (_enteredLoopSearchCount == 0)
                LeaveLoopSearch(token);
        }

        private void SourceOnEnterUpdate(int index, StormToken token)
        {
            var state = GetVisitState(out var enteredToken);
            if (_enteredCount == 0 && !state.CanEnterUpdate())
                throw new InvalidOperationException("Can't enter update now");

            if (state.HasToken() && !Equals(enteredToken, token))
                throw new InvalidOperationException("Unknown token");

            _sourceStates[index] = _sourceStates[index].EnterUpdate();
            _enteredCount++;

            if (_enteredCount == 1)
                EnterUpdate(token);
        }

        private void SourceOnLeaveUpdate(int index, StormToken token, bool hasChanged)
        {
            var state = GetVisitState(out var enteredToken);
            if (!state.CanLeaveUpdate())
                throw new InvalidOperationException("Can't leave now");

            if (!Equals(enteredToken, token))
                throw new InvalidOperationException("Unknown token");

            _sourceStates[index] = _sourceStates[index].LeaveUpdate(hasChanged);

            if (hasChanged)
                SourceOnLeaveUpdateChanged(index);

            _enteredCount--;
            _hasChanged |= hasChanged;

            if (_enteredCount == 0)
            {
                if (_hasChanged)
                    _hasChanged = Update();

                LeaveUpdate(token, _hasChanged);

                _hasChanged = false;
                for (var i = 0; i < _sourceStates.Length; i++)
                    _sourceStates[i] = StormSourceState.Idle;
            }
        }
    }
}