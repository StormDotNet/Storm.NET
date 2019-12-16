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
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class StormInputTests
    {
    }

    [TestFixture]
    public class StormInputWithNullComparerAsStormTests : StormTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = Storm.Input.Create<object>();
        }

        protected override IStorm<object> Sut { get; set; }
    }

    [TestFixture]
    public class StormInputWithComparerAsStormTests : StormTests
    {
        [SetUp]
        public void SetUp()
        {
            var mockComparer = new Mock<IEqualityComparer<object>>();
            Sut = Storm.Input.Create(mockComparer.Object);
        }

        protected override IStorm<object> Sut { get; set; }
    }

    [TestFixture]
    public class StormInputWithoutCompareAsStormTests : StormTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = Storm.Input.WithoutCompare.Create<object>();
        }

        protected override IStorm<object> Sut { get; set; }
    }
}