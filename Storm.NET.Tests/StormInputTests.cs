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

    public abstract class StormInputTests : StormTests
    {
        protected IStormInput<object> Sut { get; set; }
        protected override IStorm<object> SutStorm => Sut;

        [Test]
        public void ValueMatch()
        {
            static object OnError(StormError obj) => obj;
            static object OnValue(object obj) => obj;

            var actual = Sut.Match(OnValue, OnError);
            Assert.That(actual, Is.InstanceOf<StormError>());
        }

        [Test]
        public void SetErrorWithDefaultTokenThrow()
        {
            var error = Storm.Error.Create("error");
            Assert.Throws<ArgumentException>(() => Sut.SetError(default, error));
        }

        [Test]
        public void SetErrorWithNullErrorThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Assert.Throws<ArgumentNullException>(() => Sut.SetError(token, null));
        }

        [Test]
        public void SetValueWithDefaultTokenThrow()
        {
            Assert.Throws<ArgumentException>(() => Sut.SetValue(default, new object()));
        }

        [Test]
        public void SetValueWithNullValue()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SetValue(token, null);
        }

        [Test]
        public void TryGetEnteredTokenIdle()
        {
            var result = Sut.TryGetEnteredToken(out var token);
            Assert.That(result, Is.False);
            Assert.That(token, Is.EqualTo(new StormToken()));
        }

        [Test]
        public void TryGetEnteredTokenEntered()
        {
            var visitCount = 0;
            Sut.OnVisit += (token, type) =>
            {
                if (type == EStormVisitType.EnterUpdate)
                {
                    var result = Sut.TryGetEnteredToken(out var enteredToken);
                    Assert.That(result, Is.True);
                    Assert.That(token, Is.EqualTo(enteredToken));
                }
                else if (type == EStormVisitType.LeaveUpdateUnchanged)
                {
                    var result = Sut.TryGetEnteredToken(out _);
                    Assert.That(result, Is.False);
                }

                visitCount++;
            };

            Sut.SetValue(0);

            Assert.That(visitCount, Is.EqualTo(2));
        }

        [Test]
        public void TryGetError()
        {
            var result = Sut.TryGetError(out var error);
            Assert.That(result, Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
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
            Assert.That(Sut.ToString(), Is.EqualTo("err: 'Empty content.'"));
        }

        [Test]
        public void SetEmptyWhenEmptyDoNotRaiseOnVisit()
        {
            Sut.OnVisit += (token, type) => throw new Exception();
            Sut.SetEmpty();
        }
    }

    [TestFixture]
    public class StormInputWithDefaultComparer : StormInputTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = Storm.Input.Create<object>();
        }

        [Test]
        public void SetValueTwice()
        {
            var value = new object();
            Sut.SetValue(value);
            Sut.OnVisit += (token, visitType) => throw new Exception();
            Sut.SetValue(value);
        }
    }

    [TestFixture]
    public class StormInputWithComparer : StormInputTests
    {
        [SetUp]
        public void SetUp()
        {
            var mockComparer = new Mock<IEqualityComparer<object>>();
            Sut = Storm.Input.Create(mockComparer.Object);
        }
    }

    [TestFixture]
    public class StormInputWithoutComparer : StormInputTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = Storm.Input.WithoutCompare.Create<object>();
        }
    }
}