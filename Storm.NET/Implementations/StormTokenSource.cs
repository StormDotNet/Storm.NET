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

    internal class StormTokenSource : IStormTokenSource
    {
        private bool _isDisposed;

        public StormTokenSource() : this(false)
        {
        }

        public StormTokenSource(bool isDisposed)
        {
            Token = new StormToken(this);
            _isDisposed = isDisposed;
        }
        
        public StormToken Token { get; }

        public event Action? Disposing
        {
            add
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(IStormTokenSource));
                OnDisposeEvent += value;
            }
            remove
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(IStormTokenSource));
                OnDisposeEvent -= value;
            }
        }

        private event Action? OnDisposeEvent;

        public void Dispose()
        {
            if (_isDisposed)
                return;

            var onDispose = OnDisposeEvent;
            _isDisposed = true;
            OnDisposeEvent = null;

            onDispose?.Invoke();
        }
    }
}