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
    public class StormFuncFromValuesTests
    {
        private static IStorm<int> CreateInput(bool isValue)
        {
            return isValue ? Storm.Immutable.CreateValue(0) : Storm.Immutable.CreateError<int>("error");
        }

        private static int BitCount(int i)
        {
            var r = 0;
            while (i > 0)
            {
                r += i & 1;
                i >>= 1;
            }

            return r;
        }
        public static IEnumerable<TestCaseData> FuncWith1InputCases()
        {
            for (var i = 1; i < Math.Pow(2, 1); i++)
            {
                yield return new TestCaseData(
                    CreateInput((i & (1 << 0)) == 0)
                ).Returns(BitCount(i)).SetName($"FuncWith1Input. Case #{i:000}");
            }
        }

        [Test, TestCaseSource(nameof(FuncWith1InputCases))]
        public int FuncWith1InputWithErrors(IStorm<int> first)
        {
            static int Func(int firstInput) => 0;

            var sut = Storm.Func.Create(first, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<AggregateException>());

            // ReSharper disable once PossibleNullReferenceException
            return (error.InnerException as AggregateException).InnerExceptions.Count;
        }

        [Test]
        public void FuncWith1InputReturnError()
        {
            var first = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput) => throw Storm.Error.Create("error");

            var sut = Storm.Func.Create(first, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.Message, Is.EqualTo("error"));
        }

        [Test]
        public void FuncWith1InputReturnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput) => throw new Exception();

            var sut = Storm.Func.Create(first, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<Exception>());
        }

        [Test]
        public void FuncWith1InputReturnValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput) => 42;

            var sut = Storm.Func.Create(first, Func);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));
        }
        public static IEnumerable<TestCaseData> FuncWith2InputCases()
        {
            for (var i = 1; i < Math.Pow(2, 2); i++)
            {
                yield return new TestCaseData(
                    CreateInput((i & (1 << 0)) == 0),
                    CreateInput((i & (1 << 1)) == 0)
                ).Returns(BitCount(i)).SetName($"FuncWith2Input. Case #{i:000}");
            }
        }

        [Test, TestCaseSource(nameof(FuncWith2InputCases))]
        public int FuncWith2InputWithErrors(IStorm<int> first, IStorm<int> second)
        {
            static int Func(int firstInput, int secondInput) => 0;

            var sut = Storm.Func.Create(first, second, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<AggregateException>());

            // ReSharper disable once PossibleNullReferenceException
            return (error.InnerException as AggregateException).InnerExceptions.Count;
        }

        [Test]
        public void FuncWith2InputReturnError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput) => throw Storm.Error.Create("error");

            var sut = Storm.Func.Create(first, second, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.Message, Is.EqualTo("error"));
        }

        [Test]
        public void FuncWith2InputReturnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput) => throw new Exception();

            var sut = Storm.Func.Create(first, second, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<Exception>());
        }

        [Test]
        public void FuncWith2InputReturnValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput) => 42;

            var sut = Storm.Func.Create(first, second, Func);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));
        }
        public static IEnumerable<TestCaseData> FuncWith3InputCases()
        {
            for (var i = 1; i < Math.Pow(2, 3); i++)
            {
                yield return new TestCaseData(
                    CreateInput((i & (1 << 0)) == 0),
                    CreateInput((i & (1 << 1)) == 0),
                    CreateInput((i & (1 << 2)) == 0)
                ).Returns(BitCount(i)).SetName($"FuncWith3Input. Case #{i:000}");
            }
        }

        [Test, TestCaseSource(nameof(FuncWith3InputCases))]
        public int FuncWith3InputWithErrors(IStorm<int> first, IStorm<int> second, IStorm<int> third)
        {
            static int Func(int firstInput, int secondInput, int thirdInput) => 0;

            var sut = Storm.Func.Create(first, second, third, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<AggregateException>());

            // ReSharper disable once PossibleNullReferenceException
            return (error.InnerException as AggregateException).InnerExceptions.Count;
        }

        [Test]
        public void FuncWith3InputReturnError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput) => throw Storm.Error.Create("error");

            var sut = Storm.Func.Create(first, second, third, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.Message, Is.EqualTo("error"));
        }

        [Test]
        public void FuncWith3InputReturnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput) => throw new Exception();

            var sut = Storm.Func.Create(first, second, third, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<Exception>());
        }

        [Test]
        public void FuncWith3InputReturnValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput) => 42;

            var sut = Storm.Func.Create(first, second, third, Func);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));
        }
        public static IEnumerable<TestCaseData> FuncWith4InputCases()
        {
            for (var i = 1; i < Math.Pow(2, 4); i++)
            {
                yield return new TestCaseData(
                    CreateInput((i & (1 << 0)) == 0),
                    CreateInput((i & (1 << 1)) == 0),
                    CreateInput((i & (1 << 2)) == 0),
                    CreateInput((i & (1 << 3)) == 0)
                ).Returns(BitCount(i)).SetName($"FuncWith4Input. Case #{i:000}");
            }
        }

        [Test, TestCaseSource(nameof(FuncWith4InputCases))]
        public int FuncWith4InputWithErrors(IStorm<int> first, IStorm<int> second, IStorm<int> third, IStorm<int> fourth)
        {
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput) => 0;

            var sut = Storm.Func.Create(first, second, third, fourth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<AggregateException>());

            // ReSharper disable once PossibleNullReferenceException
            return (error.InnerException as AggregateException).InnerExceptions.Count;
        }

        [Test]
        public void FuncWith4InputReturnError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput) => throw Storm.Error.Create("error");

            var sut = Storm.Func.Create(first, second, third, fourth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.Message, Is.EqualTo("error"));
        }

        [Test]
        public void FuncWith4InputReturnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput) => throw new Exception();

            var sut = Storm.Func.Create(first, second, third, fourth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<Exception>());
        }

        [Test]
        public void FuncWith4InputReturnValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput) => 42;

            var sut = Storm.Func.Create(first, second, third, fourth, Func);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));
        }
        public static IEnumerable<TestCaseData> FuncWith5InputCases()
        {
            for (var i = 1; i < Math.Pow(2, 5); i++)
            {
                yield return new TestCaseData(
                    CreateInput((i & (1 << 0)) == 0),
                    CreateInput((i & (1 << 1)) == 0),
                    CreateInput((i & (1 << 2)) == 0),
                    CreateInput((i & (1 << 3)) == 0),
                    CreateInput((i & (1 << 4)) == 0)
                ).Returns(BitCount(i)).SetName($"FuncWith5Input. Case #{i:000}");
            }
        }

        [Test, TestCaseSource(nameof(FuncWith5InputCases))]
        public int FuncWith5InputWithErrors(IStorm<int> first, IStorm<int> second, IStorm<int> third, IStorm<int> fourth, IStorm<int> fifth)
        {
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput) => 0;

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<AggregateException>());

            // ReSharper disable once PossibleNullReferenceException
            return (error.InnerException as AggregateException).InnerExceptions.Count;
        }

        [Test]
        public void FuncWith5InputReturnError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput) => throw Storm.Error.Create("error");

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.Message, Is.EqualTo("error"));
        }

        [Test]
        public void FuncWith5InputReturnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput) => throw new Exception();

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<Exception>());
        }

        [Test]
        public void FuncWith5InputReturnValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput) => 42;

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, Func);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));
        }
        public static IEnumerable<TestCaseData> FuncWith6InputCases()
        {
            for (var i = 1; i < Math.Pow(2, 6); i++)
            {
                yield return new TestCaseData(
                    CreateInput((i & (1 << 0)) == 0),
                    CreateInput((i & (1 << 1)) == 0),
                    CreateInput((i & (1 << 2)) == 0),
                    CreateInput((i & (1 << 3)) == 0),
                    CreateInput((i & (1 << 4)) == 0),
                    CreateInput((i & (1 << 5)) == 0)
                ).Returns(BitCount(i)).SetName($"FuncWith6Input. Case #{i:000}");
            }
        }

        [Test, TestCaseSource(nameof(FuncWith6InputCases))]
        public int FuncWith6InputWithErrors(IStorm<int> first, IStorm<int> second, IStorm<int> third, IStorm<int> fourth, IStorm<int> fifth, IStorm<int> sixth)
        {
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput) => 0;

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<AggregateException>());

            // ReSharper disable once PossibleNullReferenceException
            return (error.InnerException as AggregateException).InnerExceptions.Count;
        }

        [Test]
        public void FuncWith6InputReturnError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput) => throw Storm.Error.Create("error");

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.Message, Is.EqualTo("error"));
        }

        [Test]
        public void FuncWith6InputReturnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput) => throw new Exception();

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<Exception>());
        }

        [Test]
        public void FuncWith6InputReturnValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput) => 42;

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, Func);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));
        }
        public static IEnumerable<TestCaseData> FuncWith7InputCases()
        {
            for (var i = 1; i < Math.Pow(2, 7); i++)
            {
                yield return new TestCaseData(
                    CreateInput((i & (1 << 0)) == 0),
                    CreateInput((i & (1 << 1)) == 0),
                    CreateInput((i & (1 << 2)) == 0),
                    CreateInput((i & (1 << 3)) == 0),
                    CreateInput((i & (1 << 4)) == 0),
                    CreateInput((i & (1 << 5)) == 0),
                    CreateInput((i & (1 << 6)) == 0)
                ).Returns(BitCount(i)).SetName($"FuncWith7Input. Case #{i:000}");
            }
        }

        [Test, TestCaseSource(nameof(FuncWith7InputCases))]
        public int FuncWith7InputWithErrors(IStorm<int> first, IStorm<int> second, IStorm<int> third, IStorm<int> fourth, IStorm<int> fifth, IStorm<int> sixth, IStorm<int> seventh)
        {
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput, int seventhInput) => 0;

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<AggregateException>());

            // ReSharper disable once PossibleNullReferenceException
            return (error.InnerException as AggregateException).InnerExceptions.Count;
        }

        [Test]
        public void FuncWith7InputReturnError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput, int seventhInput) => throw Storm.Error.Create("error");

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.Message, Is.EqualTo("error"));
        }

        [Test]
        public void FuncWith7InputReturnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput, int seventhInput) => throw new Exception();

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<Exception>());
        }

        [Test]
        public void FuncWith7InputReturnValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput, int seventhInput) => 42;

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, Func);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));
        }
        public static IEnumerable<TestCaseData> FuncWith8InputCases()
        {
            for (var i = 1; i < Math.Pow(2, 8); i++)
            {
                yield return new TestCaseData(
                    CreateInput((i & (1 << 0)) == 0),
                    CreateInput((i & (1 << 1)) == 0),
                    CreateInput((i & (1 << 2)) == 0),
                    CreateInput((i & (1 << 3)) == 0),
                    CreateInput((i & (1 << 4)) == 0),
                    CreateInput((i & (1 << 5)) == 0),
                    CreateInput((i & (1 << 6)) == 0),
                    CreateInput((i & (1 << 7)) == 0)
                ).Returns(BitCount(i)).SetName($"FuncWith8Input. Case #{i:000}");
            }
        }

        [Test, TestCaseSource(nameof(FuncWith8InputCases))]
        public int FuncWith8InputWithErrors(IStorm<int> first, IStorm<int> second, IStorm<int> third, IStorm<int> fourth, IStorm<int> fifth, IStorm<int> sixth, IStorm<int> seventh, IStorm<int> eighth)
        {
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput, int seventhInput, int eighthInput) => 0;

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<AggregateException>());

            // ReSharper disable once PossibleNullReferenceException
            return (error.InnerException as AggregateException).InnerExceptions.Count;
        }

        [Test]
        public void FuncWith8InputReturnError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);
            var eighth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput, int seventhInput, int eighthInput) => throw Storm.Error.Create("error");

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.Message, Is.EqualTo("error"));
        }

        [Test]
        public void FuncWith8InputReturnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);
            var eighth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput, int seventhInput, int eighthInput) => throw new Exception();

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, Func);

            Assert.That(sut.TryGetError(out var error), Is.True);
            Assert.That(error, Is.InstanceOf<StormError>());
            Assert.That(error.InnerException, Is.InstanceOf<Exception>());
        }

        [Test]
        public void FuncWith8InputReturnValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);
            var eighth = Storm.Immutable.CreateValue(0);
            static int Func(int firstInput, int secondInput, int thirdInput, int fourthInput, int fifthInput, int sixthInput, int seventhInput, int eighthInput) => 42;

            var sut = Storm.Func.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, Func);

            Assert.That(sut.GetValueOrThrow(), Is.EqualTo(42));
        }

    }
}