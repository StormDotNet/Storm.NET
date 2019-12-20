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

    internal class ImmutableError<T> : IStorm<T>
    {
        private readonly StormError _error;

        public ImmutableError(StormError error)
        {
            _error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public EStormContentType ContentType => EStormContentType.Error;

        public event Action<StormToken, EStormVisitType>? OnVisit
        {
            add { }
            remove { }
        }

        public T GetValueOr(T fallBack) => fallBack;
        public T GetValueOrThrow() => throw _error;

        public void Match(Action<StormError> onError, Action<T> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));
            
            onError(_error);
        }

        public TResult Match<TResult>(Func<StormError, TResult> onError, Func<T, TResult> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            return onError(_error);
        }

        public bool TryGetEnteredToken(out StormToken token)
        {
            token = default;
            return false;
        }

        public bool TryGetError([NotNullWhen(true)] out StormError? error)
        {
            error = _error;
            return true;
        }

        public bool TryGetValue([AllowNull] [MaybeNull] [NotNullWhen(true)] out T value)
        {
            value = default!;
            return false;
        }

        public override string ToString() => ToStringHelper.ToString(this);
    }
}