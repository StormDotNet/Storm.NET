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

    internal static class IsDescendantHelper
    {
        internal static bool IsDescendant<T>(StormToken token,
                                             IStorm<T> root,
                                             Action<StormToken, EStormVisitType>? rootOnVisit,
                                             IStormNode otherNode)
        {
            if (otherNode == root)
                return true;

            if (otherNode is IStormSocket<T> socket && socket.Target == root)
                return true;

            if (rootOnVisit == null)
                return false;

            var hasEntered = false;

            void TargetOnVisit(StormToken enteredToken, EStormVisitType visitType)
            {
                hasEntered |= Equals(enteredToken, token) && visitType == EStormVisitType.EnterLoopSearch;
            }

            otherNode.OnVisit += TargetOnVisit;
            rootOnVisit.Invoke(token, EStormVisitType.EnterLoopSearch);
            rootOnVisit.Invoke(token, EStormVisitType.LeaveLoopSearch);
            otherNode.OnVisit -= TargetOnVisit;

            return hasEntered;
        }
    }
}