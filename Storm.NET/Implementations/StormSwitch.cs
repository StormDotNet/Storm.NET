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

        protected override void OnLeave(IStormToken token)
        {
            if (_selector.TryGetError(out var error))
            {
                LeaveWithError(token, error);
            }
            else if (_target == null)
            {
                LeaveWithError(token, Error.Switch.Disconnected);
            }
            else if (_target == this)
            {
                LeaveWithError(token, Error.Switch.Looped);
            }
            else if (_target.TryGetValue(out var targetValue))
            {
                LeaveWithValue(token, targetValue);
            }
            else if (_target.TryGetError(out var targetError))
            {
                LeaveWithError(token, targetError);
            }
            else
            {
                throw new InvalidOperationException("Unexpected state");
            }

            _hasTargetChanged = false;
        }

        protected override void OnValidatedLeave(int index, bool hasChanged)
        {
            if (index == 0 && hasChanged)
            {
                UpdateTarget();
                if (GetSourceState(1) == EStormSourceState.Enter)
                {
                    Accept(1, CurrentToken, EStormVisitType.LeaveChanged);
                }
            }

            base.OnValidatedLeave(index, hasChanged);
        }

        private void SelectorOnVisit(IStormToken token, EStormVisitType visitType)
        {
            Accept(0, token, visitType);
        }

        private void TargetOnVisit(IStormToken token, EStormVisitType visitType)
        {
            Accept(_hasTargetChanged ? 2 : 1, token, visitType);
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