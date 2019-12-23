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

    public static partial class StormExtensions
    {
        public static T GetValueOr<T>(this IStormContent<T> content, T fallBack)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            return content.Match(v => v, e => fallBack);
        }

        public static T GetValueOrThrow<T>(this IStormContent<T> content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            return content.Match(v => v, e => throw e);
        }

        public static void Match<T>(this IStormContent<T> content, Action<T> onValue, Action<StormError> onError)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            object? OnValue(T v)
            {
                onValue(v);
                return null;
            }

            object? OnError(StormError e)
            {
                onError(e);
                return null;
            }

            content.Match(OnValue, OnError);
        }

        public static bool TryGetError<T>(this IStormContent<T> content, [NotNullWhen(true)] out StormError? error)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var (hasError, matchError) = content.Match<(bool, StormError?)>(v => (false, null), e => (true, e));

            error = matchError;
            return hasError;
        }

        public static bool TryGetValue<T>(this IStormContent<T> content, out T value)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var (hasValue, matchValue) = content.Match<(bool, T)>(v => (true, v), e => (false, default));

            value = matchValue;
            return hasValue;
        }
    }
}