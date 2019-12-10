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
    using System.Linq;

    internal class MultiView : IStorm
    {
        private readonly IDictionary<IStorm, bool> _enteredByStorm;
        private IStormToken _currentToken = Storm.Token.Initial;
        private State _state = State.Idle;
        private bool _hasChanged;

        public MultiView(params IStorm[] storms)
        {
            _enteredByStorm = storms.ToDictionary(m => m, m => false);

            foreach (var storm in _enteredByStorm.Keys)
            {
                storm.OnEnter += token => OnSourceEnter(storm, token);
                storm.OnLeave += (token, hasChanged) => OnSourceLeave(storm, token, hasChanged);
            }
        }

        public event StormOnTokenEnterDelegate? OnEnter;
        public event StormOnTokenLeaveDelegate? OnLeave;
        
        public void Accept(IStormToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (!token.IsDisposed) throw new ArgumentException("Object is not disposed.", nameof(token));

            OnEnter?.Invoke(token);
        }

        private void OnSourceEnter(IStorm source, IStormToken token)
        {
            if (token.IsDisposed)
            {
                OnEnter?.Invoke(token);
                return;
            }

            var known = _enteredByStorm.TryGetValue(source, out var entered);
            if (!known)
                throw new InvalidOperationException("Unknown source");
            if (entered)
                throw new InvalidOperationException("Already entered");

            switch (_state)
            {
                case State.Idle:
                    _state = State.FirstEnter;
                    _currentToken = token;
                    OnEnter?.Invoke(token);
                    _state = State.Entering;
                    break;
                case State.Entering:
                    if (_currentToken != token)
                        throw new InvalidOperationException("Unknown token");
                    break;
                default:
                    throw new InvalidOperationException("Can't enter now");
            }

            _enteredByStorm[source] = true;
        }

        private void OnSourceLeave(IStorm source, IStormToken token, bool hasChanged)
        {
            if (_currentToken != token)
                throw new InvalidOperationException("Unknown token");

            var known = _enteredByStorm.TryGetValue(source, out var entered);
            if (!known)
                throw new InvalidOperationException("Unknown source");
            if (!entered)
                throw new InvalidOperationException("Already leave");

            if (_state == State.Entering)
                _state = State.Leaving;
            else if (_state != State.Leaving)
                throw new InvalidOperationException("Can't enter now");
            
            _enteredByStorm[source] = false;
            _hasChanged |= hasChanged;

            if (_enteredByStorm.Values.Any(b => b))
                return;

            _state = State.LastLeave;
            OnLeave?.Invoke(token, _hasChanged);

            _hasChanged = false;
            _state = State.Idle;
        }

        private enum State
        {
            Idle,
            FirstEnter,
            Entering,
            Leaving,
            LastLeave
        }
    }
}