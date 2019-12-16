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
    public class StormExtensionsIStormInputTests
    {
        [Test]
        public void SetEmptyCallSetError()
        {
            var input = new Mock<IStormInput<object>>(MockBehavior.Strict);
            input.Setup(i => i.SetError(It.IsAny<IStormToken>(), Storm.Error.EmptyContent));
            StormExtensions.SetEmpty(input.Object);
        }

        [Test]
        public void SetEmptyThrownOnNullThisArgument()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.SetEmpty<object>(null));
        }


        [Test]
        public void SetErrorCallSetError()
        {
            const string message = "test";
            var input = new Mock<IStormInput<object>>(MockBehavior.Strict);
            input.Setup(i => i.SetError(It.IsAny<IStormToken>(), It.Is<StormError>(e => e.Message == message)));
            StormExtensions.SetError(input.Object, message);
        }

        [Test]
        public void SetErrorThrownOnNullThisArgument()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.SetError<object>(null, "test"));
        }

        [Test]
        public void SetErrorThrownOnNullMessage()
        {
            var input = Mock.Of<IStormInput<object>>();
            Assert.Throws<ArgumentNullException>(() => StormExtensions.SetError<object>(input, null));
        }


        [Test]
        public void SetErrorThrownOnEmptyMessage()
        {
            var input = Mock.Of<IStormInput<object>>();
            var message = string.Empty;
            Assert.Throws<ArgumentException>(() => StormExtensions.SetError<object>(input, message));
        }

        [Test]
        public void SetValueCallSetValue()
        {
            var value = new object();
            var input = new Mock<IStormInput<object>>(MockBehavior.Strict);
            input.Setup(i => i.SetValue(It.IsAny<IStormToken>(), value));
            StormExtensions.SetValue(input.Object, value);
        }

        [Test]
        public void SetValueThrownOnNullThisArgument()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.SetValue(null, new object()));
        }

        [Test]
        public void SetValueDoNotThrowOnNullValue()
        {
            var input = Mock.Of<IStormInput<object>>();
            StormExtensions.SetValue(input, null);
        }
    }
}