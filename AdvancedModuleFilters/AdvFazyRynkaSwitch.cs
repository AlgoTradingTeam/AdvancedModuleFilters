using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("FazyRynka Switch")]
  [InputsCount(2)]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Input(1, TemplateTypes.DOUBLE, Name = "Фазы рынка (единый)")]
  [Description("Переключатель по фазам рынка. На первый вход подается кубик сжатия.")]
  public class AdvFazyRynkaSwitch : IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler
  {
    [HandlerParameter(Default = "false", IsShown = false, Name = "1.Вверх + ADXup", NotOptimized = true)]
    public bool ParBolTrendUp { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "2.Вверх + ADXdn", NotOptimized = true)]
    public bool ParMenTrendUp { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "3.Флет + ADXup", NotOptimized = true)]
    public bool ParBolFlet { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "4.Флет + ADXdn", NotOptimized = true)]
    public bool ParMenFlet { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "5.Вниз + ADXup", NotOptimized = true)]
    public bool ParBolTrendDn { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "6.Вниз + ADXdn", NotOptimized = true)]
    public bool ParMenTrendDn { get; set; }

    public IList<bool> Execute(ISecurity source, IList<double> InputTrend)
    {
      int count = source.Bars.Count;
      IList<bool> boolList = (IList<bool>) new bool[count];
      for (int index = 0; index < count; ++index)
      {
        boolList[index] = false;
        if (InputTrend[index] == 1.0)
          boolList[index] = this.ParBolTrendUp;
        if (InputTrend[index] == 2.0)
          boolList[index] = this.ParMenTrendUp;
        if (InputTrend[index] == 3.0)
          boolList[index] = this.ParBolFlet;
        if (InputTrend[index] == 4.0)
          boolList[index] = this.ParMenFlet;
        if (InputTrend[index] == 5.0)
          boolList[index] = this.ParBolTrendDn;
        if (InputTrend[index] == 6.0)
          boolList[index] = this.ParMenTrendDn;
      }
      return boolList;
    }
  }
}
