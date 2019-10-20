// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvOprimYear
// Assembly: AdvancedModuleFilters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2D333C3-D24A-49AB-A5A2-ABA962EADE8F
// Assembly location: C:\Users\zuzuka\AppData\Local\TSLab\TSLab 2.0\Handlers\AdvancedModuleFilters.dll

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
