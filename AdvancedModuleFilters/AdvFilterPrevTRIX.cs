
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter PrevTRIX")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора TRIX больше или меньше значения индикатора предыдущего TRIX.")]
  public class AdvFilterPrevTRIX : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "TRIX > TRIX[-i]", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "20", IsShown = false, Max = "205", Min = "5", Name = "Период", NotOptimized = false, Step = "100")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "1", IsShown = false, Max = "5", Min = "1", Name = "i =", NotOptimized = false, Step = "1")]
    public int PARi { get; set; }

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
      int paRi1 = this.PARi;
      for (int paRi2 = this.PARi; paRi2 < count; ++paRi2)
      {
        doubleList2[paRi2] = (doubleList1[paRi2] - doubleList1[paRi2 - 1]) / doubleList1[paRi2 - 1];
        boolList[paRi2] = this.ParBol ? doubleList2[paRi2] > doubleList2[paRi2 - paRi1] : doubleList2[paRi2] < doubleList2[paRi2 - paRi1];
      }
      return boolList;
    }
  }
}
