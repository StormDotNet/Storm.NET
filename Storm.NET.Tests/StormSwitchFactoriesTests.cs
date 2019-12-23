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
    using NUnit.Framework;

    [TestFixture]
    public class StormSwitchFactoriesTests
    {
        [Test]
        public void FactoriesFromFromStatesWithCompare()
        {
            Assert.That(Storm.Switch.FromStates.WithCompare.WithCompare, Is.EqualTo(Storm.Switch.FromStates.WithCompare));
            Assert.That(Storm.Switch.FromStates.WithCompare.WithoutCompare, Is.EqualTo(Storm.Switch.FromStates.WithoutCompare));
            Assert.That(Storm.Switch.FromStates.WithCompare.FromStates, Is.EqualTo(Storm.Switch.FromStates.WithCompare));
            Assert.That(Storm.Switch.FromStates.WithCompare.FromValues, Is.EqualTo(Storm.Switch.FromValues.WithCompare));
        }

        [Test]
        public void FactoriesFromFromValuesWithCompare()
        {
            Assert.That(Storm.Switch.FromValues.WithCompare.WithCompare, Is.EqualTo(Storm.Switch.FromValues.WithCompare));
            Assert.That(Storm.Switch.FromValues.WithCompare.WithoutCompare, Is.EqualTo(Storm.Switch.FromValues.WithoutCompare));
            Assert.That(Storm.Switch.FromValues.WithCompare.FromStates, Is.EqualTo(Storm.Switch.FromStates.WithCompare));
            Assert.That(Storm.Switch.FromValues.WithCompare.FromValues, Is.EqualTo(Storm.Switch.FromValues.WithCompare));
        }

        [Test]
        public void FactoriesFromFromStatesWithoutCompare()
        {
            Assert.That(Storm.Switch.FromStates.WithoutCompare.WithCompare, Is.EqualTo(Storm.Switch.FromStates.WithCompare));
            Assert.That(Storm.Switch.FromStates.WithoutCompare.WithoutCompare, Is.EqualTo(Storm.Switch.FromStates.WithoutCompare));
            Assert.That(Storm.Switch.FromStates.WithoutCompare.FromStates, Is.EqualTo(Storm.Switch.FromStates.WithoutCompare));
            Assert.That(Storm.Switch.FromStates.WithoutCompare.FromValues, Is.EqualTo(Storm.Switch.FromValues.WithoutCompare));
        }

        [Test]
        public void FactoriesFromFromValuesWithoutCompare()
        {
            Assert.That(Storm.Switch.FromValues.WithoutCompare.WithCompare, Is.EqualTo(Storm.Switch.FromValues.WithCompare));
            Assert.That(Storm.Switch.FromValues.WithoutCompare.WithoutCompare, Is.EqualTo(Storm.Switch.FromValues.WithoutCompare));
            Assert.That(Storm.Switch.FromValues.WithoutCompare.FromStates, Is.EqualTo(Storm.Switch.FromStates.WithoutCompare));
            Assert.That(Storm.Switch.FromValues.WithoutCompare.FromValues, Is.EqualTo(Storm.Switch.FromValues.WithoutCompare));
        }
    }
}