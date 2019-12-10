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
    using System.Diagnostics.CodeAnalysis;

    internal class StormSocket<T> : IStormSocket<T>
    {
        private event Action<IStormToken, EStormVisitType>? OnVisitCache;
        private IStorm<T>? _target;
        public EStormContentType ContentType => _target?.ContentType ?? EStormContentType.Error;
        public IStormNode? Target => _target;

        public event Action<IStormToken, EStormVisitType>? OnVisit
        {
            add
            {
                if (_target != null)
                {
                    _target.OnVisit += value;
                }
                else
                {
                    OnVisitCache += value;
                }
            }
            remove
            {
                if (_target != null)
                {
                    _target.OnVisit -= value;
                }
                else
                {
                    OnVisitCache -= value;
                }
            }
        }

        public void Connect(IStorm<T> target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (_target != null)
                throw new InvalidOperationException("Already connected");

            if (IsDescendant(target))
                throw new InvalidOperationException("This connection create a loop");

            _target = target;
            _target.OnVisit += OnVisitCache;
            OnVisitCache = null;
        }

        public void Match(Action<StormError> onError, Action<T> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            if (_target == null)
                onError(Error.Socket.Disconnected);
            else
                _target.Match(onError, onValue);
        }

        public TResult Match<TResult>(Func<StormError, TResult> onError, Func<T, TResult> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            return _target == null ? onError(Error.Socket.Disconnected) : _target.Match(onError, onValue);
        }

        public bool TryGetError([AllowNull] [NotNullWhen(true)] out StormError? error)
        {
            if (_target == null)
            {
                error = Error.Socket.Disconnected;
                return false;
            }

            return _target.TryGetError(out error);
        }

        public bool TryGetValue([AllowNull] [MaybeNull] [NotNullWhen(true)] out T value)
        {
            if (_target == null)
            {
                value = default!;
                return false;
            }

            return _target.TryGetValue(out value);
        }

        public override string ToString() => ToStringHelper.ToString(this);

        private bool IsDescendant(IStormNode target)
        {
            if (target == this)
                return true;

            if (target is IStormSocket socket && socket.Target == this)
                return true;

            if (OnVisitCache == null)
                return false;

            var token = Storm.Token.Create();
            var hasEntered = false;

            void TargetOnVisit(IStormToken enteredToken, EStormVisitType visitType)
            {
                hasEntered |= token.Equals(enteredToken) && visitType == EStormVisitType.EnterLoopSearch;
            }

            target.OnVisit += TargetOnVisit;
            OnVisitCache.Invoke(token, EStormVisitType.EnterLoopSearch);
            OnVisitCache.Invoke(token, EStormVisitType.LeaveLoopSearch);
            target.OnVisit -= TargetOnVisit;

            return hasEntered;
        }
    }
}