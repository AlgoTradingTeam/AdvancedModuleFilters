// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvFilterRSI
// Assembly: AdvancedModuleFilters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2D333C3-D24A-49AB-A5A2-ABA962EADE8F
// Assembly location: C:\Users\zuzuka\AppData\Local\TSLab\TSLab 2.0\Handlers\AdvancedModuleFilters.dll

using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter RSI")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора RSI больше или меньше заданного значения.")]
  public class AdvFilterRSI : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "RSI > k", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "20", IsShown = false, Max = "205", Min = "5", Name = "Период", NotOptimized = false, Step = "100")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Max = "100", Min = "0", Name = "Значение k", NotOptimized = false, Step = "5")]
    public double ParValue { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<double> doubleList = Series.RSI(source.GetClosePrices(this.Context), this.ParPeriod, (IMemoryContext) this.Context);
      IList<bool> boolList = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      for (int index = 0; index < count; ++index)
        boolList[index] = this.ParBol ? doubleList[index] > this.ParValue : doubleList[index] < this.ParValue;
      return boolList;
    }
  }
}
