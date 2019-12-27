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

    [TestFixture]
    public class StormInputFactoriesTests
    {
        [Test]
        public void WithCompareFromWithCompareRoundTrip()
        {
            Assert.That(Storm.Input.WithCompare, Is.EqualTo(Storm.Input.WithCompare.WithCompare));
        }

        [Test]
        public void WithCompareFromWithoutCompareRoundTrip()
        {
            Assert.That(Storm.Input.WithCompare, Is.EqualTo(Storm.Input.WithoutCompare.WithCompare));
        }

        [Test]
        public void WithoutCompareFromWithCompareRoundTrip()
        {
            Assert.That(Storm.Input.WithoutCompare, Is.EqualTo(Storm.Input.WithCompare.WithoutCompare));
        }

        [Test]
        public void WithoutCompareFromWithoutCompareRoundTrip()
        {
            Assert.That(Storm.Input.WithoutCompare, Is.EqualTo(Storm.Input.WithoutCompare.WithoutCompare));
        }

        [Test]
        public void CreateStormInputWithCompareWithNullComparerThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Storm.Input.WithCompare.Create((IEqualityComparer<int>) null));
        }

        [Test]
        public void CreateStormInputWithCompareWithInitialValue()
        {
            var mock = new Mock<IEqualityComparer<int>>();
            mock.Setup(m => m.Equals(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            
            var sut = Storm.Input.WithCompare.Create(42, mock.Object);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));

            var count = 0;
            sut.OnVisit += (token, type) => count++;

            sut.SetValue(42);

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void CreateStormInputWithDefaultCompareWithInitialValue()
        {
            var sut = Storm.Input.WithCompare.Create(42);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));

            var count = 0;
            sut.OnVisit += (token, type) => count++;

            sut.SetValue(41);

            Assert.That(count, Is.EqualTo(2));
            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(41));
        }

        [Test]
        public void CreateStormInputWithoutCompareWithInitialValue()
        {
            var sut = Storm.Input.WithoutCompare.Create(42);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));

            var count = 0;
            sut.OnVisit += (token, type) => count++;

            sut.SetValue(42);

            Assert.That(count, Is.EqualTo(2));
        }
    }
}