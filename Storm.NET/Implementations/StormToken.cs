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
    internal class StormToken : IStormToken
    {
        public StormToken()
        {
            IsDisposed = false;
        }

        public bool IsDisposed { get; private set; }
        private event StormTokenOnDisposingDelegate? Disposing;

        event StormTokenOnDisposingDelegate? IStormToken.Disposing
        {
            add
            {
                if (!IsDisposed)
                    Disposing += value;
            }
            remove => Disposing -= value;
        }

        public void Dispose()
        {
            Disposing?.Invoke(this);
            Disposing = null;
            IsDisposed = true;
        }
    }

    internal class StormTokenDisposed : IStormToken
    {
        public bool IsDisposed => true;

        public event StormTokenOnDisposingDelegate Disposing
        {
            add { }
            remove { }
        }

        public void Dispose()
        {
        }
    }
}