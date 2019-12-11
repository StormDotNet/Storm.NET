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

        public EStormContentType ContentType => GetTarget()?.ContentType ?? EStormContentType.Error;
        public IStorm<T>? Target => GetTarget();

        public event Action<IStormToken, EStormVisitType>? OnVisit
        {
            add
            {
                var target = GetTarget();
                if (target != null)
                {
                    target.OnVisit += value;
                }
                else
                {
                    OnVisitCache += value;
                }
            }
            remove
            {
                var target = GetTarget();
                if (target != null)
                {
                    target.OnVisit -= value;
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
            GetTarget()!.OnVisit += OnVisitCache;
            OnVisitCache = null;
        }

        public T GetValueOr(T fallBack)
        {
            var target = GetTarget();
            return target == null ? fallBack : target.GetValueOr(fallBack);
        }

        public T GetValueOrThrow()
        {
            var target = GetTarget();
            return target == null ? throw Error.Socket.Disconnected : target.GetValueOrThrow();
        }

        public void Match(Action<StormError> onError, Action<T> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            var target = GetTarget();
            if (target == null)
                onError(Error.Socket.Disconnected);
            else
                target.Match(onError, onValue);
        }

        public TResult Match<TResult>(Func<StormError, TResult> onError, Func<T, TResult> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            var target = GetTarget();
            return target == null ? onError(Error.Socket.Disconnected) : target.Match(onError, onValue);
        }

        public bool TryGetError([AllowNull] [NotNullWhen(true)] out StormError? error)
        {
            var target = GetTarget();
            if (target == null)
            {
                error = Error.Socket.Disconnected;
                return false;
            }

            return target.TryGetError(out error);
        }

        public bool TryGetValue([AllowNull] [MaybeNull] [NotNullWhen(true)] out T value)
        {
            var target = GetTarget();
            if (target == null)
            {
                value = default!;
                return false;
            }

            return target.TryGetValue(out value);
        }

        public override string ToString() => ToStringHelper.ToString(this);

        private IStorm<T>? GetTarget()
        {
            while (_target is IStormSocket<T> socket && socket.Target != null)
                _target = socket.Target;

            return _target;
        }

        private bool IsDescendant(IStormNode node)
        {
            if (node == this)
                return true;

            if (node is IStormSocket<T> socket && socket.Target == this)
                return true;

            if (OnVisitCache == null)
                return false;

            var token = Storm.Token.Create();
            var hasEntered = false;

            void TargetOnVisit(IStormToken enteredToken, EStormVisitType visitType)
            {
                hasEntered |= token.Equals(enteredToken) && visitType == EStormVisitType.EnterLoopSearch;
            }

            node.OnVisit += TargetOnVisit;
            OnVisitCache.Invoke(token, EStormVisitType.EnterLoopSearch);
            OnVisitCache.Invoke(token, EStormVisitType.LeaveLoopSearch);
            node.OnVisit -= TargetOnVisit;

            return hasEntered;
        }
    }
}