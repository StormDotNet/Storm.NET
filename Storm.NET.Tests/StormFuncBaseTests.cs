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
        public void GetStateOnNotVisitedReturnsNotVisited()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateEnter);
            Assert.That(Sut.GetState(1), Is.EqualTo(EStormFuncInputState.NotVisited));
        }

        [Test]
        public void GetStateOnEnteredThrows()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateEnter);
            Sut.SourceOnVisit(1, token, EStormVisitType.UpdateEnter);
            Assert.Throws<InvalidOperationException>(() => Sut.GetState(1));
        }

        [Test]
        public void GetStateOnLeaveChangedReturnsVisitedWithChange()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateEnter);
            Sut.SourceOnVisit(1, token, EStormVisitType.UpdateEnter);
            Sut.SourceOnVisit(1, token, EStormVisitType.UpdateLeaveChanged);
            Assert.That(Sut.GetState(1), Is.EqualTo(EStormFuncInputState.VisitedWithChange));
        }

        [Test]
        public void GetStateOnLeaveUnchangedReturnsVisitedWithoutChange()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateEnter);
            Sut.SourceOnVisit(1, token, EStormVisitType.UpdateEnter);
            Sut.SourceOnVisit(1, token, EStormVisitType.UpdateLeaveUnchanged);
            Assert.That(Sut.GetState(1), Is.EqualTo(EStormFuncInputState.VisitedWithoutChange));
        }

        [Test]
        public void LoopSearchVisit()
        {
            var token = Storm.TokenSource.CreateSource().Token;

            var visitCount = 0;
            Sut.OnVisit += (visitToken, visitType) =>
            {
                switch (visitCount)
                {
                    case 0:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.LoopSearchEnter));
                        break;
                    case 1:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.LoopSearchLeave));
                        break;
                }

                visitCount++;
            };

            Sut.SourceOnVisit(0, token, EStormVisitType.LoopSearchEnter);
            Assert.That(visitCount, Is.EqualTo(1));
            Sut.SourceOnVisit(0, token, EStormVisitType.LoopSearchEnter);
            Assert.That(visitCount, Is.EqualTo(1));
            Sut.SourceOnVisit(0, token, EStormVisitType.LoopSearchLeave);
            Assert.That(visitCount, Is.EqualTo(1));
            Sut.SourceOnVisit(0, token, EStormVisitType.LoopSearchLeave);
            Assert.That(visitCount, Is.EqualTo(2));
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
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.UpdateEnter));
                        break;
                    case 1:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.LoopSearchEnter));
                        break;
                    case 2:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.LoopSearchLeave));
                        break;
                    case 3:
                        Assert.That(visitToken, Is.EqualTo(token));
                        Assert.That(visitType, Is.EqualTo(EStormVisitType.UpdateLeaveUnchanged));
                        break;
                }

                visitCount++;
            };

            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateEnter);
            Assert.That(visitCount, Is.EqualTo(1));
            Sut.SourceOnVisit(0, token, EStormVisitType.LoopSearchEnter);
            Assert.That(visitCount, Is.EqualTo(2));
            Sut.SourceOnVisit(0, token, EStormVisitType.LoopSearchEnter);
            Assert.That(visitCount, Is.EqualTo(2));
            Sut.SourceOnVisit(0, token, EStormVisitType.LoopSearchLeave);
            Assert.That(visitCount, Is.EqualTo(2));
            Sut.SourceOnVisit(0, token, EStormVisitType.LoopSearchLeave);
            Assert.That(visitCount, Is.EqualTo(3));
            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateLeaveUnchanged);
            Assert.That(visitCount, Is.EqualTo(4));
        }

        [Test]
        public void LoopSearchEnterWithUnknownTokenFromUpdateThrow()
        {
            var token1 = Storm.TokenSource.CreateSource().Token;
            var token2 = Storm.TokenSource.CreateSource().Token;

            Sut.SourceOnVisit(0, token1, EStormVisitType.UpdateEnter);
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token2, EStormVisitType.LoopSearchEnter));
        }

        [Test]
        public void LoopSearchEnterWithUnknownTokenFromLoopSearchThrow()
        {
            var token1 = Storm.TokenSource.CreateSource().Token;
            var token2 = Storm.TokenSource.CreateSource().Token;

            Sut.SourceOnVisit(0, token1, EStormVisitType.LoopSearchEnter);
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token2, EStormVisitType.LoopSearchEnter));
        }

        [Test]
        public void LoopSearchLeaveWithUnknownTokenThrow()
        {
            var token1 = Storm.TokenSource.CreateSource().Token;
            var token2 = Storm.TokenSource.CreateSource().Token;

            Sut.SourceOnVisit(0, token1, EStormVisitType.LoopSearchEnter);
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token2, EStormVisitType.LoopSearchLeave));
        }

        [Test]
        public void SourceUpdateLeaveChangedWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token, EStormVisitType.UpdateLeaveChanged));
        }

        [Test]
        public void SourceUpdateLeaveUnchangedWithoutEnterThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.SourceOnVisit(0, token, EStormVisitType.UpdateLeaveUnchanged));
        }

        [Test]
        public void UpdateWithChange()
        {
            var updateCount = 0;
            Sut.UpdateDelegate = () =>
            {
                updateCount++;
                return true;
            };

            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateEnter);

            Assert.That(updateCount, Is.Zero);
            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateLeaveChanged);
            Assert.That(updateCount, Is.EqualTo(1));
        }

        [Test]
        public void UpdateWithoutChange()
        {
            var updateCount = 0;
            Sut.UpdateDelegate = () =>
            {
                updateCount++;
                return true;
            };

            var token = Storm.TokenSource.CreateSource().Token;
            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateEnter);

            Assert.That(updateCount, Is.Zero);
            Sut.SourceOnVisit(0, token, EStormVisitType.UpdateLeaveUnchanged);
            Assert.That(updateCount, Is.Zero);
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