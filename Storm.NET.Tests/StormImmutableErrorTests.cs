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

// ReSharper disable ObjectCreationAsStatement
namespace StormDotNet.Tests
{
    using System;
    using Implementations;
    using NUnit.Framework;

    [TestFixture]
    public class StormImmutableErrorTests : StormTests
    {
        [SetUp]
        public void SetUp()
        {
            Error = Storm.Error.Create("test");
            Sut = Storm.Immutable.CreateError<object>(Error);
        }

        private IStorm<object> Sut { get; set; }
        protected override IStorm<object> SutStorm => Sut;
        private StormError Error { get; set; }

        [Test]
        public void ConstructorWithNullErrorThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new ImmutableError<int>(null));
        }

        [Test]
        public void ValueMatch()
        {
            static object OnError(StormError obj) => obj;
            static object OnValue(object obj) => obj;

            var actual = Sut.Match(OnValue, OnError);
            Assert.That(actual, Is.EqualTo(Error));
        }

        [Test]
        public new void ToString()
        {
            Assert.That(Sut.ToString(), Is.EqualTo("err: 'test'"));
        }
    }
}