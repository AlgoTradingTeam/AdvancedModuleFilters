
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter VOL")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора SMA (от объема) больше или равно / меньше или равно значения индикатора SMA (от объема) умноженного на заданный коэффициент.")]
  public class AdvFilterVOL : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "VOL >= VOL*k", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "20", IsShown = false, Max = "205", Min = "5", Name = "Период", NotOptimized = false, Step = "100")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Max = "25", Min = "1", Name = "Коэффициент", NotOptimized = false, Step = "1")]
    public double ParKoef { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<double> doubleList1 = Series.SMA(source.Volumes, this.ParPeriod, (IMemoryContext) this.Context);
      IList<double> doubleList2 = (IList<double>) (this.Context?.GetArray<double>(count) ?? new double[count]);
      IList<bool> boolList = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      for (int index = 0; index < count; ++index)
      {
        doubleList2[index] = source.Bars[index].Volume;
        boolList[index] = this.ParBol ? doubleList2[index] >= doubleList1[index] * this.ParKoef : doubleList2[index] <= doubleList1[index] * this.ParKoef;
      }
      return boolList;
    }
  }
}
