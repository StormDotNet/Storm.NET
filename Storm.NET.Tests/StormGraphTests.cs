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
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class StormGraphTests
    {
        [Test]
        public void Diamond()
        {
            var input = Storm.Input.Create<int>();
            var left = Storm.Func.Create(input, v => v / 2);
            var right = Storm.Func.Create(input, v => v % 2);
            var output = Storm.Func.Create(left, right, (lValue, rValue) => lValue * 2 + rValue);

            Assert.That(output.TryGetValue(out _), Is.False);
            Assert.That(output.TryGetError(out _), Is.True);

            foreach (var v in Enumerable.Range(0, 10))
            {
                input.SetValue(v);
                Assert.That(output.TryGetValue(out var value), Is.True);
                Assert.That(value, Is.EqualTo(v));
                Assert.That(output.TryGetError(out _), Is.False);
            }

            input.SetEmpty();
            Assert.That(output.TryGetValue(out _), Is.False);
            Assert.That(output.TryGetError(out _), Is.True);
        }

        [Test]
        public void DiamondWithSocket()
        {
            var input = Storm.Input.Create<int>();
            var left = Storm.Func.Create(input, v => v / 2);
            var right = Storm.Socket.Create<int>();
            var output = Storm.Func.Create(left, right, (lValue, rValue) => lValue * 2 + rValue);
            var rightTarget = Storm.Func.Create(input, v => v % 2);
            right.Connect(rightTarget);

            Assert.That(output.TryGetValue(out _), Is.False);
            Assert.That(output.TryGetError(out _), Is.True);

            foreach (var v in Enumerable.Range(0, 10))
            {
                input.SetValue(v);
                Assert.That(output.TryGetValue(out var value), Is.True);
                Assert.That(value, Is.EqualTo(v));
                Assert.That(output.TryGetError(out _), Is.False);
            }

            input.SetEmpty();
            Assert.That(output.TryGetValue(out _), Is.False);
            Assert.That(output.TryGetError(out _), Is.True);
        }

        [Test]
        public void SwitchLoop()
        {
            var input = Storm.Input.Create<int>();
            var a = Storm.Func.Create(input, v => v);
            var b = Storm.Socket.Create<int>();
            var s = Storm.Switch.FromValues.Create(input, i => i % 2 == 0 ? a : b);
            var c = Storm.Func.Create(s, v => v);
            b.Connect(c);

            input.SetValue(0);
            Assert.That(s.TryGetError(out _), Is.False);
            input.SetValue(1);
            Assert.That(s.TryGetError(out _), Is.True);
        }

        [Test]
        public void SwitchLoop2()
        {
            var input = Storm.Input.Create<int>();
            var socket = Storm.Socket.Create<int>();
            var s = Storm.Switch.FromValues.Create(input, i => i % 2 == 0 ? Storm.Immutable.CreateValue(1) : socket);
            socket.Connect(s);

            input.SetValue(0);
            Assert.That(s.TryGetError(out _), Is.False);
            input.SetValue(1);
            Assert.That(s.TryGetError(out _), Is.True);
        }
    }
}