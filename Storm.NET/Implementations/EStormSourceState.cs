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

    [Flags]
    internal enum EStormSourceState
    {
        Idle = 0,
        Update = 1,
        LoopSearch = 2,
        Changed = 4,
        Unchanged = 8
    }

    internal static class StormSourceStateExtensions
    {
        internal static EStormSourceState EnterLoopSearch(this EStormSourceState state)
        {
            if (state.HasFlag(EStormSourceState.LoopSearch))
                throw new InvalidOperationException("Can't enter loop search now");

            return state | EStormSourceState.LoopSearch;
        }

        internal static EStormSourceState EnterUpdate(this EStormSourceState state)
        {
            if (state != EStormSourceState.Idle)
                throw new InvalidOperationException("Can't enter update now");
            return EStormSourceState.Update;
        }

        internal static EStormSourceState LeaveLoopSearch(this EStormSourceState state)
        {
            if (!state.HasFlag(EStormSourceState.LoopSearch))
                throw new InvalidOperationException("Can't leave loop search now");

            return state & ~EStormSourceState.LoopSearch;
        }

        internal static EStormSourceState LeaveUpdate(this EStormSourceState state, bool hasChanged)
        {
            if (state != EStormSourceState.Update)
                throw new InvalidOperationException("Can't leave update now");

            return hasChanged ? EStormSourceState.Changed : EStormSourceState.Unchanged;
        }
    }
}