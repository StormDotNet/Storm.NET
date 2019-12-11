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

    internal abstract class StormFuncBase<TFirst, TResult> : StormFuncBase<TResult>
    {
        protected StormFuncBase(
            IStorm<TFirst> first,
            IEqualityComparer<TResult> comparer) : base(1, comparer)
        {
            First = first;

            First.OnVisit += FirstOnVisit;
        }

        protected IStorm<TFirst> First { get; }

        protected IEnumerable<Exception> GetErrors()
        {
            StormError error;
            if (First.TryGetError(out error)) yield return error;
        }

        private void FirstOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(0, token, visitType);
    }

    internal abstract class StormFuncBase<TFirst, TSecond, TResult> : StormFuncBase<TResult>
    {
        protected StormFuncBase(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IEqualityComparer<TResult> comparer) : base(2, comparer)
        {
            First = first;
            Second = second;

            First.OnVisit += FirstOnVisit;
            Second.OnVisit += SecondOnVisit;
        }

        protected IStorm<TFirst> First { get; }
        protected IStorm<TSecond> Second { get; }

        protected IEnumerable<Exception> GetErrors()
        {
            StormError error;
            if (First.TryGetError(out error)) yield return error;
            if (Second.TryGetError(out error)) yield return error;
        }

        private void FirstOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(0, token, visitType);
        private void SecondOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(1, token, visitType);
    }

    internal abstract class StormFuncBase<TFirst, TSecond, TThird, TResult> : StormFuncBase<TResult>
    {
        protected StormFuncBase(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IEqualityComparer<TResult> comparer) : base(3, comparer)
        {
            First = first;
            Second = second;
            Third = third;

            First.OnVisit += FirstOnVisit;
            Second.OnVisit += SecondOnVisit;
            Third.OnVisit += ThirdOnVisit;
        }

        protected IStorm<TFirst> First { get; }
        protected IStorm<TSecond> Second { get; }
        protected IStorm<TThird> Third { get; }

        protected IEnumerable<Exception> GetErrors()
        {
            StormError error;
            if (First.TryGetError(out error)) yield return error;
            if (Second.TryGetError(out error)) yield return error;
            if (Third.TryGetError(out error)) yield return error;
        }

        private void FirstOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(0, token, visitType);
        private void SecondOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(1, token, visitType);
        private void ThirdOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(2, token, visitType);
    }

    internal abstract class StormFuncBase<TFirst, TSecond, TThird, TFourth, TResult> : StormFuncBase<TResult>
    {
        protected StormFuncBase(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IEqualityComparer<TResult> comparer) : base(4, comparer)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;

            First.OnVisit += FirstOnVisit;
            Second.OnVisit += SecondOnVisit;
            Third.OnVisit += ThirdOnVisit;
            Fourth.OnVisit += FourthOnVisit;
        }

        protected IStorm<TFirst> First { get; }
        protected IStorm<TSecond> Second { get; }
        protected IStorm<TThird> Third { get; }
        protected IStorm<TFourth> Fourth { get; }

        protected IEnumerable<Exception> GetErrors()
        {
            StormError error;
            if (First.TryGetError(out error)) yield return error;
            if (Second.TryGetError(out error)) yield return error;
            if (Third.TryGetError(out error)) yield return error;
            if (Fourth.TryGetError(out error)) yield return error;
        }

        private void FirstOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(0, token, visitType);
        private void SecondOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(1, token, visitType);
        private void ThirdOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(2, token, visitType);
        private void FourthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(3, token, visitType);
    }

    internal abstract class StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TResult> : StormFuncBase<TResult>
    {
        protected StormFuncBase(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IEqualityComparer<TResult> comparer) : base(5, comparer)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
            Fifth = fifth;

            First.OnVisit += FirstOnVisit;
            Second.OnVisit += SecondOnVisit;
            Third.OnVisit += ThirdOnVisit;
            Fourth.OnVisit += FourthOnVisit;
            Fifth.OnVisit += FifthOnVisit;
        }

        protected IStorm<TFirst> First { get; }
        protected IStorm<TSecond> Second { get; }
        protected IStorm<TThird> Third { get; }
        protected IStorm<TFourth> Fourth { get; }
        protected IStorm<TFifth> Fifth { get; }

        protected IEnumerable<Exception> GetErrors()
        {
            StormError error;
            if (First.TryGetError(out error)) yield return error;
            if (Second.TryGetError(out error)) yield return error;
            if (Third.TryGetError(out error)) yield return error;
            if (Fourth.TryGetError(out error)) yield return error;
            if (Fifth.TryGetError(out error)) yield return error;
        }

        private void FirstOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(0, token, visitType);
        private void SecondOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(1, token, visitType);
        private void ThirdOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(2, token, visitType);
        private void FourthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(3, token, visitType);
        private void FifthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(4, token, visitType);
    }

    internal abstract class StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> : StormFuncBase<TResult>
    {
        protected StormFuncBase(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IEqualityComparer<TResult> comparer) : base(6, comparer)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
            Fifth = fifth;
            Sixth = sixth;

            First.OnVisit += FirstOnVisit;
            Second.OnVisit += SecondOnVisit;
            Third.OnVisit += ThirdOnVisit;
            Fourth.OnVisit += FourthOnVisit;
            Fifth.OnVisit += FifthOnVisit;
            Sixth.OnVisit += SixthOnVisit;
        }

        protected IStorm<TFirst> First { get; }
        protected IStorm<TSecond> Second { get; }
        protected IStorm<TThird> Third { get; }
        protected IStorm<TFourth> Fourth { get; }
        protected IStorm<TFifth> Fifth { get; }
        protected IStorm<TSixth> Sixth { get; }

        protected IEnumerable<Exception> GetErrors()
        {
            StormError error;
            if (First.TryGetError(out error)) yield return error;
            if (Second.TryGetError(out error)) yield return error;
            if (Third.TryGetError(out error)) yield return error;
            if (Fourth.TryGetError(out error)) yield return error;
            if (Fifth.TryGetError(out error)) yield return error;
            if (Sixth.TryGetError(out error)) yield return error;
        }

        private void FirstOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(0, token, visitType);
        private void SecondOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(1, token, visitType);
        private void ThirdOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(2, token, visitType);
        private void FourthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(3, token, visitType);
        private void FifthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(4, token, visitType);
        private void SixthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(5, token, visitType);
    }

    internal abstract class StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> : StormFuncBase<TResult>
    {
        protected StormFuncBase(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            IEqualityComparer<TResult> comparer) : base(7, comparer)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
            Fifth = fifth;
            Sixth = sixth;
            Seventh = seventh;

            First.OnVisit += FirstOnVisit;
            Second.OnVisit += SecondOnVisit;
            Third.OnVisit += ThirdOnVisit;
            Fourth.OnVisit += FourthOnVisit;
            Fifth.OnVisit += FifthOnVisit;
            Sixth.OnVisit += SixthOnVisit;
            Seventh.OnVisit += SeventhOnVisit;
        }

        protected IStorm<TFirst> First { get; }
        protected IStorm<TSecond> Second { get; }
        protected IStorm<TThird> Third { get; }
        protected IStorm<TFourth> Fourth { get; }
        protected IStorm<TFifth> Fifth { get; }
        protected IStorm<TSixth> Sixth { get; }
        protected IStorm<TSeventh> Seventh { get; }

        protected IEnumerable<Exception> GetErrors()
        {
            StormError error;
            if (First.TryGetError(out error)) yield return error;
            if (Second.TryGetError(out error)) yield return error;
            if (Third.TryGetError(out error)) yield return error;
            if (Fourth.TryGetError(out error)) yield return error;
            if (Fifth.TryGetError(out error)) yield return error;
            if (Sixth.TryGetError(out error)) yield return error;
            if (Seventh.TryGetError(out error)) yield return error;
        }

        private void FirstOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(0, token, visitType);
        private void SecondOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(1, token, visitType);
        private void ThirdOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(2, token, visitType);
        private void FourthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(3, token, visitType);
        private void FifthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(4, token, visitType);
        private void SixthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(5, token, visitType);
        private void SeventhOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(6, token, visitType);
    }

    internal abstract class StormFuncBase<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult> : StormFuncBase<TResult>
    {
        protected StormFuncBase(
            IStorm<TFirst> first,
            IStorm<TSecond> second,
            IStorm<TThird> third,
            IStorm<TFourth> fourth,
            IStorm<TFifth> fifth,
            IStorm<TSixth> sixth,
            IStorm<TSeventh> seventh,
            IStorm<TEighth> eighth,
            IEqualityComparer<TResult> comparer) : base(8, comparer)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
            Fifth = fifth;
            Sixth = sixth;
            Seventh = seventh;
            Eighth = eighth;

            First.OnVisit += FirstOnVisit;
            Second.OnVisit += SecondOnVisit;
            Third.OnVisit += ThirdOnVisit;
            Fourth.OnVisit += FourthOnVisit;
            Fifth.OnVisit += FifthOnVisit;
            Sixth.OnVisit += SixthOnVisit;
            Seventh.OnVisit += SeventhOnVisit;
            Eighth.OnVisit += EighthOnVisit;
        }

        protected IStorm<TFirst> First { get; }
        protected IStorm<TSecond> Second { get; }
        protected IStorm<TThird> Third { get; }
        protected IStorm<TFourth> Fourth { get; }
        protected IStorm<TFifth> Fifth { get; }
        protected IStorm<TSixth> Sixth { get; }
        protected IStorm<TSeventh> Seventh { get; }
        protected IStorm<TEighth> Eighth { get; }

        protected IEnumerable<Exception> GetErrors()
        {
            StormError error;
            if (First.TryGetError(out error)) yield return error;
            if (Second.TryGetError(out error)) yield return error;
            if (Third.TryGetError(out error)) yield return error;
            if (Fourth.TryGetError(out error)) yield return error;
            if (Fifth.TryGetError(out error)) yield return error;
            if (Sixth.TryGetError(out error)) yield return error;
            if (Seventh.TryGetError(out error)) yield return error;
            if (Eighth.TryGetError(out error)) yield return error;
        }

        private void FirstOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(0, token, visitType);
        private void SecondOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(1, token, visitType);
        private void ThirdOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(2, token, visitType);
        private void FourthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(3, token, visitType);
        private void FifthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(4, token, visitType);
        private void SixthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(5, token, visitType);
        private void SeventhOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(6, token, visitType);
        private void EighthOnVisit(IStormToken token, EStormVisitType visitType) => SourceOnVisit(7, token, visitType);
    }

}