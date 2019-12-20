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
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class StormFuncTests
    {
        [Test]
        public void StormFuncCaptureException()
        {
            var s = Storm.Input.Create<int>();
            var f = Storm.Func.Create(s, i => i % 2 == 0 ? i : throw new Exception("odd value"));

            Assert.DoesNotThrow(() => s.SetEmpty());
            Assert.DoesNotThrow(() => f.Match(error => { },
                                              value => throw new Exception()));

            Assert.DoesNotThrow(() => s.SetValue(2));
            Assert.DoesNotThrow(() => f.Match(error => throw new Exception(),
                                              value => Assert.That(value, Is.EqualTo(2))));

            Assert.DoesNotThrow(() => s.SetValue(3));
            Assert.DoesNotThrow(() => f.Match(error => { },
                                              value => throw new Exception()));
        }

        [Test]
        public void StormFuncInitialValue()
        {
            var s = Storm.Input.Create<int>();
            s.SetValue(0);
            var f = Storm.Func.Create(s, v => v);
            Assert.That(f.GetValueOrThrow(), Is.EqualTo(0));
        }

        [Test]
        public void FuncDotNotEvaluateOnSourceUnchanged()
        {
            var s = Storm.Input.Create<int>();
            var t = Storm.Func.Create(s, v => v / 2);

            var callCount = 0;
            int Func(int v)
            {
                callCount++;
                return v;
            }

            Storm.Func.Create(t, Func);

            s.SetValue(0);
            Assert.That(callCount, Is.EqualTo(1));
            s.SetValue(1);
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void VisitEnterTwiceThrow()
        {
            var token = Mock.Of<IStormToken>();
            var s = new Mock<IStorm<int>>();
            Storm.Func.Create(s.Object, v => v);
            s.Raise(m => m.OnVisit += null, token, EStormVisitType.UpdateEnter);
            Assert.Throws<InvalidOperationException>(() => s.Raise(m => m.OnVisit += null, token, EStormVisitType.UpdateEnter));
        }

        [Test]
        public void VisitEnterWithDifferentTokenThrow()
        {
            var token1 = Mock.Of<IStormToken>();
            var token2 = Mock.Of<IStormToken>();
            var s1 = new Mock<IStorm<int>>();
            var s2 = new Mock<IStorm<int>>();
            Storm.Func.Create(s1.Object, s2.Object, (v1, v2) => 0);
            s1.Raise(m => m.OnVisit += null, token1, EStormVisitType.UpdateEnter);
            Assert.Throws<InvalidOperationException>(() => s2.Raise(m => m.OnVisit += null, token2, EStormVisitType.UpdateEnter));
        }

        [Test]
        public void VisitLeaveTwiceThrow()
        {
            var token = Mock.Of<IStormToken>();
            var s = new Mock<IStorm<int>>();
            Storm.Func.Create(s.Object, v => v);
            s.Raise(m => m.OnVisit += null, token, EStormVisitType.UpdateEnter);
            s.Raise(m => m.OnVisit += null, token, EStormVisitType.UpdateLeaveChanged);
            Assert.Throws<InvalidOperationException>(() => s.Raise(m => m.OnVisit += null, token, EStormVisitType.UpdateLeaveChanged));
        }

        [Test]
        public void VisitLeaveWithoutEnterThrow()
        {
            var token = Mock.Of<IStormToken>();
            var s = new Mock<IStorm<int>>();
            Storm.Func.Create(s.Object, v => v);
            Assert.Throws<InvalidOperationException>(() => s.Raise(m => m.OnVisit += null, token, EStormVisitType.UpdateLeaveChanged));
        }

        [Test]
        public void VisitUnknownVisitTypeThrow()
        {
            var s = new Mock<IStorm<int>>();
            Storm.Func.Create(s.Object, v => v);
            Assert.Throws<ArgumentOutOfRangeException>(() => s.Raise(m => m.OnVisit += null, null, (EStormVisitType)(-1)));
        }
    }
}