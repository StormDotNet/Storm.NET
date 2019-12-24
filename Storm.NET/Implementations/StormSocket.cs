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

    internal class StormSocket<T> : IStormSocket<T>
    {
        private event Action<StormToken, EStormVisitType>? OnVisitCache;
        private IStorm<T>? _target;

        public IStorm<T>? Target => GetDeepTarget() ?? _target;

        public event Action<StormToken, EStormVisitType>? OnVisit
        {
            add
            {
                var target = GetDeepTarget();
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
                var target = GetDeepTarget();
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

        public void Connect(StormToken token, IStorm<T> target)
        {
            if (token.Equals(default)) throw new ArgumentException("Default token not allowed", nameof(token));
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (_target != null)
                throw new InvalidOperationException("Already connected");

            if (IsDescendantHelper.IsDescendant(token, this, OnVisitCache, target))
                throw new InvalidOperationException("This connection create a loop");

            var onVisitCache = OnVisitCache;
            _target = target;
            OnVisitCache = null;

            if (onVisitCache != null)
                Connect(token, onVisitCache);
        }

        public TResult Match<TResult>(Func<T, TResult> onValue, Func<StormError, TResult> onError)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            var target = GetDeepTarget();
            return target == null ? onError(Error.Socket.Disconnected) : target.Match(onValue, onError);
        }

        public StormVisitState GetVisitState(out StormToken token)
        {
            var target = GetDeepTarget();
            if (target == null)
            {
                token = default;
                return StormVisitState.Idle;
            }

            return target.GetVisitState(out token);
        }

        public override string ToString() => ToStringHelper.ToString(this);

        private void Connect(StormToken token, Action<StormToken, EStormVisitType> onVisitCache)
        {
            var target = GetDeepTarget();
            if (target == null)
            {
                _target!.OnVisit += onVisitCache;
                return;
            }

            var visitState = target.GetVisitState(out var enteredToken);
            if (visitState.IsInUpdate())
            {
                if (!token.Equals(enteredToken))
                    throw new InvalidOperationException("Unknown token");

                var handler = new MiddleConnectionHandler(target, onVisitCache);
                target.OnVisit += handler.OnVisit;
            }
            else
            {
                if (target.TryGetError(out var error) && Equals(error, Error.Socket.Disconnected))
                {
                    target.OnVisit += onVisitCache;
                }
                else
                {
                    onVisitCache.Invoke(token, EStormVisitType.EnterUpdate);
                    token.Leave += () =>
                    {
                        onVisitCache.Invoke(token, EStormVisitType.LeaveUpdateChanged);
                        target.OnVisit += onVisitCache;
                    };
                }
            }
        }

        private IStorm<T>? GetDeepTarget()
        {
            while (_target is IStormSocket<T> socket)
            {
                if (socket.Target == null)
                    return null;

                _target = socket.Target;
            }

            return _target;
        }

        private class MiddleConnectionHandler
        {
            private readonly IStorm<T> _target;
            private readonly Action<StormToken, EStormVisitType> _onVisitCache;

            public MiddleConnectionHandler(IStorm<T> target, Action<StormToken, EStormVisitType> onVisitCache)
            {
                _target = target;
                _onVisitCache = onVisitCache;
            }

            public void OnVisit(StormToken token, EStormVisitType visitType)
            {
                switch (visitType)
                {
                    case EStormVisitType.EnterUpdate:
                        _onVisitCache.Invoke(token, EStormVisitType.EnterUpdate);
                        break;
                    case EStormVisitType.LeaveUpdateUnchanged:
                    case EStormVisitType.LeaveUpdateChanged:
                        var hasChanged = !_target.TryGetError(out var error) ||
                                         !Equals(error, Error.Socket.Disconnected);

                        _onVisitCache.Invoke(
                            token,
                            hasChanged ? EStormVisitType.LeaveUpdateChanged : EStormVisitType.LeaveUpdateUnchanged);

                        _target.OnVisit -= OnVisit;
                        _target.OnVisit += _onVisitCache;
                        break;
                    case EStormVisitType.EnterLoopSearch:
                        _onVisitCache.Invoke(token, EStormVisitType.EnterLoopSearch);
                        break;
                    case EStormVisitType.LeaveLoopSearch:
                        _onVisitCache.Invoke(token, EStormVisitType.LeaveLoopSearch);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(visitType), visitType, null);
                }
            }
        }
    }
}