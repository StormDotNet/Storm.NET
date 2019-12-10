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

    public static partial class StormExtensions
    {
        public static IStorm<T> SwitchWithCompare<T>(this IStorm<IStorm<T>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new StormSwitch<T>(selector, EqualityComparer<T>.Default);
        }

        public static IStorm<T> SwitchWithCompare<T>(this IStorm<IStorm<T>> selector, IEqualityComparer<T> comparer)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            return new StormSwitch<T>(selector, comparer);
        }

        public static IStorm<T> SwitchWithoutCompare<T>(this IStorm<IStorm<T>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new StormSwitch<T>(selector, null);
        }
    }
}