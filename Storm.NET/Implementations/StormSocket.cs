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

            if (IsDescendant(target))
                throw new InvalidOperationException("This connection create a loop");

            _target = target;
            Connect(token);
        }

        public TResult Match<TResult>(Func<T, TResult> onValue, Func<StormError, TResult> onError)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            var target = GetDeepTarget();
            return target == null ? onError(Error.Socket.Disconnected) : target.Match(onValue, onError);
        }

        public bool TryGetEnteredToken(out StormToken token)
        {
            var target = GetDeepTarget();
            if (target == null)
            {
                token = default;
                return false;
            }

            return target.TryGetEnteredToken(out token);
        }

        public override string ToString() => ToStringHelper.ToString(this);

        private void Connect(StormToken token)
        {
            var target = GetDeepTarget();
            if (target == null)
            {
                _target!.OnVisit += OnVisitCache;
                OnVisitCache = null;
                return;
            }

            var onVisit = OnVisitCache;
            if (onVisit == null)
                return;

            var isEntered = target.TryGetEnteredToken(out var enteredToken);
            if (isEntered)
            {
                if (!token.Equals(enteredToken))
                    throw new InvalidOperationException("Unknown token");
                target.OnVisit += OnMiddleConnectionVisit;
            }
            else
            {
                if (target.TryGetError(out var error) && Equals(error, Error.Socket.Disconnected))
                {
                    target.OnVisit += OnVisitCache;
                    OnVisitCache = null;
                }
                else
                {
                    onVisit.Invoke(token, EStormVisitType.UpdateEnter);
                    token.Leave += () =>
                    {
                        onVisit.Invoke(token, EStormVisitType.UpdateLeaveChanged);
                        target.OnVisit += OnVisitCache;
                        OnVisitCache = null;
                    };
                }
            }
        }

        private bool IsDescendant(IStormNode node)
        {
            if (node == this)
                return true;

            if (node is IStormSocket<T> socket && socket.Target == this)
                return true;

            if (OnVisitCache == null)
                return false;

            var tokenSource = Storm.TokenSource.CreateDisposedSource();
            var token = tokenSource.Token;
            var hasEntered = false;

            void TargetOnVisit(StormToken enteredToken, EStormVisitType visitType)
            {
                hasEntered |= token.Equals(enteredToken) && visitType == EStormVisitType.LoopSearchEnter;
            }

            node.OnVisit += TargetOnVisit;
            OnVisitCache.Invoke(token, EStormVisitType.LoopSearchEnter);
            OnVisitCache.Invoke(token, EStormVisitType.LoopSearchLeave);
            node.OnVisit -= TargetOnVisit;

            return hasEntered;
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

        private void OnMiddleConnectionVisit(StormToken token, EStormVisitType visitType)
        {
            var onVisit = OnVisitCache;
            var target = GetDeepTarget()!;
            
            switch (visitType)
            {
                case EStormVisitType.UpdateEnter:
                    onVisit?.Invoke(token, EStormVisitType.UpdateEnter);
                    break;
                case EStormVisitType.UpdateLeaveChanged:
                case EStormVisitType.UpdateLeaveUnchanged:
                    var isDisconnected = target.TryGetError(out var error) && Equals(error, Error.Socket.Disconnected);
                    onVisit?.Invoke( token, isDisconnected ? EStormVisitType.UpdateLeaveUnchanged : EStormVisitType.UpdateLeaveChanged);

                    target.OnVisit += OnVisitCache;
                    OnVisitCache = null;
                    break;
                case EStormVisitType.LoopSearchEnter:
                    OnVisitCache?.Invoke(token, EStormVisitType.LoopSearchEnter);
                    break;
                case EStormVisitType.LoopSearchLeave:
                    OnVisitCache?.Invoke(token, EStormVisitType.LoopSearchLeave);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(visitType), visitType, null);
            }
        }
    }
}