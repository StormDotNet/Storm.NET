﻿// Storm.NET - Simple Topologically Ordered Reactive Model
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
    public class StormTokenSourceTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = Storm.TokenSource.CreateSource();
        }

        private IStormTokenSource Sut { get; set; }

        [Test]
        public void DisposeAcceptMultipleCalls()
        {
            Sut.Dispose();
            Sut.Dispose();
        }

        [Test]
        public void TokenIsNotDefault()
        {
            Assert.That(Sut.Token, Is.Not.EqualTo(new StormToken()));
        }

        [Test]
        public void TokenIsUnique()
        {
            Assert.That(Sut.Token, Is.EqualTo(Sut.Token));
        }

        [Test]
        public void TokenAfterDisposeIsNotDefault()
        {
            Sut.Dispose();
            Assert.That(Sut.Token, Is.Not.EqualTo(new StormToken()));
        }

        [Test]
        public void TokenAfterDisposeIsTheSame()
        {
            var token = Sut.Token;
            Sut.Dispose();
            Assert.That(Sut.Token, Is.EqualTo(token));
        }

        [Test]
        public void DisposeRaiseLeave()
        {
            var count = 0;
            Sut.Token.Leave += () => count++;

            Assert.That(count, Is.EqualTo(0));
            Sut.Dispose();
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void UnRegisterLeaveWork()
        {
            var count = 0;
            void OnLeave() => count++;
            Sut.Token.Leave += OnLeave;
            Sut.Token.Leave -= OnLeave;
            Assert.That(count, Is.EqualTo(0));
            Sut.Dispose();
            Assert.That(count, Is.EqualTo(0));
        }
    }
}