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
    using System;
    using System.Globalization;
    using StormDotNet;

    public class MainViewModel
    {
        private const double MeterByInch = 0.0254;

        public MainViewModel()
        {
            InchIo = new WpfStormIo<string>();
            MeterIo = new WpfStormIo<string>();

            var modelFromInch = Storm.Func.Create(InchIo.Input, s => double.Parse(s) * MeterByInch);
            var modelFromMeter = Storm.Func.Create(MeterIo.Input, double.Parse);

            var model = Storm.Func.FromStates.Create(modelFromMeter, modelFromInch, GetModelValue);

            var inchFromModel = Storm.Func.Create(model, v => Convert.ToString(v / MeterByInch, CultureInfo.CurrentCulture));
            var meterFromModel = Storm.Func.Create(model, Convert.ToString);

            InchIo.Update.Connect(inchFromModel);
            MeterIo.Update.Connect(meterFromModel);
        }

        private static double GetModelValue(StormFuncInput<double> meterValue, StormFuncInput<double> inchValue)
        {
            return (meterValue.State, inchValue.State) switch
            {
                (_, EStormFuncInputState.NotVisited) => meterValue.Content.GetValueOr(double.NaN),
                (EStormFuncInputState.NotVisited, _) => inchValue.Content.GetValueOr(double.NaN),
                _ => double.NaN
            };
        }

        public WpfStormIo<string> InchIo { get; }
        public WpfStormIo<string> MeterIo { get; }
    }
}