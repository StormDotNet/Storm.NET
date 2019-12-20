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

namespace StormDotNet
{
    using System;
    using Implementations;

    public readonly struct StormToken
    {
        private readonly StormTokenSource _source;

        internal StormToken(StormTokenSource source)
        {
            _source = source;
        }

        public event Action? Leave
        {
            add
            {
                if (_source == null)
                    throw new ObjectDisposedException(nameof(IStormTokenSource));
                _source.Disposing += value;
            }
            remove
            {
                if (_source == null)
                    throw new ObjectDisposedException(nameof(IStormTokenSource));
                _source.Disposing -= value;
            }
        }

        public bool Equals(StormToken other) => Equals(_source, other._source);
        public override bool Equals(object? obj) => obj is StormToken other && Equals(other);
        public override int GetHashCode() => _source?.GetHashCode() ?? 0;
    }
}