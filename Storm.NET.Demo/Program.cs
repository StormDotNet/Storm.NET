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

namespace StormDotNet.Demo
{
    using System;

    internal static class Program
    {
        private static void Main()
        {
            var a = Storm.Input.Create<int>();
            var b = Storm.Input.Create<int>();

            var f = Storm.Func.FromStates.Create(a, b, EvalF);
            f.OnVisit += (token, type) =>
            {
                Console.WriteLine($"{type, -15} {f}");
            };

            var quit = false;
            while (!quit)
            {
                var parts = Console.ReadLine()?.Split(' ');
                IStormInput<int> target = null;
                if (parts == null || parts.Length < 0)
                    continue;

                switch (parts[0])
                {
                    case "a":
                        target = a;
                        break;
                    case "b":
                        target = b;
                        break;
                    case "q":
                        quit = true;
                        break;
                }

                if (target != null)
                {
                    if (parts.Length != 2)
                    {
                        target.SetError("trop d'item");
                        continue;
                    }

                    if (int.TryParse(parts[1], out var value))
                    {
                        target.SetValue(value);
                        continue;
                    }

                    target.SetError("parse failed");
                }
            }

            Console.WriteLine("bye !");
        }

        private static string EvalF(StormFuncInput<int> firstState,
                                    StormFuncInput<int> secondState)
        {
            return $"First: {firstState}, Second: {secondState}";
        }
    }
}
