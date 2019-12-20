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
    using Implementations;
    using NUnit.Framework;

    [TestFixture]
    public class StormBaseTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new TestableStormBase(EqualityComparer<int>.Default);
        }

        private TestableStormBase Sut { get; set; }

        [Test]
        public void RaiseLoopSearchEnterTwiceThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.RaiseLoopSearchEnter(token);
            Assert.Throws<InvalidOperationException>(() => Sut.RaiseLoopSearchEnter(token));
        }

        [Test]
        public void RaiseLoopSearchEnterAfterUpdateEnterWithDifferentTokensThrow()
        {
            var token1 = Storm.TokenSource.CreateDisposedSource().Token;
            var token2 = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.RaiseUpdateEnter(token1);
            Assert.Throws<InvalidOperationException>(() => Sut.RaiseLoopSearchEnter(token2));
        }

        [Test]
        public void RaiseLoopSearchLeaveWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.RaiseLoopSearchLeave(token));
        }

        [Test]
        public void RaiseLoopSearchLeaveAfterUpdateEnterWithDifferentTokensThrow()
        {
            var token1 = Storm.TokenSource.CreateDisposedSource().Token;
            var token2 = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.RaiseUpdateEnter(token1);
            Sut.RaiseLoopSearchEnter(token1);
            Assert.Throws<InvalidOperationException>(() => Sut.RaiseLoopSearchLeave(token2));
        }

        [Test]
        public void RaiseUpdateEnterTwiceThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.RaiseUpdateEnter(token);
            Assert.Throws<InvalidOperationException>(() => Sut.RaiseUpdateEnter(token));
        }

        [Test]
        public void RaiseUpdateEnterAfterEnterLoopSearchThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.RaiseLoopSearchEnter(token);
            Assert.Throws<InvalidOperationException>(() => Sut.RaiseUpdateEnter(token));
        }

        [Test]
        public void RaiseUpdateLeaveWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.RaiseUpdateLeave(token, true));
        }

        [Test]
        public void RaiseUpdateLeaveWithDifferentTokenThrow()
        {
            var token1 = Storm.TokenSource.CreateDisposedSource().Token;
            var token2 = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.RaiseUpdateEnter(token1);
            Assert.Throws<InvalidOperationException>(() => Sut.RaiseUpdateLeave(token2, true));
        }

        [Test]
        public void RaiseUpdateLeaveWhileInLoopSearchThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.RaiseUpdateEnter(token);
            Sut.RaiseLoopSearchEnter(token);
            Assert.Throws<InvalidOperationException>(() => Sut.RaiseUpdateLeave(token, true));
        }

        [Test]
        public void RaiseUpdateEnterWhileRaiseUpdateThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;

            Sut.OnVisit += (visitToken, visitType) => Sut.RaiseUpdateEnter(token);

            Assert.Throws<InvalidOperationException>(() => Sut.RaiseUpdateEnter(token));
        }

        [Test]
        public void RaiseUpdateLeaveWhileRaiseUpdateThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;

            Sut.OnVisit += (visitToken, visitType) => Sut.RaiseUpdateLeave(token, true);

            Assert.Throws<InvalidOperationException>(() => Sut.RaiseUpdateEnter(token));
        }

        private class TestableStormBase : StormBase<int>
        {
            public TestableStormBase(IEqualityComparer<int> comparer) : base(comparer)
            {
            }

            public new void RaiseLoopSearchEnter(StormToken token) => base.RaiseLoopSearchEnter(token);
            public new void RaiseLoopSearchLeave(StormToken token) => base.RaiseLoopSearchLeave(token);
            public new void RaiseUpdateEnter(StormToken token) => base.RaiseUpdateEnter(token);
            public new void RaiseUpdateLeave(StormToken token, bool hasChanged) => base.RaiseUpdateLeave(token, hasChanged);
        }
    }
}