
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter PrevCCI")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора CCI больше или меньше значения индикатора предыдущего CCI.")]
  public class AdvFilterPrevCCI : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "CCI > CCI[-i]", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "20", IsShown = false, Max = "205", Min = "5", Name = "Период", NotOptimized = false, Step = "100")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "1", IsShown = false, Max = "5", Min = "1", Name = "i =", NotOptimized = false, Step = "1")]
    public int PARi { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      IReadOnlyList<IDataBar> bars = source.Bars;
      int count = bars.Count;
      IList<double> doubleList = Series.CCI(bars, this.ParPeriod, (IMemoryContext) this.Context);
      IList<bool> boolList = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      int paRi1 = this.PARi;
      for (int paRi2 = this.PARi; paRi2 < count; ++paRi2)
        boolList[paRi2] = this.ParBol ? doubleList[paRi2] > doubleList[paRi2 - paRi1] : doubleList[paRi2] < doubleList[paRi2 - paRi1];
      return boolList;
    }
  }
}
