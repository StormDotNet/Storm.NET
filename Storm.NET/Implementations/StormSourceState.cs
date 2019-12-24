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

    internal readonly struct StormSourceState
    {
        internal static StormSourceState Idle { get; } = new StormSourceState();

        private const int FlagUpdate = 1;
        private const int FlagLoopSearch = 2;
        private const int FlagChanged = 4;
        private const int FlagUnchanged = 8;

        private readonly int _state;

        private StormSourceState(in int state)
        {
            _state = state;
        }

        private bool CanEnterLoopSearch() => (_state & ~FlagUpdate) == 0;
        private bool CanLeaveLoopSearch() => (_state & ~FlagUpdate) == FlagLoopSearch;
        private bool CanEnterUpdate() => _state == 0;
        private bool CanLeaveUpdate() => _state == FlagUpdate;

        public StormSourceState EnterLoopSearch()
        {
            if (!CanEnterLoopSearch())
                throw new InvalidOperationException("Can't enter loop search now");

            return new StormSourceState(_state | FlagLoopSearch);
        }

        public StormSourceState EnterUpdate()
        {
            if (!CanEnterUpdate())
                throw new InvalidOperationException("Can't enter update now");

            return new StormSourceState(FlagUpdate);
        }

        public StormSourceState LeaveLoopSearch()
        {
            if (!CanLeaveLoopSearch())
                throw new InvalidOperationException("Can't leave loop search now");

            return new StormSourceState(_state & ~FlagLoopSearch);
        }

        public StormSourceState LeaveUpdate(bool hasChanged)
        {
            if (!CanLeaveUpdate())
                throw new InvalidOperationException("Can't leave update now");

            return new StormSourceState(hasChanged ? FlagChanged : FlagUnchanged);
        }

        public EStormFuncInputState AsEStormFuncInputState()
        {
            return _state switch
            {
                0 => EStormFuncInputState.NotVisited,
                FlagChanged => EStormFuncInputState.VisitedWithChange,
                FlagUnchanged => EStormFuncInputState.VisitedWithoutChange,
                _ => throw new InvalidOperationException("Token is still here."),
            };
        }
    }
}