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

    internal class StormSwitch<TResult> : StormBase<TResult>
    {
        private readonly IStorm<IStorm<TResult>> _selector;
        private readonly bool[] _entered = new bool[2];

        private IStorm<TResult>? _target;
        private State _state = State.Idle;
        private IStormToken? _token;
        private bool _anySourceHasChanged;

        public StormSwitch(IStorm<IStorm<TResult>> selector,
                           IEqualityComparer<TResult>? comparer) : base(comparer)
        {
            _selector = selector;
            UpdateTarget();

            _selector.OnEnter += OnEnterSelector;
            _selector.OnLeave += OnLeaveSelector;
        }

        private void UpdateTarget()
        {
            if (_target != null)
            {
                _target.OnEnter -= OnEnterTarget;
                _target.OnLeave -= OnLeaveTarget;
            }

            _selector.TryGetValue(out _target);
            _entered[1] = false;

            if (_target != null)
            {
                _target.OnEnter += OnEnterTarget;
                _target.OnLeave += OnLeaveTarget;
            }
        }

        private void OnEnterByIndex(int index, IStormToken token)
        {
            if (_state == State.Idle)
            {
                _state = State.Entering;
                _token = token;

                Enter(token);
            }
            else if (_state != State.Entering || _token != token)
            {
                throw new InvalidOperationException();
            }

            if (_entered[index])
                throw new InvalidOperationException();

            _entered[index] = true;
        }

        private void OnLeaveByIndex(int index, IStormToken token, bool hasChanged)
        {
            if (_token != token)
                throw new InvalidOperationException();

            if (_state == State.Entering)
            {
                _state = State.Leaving;
            }
            else if (_state != State.Leaving)
            {
                throw new InvalidOperationException();
            }

            if (!_entered[index])
                throw new InvalidOperationException();

            _entered[index] = false;
            _anySourceHasChanged |= hasChanged;

            if (_entered.Any(b => b))
                return;

            if (_anySourceHasChanged)
            {
                if (_target != null && _target.TryGetValue(out var value))
                {
                    LeaveWithValue(token, value);
                }
                else if (_selector.TryGetError(out var error) || _target != null && _target.TryGetError(out error))
                {
                    LeaveWithError(token, new Exception("Source have an error", error));
                }
                else
                {
                    LeaveEmpty(token);
                }
            }
            else
            {
                LeaveUnchanged(token);
            }

            _anySourceHasChanged = false;
            _state = State.Idle;
        }

        private void OnEnterSelector(IStormToken token) => OnEnterByIndex(0, token);

        private void OnLeaveSelector(IStormToken token, bool hasChanged)
        {
            if (hasChanged)
                UpdateTarget();

            OnLeaveByIndex(0, token, hasChanged);
        }

        private void OnEnterTarget(IStormToken token) => OnEnterByIndex(1, token);

        private void OnLeaveTarget(IStormToken token, bool hasChanged) => OnLeaveByIndex(1, token, hasChanged);

        private enum State
        {
            Idle,
            Entering,
            Leaving
        }
    }
}