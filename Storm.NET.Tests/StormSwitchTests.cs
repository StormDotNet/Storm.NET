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

// ReSharper disable IdentifierTypo
// ReSharper disable AccessToModifiedClosure
// ReSharper disable PossibleNullReferenceException
namespace StormDotNet.Tests
{
    using System;
    using Implementations;
    using NUnit.Framework;

    [TestFixture]
    public class StormSwitchTests
    {
        [Test]
        public void SelfConnectIsLoop()
        {
            var input = Storm.Input.Create<int>();
            IStorm<int> swicht = null;

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => input,
                    1 => swicht,
                    _ => null
                };
            }

            swicht = Storm.Switch.Create(input, Selector);

            input.SetValue(1);

            Assert.That(swicht.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Error.Switch.Looped));
        }

        [Test]
        public void SelfConnectViaSocketIsLoop()
        {
            var input = Storm.Input.Create<int>();
            var socket = Storm.Socket.Create<int>();

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => input,
                    1 => socket,
                    _ => null
                };
            }

            var swicht = Storm.Switch.Create(input, Selector);
            socket.Connect(swicht);

            input.SetValue(1);

            Assert.That(swicht.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Error.Switch.Looped));
        }

        [Test]
        public void ConnectLoopIsLoop()
        {
            var input = Storm.Input.Create<int>();
            var socket = Storm.Socket.Create<int>();
            var func = Storm.Func.Create(socket, v => v);

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => input,
                    1 => func,
                    _ => null
                };
            }

            var swicht = Storm.Switch.Create(input, Selector);
            socket.Connect(swicht);

            input.SetValue(1);

            Assert.That(swicht.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Error.Switch.Looped));
        }

        [Test]
        public void ConnectToNullIsDisconnected()
        {
            var input = Storm.Input.Create<int>();

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => input,
                    _ => null
                };
            }

            var swicht = Storm.Switch.Create(input, Selector);

            input.SetValue(1);

            Assert.That(swicht.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Error.Switch.Disconnected));
        }

        [Test]
        public void ConnectedToValueIsValue()
        {
            var input = Storm.Input.Create<int>();
            var target = Storm.Immutable.CreateValue(42);

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => target,
                    _ => null
                };
            }

            var swicht = Storm.Switch.Create(input, Selector);

            input.SetValue(0);

            Assert.That(swicht.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void ConnectedToErrorIsError()
        {
            var input = Storm.Input.Create<int>();
            var target = Storm.Immutable.CreateError<int>("42");

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => target,
                    _ => null
                };
            }

            var swicht = Storm.Switch.Create(input, Selector);

            input.SetValue(0);

            Assert.That(swicht.TryGetError(out var error), Is.True);
            Assert.That(error.Message, Is.EqualTo("42"));
        }

        [Test]
        public void SelectorErrorIsError()
        {
            var input = Storm.Input.Create<int>();

            IStorm<int> Selector(int value)
            {
                throw new Exception("42");
            }

            var swicht = Storm.Switch.Create(input, Selector);

            input.SetValue(0);

            Assert.That(swicht.TryGetError(out var error), Is.True);
            Assert.That(error.InnerException.Message, Is.EqualTo("42"));
        }

        [Test]
        public void OldTargetIsIdleNewTargetIsIdle()
        {
            var select = Storm.Input.Create<int>();
            var oldTarget = Storm.Immutable.CreateValue(42);
            var newTarget = Storm.Immutable.CreateValue(69);

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => oldTarget,
                    1 => newTarget,
                    _ => null
                };
            }

            var swicht = Storm.Switch.Create(select, Selector);

            Assert.That(swicht.TryGetError(out _), Is.True);
            select.SetValue(0);
            Assert.That(swicht.GetValueOrThrow(), Is.EqualTo(42));
            select.SetValue(1);
            Assert.That(swicht.GetValueOrThrow(), Is.EqualTo(69));
        }

        [Test]
        public void OldTargetIsInGraphNewTargetIsIdle()
        {
            var select = Storm.Input.Create<int>();
            var oldTarget = Storm.Func.Create(select, v => 42);
            var newTarget = Storm.Immutable.CreateValue(69);

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => oldTarget,
                    1 => newTarget,
                    _ => null
                };
            }

            var swicht = Storm.Switch.Create(select, Selector);

            Assert.That(swicht.TryGetError(out _), Is.True);
            select.SetValue(0);
            Assert.That(swicht.GetValueOrThrow(), Is.EqualTo(42));
            select.SetValue(1);
            Assert.That(swicht.GetValueOrThrow(), Is.EqualTo(69));
        }

        [Test]
        public void OldTargetIsIdleNewTargetIsInGraph()
        {
            var select = Storm.Input.Create<int>();
            var oldTarget = Storm.Immutable.CreateValue(42);
            var newTarget = Storm.Func.Create(select, v => 69);

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => oldTarget,
                    1 => newTarget,
                    _ => null
                };
            }

            var swicht = Storm.Switch.Create(select, Selector);

            Assert.That(swicht.TryGetError(out _), Is.True);
            select.SetValue(0);
            Assert.That(swicht.GetValueOrThrow(), Is.EqualTo(42));
            select.SetValue(1);
            Assert.That(swicht.GetValueOrThrow(), Is.EqualTo(69));
        }

        [Test]
        public void OldTargetIsInGraphNewTargetIsInGraph()
        {
            var select = Storm.Input.Create<int>();
            var oldTarget = Storm.Func.Create(select, v => 42);
            var newTarget = Storm.Func.Create(select, v => 69);

            IStorm<int> Selector(int value)
            {
                return value switch
                {
                    0 => oldTarget,
                    1 => newTarget,
                    _ => null
                };
            }

            var swicht = Storm.Switch.Create(select, Selector);

            Assert.That(swicht.TryGetError(out _), Is.True);
            select.SetValue(0);
            Assert.That(swicht.GetValueOrThrow(), Is.EqualTo(42));
            select.SetValue(1);
            Assert.That(swicht.GetValueOrThrow(), Is.EqualTo(69));
        }
    }
}