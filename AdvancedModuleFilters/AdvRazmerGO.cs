// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvRazmerGO
// Assembly: AdvancedModuleFilters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2D333C3-D24A-49AB-A5A2-ABA962EADE8F
// Assembly location: C:\Users\zuzuka\AppData\Local\TSLab\TSLab 2.0\Handlers\AdvancedModuleFilters.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Razmer GO")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник1")]
  [Input(1, TemplateTypes.SECURITY, Name = "Источник2")]
  [Description("Расчет гарантийного обеспечения по формуле. ГО = Коэффициент * closeD * 0.05 * closeSI / 5000 / 10 * (1 + closeSI / 100000)")]
  public class AdvRazmerGO : ISecurityInputs, IDoubleReturns, ITwoSourcesHandler, IStreamHandler, IHandler, IContextUses
  {
    [HandlerParameter(Default = "1.7", IsShown = false, Name = "Коэффициент", NotOptimized = true)]
    public double ParKoeff { get; set; }

    [HandlerParameter(Default = "0.05", IsShown = false, Name = "Лимит, %", NotOptimized = true)]
    public double ParLimitPrc { get; set; }

    [HandlerParameter(Default = "5000", IsShown = false, Name = "Базовое ГО", NotOptimized = true)]
    public double ParBaseGO { get; set; }

    [HandlerParameter(Default = "10", IsShown = false, Name = "Мин. шаг цены", NotOptimized = true)]
    public double ParStepMin { get; set; }

    [HandlerParameter(Default = "100000", IsShown = false, Name = "Расчетная цена", NotOptimized = true)]
    public double ParCalcPrice { get; set; }

    public IContext Context { get; set; }

    public IList<double> Execute(ISecurity source1, ISecurity source2)
    {
      ISecurity security1 = source1.CompressTo(Interval.D1, 0, 1440, 600);
      ISecurity security2 = source2.CompressTo(Interval.D1, 0, 1440, 600);
      int count1 = source1.Bars.Count;
      IList<double> closePrices1 = security1.GetClosePrices(this.Context);
      IList<double> closePrices2 = security2.GetClosePrices(this.Context);
      int count2 = security1.Bars.Count;
      int count3 = security2.Bars.Count;
      IList<double> doubleList = (IList<double>) (this.Context?.GetArray<double>(count1) ?? new double[count1]);
      int index1 = 0;
      for (int index2 = 0; index2 < count1; ++index2)
      {
        DateTime date1 = source1.Bars[index2].Date;
        DateTime date2 = date1.Date;
        while (true)
        {
          int num;
          if (index1 < count2)
          {
            date1 = security2.Bars[index1].Date;
            num = date1.Date < date2 ? 1 : 0;
          }
          else
            num = 0;
          if (num != 0)
          {
            doubleList[index2] = this.ParKoeff * closePrices1[index1] * this.ParLimitPrc * closePrices2[index1] / this.ParBaseGO / this.ParStepMin * (1.0 + closePrices2[index1] / this.ParCalcPrice);
            ++index1;
          }
          else
            break;
        }
        int num1;
        if (index2 + 1 < count1)
        {
          date1 = source1.Bars[index2 + 1].Date;
          if (date1.Date == date2)
          {
            num1 = 1;
            goto label_11;
          }
        }
        num1 = index1 == count2 ? 1 : 0;
label_11:
        if (num1 != 0)
          --index1;
        if (index1 <= 0)
        {
          index1 = 0;
          doubleList[index2] = this.ParKoeff * closePrices1[index1] * this.ParLimitPrc * closePrices2[index1] / this.ParBaseGO / this.ParStepMin * (1.0 + closePrices2[index1] / this.ParCalcPrice);
        }
      }
      return doubleList;
    }
  }
}
