// Storm.NET - Simple Topologically Ordered Reactive Model
// Copyright � 2019 Storm.NET. All rights reserved.
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
    public class StormInputWithNullComparerAsStormTests : StormNodeTests
    {
        protected override IStormNode CreateSubject() => Storm.Input.Create<object>();
    }

    [TestFixture]
    public class StormInputWithComparerAsStormTests : StormNodeTests
    {
        protected override IStormNode CreateSubject()
        {
            var mockComparer = new Mock<IEqualityComparer<object>>();
            return Storm.Input.Create(mockComparer.Object);
        }
    }

    [TestFixture]
    public class StormInputWithoutCompareAsStormTests : StormNodeTests
    {
        protected override IStormNode CreateSubject() => Storm.Input.WithoutCompare.Create<object>();
    }
}