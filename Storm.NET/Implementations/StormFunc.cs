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
    using Exceptions;

    internal class StormFunc<TSource, TResult> : StormBase<TResult>
    {
        private readonly IStorm<TSource> _source;
        private readonly Func<TSource, TResult> _func;

        public StormFunc(IStorm<TSource> source,
                         Func<TSource, TResult> func,
                         IEqualityComparer<TResult> comparer) : base(comparer)
        {
            _source = source;
            _func = func;
            _source.OnEnter += Enter;
            _source.OnLeave += Leave;
        }

        private void Leave(IStormToken token, bool hasChanged)
        {
            if (!hasChanged)
            {
                LeaveUnchanged(token);
                return;
            }

            void OnEmpty() => LeaveEmpty(token);

            void OnError(Exception error) => LeaveWithError(token, error);

            void OnValue(TSource sourceValue)
            {
                try
                {
                    var value = _func(sourceValue);
                    LeaveWithValue(token, value);
                }
                catch (Exception e)
                {
                    LeaveWithError(token, new StormFuncEvaluationException(e));
                }
            }

            _source.Match(OnEmpty, OnError, OnValue);
        }
    }

    internal class StormFunc<TFirst, TSecond, TResult> : StormBase<TResult>
    {
        private readonly IStorm<TFirst> _first;
        private readonly IStorm<TSecond> _second;
        private readonly Func<TFirst, TSecond, TResult> _func;

        public StormFunc(IStorm<TFirst> first,
                         IStorm<TSecond> second,
                         Func<TFirst, TSecond, TResult> func,
                         IEqualityComparer<TResult> comparer) : base(comparer)
        {
            _first = first;
            _second = second;
            _func = func;

            var multiView = new MultiView(first, second);
            multiView.OnEnter += OnFirstEnter;
            multiView.OnLeave += OnLastLeave;
        }

        private void OnFirstEnter(IStormToken token) => Enter(token);

        private void OnLastLeave(IStormToken token, bool hasChanged)
        {
            if (!hasChanged)
            {
                LeaveUnchanged(token);
                return;
            }

            if (_first.TryGetValue(out var firstValue) &&
                _second.TryGetValue(out var second))
            {
                var result = _func(firstValue, second);
                LeaveWithValue(token, result);
                return;
            }

            if (_first.TryGetError(out var firstError) |
                _second.TryGetError(out var secondError))
            {
                LeaveWithError(token,
                               new Exception("Source have an error", new AggregateException(firstError, secondError)));
                return;
            }

            LeaveEmpty(token);
        }
    }
}