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
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class StormExtensionsIStormSocketTests
    {
        [Test]
        public void ConnectCallConnect()
        {
            var target = Mock.Of<IStorm<object>>();
            var input = new Mock<IStormSocket<object>>(MockBehavior.Strict);
            input.Setup(i => i.Connect(It.IsAny<StormToken>(), target));
            StormExtensions.Connect(input.Object, target);
        }

        [Test]
        public void ConnectThrownOnNullThisArgument()
        {
            var target = Mock.Of<IStorm<object>>();
            Assert.Throws<ArgumentNullException>(() => StormExtensions.Connect(null, target));
        }

        [Test]
        public void ConnectThrowOnNullTarget()
        {
            var socket = Mock.Of<IStormSocket<object>>();
            Assert.Throws<ArgumentNullException>(() => StormExtensions.Connect(socket, null));
        }
    }
}