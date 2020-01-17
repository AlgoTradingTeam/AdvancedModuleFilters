
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("ГодОпт")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [OutputType(TemplateTypes.BOOL)]
  [Description("Выводит колонку Год (сделки) в результатах оптимизации.")]
  public class AdvOprimYear : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs
  {
    [HandlerParameter(Default = "2016", IsShown = false, Max = "2016", Min = "2008", Name = "Год", NotOptimized = false, Step = "1")]
    public int ParYear { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<bool> boolList = (IList<bool>) new bool[count];
      for (int index = 0; index < count; ++index)
      {
        DateTime date = source.Bars[index].Date;
        boolList[index] = date.Year == this.ParYear;
      }
      return boolList;
    }
  }
}
