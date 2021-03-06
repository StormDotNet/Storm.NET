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

// ReSharper disable MemberCanBeMadeStatic.Global
namespace StormDotNet.Factories.Input
{
    using System;
    using System.Collections.Generic;
    using Implementations;

    public class StormInputFactoryWithCompare
    {
        internal StormInputFactoryWithCompare()
        {
        }

        public StormInputFactoryWithCompare WithCompare => InputFactories.WithCompare;
        public StormInputFactoryWithoutCompare WithoutCompare => InputFactories.WithoutCompare;

        public IStormInput<T> Create<T>()
        {
            return new StormInput<T>(EqualityComparer<T>.Default);
        }

        public IStormInput<T> Create<T>(T initialValue)
        {
            return new StormInput<T>(initialValue, EqualityComparer<T>.Default);
        }

        public IStormInput<T> Create<T>(IEqualityComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            return new StormInput<T>(comparer);
        }

        public IStormInput<T> Create<T>(T initialValue, IEqualityComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            return new StormInput<T>(initialValue, comparer);
        }
    }
}