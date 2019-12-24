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
        public void UpdateEnterLeave()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterUpdate(token);
            Sut.LeaveUpdate(token, true);
        }

        [Test]
        public void UpdateEnterLeaveWithLoopSearch()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterUpdate(token);
            Sut.EnterLoopSearch(token);
            Sut.LeaveLoopSearch(token);
            Sut.LeaveUpdate(token, true);
        }

        [Test]
        public void LoopSearchEnterLeave()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterLoopSearch(token);
            Sut.LeaveLoopSearch(token);
        }

        [Test]
        public void RaiseLoopSearchEnterTwiceThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterLoopSearch(token);
            Assert.Throws<InvalidOperationException>(() => Sut.EnterLoopSearch(token));
        }

        [Test]
        public void RaiseLoopSearchEnterAfterUpdateEnterWithDifferentTokensThrow()
        {
            var token1 = Storm.TokenSource.CreateDisposedSource().Token;
            var token2 = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterUpdate(token1);
            Assert.Throws<InvalidOperationException>(() => Sut.EnterLoopSearch(token2));
        }

        [Test]
        public void RaiseLoopSearchLeaveWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.LeaveLoopSearch(token));
        }

        [Test]
        public void RaiseLoopSearchLeaveAfterUpdateEnterWithDifferentTokensThrow()
        {
            var token1 = Storm.TokenSource.CreateDisposedSource().Token;
            var token2 = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterUpdate(token1);
            Sut.EnterLoopSearch(token1);
            Assert.Throws<InvalidOperationException>(() => Sut.LeaveLoopSearch(token2));
        }

        [Test]
        public void RaiseUpdateEnterTwiceThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterUpdate(token);
            Assert.Throws<InvalidOperationException>(() => Sut.EnterUpdate(token));
        }

        [Test]
        public void RaiseUpdateEnterAfterEnterLoopSearchThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterLoopSearch(token);
            Assert.Throws<InvalidOperationException>(() => Sut.EnterUpdate(token));
        }

        [Test]
        public void RaiseUpdateLeaveWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.LeaveUpdate(token, true));
        }

        [Test]
        public void RaiseUpdateLeaveWithDifferentTokenThrow()
        {
            var token1 = Storm.TokenSource.CreateDisposedSource().Token;
            var token2 = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterUpdate(token1);
            Assert.Throws<InvalidOperationException>(() => Sut.LeaveUpdate(token2, true));
        }

        [Test]
        public void RaiseUpdateLeaveWhileInLoopSearchThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterUpdate(token);
            Sut.EnterLoopSearch(token);
            Assert.Throws<InvalidOperationException>(() => Sut.LeaveUpdate(token, true));
        }

        [Test]
        public void RaiseUpdateEnterWhileRaiseUpdateEnterThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;

            Sut.OnVisit += (visitToken, visitType) => Sut.EnterUpdate(token);

            Assert.Throws<InvalidOperationException>(() => Sut.EnterUpdate(token));
        }

        [Test]
        public void RaiseUpdateLeaveWhileRaiseUpdateEnterThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;

            Sut.OnVisit += (visitToken, visitType) => Sut.LeaveUpdate(token, true);

            Assert.Throws<InvalidOperationException>(() => Sut.EnterUpdate(token));
        }

        [Test]
        public void RaiseUpdateEnterWhileRaiseUpdateLeaveThrow()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                if (visitType != EStormVisitType.EnterUpdate)
                    Sut.LeaveUpdate(token, true);
            };
            
            Sut.EnterUpdate(token);
            Assert.Throws<InvalidOperationException>(() => Sut.LeaveUpdate(token, true));
        }

        [Test]
        public void RegisterWhileEnteredRaise()
        {
            var token = Storm.TokenSource.CreateDisposedSource().Token;
            Sut.EnterUpdate(token);

            Sut.OnVisit += null;
            Sut.OnVisit += (enteredToken, visitType) =>
            {
                Assert.That(enteredToken, Is.EqualTo(token));
                Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterUpdate));
            };
        }

        [Test]
        public void TryGetUpdateTokenWhenIdle()
        {
            var state = Sut.GetVisitState(out var enteredToken);
            Assert.That(state.IsInUpdate, Is.False);
            Assert.That(state.IsInLoopSearch, Is.False);
            Assert.That(enteredToken, Is.EqualTo(default(StormToken)));
        }

        [Test]
        public void TryGetUpdateTokenAfterUpdateEnter()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.EnterUpdate(token);

            var state = Sut.GetVisitState(out var enteredToken);
            Assert.That(state.IsInUpdate, Is.True);
            Assert.That(state.IsInLoopSearch, Is.False);
            Assert.That(enteredToken, Is.EqualTo(token));
        }

        [Test]
        public void TryGetUpdateTokenAfterUpdateEnterAndLeave()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.EnterUpdate(token);
            Sut.LeaveUpdate(token, true);

            var state = Sut.GetVisitState(out var enteredToken);
            Assert.That(state.IsInUpdate, Is.False);
            Assert.That(state.IsInLoopSearch, Is.False);
            Assert.That(enteredToken, Is.EqualTo(default(StormToken)));
        }

        [Test]
        public void TryGetUpdateTokenAfterLoopSearchEnter()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.EnterLoopSearch(token);

            var state = Sut.GetVisitState(out var enteredToken);
            Assert.That(state.IsInUpdate, Is.False);
            Assert.That(state.IsInLoopSearch, Is.True);
            Assert.That(enteredToken, Is.EqualTo(token));
        }

        [Test]
        public void TryGetUpdateTokenAfterUpdateEnterAndLoopSearchEnter()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.EnterUpdate(token);
            Sut.EnterLoopSearch(token);

            var state = Sut.GetVisitState(out var enteredToken);
            Assert.That(state.IsInUpdate, Is.True);
            Assert.That(state.IsInLoopSearch, Is.True);
            Assert.That(enteredToken, Is.EqualTo(token));
        }

        private class TestableStormBase : StormBase<int>
        {
            public TestableStormBase(IEqualityComparer<int> comparer) : base(comparer)
            {
            }

            public new void EnterLoopSearch(StormToken token) => base.EnterLoopSearch(token);
            public new void LeaveLoopSearch(StormToken token) => base.LeaveLoopSearch(token);
            public new void EnterUpdate(StormToken token) => base.EnterUpdate(token);
            public new void LeaveUpdate(StormToken token, bool hasChanged) => base.LeaveUpdate(token, hasChanged);
            public new StormVisitState GetVisitState(out StormToken token) => base.GetVisitState(out token);
        }
    }
}