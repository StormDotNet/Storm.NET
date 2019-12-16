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

namespace StormDotNet.Tests
{
    using System;
    using NUnit.Framework;

    public abstract class StormTests : StormNodeTests
    {
        protected abstract IStorm<object> SutStorm { get; }

        protected sealed override IStormNode SutNode => SutStorm;

        [Test]
        public void VoidMatchThrownOnNullOnErrorParameter()
        {
            Assert.Throws<ArgumentNullException>(() => SutStorm.Match(null, o => { }));
        }

        [Test]
        public void VoidMatchThrownOnNullOnValueParameter()
        {
            Assert.Throws<ArgumentNullException>(() => SutStorm.Match(e => { }, null));
        }

        [Test]
        public void ValueMatchThrownOnNullOnErrorParameter()
        {
            Assert.Throws<ArgumentNullException>(() => SutStorm.Match(null, o => o));
        }

        [Test]
        public void ValueMatchThrownOnNullOnValueParameter()
        {
            Assert.Throws<ArgumentNullException>(() => SutStorm.Match(e => e, null));
        }
    }
}