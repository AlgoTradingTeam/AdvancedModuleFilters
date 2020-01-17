
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter MACD")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора MACD больше или меньше нуля.")]
  public class AdvFilterMACD : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "MACD > 0", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "12", IsShown = false, Max = "60", Min = "4", Name = "Первый период", NotOptimized = false, Step = "4")]
    public int ParPeriod1 { get; set; }

    [HandlerParameter(Default = "26", IsShown = false, Max = "130", Min = "6", Name = "Второй период", NotOptimized = false, Step = "4")]
    public int ParPeriod2 { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<double> closePrices = source.GetClosePrices(this.Context);
      IList<double> doubleList1 = Series.EMA(closePrices, this.ParPeriod1, (IMemoryContext) this.Context);
      IList<double> doubleList2 = Series.EMA(closePrices, this.ParPeriod2, (IMemoryContext) this.Context);
      double[] numArray = this.Context?.GetArray<double>(count) ?? new double[count];
      bool[] flagArray = this.Context?.GetArray<bool>(count) ?? new bool[count];
      for (int index = 0; index < count; ++index)
      {
        numArray[index] = doubleList1[index] - doubleList2[index];
        flagArray[index] = this.ParBol ? numArray[index] > 0.0 : numArray[index] < 0.0;
      }
      return (IList<bool>) flagArray;
    }
  }
}
