
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter PrevADX")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора ADX больше или меньше значения индикатора предыдущего ADX.")]
  public class AdvFilterPrevADX : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "ADX > ADX[-i]", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "20", IsShown = false, Max = "205", Min = "5", Name = "Период", NotOptimized = false, Step = "100")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "1", IsShown = false, Max = "5", Min = "1", Name = "i =", NotOptimized = false, Step = "1")]
    public int PARi { get; set; }

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
      int paRi1 = this.PARi;
      for (int paRi2 = this.PARi; paRi2 < count; ++paRi2)
        boolList[paRi2] = this.ParBol ? doubleList[paRi2] > doubleList[paRi2 - paRi1] : doubleList[paRi2] < doubleList[paRi2 - paRi1];
      return boolList;
    }
  }
}
