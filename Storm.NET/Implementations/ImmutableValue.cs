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

    internal class ImmutableValue<T> : IStorm<T>
    {
        private readonly T _value;

        public ImmutableValue(T value)
        {
            _value = value;
        }

        public EStormContentType ContentType => EStormContentType.Value;

        public T GetValueOr(T fallBack) => _value;

        public void Match(Action<StormError> onError, Action<T> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            onValue(_value);
        }

        public TResult Match<TResult>(Func<StormError, TResult> onError, Func<T, TResult> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            return onValue(_value);
        }

        public bool TryGetError([NotNullWhen(true)] out StormError? error)
        {
            error = null;
            return false;
        }

        public bool TryGetValue(out T value)
        {
            value = _value;
            return true;
        }

        public event Action<IStormToken, EStormVisitType>? OnVisit
        {
            add { }
            remove { }
        }

        public override string ToString() => ToStringHelper.ToString(this);
    }
}