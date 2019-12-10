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

namespace StormDotNet.Factories.Func
{
    using System;
    using Implementations;

    public class StormFuncFactoryFromStatesWithoutCompare
    {
        internal StormFuncFactoryFromStatesWithoutCompare()
        {
        }

        public StormFuncFactoryFromValuesWithoutCompare FromValues => FuncFactories.FromValuesWithoutCompare;
        public StormFuncFactoryFromStatesWithoutCompare FromStates => FuncFactories.FromStatesWithoutCompare;
        public StormFuncFactoryFromStatesWithCompare WithCompare => FuncFactories.FromStatesWithCompare;
        public StormFuncFactoryFromStatesWithoutCompare WithoutCompare => FuncFactories.FromStatesWithoutCompare;

        public IStorm<TResult> Create<TFirst, TResult>(
            IStorm<TFirst> first,
            Func<StormFuncInput<TFirst>, TResult> func)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return new StormFuncFromStates<TFirst, TResult>(first, func, null);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, TResult> func)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return new StormFuncFromStates<TFirst, TSecond, TResult>(first, second, func, null);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, TResult> func)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return new StormFuncFromStates<TFirst, TSecond, TThird, TResult>(first, second, third, func, null);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, TResult> func)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return new StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TResult>(first, second, third, fourth, func, null);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, TResult> func)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (fifth == null) throw new ArgumentNullException(nameof(fifth));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return new StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(first, second, third, fourth, fifth, func, null);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, TResult> func)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (fifth == null) throw new ArgumentNullException(nameof(fifth));
            if (sixth == null) throw new ArgumentNullException(nameof(sixth));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return new StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(first, second, third, fourth, fifth, sixth, func, null);
        }

        public IStorm<TResult> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, StormFuncInput<TSeventh>, TResult> func)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (fifth == null) throw new ArgumentNullException(nameof(fifth));
            if (sixth == null) throw new ArgumentNullException(nameof(sixth));
            if (seventh == null) throw new ArgumentNullException(nameof(seventh));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return new StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(first, second, third, fourth, fifth, sixth, seventh, func, null);
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
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, StormFuncInput<TSeventh>, StormFuncInput<TEighth>, TResult> func)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (fifth == null) throw new ArgumentNullException(nameof(fifth));
            if (sixth == null) throw new ArgumentNullException(nameof(sixth));
            if (seventh == null) throw new ArgumentNullException(nameof(seventh));
            if (eighth == null) throw new ArgumentNullException(nameof(eighth));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return new StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(first, second, third, fourth, fifth, sixth, seventh, eighth, func, null);
        }

    }
}