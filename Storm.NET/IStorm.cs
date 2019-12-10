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
    using System.Diagnostics.CodeAnalysis;

    public delegate void StormOnTokenEnterDelegate(IStormToken token);

    public delegate void StormOnTokenLeaveDelegate(IStormToken token, bool hasChanged);

    public interface IStorm
    {
        event StormOnTokenEnterDelegate? OnEnter;
        event StormOnTokenLeaveDelegate? OnLeave;

        /// <summary>
        /// Accept a <see cref="IStormToken"/> to graph traversal purpose.
        /// This cause <see cref="OnEnter"/> event to be raise with the passed <see cref="IStormToken"/> as argument.
        /// </summary>
        /// <param name="token">The <see cref="IStormToken"/> passed to listener.</param>
        /// <remarks>
        /// Only disposed token are accepted.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If the passed <see cref="IStormToken"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the passed <see cref="IStormToken"/> is not disposed.</exception>
        void Accept(IStormToken token);
    }

    public delegate void StormMatchEmptyDelegate();

    public delegate void StormMatchErrorDelegate(Exception exception);

    public delegate void StormMatchValueDelegate<in T>([DisallowNull] T value);

    public interface IStorm<T> : IStorm
    {
        void Match(in StormMatchEmptyDelegate onEmpty,
                   in StormMatchErrorDelegate onError,
                   in StormMatchValueDelegate<T> onValue);

        bool TryGetError([AllowNull] [NotNullWhen(true)] out Exception? error);

        bool TryGetValue([AllowNull] [MaybeNull] [NotNullWhen(true)] out T value);
    }
}