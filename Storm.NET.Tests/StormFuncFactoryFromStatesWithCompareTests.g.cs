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

namespace StormDotNet.Tests
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using StormDotNet.Factories.Func;

    [TestFixture]
    public class StormFuncFactoryFromStatesWithCompareTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new StormFuncFactoryFromStatesWithCompare();
        }

        private StormFuncFactoryFromStatesWithCompare Sut { get; set; }

        [Test]
        public void CreateWith1SourceThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object>(null, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object>(first, null));
        }

        [Test]
        public void CreateWith1SourceAndComparerThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, object>>();
            var comparer = Mock.Of<IEqualityComparer<object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object>(null, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object>(first, null, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object>(first, func, null));
        }

        [Test]
        public void CreateWith2SourceThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object>(null, second, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object>(first, null, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object>(first, second, null));
        }

        [Test]
        public void CreateWith2SourceAndComparerThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, object>>();
            var comparer = Mock.Of<IEqualityComparer<object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object>(null, second, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object>(first, null, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object>(first, second, null, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object>(first, second, func, null));
        }

        [Test]
        public void CreateWith3SourceThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object>(null, second, third, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object>(first, null, third, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object>(first, second, null, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object>(first, second, third, null));
        }

        [Test]
        public void CreateWith3SourceAndComparerThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();
            var comparer = Mock.Of<IEqualityComparer<object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object>(null, second, third, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object>(first, null, third, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object>(first, second, null, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object>(first, second, third, null, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object>(first, second, third, func, null));
        }

        [Test]
        public void CreateWith4SourceThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(null, second, third, fourth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(first, null, third, fourth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(first, second, null, fourth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(first, second, third, null, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(first, second, third, fourth, null));
        }

        [Test]
        public void CreateWith4SourceAndComparerThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();
            var comparer = Mock.Of<IEqualityComparer<object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(null, second, third, fourth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(first, null, third, fourth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(first, second, null, fourth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(first, second, third, null, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(first, second, third, fourth, null, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object>(first, second, third, fourth, func, null));
        }

        [Test]
        public void CreateWith5SourceThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var fifth = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(null, second, third, fourth, fifth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, null, third, fourth, fifth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, second, null, fourth, fifth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, second, third, null, fifth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, second, third, fourth, null, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, second, third, fourth, fifth, null));
        }

        [Test]
        public void CreateWith5SourceAndComparerThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var fifth = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();
            var comparer = Mock.Of<IEqualityComparer<object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(null, second, third, fourth, fifth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, null, third, fourth, fifth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, second, null, fourth, fifth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, second, third, null, fifth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, second, third, fourth, null, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, second, third, fourth, fifth, null, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object>(first, second, third, fourth, fifth, func, null));
        }

        [Test]
        public void CreateWith6SourceThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var fifth = Mock.Of<IStorm<object>>();
            var sixth = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(null, second, third, fourth, fifth, sixth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, null, third, fourth, fifth, sixth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, null, fourth, fifth, sixth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, third, null, fifth, sixth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, third, fourth, null, sixth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, third, fourth, fifth, null, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, null));
        }

        [Test]
        public void CreateWith6SourceAndComparerThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var fifth = Mock.Of<IStorm<object>>();
            var sixth = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();
            var comparer = Mock.Of<IEqualityComparer<object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(null, second, third, fourth, fifth, sixth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, null, third, fourth, fifth, sixth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, null, fourth, fifth, sixth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, third, null, fifth, sixth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, third, fourth, null, sixth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, third, fourth, fifth, null, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, null, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, func, null));
        }

        [Test]
        public void CreateWith7SourceThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var fifth = Mock.Of<IStorm<object>>();
            var sixth = Mock.Of<IStorm<object>>();
            var seventh = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(null, second, third, fourth, fifth, sixth, seventh, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, null, third, fourth, fifth, sixth, seventh, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, null, fourth, fifth, sixth, seventh, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, null, fifth, sixth, seventh, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, fourth, null, sixth, seventh, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, null, seventh, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, null, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, seventh, null));
        }

        [Test]
        public void CreateWith7SourceAndComparerThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var fifth = Mock.Of<IStorm<object>>();
            var sixth = Mock.Of<IStorm<object>>();
            var seventh = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();
            var comparer = Mock.Of<IEqualityComparer<object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(null, second, third, fourth, fifth, sixth, seventh, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, null, third, fourth, fifth, sixth, seventh, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, null, fourth, fifth, sixth, seventh, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, null, fifth, sixth, seventh, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, fourth, null, sixth, seventh, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, null, seventh, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, null, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, seventh, null, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, seventh, func, null));
        }

        [Test]
        public void CreateWith8SourceThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var fifth = Mock.Of<IStorm<object>>();
            var sixth = Mock.Of<IStorm<object>>();
            var seventh = Mock.Of<IStorm<object>>();
            var eighth = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(null, second, third, fourth, fifth, sixth, seventh, eighth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, null, third, fourth, fifth, sixth, seventh, eighth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, null, fourth, fifth, sixth, seventh, eighth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, null, fifth, sixth, seventh, eighth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, null, sixth, seventh, eighth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, null, seventh, eighth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, null, eighth, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, seventh, null, func));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, seventh, eighth, null));
        }

        [Test]
        public void CreateWith8SourceAndComparerThrowOnNullArgument()
        {
            var first = Mock.Of<IStorm<object>>();
            var second = Mock.Of<IStorm<object>>();
            var third = Mock.Of<IStorm<object>>();
            var fourth = Mock.Of<IStorm<object>>();
            var fifth = Mock.Of<IStorm<object>>();
            var sixth = Mock.Of<IStorm<object>>();
            var seventh = Mock.Of<IStorm<object>>();
            var eighth = Mock.Of<IStorm<object>>();
            var func = Mock.Of<Func<StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, StormFuncInput<object>, object>>();
            var comparer = Mock.Of<IEqualityComparer<object>>();

            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(null, second, third, fourth, fifth, sixth, seventh, eighth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, null, third, fourth, fifth, sixth, seventh, eighth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, null, fourth, fifth, sixth, seventh, eighth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, null, fifth, sixth, seventh, eighth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, null, sixth, seventh, eighth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, null, seventh, eighth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, null, eighth, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, seventh, null, func, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, seventh, eighth, null, comparer));
            Assert.Throws<ArgumentNullException>(() => Sut.Create<object, object, object, object, object, object, object, object, object>(first, second, third, fourth, fifth, sixth, seventh, eighth, func, null));
        }

    }
}