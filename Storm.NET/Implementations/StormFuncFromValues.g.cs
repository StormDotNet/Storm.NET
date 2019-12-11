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
    using System.Linq;

    internal class StormFuncFromValues<TFirst, TResult>
                 : StormFuncBase<TFirst, TResult>
    {
        private readonly Func<TFirst, TResult> _func;

        public StormFuncFromValues(
            IStorm<TFirst> first,
            Func<TFirst, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {            
            if (
                First.TryGetValue(out var firstValue))
            {
                try
                {
                    return SetValue(_func(firstValue));
                }
                catch (StormError e)
                {
                    return SetError(e);
                }
                catch (Exception e)
                {
                    return SetError(Error.Func.Evaluation(e));
                }
            }

            var errors = GetErrors().ToArray();
            if (errors.Length > 0)
            {
                return SetError(Error.Func.SourceError(errors));
            }

            return SetError(Error.EmptyContent);
        }
    }

    internal class StormFuncFromValues<TFirst, TSecond, TResult>
                 : StormFuncBase<TFirst, TSecond, TResult>
    {
        private readonly Func<TFirst, TSecond, TResult> _func;

        public StormFuncFromValues(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            Func<TFirst, TSecond, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {            
            if (
                First.TryGetValue(out var firstValue) &&
                Second.TryGetValue(out var secondValue))
            {
                try
                {
                    return SetValue(_func(firstValue, secondValue));
                }
                catch (StormError e)
                {
                    return SetError(e);
                }
                catch (Exception e)
                {
                    return SetError(Error.Func.Evaluation(e));
                }
            }

            var errors = GetErrors().ToArray();
            if (errors.Length > 0)
            {
                return SetError(Error.Func.SourceError(errors));
            }

            return SetError(Error.EmptyContent);
        }
    }

    internal class StormFuncFromValues<TFirst, TSecond, TThird, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TResult>
    {
        private readonly Func<TFirst, TSecond, TThird, TResult> _func;

        public StormFuncFromValues(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            Func<TFirst, TSecond, TThird, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {            
            if (
                First.TryGetValue(out var firstValue) &&
                Second.TryGetValue(out var secondValue) &&
                Third.TryGetValue(out var thirdValue))
            {
                try
                {
                    return SetValue(_func(firstValue, secondValue, thirdValue));
                }
                catch (StormError e)
                {
                    return SetError(e);
                }
                catch (Exception e)
                {
                    return SetError(Error.Func.Evaluation(e));
                }
            }

            var errors = GetErrors().ToArray();
            if (errors.Length > 0)
            {
                return SetError(Error.Func.SourceError(errors));
            }

            return SetError(Error.EmptyContent);
        }
    }

    internal class StormFuncFromValues<TFirst, TSecond, TThird, TFourth, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TResult>
    {
        private readonly Func<TFirst, TSecond, TThird, TFourth, TResult> _func;

        public StormFuncFromValues(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            Func<TFirst, TSecond, TThird, TFourth, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {            
            if (
                First.TryGetValue(out var firstValue) &&
                Second.TryGetValue(out var secondValue) &&
                Third.TryGetValue(out var thirdValue) &&
                Fourth.TryGetValue(out var fourthValue))
            {
                try
                {
                    return SetValue(_func(firstValue, secondValue, thirdValue, fourthValue));
                }
                catch (StormError e)
                {
                    return SetError(e);
                }
                catch (Exception e)
                {
                    return SetError(Error.Func.Evaluation(e));
                }
            }

            var errors = GetErrors().ToArray();
            if (errors.Length > 0)
            {
                return SetError(Error.Func.SourceError(errors));
            }

            return SetError(Error.EmptyContent);
        }
    }

    internal class StormFuncFromValues<TFirst, TSecond, TThird, TFourth, TFifth, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TResult>
    {
        private readonly Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> _func;

        public StormFuncFromValues(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, fifth, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {            
            if (
                First.TryGetValue(out var firstValue) &&
                Second.TryGetValue(out var secondValue) &&
                Third.TryGetValue(out var thirdValue) &&
                Fourth.TryGetValue(out var fourthValue) &&
                Fifth.TryGetValue(out var fifthValue))
            {
                try
                {
                    return SetValue(_func(firstValue, secondValue, thirdValue, fourthValue, fifthValue));
                }
                catch (StormError e)
                {
                    return SetError(e);
                }
                catch (Exception e)
                {
                    return SetError(Error.Func.Evaluation(e));
                }
            }

            var errors = GetErrors().ToArray();
            if (errors.Length > 0)
            {
                return SetError(Error.Func.SourceError(errors));
            }

            return SetError(Error.EmptyContent);
        }
    }

    internal class StormFuncFromValues<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>
    {
        private readonly Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> _func;

        public StormFuncFromValues(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, fifth, sixth, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {            
            if (
                First.TryGetValue(out var firstValue) &&
                Second.TryGetValue(out var secondValue) &&
                Third.TryGetValue(out var thirdValue) &&
                Fourth.TryGetValue(out var fourthValue) &&
                Fifth.TryGetValue(out var fifthValue) &&
                Sixth.TryGetValue(out var sixthValue))
            {
                try
                {
                    return SetValue(_func(firstValue, secondValue, thirdValue, fourthValue, fifthValue, sixthValue));
                }
                catch (StormError e)
                {
                    return SetError(e);
                }
                catch (Exception e)
                {
                    return SetError(Error.Func.Evaluation(e));
                }
            }

            var errors = GetErrors().ToArray();
            if (errors.Length > 0)
            {
                return SetError(Error.Func.SourceError(errors));
            }

            return SetError(Error.EmptyContent);
        }
    }

    internal class StormFuncFromValues<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>
    {
        private readonly Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> _func;

        public StormFuncFromValues(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, fifth, sixth, seventh, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {            
            if (
                First.TryGetValue(out var firstValue) &&
                Second.TryGetValue(out var secondValue) &&
                Third.TryGetValue(out var thirdValue) &&
                Fourth.TryGetValue(out var fourthValue) &&
                Fifth.TryGetValue(out var fifthValue) &&
                Sixth.TryGetValue(out var sixthValue) &&
                Seventh.TryGetValue(out var seventhValue))
            {
                try
                {
                    return SetValue(_func(firstValue, secondValue, thirdValue, fourthValue, fifthValue, sixthValue, seventhValue));
                }
                catch (StormError e)
                {
                    return SetError(e);
                }
                catch (Exception e)
                {
                    return SetError(Error.Func.Evaluation(e));
                }
            }

            var errors = GetErrors().ToArray();
            if (errors.Length > 0)
            {
                return SetError(Error.Func.SourceError(errors));
            }

            return SetError(Error.EmptyContent);
        }
    }

    internal class StormFuncFromValues<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>
                 : StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>
    {
        private readonly Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult> _func;

        public StormFuncFromValues(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            IStorm<TEighth> eighth,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult> func,
            IEqualityComparer<TResult> comparer) : base(first, second, third, fourth, fifth, sixth, seventh, eighth, comparer)
        {
            _func = func;
            Update();
        }

        protected sealed override bool Update()
        {            
            if (
                First.TryGetValue(out var firstValue) &&
                Second.TryGetValue(out var secondValue) &&
                Third.TryGetValue(out var thirdValue) &&
                Fourth.TryGetValue(out var fourthValue) &&
                Fifth.TryGetValue(out var fifthValue) &&
                Sixth.TryGetValue(out var sixthValue) &&
                Seventh.TryGetValue(out var seventhValue) &&
                Eighth.TryGetValue(out var eighthValue))
            {
                try
                {
                    return SetValue(_func(firstValue, secondValue, thirdValue, fourthValue, fifthValue, sixthValue, seventhValue, eighthValue));
                }
                catch (StormError e)
                {
                    return SetError(e);
                }
                catch (Exception e)
                {
                    return SetError(Error.Func.Evaluation(e));
                }
            }

            var errors = GetErrors().ToArray();
            if (errors.Length > 0)
            {
                return SetError(Error.Func.SourceError(errors));
            }

            return SetError(Error.EmptyContent);
        }
    }

}