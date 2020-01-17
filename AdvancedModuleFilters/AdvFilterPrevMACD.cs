

using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter PrevMACD")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора MACD больше или меньше значения индикатора предыдущего MACD.")]
  public class AdvFilterPrevMACD : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "MACD > MACD[-i]", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "12", IsShown = false, Max = "60", Min = "4", Name = "Первый период", NotOptimized = false, Step = "4")]
    public int ParPeriod1 { get; set; }

    [HandlerParameter(Default = "26", IsShown = false, Max = "130", Min = "6", Name = "Второй период", NotOptimized = false, Step = "4")]
    public int ParPeriod2 { get; set; }

    [HandlerParameter(Default = "1", IsShown = false, Max = "5", Min = "1", Name = "i =", NotOptimized = false, Step = "1")]
    public int PARi { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<double> closePrices = source.GetClosePrices(this.Context);
      IList<double> doubleList1 = Series.EMA(closePrices, this.ParPeriod1, (IMemoryContext) this.Context);
      IList<double> doubleList2 = Series.EMA(closePrices, this.ParPeriod2, (IMemoryContext) this.Context);
      double[] numArray = this.Context?.GetArray<double>(count) ?? new double[count];
      bool[] flagArray = this.Context?.GetArray<bool>(count) ?? new bool[count];
      int paRi1 = this.PARi;
      for (int paRi2 = this.PARi; paRi2 < count; ++paRi2)
      {
        numArray[paRi2] = doubleList1[paRi2] - doubleList2[paRi2];
        flagArray[paRi2] = this.ParBol ? numArray[paRi2] > numArray[paRi2 - paRi1] : numArray[paRi2] < numArray[paRi2 - paRi1];
      }
      return (IList<bool>) flagArray;
    }
  }
}
