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
    public class StormFuncBaseTests : StormTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new TestableStormFuncBase(2, null);
        }

        private TestableStormFuncBase Sut { get; set; }

        protected override IStorm<object> SutStorm => Sut;


        [Test]
        public void SourceOnVisitWithDefaultTokenThrow()
        {
            Assert.Throws<ArgumentException>(() => Sut.SourceOnVisit(0, new StormToken(), EStormVisitType.EnterLoopSearch));
        }

        [Test]
        public void GetStateOnNotVisitedReturnsNotVisited()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);
            Assert.That(Sut.GetState(1), Is.EqualTo(EStormFuncInputState.NotVisited));
        }

        [Test]
        public void GetStateOnEnteredThrows()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);
            Sut.SourceOnVisit(1, token, EStormVisitType.EnterUpdate);
            Assert.Throws<InvalidOperationException>(() => Sut.GetState(1));
        }

        [Test]
        public void GetStateOnLeaveChangedReturnsVisitedWithChange()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);
            Sut.SourceOnVisit(1, token, EStormVisitType.EnterUpdate);
            Sut.SourceOnVisit(1, token, EStormVisitType.LeaveUpdateChanged);
            Assert.That(Sut.GetState(1), Is.EqualTo(EStormFuncInputState.VisitedWithChange));
        }

        [Test]
        public void GetStateOnLeaveUnchangedReturnsVisitedWithoutChange()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);
            Sut.SourceOnVisit(1, token, EStormVisitType.EnterUpdate);
            Sut.SourceOnVisit(1, token, EStormVisitType.LeaveUpdateUnchanged);
            Assert.That(Sut.GetState(1), Is.EqualTo(EStormFuncInputState.VisitedWithoutChange));
        }

        [Test]
        public void UpdateAndLoopSearchVisit()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterUpdate));
                        break;
                    case 1:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterLoopSearch));
                        break;
                    case 2:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.LeaveLoopSearch));
                        break;
                    case 3:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.LeaveUpdateUnchanged));
                        break;
                }

                visitCount++;
            };

            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);
            Assert.That(visitCount, Is.EqualTo(1));
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(2));
            Sut.SourceOnVisit(0, token, EStormVisitType.LeaveLoopSearch);
            Assert.That(visitCount, Is.EqualTo(3));
            Sut.SourceOnVisit(0, token, EStormVisitType.LeaveUpdateUnchanged);
            Assert.That(visitCount, Is.EqualTo(4));
        }

        [Test]
        public void UpdateAndMultipleLoopSearchVisit()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterUpdate));
                        break;
                    case 1:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterLoopSearch));
                        break;
                    case 2:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.LeaveLoopSearch));
                        break;
                    case 3:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.LeaveUpdateUnchanged));
                        break;
                }

                visitCount++;
            };

            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);
            Assert.That(visitCount, Is.EqualTo(1));
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(2));
            Sut.SourceOnVisit(1, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(2));
            Sut.SourceOnVisit(0, token, EStormVisitType.LeaveLoopSearch);
            Assert.That(visitCount, Is.EqualTo(2));
            Sut.SourceOnVisit(1, token, EStormVisitType.LeaveLoopSearch);
            Assert.That(visitCount, Is.EqualTo(3));
            Sut.SourceOnVisit(0, token, EStormVisitType.LeaveUpdateUnchanged);
            Assert.That(visitCount, Is.EqualTo(4));
        }

        [Test]
        public void SourceOnEnterUpdateAfterSourceOnEnterLoopSearchThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate));
        }

        [Test]
        public void SourceOnEnterUpdateAfterSourceOnEnterLoopSearchOnDifferentSourceThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(1, token, EStormVisitType.EnterUpdate));
        }

        [Test]
        public void SourceOnEnterLoopSearch()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterLoopSearch));
                        break;
                }

                visitCount++;
            };

            Assert.That(visitCount, Is.EqualTo(0));
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(1));
        }

        [Test]
        public void SourceOnEnterLoopSearchWithKnownToken()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterUpdate));
                        break;
                    case 1:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterLoopSearch));
                        break;
                }

                visitCount++;
            };

            Assert.That(visitCount, Is.EqualTo(0));
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);
            Assert.That(visitCount, Is.EqualTo(1));
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(2));
        }

        [Test]
        public void SourceOnEnterLoopSearchWithUnknownTokenThrow()
        {
            var token1 = Storm.TokenSource.CreateSource().Token;
            var token2 = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token1));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterUpdate));
                        break;
                }

                visitCount++;
            };

            Assert.That(visitCount, Is.EqualTo(0));
            Sut.SourceOnVisit(0, token1, EStormVisitType.EnterUpdate);
            Assert.That(visitCount, Is.EqualTo(1));
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token2, EStormVisitType.EnterLoopSearch));
            Assert.That(visitCount, Is.EqualTo(1));
        }

        [Test]
        public void SourceOnEnterLoopSearchTwiceWithDifferentSource()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterLoopSearch));
                        break;
                }

                visitCount++;
            };

            Assert.That(visitCount, Is.EqualTo(0));
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(1));
            Sut.SourceOnVisit(1, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(1));
        }

        [Test]
        public void SourceOnEnterLoopSearchTwiceWithSameSourceThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterLoopSearch));
                        break;
                }

                visitCount++;
            };

            Assert.That(visitCount, Is.EqualTo(0));
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(1));
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch));
            Assert.That(visitCount, Is.EqualTo(1));
        }

        [Test]
        public void SourceOnLeaveLoopSearchThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) => { visitCount++; };

            Assert.That(visitCount, Is.EqualTo(0));
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token, EStormVisitType.LeaveLoopSearch));
            Assert.That(visitCount, Is.EqualTo(0));
        }

        [Test]
        public void SourceOnLeaveLoopSearchWithUnknownTokenThrow()
        {
            var token1 = Storm.TokenSource.CreateSource().Token;
            var token2 = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token1));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterLoopSearch));
                        break;
                }

                visitCount++;
            };

            Assert.That(visitCount, Is.EqualTo(0));
            Sut.SourceOnVisit(0, token1, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(1));
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token2, EStormVisitType.LeaveLoopSearch));
            Assert.That(visitCount, Is.EqualTo(1));
        }

        [Test]
        public void SourceOnLeaveLoopSearchWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterUpdate));
                        break;
                }

                visitCount++;
            };

            Assert.That(visitCount, Is.EqualTo(0));
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);
            Assert.That(visitCount, Is.EqualTo(1));
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token, EStormVisitType.LeaveLoopSearch));
            Assert.That(visitCount, Is.EqualTo(1));
        }

        [Test]
        public void SourceOnLeaveLoopSearchAfterEnter()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterLoopSearch));
                        break;
                    case 1:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.LeaveLoopSearch));
                        break;
                }

                visitCount++;
            };

            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(1));
            Sut.SourceOnVisit(0, token, EStormVisitType.LeaveLoopSearch);
            Assert.That(visitCount, Is.EqualTo(2));
        }

        [Test]
        public void SourceOnLeaveUpdateChangedWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token, EStormVisitType.LeaveUpdateChanged));
        }

        [Test]
        public void SourceOnLeaveUpdateUnchangedWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token, EStormVisitType.LeaveUpdateUnchanged));
        }

        [Test]
        public void SourceOnLeaveUpdateChangedAfterEnter()
        {
            var updateCount = 0;
            Sut.UpdateDelegate = () =>
            {
                updateCount++;
                return true;
            };

            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);

            Assert.That(updateCount, Is.Zero);
            Sut.SourceOnVisit(0, token, EStormVisitType.LeaveUpdateChanged);
            Assert.That(updateCount, Is.EqualTo(1));
        }

        [Test]
        public void SourceOnLeaveUpdateUnchangedAfterEnter()
        {
            var updateCount = 0;
            Sut.UpdateDelegate = () =>
            {
                updateCount++;
                return true;
            };

            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterUpdate);
            Assert.That(updateCount, Is.Zero);
            Sut.SourceOnVisit(0, token, EStormVisitType.LeaveUpdateUnchanged);
            Assert.That(updateCount, Is.Zero);
        }

        [Test]
        public void SourceOnLeaveUpdateWithUnknownTokenThrow()
        {
            var token1 = Storm.TokenSource.CreateSource().Token;
            var token2 = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token1));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterUpdate));
                        break;
                }

                visitCount++;
            };

            Assert.That(visitCount, Is.EqualTo(0));
            Sut.SourceOnVisit(0, token1, EStormVisitType.EnterUpdate);
            Assert.That(visitCount, Is.EqualTo(1));
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token2, EStormVisitType.LeaveUpdateChanged));
            Assert.That(visitCount, Is.EqualTo(1));
        }

        [Test]
        public void SourceOnLeaveUpdateWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.EnterLoopSearch));
                        break;
                }

                visitCount++;
            };

            Assert.That(visitCount, Is.EqualTo(0));
            Sut.SourceOnVisit(0, token, EStormVisitType.EnterLoopSearch);
            Assert.That(visitCount, Is.EqualTo(1));
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token, EStormVisitType.LeaveUpdateChanged));
            Assert.That(visitCount, Is.EqualTo(1));
        }

        private class TestableStormFuncBase : StormFuncBase<object>
        {
            public TestableStormFuncBase(int length, IEqualityComparer<object> comparer) : base(length, comparer)
            {
            }

            protected override bool Update() => UpdateDelegate?.Invoke() ?? false;

            public Func<bool> UpdateDelegate { get; set; }

            public new EStormFuncInputState GetState(int index) => base.GetState(index);

            public new void SourceOnVisit(int index, StormToken token, EStormVisitType visitType) => base.SourceOnVisit(index, token, visitType);
        }
    }
}