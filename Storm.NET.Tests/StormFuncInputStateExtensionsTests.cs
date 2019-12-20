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
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class StormFuncInputStateExtensionsTests
    {
        [Test]
        public void GetDescriptionReturns([Values] EStormFuncInputState state)
        {
            Assert.That(string.IsNullOrWhiteSpace(state.GetDescription()), Is.False);
        }

        [Test]
        public void GetDescriptionWithInvalidStateThrow()
        {
            var state = (EStormFuncInputState)(Enum.GetValues(typeof(EStormFuncInputState))
                                                   .Cast<EStormFuncInputState>().Min(e => (int)e) - 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => state.GetDescription());
        }
    }
}