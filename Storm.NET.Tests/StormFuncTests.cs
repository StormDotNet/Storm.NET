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
    }
}