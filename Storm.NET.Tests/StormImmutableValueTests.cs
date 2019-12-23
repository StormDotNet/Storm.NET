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

        private IStorm<object> Sut { get; set; }
        protected override IStorm<object> SutStorm => Sut;
        private object Value { get; set; }

        [Test]
        public void ValueMatch()
        {
            static object OnError(StormError obj) => obj;
            static object OnValue(object obj) => obj;

            var actual = Sut.Match(OnValue, OnError);
            Assert.That(actual, Is.EqualTo(Value));
        }

        [Test]
        public new void ToString()
        {
            Assert.That(Sut.ToString(), Is.EqualTo("val: 'System.Object'"));
        }
    }
}