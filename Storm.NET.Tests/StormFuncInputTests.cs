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
    using System.Linq;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class StormFuncInputTests
    {
        [Test]
        public void ConstructorThrowOnNullContent()
        {
            Assert.Throws<ArgumentNullException>(
                () => new StormFuncInput<object>(null, EStormFuncInputState.NotVisited));
        }

        [Test]
        public void ConstructorThrowOnOutOfRangeState()
        {
            var content = Mock.Of<IStormContent<object>>();
            var state = (EStormFuncInputState) (Enum.GetValues(typeof(EStormFuncInputState))
                                                    .Cast<EStormFuncInputState>().Min(e => (int) e) - 1);

            Assert.Throws<ArgumentOutOfRangeException>(() => new StormFuncInput<object>(content, state));
        }

        [Test]
        public void ExposeProvidedContent()
        {
            var content = Mock.Of<IStormContent<object>>();
            var sut = new StormFuncInput<object>(content, EStormFuncInputState.NotVisited);

            Assert.That(sut.Content, Is.EqualTo(content));
        }

        [Test]
        public void ExposeProvidedStatus([Values] EStormFuncInputState state)
        {
            var content = Mock.Of<IStormContent<object>>();
            var sut = new StormFuncInput<object>(content, state);

            Assert.That(sut.State, Is.EqualTo(state));
        }

        [Test]
        public void ToStringCallContentToString()
        {
            var content = new Mock<IStormContent<object>>(MockBehavior.Strict);
            content.Setup(c => c.ToString());
            var sut = new StormFuncInput<object>(content.Object, EStormFuncInputState.NotVisited);
            sut.ToString();

            content.Verify(c => c.ToString(), Times.Once);
        }

        private static IStormContent<object> ToStringContentMock(string content)
        {
            var mock = new Mock<IStormContent<object>>();
            mock.Setup(m => m.ToString()).Returns(content);
            return mock.Object;
        }

        public static IEnumerable<TestCaseData> ToStringTestCases { get; } =
            from t in new[]
            {
                new {Content = "test", State = EStormFuncInputState.NotVisited, Expected = "not visited, test"},
                new {Content = "test", State = EStormFuncInputState.VisitedWithChange, Expected = "changed, test"},
                new {Content = "test", State = EStormFuncInputState.VisitedWithoutChange, Expected = "unchanged, test"}
            }
            select new TestCaseData(ToStringContentMock(t.Content), t.State).Returns(t.Expected);

        [Test, TestCaseSource(nameof(ToStringTestCases))]
        public string ToStringCallContentToString(IStormContent<object> content, EStormFuncInputState state)
        {
            var sut = new StormFuncInput<object>(content, state);
            return sut.ToString();
        }
    }
}