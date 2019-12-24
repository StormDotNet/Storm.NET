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

// ReSharper disable UnusedVariable
namespace StormDotNet.Tests
{
    using System;
    using System.Linq;
    using Implementations;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class StormSocketTests : StormTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = Storm.Socket.Create<object>();
        }

        private IStormSocket<object> Sut { get; set; }
        protected override IStorm<object> SutStorm => Sut;

        [Test]
        public void ConnectTwiceThrow()
        {
            var mock = Mock.Of<IStorm<object>>();
            var token = Storm.TokenSource.CreateSource().Token;
            Sut.Connect(token, mock);
            Assert.Throws<InvalidOperationException>(() => Sut.Connect(token, mock));
        }

        [Test]
        public void SelfConnectThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.Connect(token, Sut));
        }

        [Test]
        public void Connect69Throw()
        {
            var other = Storm.Socket.Create<object>();
            other.Connect(Sut);
            Assert.Throws<InvalidOperationException>(() => Sut.Connect(other));
        }

        [Test]
        public void Connect69InBatchThrow()
        {
            static void Code()
            {
                var socket1 = Storm.Socket.Create<object>();
                var socket2 = Storm.Socket.Create<object>();
                using var tokenSource = Storm.TokenSource.CreateSource();
                var token = tokenSource.Token;
                socket1.Connect(token, socket2);
                socket2.Connect(token, socket1);
            }

            Assert.Throws<InvalidOperationException>(Code);
        }

        [Test]
        public void BatchConnectAndModifyTargetToValue()
        {
            var listener = new Mock<Action<StormToken, EStormVisitType>>(MockBehavior.Strict);
            var sequence = new MockSequence();
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.EnterLoopSearch));
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.LeaveLoopSearch));
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.EnterUpdate));
            listener.InSequence(sequence)
                    .Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.LeaveUpdateChanged));

            var target = Storm.Input.Create<int>();
            var socket = Storm.Socket.Create<int>();

            socket.OnVisit += listener.Object;

            using (var tokenSource = Storm.TokenSource.CreateSource())
            {
                var token = tokenSource.Token;
                target.SetValue(token, 42);
                socket.Connect(token, target);
            }

            Assert.That(socket.GetValueOrThrow(), Is.EqualTo(42));
            listener.Verify(a => a.Invoke(It.IsAny<StormToken>(), It.IsAny<EStormVisitType>()), Times.Exactly(4));
        }

        [Test]
        public void BatchConnectAndModifyTargetToDisconnected()
        {
            var listener = new Mock<Action<StormToken, EStormVisitType>>(MockBehavior.Strict);
            var sequence = new MockSequence();
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.EnterLoopSearch));
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.LeaveLoopSearch));
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.EnterUpdate));
            listener.InSequence(sequence)
                    .Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.LeaveUpdateUnchanged));

            var target = Storm.Input.Create<int>();
            var socket = Storm.Socket.Create<int>();

            socket.OnVisit += listener.Object;

            using (var tokenSource = Storm.TokenSource.CreateSource())
            {
                var token = tokenSource.Token;
                target.SetError(token, Error.Socket.Disconnected);
                socket.Connect(token, target);
            }

            Assert.That(socket.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Error.Socket.Disconnected));
            listener.Verify(a => a.Invoke(It.IsAny<StormToken>(), It.IsAny<EStormVisitType>()), Times.Exactly(4));
        }

        [Test]
        public void BatchConnectAndModifyTargetWithDifferentTokenThrow()
        {
            var target = Storm.Input.Create<int>();
            var socket = Storm.Socket.Create<int>();
            var f = Storm.Func.Create(socket, v => v);

            var tokenSource1 = Storm.TokenSource.CreateSource();
            var tokenSource2 = Storm.TokenSource.CreateSource();

            target.SetError(tokenSource1.Token, Error.Socket.Disconnected);
            Assert.Throws<InvalidOperationException>(() => socket.Connect(tokenSource2.Token, target));
        }

        [Test]
        public void ConnectToValueRaiseChanged()
        {
            var listener = new Mock<Action<StormToken, EStormVisitType>>(MockBehavior.Strict);
            var sequence = new MockSequence();
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.EnterLoopSearch));
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.LeaveLoopSearch));
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.EnterUpdate));
            listener.InSequence(sequence)
                    .Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.LeaveUpdateChanged));

            var target = Storm.Immutable.CreateValue(42);
            var socket = Storm.Socket.Create<int>();

            socket.OnVisit += listener.Object;

            socket.Connect(target);

            Assert.That(socket.GetValueOrThrow(), Is.EqualTo(42));
            listener.Verify(a => a.Invoke(It.IsAny<StormToken>(), It.IsAny<EStormVisitType>()), Times.Exactly(4));
        }

        [Test]
        public void ConnectToNotConnectedSocketDoNotRaiseUpdate()
        {
            var listener = new Mock<Action<StormToken, EStormVisitType>>(MockBehavior.Strict);
            var sequence = new MockSequence();
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.EnterLoopSearch));
            listener.InSequence(sequence).Setup(a => a.Invoke(It.IsAny<StormToken>(), EStormVisitType.LeaveLoopSearch));

            var target = Storm.Socket.Create<int>();
            var socket = Storm.Socket.Create<int>();

            socket.OnVisit += listener.Object;
            socket.Connect(target);

            Assert.That(socket.TryGetError(out var error), Is.True);
            Assert.That(error, Is.EqualTo(Error.Socket.Disconnected));
            listener.Verify(a => a.Invoke(It.IsAny<StormToken>(), It.IsAny<EStormVisitType>()), Times.Exactly(2));
        }

        [Test]
        public void ConnectDescendantThrow()
        {
            var descendant = Storm.Func.Create(Sut, v => v);
            var token = Storm.TokenSource.CreateSource().Token;
            Assert.Throws<InvalidOperationException>(() => Sut.Connect(token, descendant));
        }

        [Test]
        public void ConnectNullTargetThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            Assert.Throws<ArgumentNullException>(() => Sut.Connect(token, null));
        }

        [Test]
        public void ConnectToDisconnectedDoNotRaiseUpdateEvent()
        {
            Sut.OnVisit += (token, type) =>
            {
                switch (type)
                {
                    case EStormVisitType.EnterUpdate:
                    case EStormVisitType.LeaveUpdateChanged:
                    case EStormVisitType.LeaveUpdateUnchanged:
                        throw new Exception();
                }
            };
            Sut.Connect(Storm.Immutable.CreateError<object>(Error.Socket.Disconnected));
        }


        [Test]
        public void ConnectDefaultTokenThrow()
        {
            var target = Mock.Of<IStorm<object>>();
            Assert.Throws<ArgumentException>(() => Sut.Connect(default, target));
        }

        [Test]
        public void Target()
        {
            Assert.That(Sut.Target, Is.Null);
        }

        [Test]
        public void ValueMatch()
        {
            static object OnError(StormError obj) => obj;
            static object OnValue(object obj) => obj;

            var actual = Sut.Match(OnValue, OnError);
            Assert.That(actual, Is.InstanceOf<StormError>());
        }

        [Test]
        public void TryGetEnteredTokenWithNoTarget()
        {
            var state = SutNode.GetVisitState(out var enteredToken);
            Assert.That(state.IsInUpdate, Is.False);
            Assert.That(state.IsInLoopSearch, Is.False);
            Assert.That(enteredToken, Is.EqualTo(default(StormToken)));
        }

        [Test]
        public void TryGetEnteredTokenWithNoDeepTarget()
        {
            Sut.Connect(Storm.Socket.Create<object>());

            var state = Sut.GetVisitState(out var enteredToken);
            Assert.That(state.IsInUpdate, Is.False);
            Assert.That(state.IsInLoopSearch, Is.False);
            Assert.That(enteredToken, Is.EqualTo(default(StormToken)));
        }

        [Test]
        public void TryGetEnteredTokenWithTarget()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            var mock = new Mock<IStorm<object>>(MockBehavior.Strict);
            mock.Setup(m => m.GetVisitState(out token)).Returns(StormVisitState.Update);
            Sut.Connect(mock.Object);

            var state = Sut.GetVisitState(out var enteredToken);
            Assert.That(state.IsInUpdate, Is.True);
            Assert.That(state.IsInLoopSearch, Is.False);
            Assert.That(enteredToken, Is.EqualTo(token));
        }

        [Test]
        public void UnknownVisitTypeInMiddleOfAConnectionThrow()
        {
            var token = Storm.TokenSource.CreateSource().Token;
            var mock = new Mock<IStorm<object>>(MockBehavior.Strict);
            mock.Setup(m => m.GetVisitState(out token)).Returns(StormVisitState.Update);

            var f = Storm.Func.Create(Sut, v => v);
            Sut.Connect(token, mock.Object);
            var visitType = (EStormVisitType) (Enum.GetValues(typeof(EStormVisitType)).Cast<EStormVisitType>().Min(e => (int) e) - 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => mock.Raise(m => m.OnVisit += null, token, visitType));
        }

        [Test]
        public new void ToString()
        {
            Assert.That(Sut.ToString(), Is.EqualTo("err: 'Disconnected socket.'"));
        }
    }
}