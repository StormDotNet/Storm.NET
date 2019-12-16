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

    public abstract class StormContentBase<T> : IStormContent<T>
    {
        private readonly IEqualityComparer<T>? _comparer;

        private StormError? _error;
        [AllowNull] private T _value;

        protected StormContentBase(IEqualityComparer<T>? comparer)
        {
            _comparer = comparer;
            _error = Error.EmptyContent;
            _value = default;
            ContentType = EStormContentType.Error;
        }

        public EStormContentType ContentType { get; private set; }

        public T GetValueOr(T fallBack)
        {
            return ContentType switch
            {
                EStormContentType.Error => fallBack,
                EStormContentType.Value => _value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public T GetValueOrThrow()
        {
            return ContentType switch
            {
                EStormContentType.Error => throw _error!,
                EStormContentType.Value => _value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Match(Action<StormError> onError, Action<T> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            switch (ContentType)
            {
                case EStormContentType.Error:
                    onError(_error!);
                    break;
                case EStormContentType.Value:
                    onValue(_value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Match<TResult>(Func<StormError, TResult> onError, Func<T, TResult> onValue)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            return ContentType switch
            {
                EStormContentType.Error => onError(_error!),
                EStormContentType.Value => onValue(_value),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override string ToString() => ToStringHelper.ToString(this);

        public bool TryGetError([NotNullWhen(true)] out StormError? error)
        {
            if (ContentType != EStormContentType.Error)
            {
                error = null;
                return false;
            }

            error = _error;
            return true;
        }

        public bool TryGetValue([AllowNull] [MaybeNull] [NotNullWhen(true)] out T value)
        {
            if (ContentType != EStormContentType.Value)
            {
                value = default!;
                return false;
            }

            value = _value;
            return true;
        }

        protected bool SetError(StormError error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            if (ContentType == EStormContentType.Error && Equals(error, _error))
                return false;

            ContentType = EStormContentType.Error;
            _error = error;
            _value = default;
            return true;
        }

        protected bool SetValue(T value)
        {
            if (_comparer != null && ContentType == EStormContentType.Value && _comparer.Equals(value, _value))
                return false;

            ContentType = EStormContentType.Value;
            _error = null;
            _value = value;
            return true;
        }
    }
}