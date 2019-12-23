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

// ReSharper disable RedundantTypeArgumentsOfMethod
// ReSharper disable ExpressionIsAlwaysNull
namespace StormDotNet.Tests
{
    using System;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class StormExtensionsIStormContentTests
    {
        [Test]
        public void GetValueOrOnNullContentThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.GetValueOr<object>(null, new object()));
        }

        [Test]
        public void GetValueOrOnContentWithValueReturnValue()
        {
            var value = new object();
            var content = Storm.Immutable.CreateValue(value);
            var fallBack = new object();
            var result = content.GetValueOr(fallBack);
            
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void GetValueOrOnContentWithNullValueReturnValue()
        {
            object value = null;
            var content = Storm.Immutable.CreateValue(value);
            var fallBack = new object();
            var result = content.GetValueOr(fallBack);

            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void GetValueOrOnErrorReturnFallback()
        {
            var content = Storm.Immutable.CreateError<object>("error");
            var fallBack = new object();
            var result = content.GetValueOr(fallBack);

            Assert.That(result, Is.EqualTo(fallBack));
        }

        [Test]
        public void GetValueOrThrowOnNullContentThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.GetValueOrThrow<object>(null));
        }

        [Test]
        public void GetValueOrThrowOnContentWithValueReturnValue()
        {
            var value = new object();
            var content = Storm.Immutable.CreateValue(value);
            var result = content.GetValueOrThrow();

            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void GetValueOrThrowOnContentWithNullValueReturnValue()
        {
            object value = null;
            var content = Storm.Immutable.CreateValue(value);
            var result = content.GetValueOrThrow();

            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void GetValueOrThrowOnErrorThrow()
        {
            var error = Storm.Error.Create("error");
            var content = Storm.Immutable.CreateError<object>(error);
            Assert.Throws(Is.EqualTo(error), () => content.GetValueOrThrow());
        }

        [Test]
        public void MatchOnNullContentThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.Match<object>(null, v => throw new Exception(), v => throw new Exception()));
        }

        [Test]
        public void MatchOnNullOnValueThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.Match<object>(Mock.Of<IStormContent<object>>(), null, v => throw new Exception()));
        }

        [Test]
        public void MatchOnNullOnErrorThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.Match<object>(Mock.Of<IStormContent<object>>(), v => throw new Exception(), null));
        }

        [Test]
        public void MatchOnValueCallOnValueOnce()
        {
            var value = new object();
            var content = Storm.Immutable.CreateValue(value);

            var callCount = 0;
            void OnValue(object v)
            {
                Assert.That(v, Is.EqualTo(value));
                callCount++;
            }

            content.Match(OnValue, e => throw new Exception());
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void MatchOnErrorCallOnErrorOnce()
        {
            var error = Storm.Error.Create("error");
            var content = Storm.Immutable.CreateError<object>(error);

            var callCount = 0;
            void OnError(StormError e)
            {
                Assert.That(e, Is.EqualTo(error));
                callCount++;
            }

            content.Match(e => throw new Exception(), OnError);
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void TryGetErrorOnNullContentThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.TryGetError<object>(null, out _));
        }

        [Test]
        public void TryGetErrorOnValueReturnFalseAndNull()
        {
            var value = new object();
            var content = Storm.Immutable.CreateValue(value);
            Assert.That(content.TryGetError(out var contentError), Is.False);
            Assert.That(contentError, Is.Null);
        }

        [Test]
        public void TryGetErrorOnErrorReturnTrueAndError()
        {
            var error = Storm.Error.Create("error");
            var content = Storm.Immutable.CreateError<object>(error);
            Assert.That(content.TryGetError(out var contentError), Is.True);
            Assert.That(contentError, Is.EqualTo(error));
        }

        [Test]
        public void TryGetValueOnNullContentThrow()
        {
            Assert.Throws<ArgumentNullException>(() => StormExtensions.TryGetValue<object>(null, out _));
        }

        [Test]
        public void TryGetValueOnErrorReturnFalseAndDefault()
        {
            var error = Storm.Error.Create("error");
            var content = Storm.Immutable.CreateError<object>(error);
            Assert.That(content.TryGetValue(out var contentValue), Is.False);
            Assert.That(contentValue, Is.EqualTo(default));
        }

        [Test]
        public void TryGetValueOnValueReturnTrueAndValue()
        {
            var value = new object();
            var content = Storm.Immutable.CreateValue(value);
            Assert.That(content.TryGetValue(out var contentValue), Is.True);
            Assert.That(contentValue, Is.EqualTo(value));
        }
    }
}
