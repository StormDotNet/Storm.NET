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

namespace WpfUnitConverter
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using StormDotNet;

    public class WpfStormBase : INotifyPropertyChanged
    {
        protected void Update<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return;

            field = value;
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class WpfStorm<T> : WpfStormBase
    {
        private T _value;

        public T Value
        {
            get => _value;
            set => Update(ref _value, value);
        }
    }

    public class WpfStormIo<T> : INotifyPropertyChanged
    {
        private readonly IStorm<T> _output;

        public WpfStormIo()
        {
            UserInput = Storm.Input.Create<T>();
            ModelSocket = Storm.Socket.Create<T>();

            _output = Storm.Func.FromStates.Create(UserInput, ModelSocket, Resolve);
            _output.OnVisit += OutputOnVisit;
        }

        public IStormInput<T> UserInput { get; }

        public IStormSocket<T> ModelSocket { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get => _output.TryGetValue(out var value) ? value : default;
            set => UserInput.SetValue(value);
        }

        private void OutputOnVisit(IStormToken token, EStormVisitType visitType)
        {
            if (visitType == EStormVisitType.LeaveChanged)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        private static T Resolve(StormFuncInput<T> input, StormFuncInput<T> update)
        {
            return (input.State, update.State) switch
            {
                (EStormFuncInputState.NotVisited, _) => update.Content.GetValueOr(default),
                _ => input.Content.GetValueOr(default)
            };
        }
    }
}