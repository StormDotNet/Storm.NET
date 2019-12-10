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
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class StormFuncTests
    {
        [Test]
        public void StormFuncCaptureException()
        {
            var s = Storm.Input.WithCompare<int>();
            var f = Storm.Func.WithCompare(s, i => i % 2 == 0 ? i : throw new Exception("odd value"));
            
            Assert.DoesNotThrow(() => s.SetError(new Exception("original error")));
            Assert.DoesNotThrow(() => f.Match(() => throw new Exception(),
                                              error => { },
                                              value => throw new Exception()));

            var v = 2;
            Assert.DoesNotThrow(() => s.SetValue(v));
            Assert.DoesNotThrow(() => f.Match(() => throw new Exception(),
                                              error => throw new Exception(),
                                              value => Assert.That(value, Is.EqualTo(v))));

            v = 3;
            Assert.DoesNotThrow(() => s.SetValue(v));
            Assert.DoesNotThrow(() => f.Match(() => throw new Exception(),
                                              error => { },
                                              value => throw new Exception()));
        }
    }
}