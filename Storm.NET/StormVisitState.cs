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

namespace StormDotNet
{
    using System;

    public readonly struct StormVisitState
    {
        internal static StormVisitState Idle { get; } = new StormVisitState();
        internal static StormVisitState Update { get; } = new StormVisitState(FlagUpdate);

        private const int FlagUpdate = 1;
        private const int FlagLoopSearch = 2;

        private readonly int _state;

        private StormVisitState(in int state)
        {
            _state = state;
        }

        public bool CanEnterLoopSearch() => (_state & ~FlagUpdate) == 0;
        public bool CanLeaveLoopSearch() => (_state & ~FlagUpdate) == FlagLoopSearch;
        public bool CanEnterUpdate() => _state == 0;
        public bool CanLeaveUpdate() => _state == FlagUpdate;

        public StormVisitState EnterLoopSearch()
        {
            if (!CanEnterLoopSearch())
                throw new InvalidOperationException("Can't enter loop search now");

            return new StormVisitState(_state | FlagLoopSearch);
        }

        public StormVisitState EnterUpdate()
        {
            if (!CanEnterUpdate())
                throw new InvalidOperationException("Can't enter update now");

            return new StormVisitState(FlagUpdate);
        }

        public bool HasToken() => _state != 0;
        public bool IsInLoopSearch() => (_state & FlagLoopSearch) == FlagLoopSearch;
        public bool IsInUpdate() => (_state & FlagUpdate) == FlagUpdate;

        public StormVisitState LeaveLoopSearch()
        {
            if (!CanLeaveLoopSearch())
                throw new InvalidOperationException("Can't leave loop search now");

            return new StormVisitState(_state & ~FlagLoopSearch);
        }

        public StormVisitState LeaveUpdate()
        {
            if (!CanLeaveUpdate())
                throw new InvalidOperationException("Can't leave update now");

            return new StormVisitState(0);
        }
    }
}