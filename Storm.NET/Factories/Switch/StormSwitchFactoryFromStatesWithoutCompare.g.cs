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

    public class StormSwitchFactoryFromStatesWithoutCompare
    {
        internal StormSwitchFactoryFromStatesWithoutCompare()
        {
        }

        public StormSwitchFactoryFromValuesWithoutCompare FromValues => SwitchFactories.FromValuesWithoutCompare;
        public StormSwitchFactoryFromStatesWithoutCompare FromStates => SwitchFactories.FromStatesWithoutCompare;
        public StormSwitchFactoryFromStatesWithCompare WithCompare => SwitchFactories.FromStatesWithCompare;
        public StormSwitchFactoryFromStatesWithoutCompare WithoutCompare => SwitchFactories.FromStatesWithoutCompare;

        public IStorm<TResult> Create<TFirst, TResult>(
            IStorm<TFirst> first,
            Func<StormFuncInput<TFirst>, IStorm<TResult>> func)
        {
            return Storm.Func.FromStates.Create(first, func).SwitchWithoutCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, IStorm<TResult>> func)
        {
            return Storm.Func.FromStates.Create(first, second, func).SwitchWithoutCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, IStorm<TResult>> func)
        {
            return Storm.Func.FromStates.Create(first, second, third, func).SwitchWithoutCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, IStorm<TResult>> func)
        {
            return Storm.Func.FromStates.Create(first, second, third, fourth, func).SwitchWithoutCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, IStorm<TResult>> func)
        {
            return Storm.Func.FromStates.Create(first, second, third, fourth, fifth, func).SwitchWithoutCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, IStorm<TResult>> func)
        {
            return Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, func).SwitchWithoutCompare();
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, StormFuncInput<TSeventh>, IStorm<TResult>> func)
        {
            return Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, seventh, func).SwitchWithoutCompare();
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
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, StormFuncInput<TSeventh>, StormFuncInput<TEighth>, IStorm<TResult>> func)
        {
            return Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, func).SwitchWithoutCompare();
        }

    }
}