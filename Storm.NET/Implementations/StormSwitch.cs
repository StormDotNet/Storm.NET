﻿// Storm.NET - Simple Topologically Ordered Reactive Model
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

    internal class StormSwitch<TResult> : StormFuncBase<TResult>
    {
        private readonly IStorm<IStorm<TResult>> _selector;
        private IStorm<TResult>? _target;
        private bool _hasTargetChanged;

        public StormSwitch(IStorm<IStorm<TResult>> selector,
                           IEqualityComparer<TResult>? comparer) : base(3, comparer)
        {
            _selector = selector;
            UpdateTarget();
            _hasTargetChanged = false;
            _selector.OnVisit += SelectorOnVisit;
        }

        protected override bool Update()
        {
            _hasTargetChanged = false;

            if (_selector.TryGetError(out var error))
                return SetError(error);

            if (_target == null)
                return SetError(Error.Switch.Disconnected);

            if (_target == this)
                return SetError(Error.Switch.Looped);

            if (_target.TryGetValue(out var targetValue))
                return SetValue(targetValue);

            if (_target.TryGetError(out var targetError))
                return SetError(targetError);
            
            return SetError(Storm.Error.Create("Unknown error"));
        }

        protected override void OnValidatedLeave(int index, bool hasChanged)
        {
            if (index == 0 && hasChanged)
            {
                UpdateTarget();
                if (GetSourceState(1) == EStormSourceState.Enter)
                {
                    SourceOnVisit(1, CurrentToken, EStormVisitType.LeaveChanged);
                }
            }

            base.OnValidatedLeave(index, hasChanged);
        }

        private void SelectorOnVisit(IStormToken token, EStormVisitType visitType)
        {
            SourceOnVisit(0, token, visitType);
        }

        private void TargetOnVisit(IStormToken token, EStormVisitType visitType)
        {
            SourceOnVisit(_hasTargetChanged ? 2 : 1, token, visitType);
        }

        private void UpdateTarget()
        {
            if (_hasTargetChanged)
                throw new InvalidOperationException("Target already changed");

            _hasTargetChanged = true;

            if (_target != null)
            {
                _target.OnVisit -= TargetOnVisit;
            }

            _selector.TryGetValue(out _target);

            if (_target != null)
            {
                if (IsDescendant(_target))
                {
                    _target = this;
                }
                else
                {
                    _target.OnVisit += TargetOnVisit;
                }
            }
        }
    }
}