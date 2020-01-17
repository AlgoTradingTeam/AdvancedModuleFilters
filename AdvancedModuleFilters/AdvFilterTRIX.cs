
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter TRIX")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора TRIX больше или меньше нуля.")]
  public class AdvFilterTRIX : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "TRIX > 0", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "20", IsShown = false, Max = "205", Min = "5", Name = "Период", NotOptimized = false, Step = "100")]
    public int ParPeriod { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<double> candles1 = Series.EMA(source.GetClosePrices(this.Context), this.ParPeriod, (IMemoryContext) this.Context);
      IList<double> candles2 = Series.EMA(candles1, this.ParPeriod, (IMemoryContext) this.Context);
      this.Context?.ReleaseArray((Array) candles1);
      IList<double> doubleList1 = Series.EMA(candles2, this.ParPeriod, (IMemoryContext) this.Context);
      this.Context?.ReleaseArray((Array) candles2);
      IList<double> doubleList2 = (IList<double>) (this.Context?.GetArray<double>(count) ?? new double[count]);
      IList<bool> boolList = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      for (int index = 1; index < count; ++index)
      {
        doubleList2[index] = (doubleList1[index] - doubleList1[index - 1]) / doubleList1[index - 1];
        boolList[index] = this.ParBol ? doubleList2[index] > 0.0 : doubleList2[index] < 0.0;
      }
      return boolList;
    }
  }
}
