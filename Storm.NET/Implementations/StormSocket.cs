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

    internal class StormSocket<T> : IStormSocket<T>
    {
        private List<StormOnTokenEnterDelegate>? _enterDelegates;
        private List<StormOnTokenLeaveDelegate>? _leaveDelegates;
        private IStorm<T>? _target;

        public void Accept(IStormToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (!token.IsDisposed) throw new ArgumentException("Object is not disposed.", nameof(token));

            if (_target == null)
            {
                if (_enterDelegates != null)
                {
                    foreach (var enterDelegate in _enterDelegates)
                        enterDelegate?.Invoke(token);
                }
            }
            else
            {
                _target.Accept(token);
            }
        }

        public void Connect(IStorm<T> target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (_target != null)
                throw new InvalidOperationException("Already connected");

            if (this.IsAncestorOf(target))
                throw new InvalidOperationException("This connection create a loop");

            _target = target;

            if (_enterDelegates != null)
            {
                foreach (var enterDelegate in _enterDelegates)
                    _target.OnEnter += enterDelegate;

                _enterDelegates = null;
            }

            if (_leaveDelegates != null)
            {
                foreach (var leaveDelegate in _leaveDelegates)
                    _target.OnLeave += leaveDelegate;

                _leaveDelegates = null;
            }
        }

        public void Match(in StormMatchEmptyDelegate onEmpty,
                          in StormMatchErrorDelegate onError,
                          in StormMatchValueDelegate<T> onValue)
        {
            if (onEmpty == null) throw new ArgumentNullException(nameof(onEmpty));
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            if (_target == null)
            {
                onEmpty();
                return;
            }

            _target.Match(onEmpty, onError, onValue);
        }

        public bool TryGetError([AllowNull] [NotNullWhen(true)] out Exception? error)
        {
            if (_target == null)
            {
                error = null;
                return false;
            }

            return _target.TryGetError(out error);
        }

        public bool TryGetValue([AllowNull] [MaybeNull] [NotNullWhen(true)]
                                out T value)
        {
            if (_target == null)
            {
                value = default!;
                return false;
            }

            return _target.TryGetValue(out value);
        }

        public event StormOnTokenEnterDelegate? OnEnter
        {
            add
            {
                if (_target != null)
                {
                    _target.OnEnter += value;
                }
                else if (value != null)
                {
                    _enterDelegates ??= new List<StormOnTokenEnterDelegate>();
                    _enterDelegates.Add(value);
                }
            }
            remove
            {
                if (_target != null)
                {
                    _target.OnEnter -= value;
                }
                else if (value != null)
                {
                    _enterDelegates?.Remove(value);
                }
            }
        }

        public event StormOnTokenLeaveDelegate? OnLeave
        {
            add
            {
                if (_target != null)
                {
                    _target.OnLeave += value;
                }
                else if (value != null)
                {
                    _leaveDelegates ??= new List<StormOnTokenLeaveDelegate>();
                    _leaveDelegates.Add(value);
                }
            }
            remove
            {
                if (_target != null)
                {
                    _target.OnLeave -= value;
                }
                else if (value != null)
                {
                    _leaveDelegates?.Remove(value);
                }
            }
        }
    }
}