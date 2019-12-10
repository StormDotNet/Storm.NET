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

namespace StormDotNet.Factories.Switch
{
    using System;
    using System.Collections.Generic;

    public class StormSwitchFactoryFromValuesWithCompare
    {
        internal StormSwitchFactoryFromValuesWithCompare()
        {
        }

        public StormSwitchFactoryFromValuesWithCompare FromValues => SwitchFactories.FromValuesWithCompare;
        public StormSwitchFactoryFromStatesWithCompare FromStates => SwitchFactories.FromStatesWithCompare;
        public StormSwitchFactoryFromValuesWithCompare WithCompare => SwitchFactories.FromValuesWithCompare;
        public StormSwitchFactoryFromValuesWithoutCompare WithoutCompare => SwitchFactories.FromValuesWithoutCompare;

        public IStorm<TResult> Create<TFirst, TResult>(
            IStorm<TFirst> first,
            Func<TFirst, IStorm<TResult>> func)
        {
            return Storm.Func.Create(first, func).SwitchWithCompare();
        }

        public IStorm<TResult> Create<TFirst, TResult>(
            IStorm<TFirst> first,
            Func<TFirst, IStorm<TResult>> func,
            IEqualityComparer<TResult> comparer)
        {
            return Storm.Func.Create(first, func).SwitchWithCompare(comparer);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            Func<TFirst, TSecond, IStorm<TResult>> func)
        {
            return Storm.Func.Create(first, second, func).SwitchWithCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            Func<TFirst, TSecond, IStorm<TResult>> func,
            IEqualityComparer<TResult> comparer)
        {
            return Storm.Func.Create(first, second, func).SwitchWithCompare(comparer);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            Func<TFirst, TSecond, TThird, IStorm<TResult>> func)
        {
            return Storm.Func.Create(first, second, third, func).SwitchWithCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            Func<TFirst, TSecond, TThird, IStorm<TResult>> func,
            IEqualityComparer<TResult> comparer)
        {
            return Storm.Func.Create(first, second, third, func).SwitchWithCompare(comparer);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            Func<TFirst, TSecond, TThird, TFourth, IStorm<TResult>> func)
        {
            return Storm.Func.Create(first, second, third, fourth, func).SwitchWithCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            Func<TFirst, TSecond, TThird, TFourth, IStorm<TResult>> func,
            IEqualityComparer<TResult> comparer)
        {
            return Storm.Func.Create(first, second, third, fourth, func).SwitchWithCompare(comparer);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, IStorm<TResult>> func)
        {
            return Storm.Func.Create(first, second, third, fourth, fifth, func).SwitchWithCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, IStorm<TResult>> func,
            IEqualityComparer<TResult> comparer)
        {
            return Storm.Func.Create(first, second, third, fourth, fifth, func).SwitchWithCompare(comparer);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, IStorm<TResult>> func)
        {
            return Storm.Func.Create(first, second, third, fourth, fifth, sixth, func).SwitchWithCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, IStorm<TResult>> func,
            IEqualityComparer<TResult> comparer)
        {
            return Storm.Func.Create(first, second, third, fourth, fifth, sixth, func).SwitchWithCompare(comparer);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, IStorm<TResult>> func)
        {
            return Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, func).SwitchWithCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, IStorm<TResult>> func,
            IEqualityComparer<TResult> comparer)
        {
            return Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, func).SwitchWithCompare(comparer);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            IStorm<TEighth> eighth,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, IStorm<TResult>> func)
        {
            return Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, func).SwitchWithCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            IStorm<TEighth> eighth,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, IStorm<TResult>> func,
            IEqualityComparer<TResult> comparer)
        {
            return Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, func).SwitchWithCompare(comparer);
        }

    }
}