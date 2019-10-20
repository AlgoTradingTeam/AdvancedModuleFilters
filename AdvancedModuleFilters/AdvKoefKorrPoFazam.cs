// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvKoefKorrPoFazam
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
  [HandlerName("KoefKorr2 (D1)")]
  [InputsCount(10)]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Input(1, TemplateTypes.BOOL, Name = "Тренд вверх")]
  [Input(2, TemplateTypes.BOOL, Name = "Тренд вверх + ADXup")]
  [Input(3, TemplateTypes.BOOL, Name = "Тренд вверх + ADXdn")]
  [Input(4, TemplateTypes.BOOL, Name = "Флет")]
  [Input(5, TemplateTypes.BOOL, Name = "Флет + ADXup")]
  [Input(6, TemplateTypes.BOOL, Name = "Флет + ADXdn")]
  [Input(7, TemplateTypes.BOOL, Name = "Тренд вниз")]
  [Input(8, TemplateTypes.BOOL, Name = "Тренд вниз + ADXup")]
  [Input(9, TemplateTypes.BOOL, Name = "Тренд вниз + ADXdn")]
  [Description("Позволяет задать числовое значение по фазам рынка. Подключается к источнику и кубикам FazyRynka2 (D1).")]
  public class AdvKoefKorrPoFazam : IOneSourceHandler, IDoubleReturns, IStreamHandler, IHandler
  {
    [HandlerParameter(Default = "5", IsShown = false, Name = "Тренд вверх", NotOptimized = true)]
    public int ParTrendUp { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "ADX > ADX[-1]", NotOptimized = true)]
    public int ParBolTrendUp { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "ADX < ADX[-1]", NotOptimized = true)]
    public int ParMenTrendUp { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "Флет", NotOptimized = true)]
    public int ParFlet { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "ADX > ADX[-1]", NotOptimized = true)]
    public int ParBolFlet { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "ADX < ADX[-1]", NotOptimized = true)]
    public int ParMenFlet { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "Тренд вниз", NotOptimized = true)]
    public int ParTrendDn { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "ADX > ADX[-1]", NotOptimized = true)]
    public int ParBolTrendDn { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Name = "ADX < ADX[-1]", NotOptimized = true)]
    public int ParMenTrendDn { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "Задать значение", NotOptimized = true)]
    public bool ParGetDefault { get; set; }

    [HandlerParameter(Default = "6", IsShown = false, Name = "По умолчанию", NotOptimized = true)]
    public int ParValue { get; set; }

    public IList<double> Execute(
      ISecurity source,
      IList<bool> TRENDup,
      IList<bool> TRENDupADXup,
      IList<bool> TRENDupADXdn,
      IList<bool> FLET,
      IList<bool> FLETADXup,
      IList<bool> FLETADXdn,
      IList<bool> TRENDdn,
      IList<bool> TRENDdnADXup,
      IList<bool> TRENDdnADXdn)
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
          if (TRENDup[index] && !TRENDupADXup[index] && !TRENDupADXdn[index])
            doubleList[index] = (double) this.ParTrendUp;
          if (TRENDupADXup[index])
            doubleList[index] = (double) this.ParBolTrendUp;
          if (TRENDupADXdn[index])
            doubleList[index] = (double) this.ParMenTrendUp;
          if (FLET[index] && !FLETADXup[index] && !FLETADXdn[index])
            doubleList[index] = (double) this.ParFlet;
          if (FLETADXup[index])
            doubleList[index] = (double) this.ParBolFlet;
          if (FLETADXdn[index])
            doubleList[index] = (double) this.ParMenFlet;
          if (TRENDdn[index] && !TRENDdnADXup[index] && !TRENDdnADXdn[index])
            doubleList[index] = (double) this.ParTrendDn;
          if (TRENDdnADXup[index])
            doubleList[index] = (double) this.ParBolTrendDn;
          if (TRENDdnADXdn[index])
            doubleList[index] = (double) this.ParMenTrendDn;
        }
      }
      return doubleList;
    }
  }
}
