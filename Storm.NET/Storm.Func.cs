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
    using System.Collections.Generic;
    using Implementations;

    public static partial class Storm
    {
        public static class Func
        {
            public static IStorm<TResult> WithCompare<TFirst, TResult>(
                IStorm<TFirst> first,
                Func<TFirst, TResult> func)
            {
                if (first == null) throw new ArgumentNullException(nameof(first));
                if (func == null) throw new ArgumentNullException(nameof(func));
                
                return new StormFunc<TFirst, TResult>(first, func, EqualityComparer<TResult>.Default);
            }

            public static IStorm<TResult> WithCompare<TFirst, TSecond, TResult>(
                IStorm<TFirst> first,
                IStorm<TSecond> second,
                Func<TFirst, TSecond, TResult> func)
            {
                if (first == null) throw new ArgumentNullException(nameof(first));
                if (second == null) throw new ArgumentNullException(nameof(second));
                if (func == null) throw new ArgumentNullException(nameof(func));

                return new StormFunc<TFirst, TSecond, TResult>(first, second, func, EqualityComparer<TResult>.Default);
            }
        }
    }
}