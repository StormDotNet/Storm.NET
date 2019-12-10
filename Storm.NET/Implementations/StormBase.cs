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

namespace StormDotNet.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public abstract class StormBase<T> : IStorm<T>
    {
        private readonly IEqualityComparer<T>? _comparer;
        private Exception? _error;
        private Content _content;
        private IStormToken _currentToken = Storm.Token.Initial;
        private State _state = State.Idle;

        [AllowNull]
        [MaybeNull]
        private T _value;

        protected StormBase(IEqualityComparer<T>? comparer)
        {
            _comparer = comparer;
            _value = default;
        }

        public void Accept(IStormToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (!token.IsDisposed) throw new ArgumentException("Object is not disposed.", nameof(token));

            Enter(token);
        }

        public void Match(in StormMatchEmptyDelegate onEmpty,
                          in StormMatchErrorDelegate onError,
                          in StormMatchValueDelegate<T> onValue)
        {
            switch (_content)
            {
                case Content.Empty:
                    onEmpty();
                    break;
                case Content.Error:
                    onError(_error!);
                    break;
                case Content.Value:
                    onValue(_value!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool TryGetError([AllowNull][NotNullWhen(true)] out Exception? error)
        {
            if (_content != Content.Error)
            {
                error = null;
                return false;
            }

            error = _error;
            return true;
        }

        public bool TryGetValue([AllowNull][MaybeNull][NotNullWhen(true)] out T value)
        {
            if (_content != Content.Value)
            {
                value = default!;
                return false;
            }

            value = _value;
            return true;
        }

        protected void Enter(IStormToken token)
        {
            if (token.IsDisposed)
            {
                OnEnter?.Invoke(token);
                return;
            }

            if (_state != State.Idle)
                throw new InvalidOperationException("Can't enter now");

            _currentToken = token;
            _state = State.Entering;
            OnEnter?.Invoke(token);
            _state = State.Entered;
        }

        protected void LeaveUnchanged(IStormToken token)
        {
            if (_state != State.Entered)
                throw new InvalidOperationException("Can't leave now");

            _state = State.Leaving;
            OnLeave?.Invoke(token, false);
            _state = State.Idle;
        }

        protected void LeaveEmpty(IStormToken token)
        {
            if (_state != State.Entered)
                throw new InvalidOperationException("Can't leave now");

            var hasChanged = _content != Content.Empty;
            if (hasChanged)
            {
                _content = Content.Empty;
                _error = null;
                _value = default;
            }
            
            _state = State.Leaving;
            OnLeave?.Invoke(token, hasChanged);
            _state = State.Idle;
        }

        protected void LeaveWithError(IStormToken token, Exception error)
        {
            if (_state != State.Entered)
                throw new InvalidOperationException("Can't leave now");
            
            var hasChanged = _content != Content.Error || _error != error;
            if (hasChanged)
            {
                _content = Content.Error;
                _error = error;
                _value = default;
            }

            _state = State.Leaving;
            OnLeave?.Invoke(token, hasChanged);
            _state = State.Idle;
        }

        protected void LeaveWithValue(IStormToken token, T value)
        {
            if (_state != State.Entered)
                throw new InvalidOperationException("Can't leave now");

            var hasChanged = _content != Content.Value || _comparer == null || !_comparer.Equals(_value, value);
            if (hasChanged)
            {
                _content = Content.Value;
                _error = null;
                _value = value;
            }

            _state = State.Leaving;
            OnLeave?.Invoke(token, true);
            _state = State.Idle;
        }

        private event StormOnTokenEnterDelegate? OnEnter;

        event StormOnTokenEnterDelegate? IStorm.OnEnter
        {
            add
            {
                OnEnter += value;
                if (_state == State.Entered)
                {
                    value?.Invoke(_currentToken);
                }
            }
            remove => OnEnter -= value;
        }

        public event StormOnTokenLeaveDelegate? OnLeave;

        private enum Content
        {
            Empty,
            Error,
            Value
        }

        private enum State
        {
            Idle,
            Entering,
            Entered,
            Leaving
        }
    }
}