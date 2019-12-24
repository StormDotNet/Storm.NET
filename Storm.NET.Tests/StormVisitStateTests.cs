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

using NUnit.Framework;

namespace StormDotNet.Tests
{
    using System;

    [TestFixture]
    public class StormVisitStateTests
    {
        [Test]
        public void EnterLoopSearchReturns()
        {
            var sut = new StormVisitState();
            sut.EnterLoopSearch();
        }

        [Test]
        public void EnterLoopSearchAndEnterLoopSearchThrows()
        {
            var sut = new StormVisitState();
            sut = sut.EnterLoopSearch();

            Assert.Throws<InvalidOperationException>(() => sut.EnterLoopSearch());
        }

        [Test]
        public void EnterLoopSearchAndEnterUpdateThrows()
        {
            var sut = new StormVisitState();
            sut = sut.EnterLoopSearch();

            Assert.Throws<InvalidOperationException>(() => sut.EnterUpdate());
        }

        [Test]
        public void EnterLoopSearchAndLeaveLoopSearchReturns()
        {
            var sut = new StormVisitState();
            sut = sut.EnterLoopSearch();
            sut.LeaveLoopSearch();
        }

        [Test]
        public void EnterLoopSearchAndLeaveUpdateThrows()
        {
            var sut = new StormVisitState();
            sut = sut.EnterLoopSearch();

            Assert.Throws<InvalidOperationException>(() => sut.LeaveUpdate());
        }

        [Test]
        public void EnterUpdateReturns()
        {
            var sut = new StormVisitState();
            sut.EnterUpdate();
        }

        [Test]
        public void EnterUpdateAndEnterLoopSearchReturns()
        {
            var sut = new StormVisitState();
            sut = sut.EnterUpdate();
            sut.EnterLoopSearch();
        }

        [Test]
        public void EnterUpdateAndEnterUpdateThrows()
        {
            var sut = new StormVisitState();
            sut = sut.EnterUpdate();

            Assert.Throws<InvalidOperationException>(() => sut.EnterUpdate());
        }

        [Test]
        public void EnterUpdateAndLeaveLoopSearchThrows()
        {
            var sut = new StormVisitState();
            sut = sut.EnterUpdate();

            Assert.Throws<InvalidOperationException>(() => sut.LeaveLoopSearch());
        }

        [Test]
        public void EnterUpdateAndLeaveUpdateReturns()
        {
            var sut = new StormVisitState();
            sut = sut.EnterUpdate();
            sut.LeaveUpdate();
        }

        [Test]
        public void LeaveLoopSearchThrows()
        {
            var sut = new StormVisitState();

            Assert.Throws<InvalidOperationException>(() => sut.LeaveLoopSearch());
        }

        [Test]
        public void LeaveUpdateThrows()
        {
            var sut = new StormVisitState();

            Assert.Throws<InvalidOperationException>(() => sut.LeaveUpdate());
        }
    }
}