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

    internal class StormFuncFromStates<TFirst, TResult>
                 : StormFuncBase<TFirst, TResult>
    {
        private readonly Func<StormFuncInput<TFirst>, TResult> _func;

        public StormFuncFromStates(
            IStorm<TFirst> first,
            Func<StormFuncInput<TFirst>, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {
            try
            {
                var firstState = new StormFuncInput<TFirst>(First, GetState(0));

                return TrySetValue(_func(firstState));
            }
            catch (StormError e)
            {
                return TrySetError(e);
            }
            catch (Exception e)
            {
                return TrySetError(Error.Func.Evaluation(e));
            }
        }
    }

    internal class StormFuncFromStates<TFirst, TSecond, TResult>
                 : StormFuncBase<TFirst, TSecond, TResult>
    {
        private readonly Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, TResult> _func;

        public StormFuncFromStates(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {
            try
            {
                var firstState = new StormFuncInput<TFirst>(First, GetState(0));
                var secondState = new StormFuncInput<TSecond>(Second, GetState(1));

                return TrySetValue(_func(firstState, secondState));
            }
            catch (StormError e)
            {
                return TrySetError(e);
            }
            catch (Exception e)
            {
                return TrySetError(Error.Func.Evaluation(e));
            }
        }
    }

    internal class StormFuncFromStates<TFirst, TSecond, TThird, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TResult>
    {
        private readonly Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, TResult> _func;

        public StormFuncFromStates(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {
            try
            {
                var firstState = new StormFuncInput<TFirst>(First, GetState(0));
                var secondState = new StormFuncInput<TSecond>(Second, GetState(1));
                var thirdState = new StormFuncInput<TThird>(Third, GetState(2));

                return TrySetValue(_func(firstState, secondState, thirdState));
            }
            catch (StormError e)
            {
                return TrySetError(e);
            }
            catch (Exception e)
            {
                return TrySetError(Error.Func.Evaluation(e));
            }
        }
    }

    internal class StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TResult>
    {
        private readonly Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, TResult> _func;

        public StormFuncFromStates(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {
            try
            {
                var firstState = new StormFuncInput<TFirst>(First, GetState(0));
                var secondState = new StormFuncInput<TSecond>(Second, GetState(1));
                var thirdState = new StormFuncInput<TThird>(Third, GetState(2));
                var fourthState = new StormFuncInput<TFourth>(Fourth, GetState(3));

                return TrySetValue(_func(firstState, secondState, thirdState, fourthState));
            }
            catch (StormError e)
            {
                return TrySetError(e);
            }
            catch (Exception e)
            {
                return TrySetError(Error.Func.Evaluation(e));
            }
        }
    }

    internal class StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TFifth, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TResult>
    {
        private readonly Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, TResult> _func;

        public StormFuncFromStates(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, fifth, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {
            try
            {
                var firstState = new StormFuncInput<TFirst>(First, GetState(0));
                var secondState = new StormFuncInput<TSecond>(Second, GetState(1));
                var thirdState = new StormFuncInput<TThird>(Third, GetState(2));
                var fourthState = new StormFuncInput<TFourth>(Fourth, GetState(3));
                var fifthState = new StormFuncInput<TFifth>(Fifth, GetState(4));

                return TrySetValue(_func(firstState, secondState, thirdState, fourthState, fifthState));
            }
            catch (StormError e)
            {
                return TrySetError(e);
            }
            catch (Exception e)
            {
                return TrySetError(Error.Func.Evaluation(e));
            }
        }
    }

    internal class StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>
    {
        private readonly Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, TResult> _func;

        public StormFuncFromStates(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, fifth, sixth, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {
            try
            {
                var firstState = new StormFuncInput<TFirst>(First, GetState(0));
                var secondState = new StormFuncInput<TSecond>(Second, GetState(1));
                var thirdState = new StormFuncInput<TThird>(Third, GetState(2));
                var fourthState = new StormFuncInput<TFourth>(Fourth, GetState(3));
                var fifthState = new StormFuncInput<TFifth>(Fifth, GetState(4));
                var sixthState = new StormFuncInput<TSixth>(Sixth, GetState(5));

                return TrySetValue(_func(firstState, secondState, thirdState, fourthState, fifthState, sixthState));
            }
            catch (StormError e)
            {
                return TrySetError(e);
            }
            catch (Exception e)
            {
                return TrySetError(Error.Func.Evaluation(e));
            }
        }
    }

    internal class StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>
    {
        private readonly Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, StormFuncInput<TSeventh>, TResult> _func;

        public StormFuncFromStates(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, StormFuncInput<TSeventh>, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, fifth, sixth, seventh, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {
            try
            {
                var firstState = new StormFuncInput<TFirst>(First, GetState(0));
                var secondState = new StormFuncInput<TSecond>(Second, GetState(1));
                var thirdState = new StormFuncInput<TThird>(Third, GetState(2));
                var fourthState = new StormFuncInput<TFourth>(Fourth, GetState(3));
                var fifthState = new StormFuncInput<TFifth>(Fifth, GetState(4));
                var sixthState = new StormFuncInput<TSixth>(Sixth, GetState(5));
                var seventhState = new StormFuncInput<TSeventh>(Seventh, GetState(6));

                return TrySetValue(_func(firstState, secondState, thirdState, fourthState, fifthState, sixthState, seventhState));
            }
            catch (StormError e)
            {
                return TrySetError(e);
            }
            catch (Exception e)
            {
                return TrySetError(Error.Func.Evaluation(e));
            }
        }
    }

    internal class StormFuncFromStates<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>
    {
        private readonly Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, StormFuncInput<TSeventh>, StormFuncInput<TEighth>, TResult> _func;

        public StormFuncFromStates(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            IStorm<TEighth> eighth,
            Func<StormFuncInput<TFirst>, StormFuncInput<TSecond>, StormFuncInput<TThird>, StormFuncInput<TFourth>, StormFuncInput<TFifth>, StormFuncInput<TSixth>, StormFuncInput<TSeventh>, StormFuncInput<TEighth>, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, fifth, sixth, seventh, eighth, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {
            try
            {
                var firstState = new StormFuncInput<TFirst>(First, GetState(0));
                var secondState = new StormFuncInput<TSecond>(Second, GetState(1));
                var thirdState = new StormFuncInput<TThird>(Third, GetState(2));
                var fourthState = new StormFuncInput<TFourth>(Fourth, GetState(3));
                var fifthState = new StormFuncInput<TFifth>(Fifth, GetState(4));
                var sixthState = new StormFuncInput<TSixth>(Sixth, GetState(5));
                var seventhState = new StormFuncInput<TSeventh>(Seventh, GetState(6));
                var eighthState = new StormFuncInput<TEighth>(Eighth, GetState(7));

                return TrySetValue(_func(firstState, secondState, thirdState, fourthState, fifthState, sixthState, seventhState, eighthState));
            }
            catch (StormError e)
            {
                return TrySetError(e);
            }
            catch (Exception e)
            {
                return TrySetError(Error.Func.Evaluation(e));
            }
        }
    }

}