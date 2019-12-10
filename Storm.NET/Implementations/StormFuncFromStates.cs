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

    internal class StormFuncFromStates<TSource, TResult> : StormBase<TResult>
    {
        private readonly IStorm<TSource> _source;
        private readonly Func<StormFuncInput<TSource>, TResult> _func;

        public StormFuncFromStates(IStorm<TSource> source,
                                   Func<StormFuncInput<TSource>, TResult> func,
                                   IEqualityComparer<TResult> comparer) : base(comparer)
        {
            _source = source;
            _func = func;
            _source.OnVisit += SourceOnVisit;
        }

        private void SourceOnVisit(IStormToken token, EStormVisitType visitType)
        {
            switch (visitType)
            {
                case EStormVisitType.Enter:
                    Enter(token);
                    break;
                case EStormVisitType.LeaveChanged:
                    TResult value;
                    try
                    {
                        var sourceState = new StormFuncInput<TSource>(_source, EStormFuncInputState.VisitedWithChange);
                        value = _func(sourceState);
                    }
                    catch (StormError e)
                    {
                        LeaveWithError(token, e);
                        return;
                    }
                    catch (Exception e)
                    {
                        LeaveWithError(token, Error.Func.Evaluation(e));
                        return;
                    }

                    LeaveWithValue(token, value);
                    break;
                case EStormVisitType.LeaveUnchanged:
                    LeaveUnchanged(token);
                    break;
                case EStormVisitType.EnterLoopSearch:
                    EnterLoopSearch(token);
                    break;
                case EStormVisitType.LeaveLoopSearch:
                    LeaveLoopSearch(token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(visitType), visitType, null);
            }
        }
    }
}