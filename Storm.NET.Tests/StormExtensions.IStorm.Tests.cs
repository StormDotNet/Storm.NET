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

// ReSharper disable InvokeAsExtensionMethod
namespace StormDotNet.Tests
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class StormExtensionsIStormTests
    {
        [Test]
        public void SwitchWithCompare1WithNullSelectorThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.SwitchWithCompare<int>(null));
        }

        [Test]
        public void SwitchWithCompare1Returns()
        {
            Assert.That(StormExtensions.SwitchWithCompare(Mock.Of<IStorm<IStorm<int>>>()), Is.Not.Null);
        }

        [Test]
        public void SwitchWithCompare2WithNullSelectorThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.SwitchWithCompare(null, Mock.Of<IEqualityComparer<int>>()));
        }

        [Test]
        public void SwitchWithCompare2WithNullComparerThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.SwitchWithCompare(Mock.Of<IStorm<IStorm<int>>>(), null));
        }

        [Test]
        public void SwitchWithCompare2Returns()
        {
            Assert.That(StormExtensions.SwitchWithCompare(Mock.Of<IStorm<IStorm<int>>>(), Mock.Of<IEqualityComparer<int>>()), Is.Not.Null);
        }

        [Test]
        public void SwitchWithoutCompareWithNullSelectorThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.SwitchWithoutCompare<int>(null));
        }

        [Test]
        public void SwitchWithoutCompare2Returns()
        {
            Assert.That(StormExtensions.SwitchWithoutCompare(Mock.Of<IStorm<IStorm<int>>>()), Is.Not.Null);
        }
    }
}
