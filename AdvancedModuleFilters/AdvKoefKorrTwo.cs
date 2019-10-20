// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvKoefKorrTwo
// Assembly: AdvancedModuleFilters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2D333C3-D24A-49AB-A5A2-ABA962EADE8F
// Assembly location: C:\Users\zuzuka\AppData\Local\TSLab\TSLab 2.0\Handlers\AdvancedModuleFilters.dll

using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("KoefKorr2")]
  [InputsCount(7)]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Input(1, TemplateTypes.BOOL, Name = "1.Тренд вверх + ADXup")]
  [Input(2, TemplateTypes.BOOL, Name = "2.Тренд вверх + ADXdn")]
  [Input(3, TemplateTypes.BOOL, Name = "3.Флет + ADXup")]
  [Input(4, TemplateTypes.BOOL, Name = "4.Флет + ADXdn")]
  [Input(5, TemplateTypes.BOOL, Name = "5.Тренд вниз + ADXup")]
  [Input(6, TemplateTypes.BOOL, Name = "6.Тренд вниз + ADXdn")]
  [Description("Позволяет задать числовое значение по фазам рынка. Подключается к источнику и кубикам FazyRynka(2).")]
  public class AdvKoefKorrTwo : IOneSourceHandler, IDoubleReturns, IStreamHandler, IHandler
  {
    [HandlerParameter(Default = "5", IsShown = false, Name = "1.Вверх + ADXup", NotOptimized = true)]
    public int ParBolTrendUp { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "2.Вверх + ADXdn", NotOptimized = true)]
    public int ParMenTrendUp { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "3.Флет + ADXup", NotOptimized = true)]
    public int ParBolFlet { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "4.Флет + ADXdn", NotOptimized = true)]
    public int ParMenFlet { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "5.Вниз + ADXup", NotOptimized = true)]
    public int ParBolTrendDn { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "6.Вниз + ADXdn", NotOptimized = true)]
    public int ParMenTrendDn { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "Задать значение", NotOptimized = true)]
    public bool ParGetDefault { get; set; }

    [HandlerParameter(Default = "6", IsShown = false, Name = "По умолчанию", NotOptimized = true)]
    public int ParValue { get; set; }

    public IList<double> Execute(
      ISecurity source,
      IList<bool> TrendUpADXup,
      IList<bool> TrendUpADXdn,
      IList<bool> FletADXup,
      IList<bool> FletADXdn,
      IList<bool> TrendDnADXup,
      IList<bool> TrendDnADXdn)
    {
      int count = source.Bars.Count;
      IList<double> doubleList = (IList<double>) new double[count];
      for (int index = 0; index < count; ++index)
      {
        if (this.ParGetDefault)
        {
          doubleList[index] = (double) this.ParValue;
        }
        else
        {
          if (TrendUpADXup[index])
            doubleList[index] = (double) this.ParBolTrendUp;
          if (TrendUpADXdn[index])
            doubleList[index] = (double) this.ParMenTrendUp;
          if (FletADXup[index])
            doubleList[index] = (double) this.ParBolFlet;
          if (FletADXdn[index])
            doubleList[index] = (double) this.ParMenFlet;
          if (TrendDnADXup[index])
            doubleList[index] = (double) this.ParBolTrendDn;
          if (TrendDnADXdn[index])
            doubleList[index] = (double) this.ParMenTrendDn;
        }
      }
      return doubleList;
    }
  }
}
