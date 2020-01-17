
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Выключатель")]
  [InputsCount(1)]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Выключатель скриптов.")]
  public class AdvScriptSwitch : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "false", IsShown = false, Name = "Выключить", NotOptimized = true)]
    public bool ParSwitch { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<bool> boolList = (IList<bool>) new bool[count];
      for (int index = 0; index < count; ++index)
      {
        DateTime date = source.Bars[index].Date;
        int num = date.Hour * 10000 + date.Minute * 100 + date.Second;
        boolList[index] = true;
        if (this.ParSwitch && (num >= 100000 && num <= 2350000))
          boolList[index] = false;
      }
      return boolList;
    }
  }
}
