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
    using NUnit.Framework;

    [TestFixture]
    public class StormFuncFactoryFromStatesTests
    {
        [Test]
        public void FuncWith1InputReturnsAValue()
        {
            var first = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput)
            {
                return 42;
            }

            var result = Storm.Func.FromStates.Create(first, Func);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void FuncWith1InputThrowAStormError()
        {
            var first = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput)
            {
                throw Storm.Error.EmptyContent;
            }

            var result = Storm.Func.FromStates.Create(first, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Storm.Error.EmptyContent));
        }

        [Test]
        public void FuncWith1InputThrowAnException()
        {
            var first = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput)
            {
                throw new Exception();
            }

            var result = Storm.Func.FromStates.Create(first, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.TypeOf<StormError>());
            Assert.That(error.InnerException, Is.TypeOf<Exception>());
        }

        [Test]
        public void FuncWith2InputReturnsAValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput)
            {
                return 42;
            }

            var result = Storm.Func.FromStates.Create(first, second, Func);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void FuncWith2InputThrowAStormError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput)
            {
                throw Storm.Error.EmptyContent;
            }

            var result = Storm.Func.FromStates.Create(first, second, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Storm.Error.EmptyContent));
        }

        [Test]
        public void FuncWith2InputThrowAnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput)
            {
                throw new Exception();
            }

            var result = Storm.Func.FromStates.Create(first, second, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.TypeOf<StormError>());
            Assert.That(error.InnerException, Is.TypeOf<Exception>());
        }

        [Test]
        public void FuncWith3InputReturnsAValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput)
            {
                return 42;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, Func);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void FuncWith3InputThrowAStormError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput)
            {
                throw Storm.Error.EmptyContent;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Storm.Error.EmptyContent));
        }

        [Test]
        public void FuncWith3InputThrowAnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput)
            {
                throw new Exception();
            }

            var result = Storm.Func.FromStates.Create(first, second, third, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.TypeOf<StormError>());
            Assert.That(error.InnerException, Is.TypeOf<Exception>());
        }

        [Test]
        public void FuncWith4InputReturnsAValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput)
            {
                return 42;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, Func);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void FuncWith4InputThrowAStormError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput)
            {
                throw Storm.Error.EmptyContent;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Storm.Error.EmptyContent));
        }

        [Test]
        public void FuncWith4InputThrowAnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput)
            {
                throw new Exception();
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.TypeOf<StormError>());
            Assert.That(error.InnerException, Is.TypeOf<Exception>());
        }

        [Test]
        public void FuncWith5InputReturnsAValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput)
            {
                return 42;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, Func);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void FuncWith5InputThrowAStormError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput)
            {
                throw Storm.Error.EmptyContent;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Storm.Error.EmptyContent));
        }

        [Test]
        public void FuncWith5InputThrowAnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput)
            {
                throw new Exception();
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.TypeOf<StormError>());
            Assert.That(error.InnerException, Is.TypeOf<Exception>());
        }

        [Test]
        public void FuncWith6InputReturnsAValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput, StormFuncInput<int> sixthInput)
            {
                return 42;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, Func);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void FuncWith6InputThrowAStormError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput, StormFuncInput<int> sixthInput)
            {
                throw Storm.Error.EmptyContent;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Storm.Error.EmptyContent));
        }

        [Test]
        public void FuncWith6InputThrowAnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput, StormFuncInput<int> sixthInput)
            {
                throw new Exception();
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.TypeOf<StormError>());
            Assert.That(error.InnerException, Is.TypeOf<Exception>());
        }

        [Test]
        public void FuncWith7InputReturnsAValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput, StormFuncInput<int> sixthInput, StormFuncInput<int> seventhInput)
            {
                return 42;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, seventh, Func);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void FuncWith7InputThrowAStormError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput, StormFuncInput<int> sixthInput, StormFuncInput<int> seventhInput)
            {
                throw Storm.Error.EmptyContent;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, seventh, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Storm.Error.EmptyContent));
        }

        [Test]
        public void FuncWith7InputThrowAnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput, StormFuncInput<int> sixthInput, StormFuncInput<int> seventhInput)
            {
                throw new Exception();
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, seventh, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.TypeOf<StormError>());
            Assert.That(error.InnerException, Is.TypeOf<Exception>());
        }

        [Test]
        public void FuncWith8InputReturnsAValue()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);
            var eighth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput, StormFuncInput<int> sixthInput, StormFuncInput<int> seventhInput, StormFuncInput<int> eighthInput)
            {
                return 42;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, Func);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void FuncWith8InputThrowAStormError()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);
            var eighth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput, StormFuncInput<int> sixthInput, StormFuncInput<int> seventhInput, StormFuncInput<int> eighthInput)
            {
                throw Storm.Error.EmptyContent;
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Storm.Error.EmptyContent));
        }

        [Test]
        public void FuncWith8InputThrowAnException()
        {
            var first = Storm.Immutable.CreateValue(0);
            var second = Storm.Immutable.CreateValue(0);
            var third = Storm.Immutable.CreateValue(0);
            var fourth = Storm.Immutable.CreateValue(0);
            var fifth = Storm.Immutable.CreateValue(0);
            var sixth = Storm.Immutable.CreateValue(0);
            var seventh = Storm.Immutable.CreateValue(0);
            var eighth = Storm.Immutable.CreateValue(0);

            static int Func(StormFuncInput<int> firstInput, StormFuncInput<int> secondInput, StormFuncInput<int> thirdInput, StormFuncInput<int> fourthInput, StormFuncInput<int> fifthInput, StormFuncInput<int> sixthInput, StormFuncInput<int> seventhInput, StormFuncInput<int> eighthInput)
            {
                throw new Exception();
            }

            var result = Storm.Func.FromStates.Create(first, second, third, fourth, fifth, sixth, seventh, eighth, Func);
            Assert.That(result.TryGetError(out var error), Is.True);
            Assert.That(error, Is.TypeOf<StormError>());
            Assert.That(error.InnerException, Is.TypeOf<Exception>());
        }

    }
}