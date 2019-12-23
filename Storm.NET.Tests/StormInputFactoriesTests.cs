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
    using NUnit.Framework;

    [TestFixture]
    public class StormInputFactoriesTests
    {
        [Test]
        public void WithCompareFromWithCompareRoundTrip()
        {
            Assert.That(Storm.Input.WithCompare, Is.EqualTo(Storm.Input.WithCompare.WithCompare));
        }

        [Test]
        public void WithCompareFromWithoutCompareRoundTrip()
        {
            Assert.That(Storm.Input.WithCompare, Is.EqualTo(Storm.Input.WithoutCompare.WithCompare));
        }

        [Test]
        public void WithoutCompareFromWithCompareRoundTrip()
        {
            Assert.That(Storm.Input.WithoutCompare, Is.EqualTo(Storm.Input.WithCompare.WithoutCompare));
        }

        [Test]
        public void WithoutCompareFromWithoutCompareRoundTrip()
        {
            Assert.That(Storm.Input.WithoutCompare, Is.EqualTo(Storm.Input.WithoutCompare.WithoutCompare));
        }

        [Test]
        public void CreateStormInputWithCompareWithNullComparerThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Storm.Input.WithCompare.Create((IEqualityComparer<int>) null));
        }
    }
}