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

        protected override IStorm<object> Sut { get; set; }
        private StormError Error { get; set; }

        [Test]
        public void ContentType()
        {
            Assert.That(Sut.ContentType, Is.EqualTo(EStormContentType.Error));
        }

        [Test]
        public void GetValueOr()
        {
            var fallBack = new object();
            Assert.That(Sut.GetValueOr(fallBack), Is.EqualTo(fallBack));
        }

        [Test]
        public void GetValueOrThrow()
        {
            Assert.Throws<StormError>(() => Sut.GetValueOrThrow());
        }

        [Test]
        public void VoidMatch()
        {
            void OnError(StormError obj) => Assert.That(obj, Is.EqualTo(Error));
            static void OnValue(object obj) => throw new Exception();

            Sut.Match(OnError, OnValue);
        }

        [Test]
        public void ValueMatch()
        {
            static object OnError(StormError obj) => obj;
            static object OnValue(object obj) => obj;

            var actual = Sut.Match(OnError, OnValue);
            Assert.That(actual, Is.EqualTo(Error));
        }

        [Test]
        public void TryGetError()
        {
            var result = Sut.TryGetError(out var error);
            Assert.That(result, Is.True);
            Assert.That(error, Is.EqualTo(Error));
        }

        [Test]
        public void TryGetValue()
        {
            var result = Sut.TryGetValue(out var value);
            Assert.That(result, Is.False);
            Assert.That(value, Is.Null);
        }

        [Test]
        public new void ToString()
        {
            Assert.That(Sut.ToString(), Is.EqualTo("err: 'test'"));
        }
    }
}