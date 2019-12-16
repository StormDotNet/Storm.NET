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
    public class StormImmutableValueTests : StormTests
    {
        [SetUp]
        public void SetUp()
        {
            Value = new object();
            Sut = Storm.Immutable.CreateValue(Value);
        }

        protected override IStorm<object> Sut { get; set; }
        private object Value { get; set; }

        [Test]
        public void ContentType()
        {
            Assert.That(Sut.ContentType, Is.EqualTo(EStormContentType.Value));
        }

        [Test]
        public void GetValueOr()
        {
            Assert.That(Sut.GetValueOr(new object()), Is.EqualTo(Value));
        }

        [Test]
        public void GetValueOrThrow()
        {
            Assert.That(Sut.GetValueOrThrow(), Is.EqualTo(Value));
        }

        [Test]
        public void VoidMatch()
        {
            static void OnError(StormError obj) => throw new Exception();
            void OnValue(object obj) => Assert.That(obj, Is.EqualTo(Value));

            Sut.Match(OnError, OnValue);
        }

        [Test]
        public void ValueMatch()
        {
            static object OnError(StormError obj) => obj;
            static object OnValue(object obj) => obj;

            var actual = Sut.Match(OnError, OnValue);
            Assert.That(actual, Is.EqualTo(Value));
        }

        [Test]
        public void TryGetError()
        {
            var result = Sut.TryGetError(out var error);
            Assert.That(result, Is.False);
            Assert.That(error, Is.Null);
        }

        [Test]
        public void TryGetValue()
        {
            var result = Sut.TryGetValue(out var value);
            Assert.That(result, Is.True);
            Assert.That(value, Is.EqualTo(Value));
        }

        [Test]
        public new void ToString()
        {
            Assert.That(Sut.ToString(), Is.EqualTo("val: 'System.Object'"));
        }
    }
}