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
        public void UnknownVisitTypeThrow()
        {
            var s = new Mock<IStorm<int>>();
            Storm.Func.Create(s.Object, v => v);
            Assert.Throws<ArgumentOutOfRangeException>(() => s.Raise(m => m.OnVisit += null, null, (EStormVisitType)(-1)));
        }

    }
}