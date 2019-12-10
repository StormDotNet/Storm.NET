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
    using Moq;
    using NUnit.Framework;

    public abstract class StormTests
    {
        protected abstract IStorm CreateNewSubject();

        [Test]
        public void StormRaiseOnEnterOnAcceptToken()
        {
            var subject = CreateNewSubject();

            var mockToken = new Mock<IStormToken>();
            mockToken.Setup(t => t.IsDisposed).Returns(true);

            var mockDelegate = new Mock<StormOnTokenEnterDelegate>(MockBehavior.Strict);
            mockDelegate.Setup(d => d.Invoke(mockToken.Object));
            subject.OnEnter += mockDelegate.Object;

            // act
            subject.Accept(mockToken.Object);

            mockDelegate.Verify(d => d.Invoke(mockToken.Object), Times.Once);
        }

        [Test]
        public void StormThrowOnAcceptNotDisposedToken()
        {
            var subject = CreateNewSubject();
            var mockToken = new Mock<IStormToken>(MockBehavior.Strict);
            mockToken.Setup(t => t.IsDisposed).Returns(false);

            // act
            Assert.Throws<ArgumentException>(() => subject.Accept(mockToken.Object));

            mockToken.VerifyGet(t => t.IsDisposed, Times.Once);
        }

        [Test]
        public void StormThrowOnAcceptNullToken()
        {
            var subject = CreateNewSubject();

            // act
            Assert.Throws<ArgumentNullException>(() => subject.Accept(null));
        }
    }
}