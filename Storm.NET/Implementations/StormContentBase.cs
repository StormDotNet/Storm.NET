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
        private bool _hasValue;

        private StormError? _error;
        [AllowNull] private T _value;

        protected StormContentBase(IEqualityComparer<T>? comparer)
        {
            _comparer = comparer;
            _error = Error.EmptyContent;
            _value = default;
            _hasValue = false;
        }

        protected StormContentBase(T initialValue, IEqualityComparer<T>? comparer)
        {
            _comparer = comparer;
            _error = null;
            _value = initialValue;
            _hasValue = true;
        }

        public TResult Match<TResult>(Func<T, TResult> onValue, Func<StormError, TResult> onError)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));

            return _hasValue ? onValue(_value) : onError(_error!);
        }

        public override string ToString() => ToStringHelper.ToString(this);

        protected bool IsError(StormError error) => !_hasValue && Equals(error, _error);

        protected void SetError(StormError error)
        {
            _hasValue = false;
            _error = error;
            _value = default;
        }

        protected bool TrySetError(StormError error)
        {
            if (IsError(error))
                return false;

            SetError(error);
            return true;
        }

        protected bool IsValue(T value) => _hasValue &&
                                           _comparer != null &&
                                           _comparer.Equals(value, _value);

        protected void SetValue(T value)
        {
            _hasValue = true;
            _error = null;
            _value = value;
        }

        protected bool TrySetValue(T value)
        {
            if (IsValue(value))
                return false;

            SetValue(value);
            return true;
        }
    }
}