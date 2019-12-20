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

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
namespace StormDotNet.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class StormTokenTests
    {
        [SetUp]
        public void SetUp()
        {
            Source = Storm.TokenSource.CreateSource();
            Token = Source.Token;
        }

        private IStormTokenSource Source { get; set; }
        private StormToken Token { get; set; }

        [Test]
        public void DefaultEqualsSelf()
        {
            Token = default;
            Assert.That(Token.Equals(Token), Is.True);
        }

        [Test]
        public void DefaultEqualsSelfAsObject()
        {
            Token = default;
            Assert.That(Token.Equals((object) Token), Is.True);
        }

        [Test]
        public void DefaultGetHashCodeReturns()
        {
            Token = default;
            Token.GetHashCode();
        }

        [Test]
        public void DefaultRegisterOnLeaveThrow()
        {
            Token = default;
            Assert.Throws<ObjectDisposedException>(() => Token.Leave += null);
        }

        [Test]
        public void DefaultUnRegisterOnLeaveThrow()
        {
            Token = default;
            Assert.Throws<ObjectDisposedException>(() => Token.Leave -= null);
        }

        [Test]
        public void EqualsSelf()
        {
            Assert.That(Token.Equals(Token), Is.True);
        }

        [Test]
        public void EqualsSelfAsObject()
        {
            Assert.That(Token.Equals((object) Token), Is.True);
        }

        [Test]
        public void NotEqualsOtherObject()
        {
            Assert.That(Token.Equals(new object()), Is.False);
        }

        [Test]
        public void NotEqualsDefault()
        {
            Assert.That(Token.Equals(new StormToken()), Is.False);
        }

        [Test]
        public void NotEqualsDefaultAsObject()
        {
            Assert.That(Token.Equals((object)new StormToken()), Is.False);
        }

        [Test]
        public void GetHashCodeReturns()
        {
            Token.GetHashCode();
        }

        [Test]
        public void RegisterLeaveWhenSourceDisposedThrow()
        {
            Source.Dispose();
            Assert.Throws<ObjectDisposedException>(() => Token.Leave += null);
        }

        [Test]
        public void UnRegisterLeaveWhenSourceDisposedThrow()
        {
            Source.Dispose();
            Assert.Throws<ObjectDisposedException>(() => Token.Leave -= null);
        }
    }
}