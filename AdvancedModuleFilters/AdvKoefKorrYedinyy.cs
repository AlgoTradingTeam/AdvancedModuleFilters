// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvKoefKorrYedinyy
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
  [HandlerName("KoefKorr Yedinyy")]
  [InputsCount(2)]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Input(1, TemplateTypes.DOUBLE, Name = "Фазы рынка (единый)")]
  [Description("Позволяет задать числовое значение коэффициента корреляции по фазам рынка. Подключается к сжатию.")]
  public class AdvKoefKorrYedinyy : IOneSourceHandler, IDoubleReturns, IStreamHandler, IHandler
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

    public IList<double> Execute(ISecurity source, IList<double> InputTrend)
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
          if (InputTrend[index] == 1.0)
            doubleList[index] = (double) this.ParBolTrendUp;
          if (InputTrend[index] == 2.0)
            doubleList[index] = (double) this.ParMenTrendUp;
          if (InputTrend[index] == 3.0)
            doubleList[index] = (double) this.ParBolFlet;
          if (InputTrend[index] == 4.0)
            doubleList[index] = (double) this.ParMenFlet;
          if (InputTrend[index] == 5.0)
            doubleList[index] = (double) this.ParBolTrendDn;
          if (InputTrend[index] == 6.0)
            doubleList[index] = (double) this.ParMenTrendDn;
        }
      }
      return doubleList;
    }
  }
}
