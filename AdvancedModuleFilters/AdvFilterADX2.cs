using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter ADX2")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора ADX больше Dn и меньше Up заданного значения.")]
  public class AdvFilterADX2 : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "20", IsShown = false, Max = "205", Min = "5", Name = "Период", NotOptimized = false, Step = "100")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Max = "100", Min = "0", Name = "Значение Up", NotOptimized = false, Step = "5")]
    public double ParValueUp { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Max = "100", Min = "0", Name = "Значение Dn", NotOptimized = false, Step = "5")]
    public double ParValueDn { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<double> source1 = ADXHelper.CalcDIP(source, this.ParPeriod, (IMemoryContext) this.Context);
      IList<double> source2 = ADXHelper.CalcDIM(source, this.ParPeriod, (IMemoryContext) this.Context);
      IList<double> doubleList = ADXHelper.CalcADX(source1, source2, this.ParPeriod, (IMemoryContext) this.Context);
      this.Context?.ReleaseArray((Array) source1);
      this.Context?.ReleaseArray((Array) source2);
      IList<bool> boolList = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      for (int index = 0; index < count; ++index)
      {
        int num = doubleList[index] <= this.ParValueDn ? 0 : (doubleList[index] < this.ParValueUp ? 1 : 0);
        boolList[index] = num != 0;
      }
      return boolList;
    }
  }
}
