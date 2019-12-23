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
    using NUnit.Framework;

    [TestFixture]
    public class StormSocketChainTests
    {
        [Test]
        public void Test3ElementsChainConnect1Then2()
        {
            var i = Storm.Input.WithCompare.Create<int>();
            var s1 = Storm.Socket.Create<int>();
            var s2 = Storm.Socket.Create<int>();

            s1.Connect(i);
            s2.Connect(s1);

            i.SetValue(0);
            Assert.That(s2.GetValueOrThrow(), Is.EqualTo(0));
            Assert.That(s2.Target, Is.EqualTo(i));
        }

        [Test]
        public void Test3ElementsChainConnect2Then1()
        {
            var i = Storm.Input.WithCompare.Create<int>();
            var s1 = Storm.Socket.Create<int>();
            var s2 = Storm.Socket.Create<int>();

            s2.Connect(s1);
            s1.Connect(i);

            i.SetValue(0);
            Assert.That(s2.GetValueOrThrow(), Is.EqualTo(0));
            Assert.That(s2.Target, Is.EqualTo(i));
        }

        [Test]
        public void Test3ElementsChainConnect1Then2WithListener()
        {
            var i = Storm.Input.WithCompare.Create<int>();
            var s1 = Storm.Socket.Create<int>();
            var s2 = Storm.Socket.Create<int>();
            var f = Storm.Func.Create(s2, v => v);

            s1.Connect(i);
            s2.Connect(s1);

            i.SetValue(0);
            Assert.That(s2.GetValueOrThrow(), Is.EqualTo(0));
            Assert.That(s2.Target, Is.EqualTo(i));
        }

        [Test]
        public void Test3ElementsChainConnect2Then1WithListener()
        {
            var i = Storm.Input.WithCompare.Create<int>();
            var s1 = Storm.Socket.Create<int>();
            var s2 = Storm.Socket.Create<int>();
            var f = Storm.Func.Create(s2, v => v);

            s2.Connect(s1);
            s1.Connect(i);

            i.SetValue(0);
            Assert.That(s2.GetValueOrThrow(), Is.EqualTo(0));
            Assert.That(s2.Target, Is.EqualTo(i));
        }

        [Test]
        public void Test3ElementsInBatchChainConnect1Then2()
        {
            var i = Storm.Input.WithCompare.Create<int>();
            var s1 = Storm.Socket.Create<int>();
            var s2 = Storm.Socket.Create<int>();

            using (var tokenSource = Storm.TokenSource.CreateSource())
            {
                var token = tokenSource.Token;
                s1.Connect(token, i);
                s2.Connect(token, s1);
                i.SetValue(token, 0);
            }
            
            Assert.That(s2.GetValueOrThrow(), Is.EqualTo(0));
            Assert.That(s2.Target, Is.EqualTo(i));
        }

        [Test]
        public void Test3ElementsInBatchChainConnect2Then1()
        {
            var i = Storm.Input.WithCompare.Create<int>();
            var s1 = Storm.Socket.Create<int>();
            var s2 = Storm.Socket.Create<int>();

            using (var tokenSource = Storm.TokenSource.CreateSource())
            {
                var token = tokenSource.Token;
                s2.Connect(token, s1);
                s1.Connect(token, i);
                i.SetValue(token, 0);
            }

            Assert.That(s2.GetValueOrThrow(), Is.EqualTo(0));
            Assert.That(s2.Target, Is.EqualTo(i));
        }

        [Test]
        public void DifficultCase1()
        {
            var i0 = Storm.Input.WithCompare.Create<int>();
            var i1 = Storm.Input.WithCompare.Create<int>();
            var f1 = Storm.Func.Create(i1, v => v);
            var f2 = Storm.Func.Create(i1, v => v);
            var s1 = Storm.Switch.Create(i0, i => i % 2 == 0 ? f1 : f2);
            var t1 = Storm.Socket.Create<int>();
            var l1 = Storm.Func.Create(t1, v => v);

            i0.SetValue(0);

            using (var tokenSource = Storm.TokenSource.CreateSource())
            {
                var token = tokenSource.Token;
                i1.SetValue(token, 42);
                t1.Connect(token, s1);
                i0.SetValue(token, 1);
            }

            Assert.That(l1.GetValueOrThrow(), Is.EqualTo(42));
        }

        [Test]
        public void DifficultCase2()
        {
            var i0 = Storm.Input.WithCompare.Create<int>();
            var i1 = Storm.Input.WithCompare.Create<int>();
            var i2 = Storm.Input.WithCompare.Create<int>();
            var f1 = Storm.Func.Create(i1, v => v);
            var f2 = Storm.Func.Create(i2, v => v);
            var s1 = Storm.Switch.Create(i0, i => i % 2 == 0 ? f1 : f2);
            var t1 = Storm.Socket.Create<int>();
            var l1 = Storm.Func.Create(t1, v => v);

            i0.SetValue(0);
            i1.SetValue(42);

            using (var tokenSource = Storm.TokenSource.CreateSource())
            {
                var token = tokenSource.Token;
                i1.SetValue(token, 0);
                i2.SetValue(token, 42);
                t1.Connect(token, s1);
                i0.SetValue(token, 1);
            }

            Assert.That(l1.GetValueOrThrow(), Is.EqualTo(42));
        }
    }
}