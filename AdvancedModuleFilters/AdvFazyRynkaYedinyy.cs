// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvFazyRynkaYedinyy
// Assembly: AdvancedModuleFilters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2D333C3-D24A-49AB-A5A2-ABA962EADE8F
// Assembly location: C:\Users\zuzuka\AppData\Local\TSLab\TSLab 2.0\Handlers\AdvancedModuleFilters.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("FazyRynka Yedinyy")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Фильтр фаз рынка с использованием данных индикатора ADX. Подключается к сжатию. На выходе числовое значение.")]
  public class AdvFazyRynkaYedinyy : IBar2DoubleHandler, IOneSourceHandler, IDoubleReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "1.Вверх + ADXup", NotOptimized = true)]
    public bool ParBolTrendUp { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "2.Вверх + ADXdn", NotOptimized = true)]
    public bool ParMenTrendUp { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "3.Флет + ADXup", NotOptimized = true)]
    public bool ParBolFlet { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "4.Флет + ADXdn", NotOptimized = true)]
    public bool ParMenFlet { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "5.Вниз + ADXup", NotOptimized = true)]
    public bool ParBolTrendDn { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "6.Вниз + ADXdn", NotOptimized = true)]
    public bool ParMenTrendDn { get; set; }

    [HandlerParameter(Default = "14", IsShown = false, Max = "20", Min = "1", Name = "Период", NotOptimized = false, Step = "1")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "30", IsShown = false, Max = "30", Min = "5", Name = "Коэф. ADXD1", NotOptimized = false, Step = "5")]
    public double KoefADXD1 { get; set; }

    public IContext Context { get; set; }

    public IList<double> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<double> source1 = ADXHelper.CalcDIP(source, this.ParPeriod, (IMemoryContext) this.Context);
      IList<double> source2 = ADXHelper.CalcDIM(source, this.ParPeriod, (IMemoryContext) this.Context);
      IList<double> doubleList1 = ADXHelper.CalcADX(source1, source2, this.ParPeriod, (IMemoryContext) this.Context);
      this.Context?.ReleaseArray((Array) source1);
      this.Context?.ReleaseArray((Array) source2);
      IList<double> closePrices = source.GetClosePrices(this.Context);
      IList<double> doubleList2 = Series.EMA(closePrices, this.ParPeriod, (IMemoryContext) this.Context);
      IList<bool> boolList1 = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      IList<bool> boolList2 = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      IList<double> doubleList3 = (IList<double>) (this.Context?.GetArray<double>(count) ?? new double[count]);
      for (int index = 1; index < count; ++index)
      {
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        double num5 = 0.0;
        double num6 = 0.0;
        boolList1[index] = doubleList1[index] > doubleList1[index - 1];
        boolList2[index] = doubleList1[index] < doubleList1[index - 1];
        if (this.ParBolTrendUp)
          num1 = AdvFazyRynkaYedinyy.GetResADX(this.ParBolTrendUp, false, closePrices[index] > doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]) ? 1.0 : 0.0;
        if (this.ParMenTrendUp)
          num2 = AdvFazyRynkaYedinyy.GetResADX(false, this.ParMenTrendUp, closePrices[index] > doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]) ? 2.0 : 0.0;
        if (this.ParBolFlet)
          num3 = AdvFazyRynkaYedinyy.GetResADX(this.ParBolFlet, false, doubleList1[index] < this.KoefADXD1, boolList1[index], boolList2[index]) ? 3.0 : 0.0;
        if (this.ParMenFlet)
          num4 = AdvFazyRynkaYedinyy.GetResADX(false, this.ParMenFlet, doubleList1[index] < this.KoefADXD1, boolList1[index], boolList2[index]) ? 4.0 : 0.0;
        if (this.ParBolTrendDn)
          num5 = AdvFazyRynkaYedinyy.GetResADX(this.ParBolTrendDn, false, closePrices[index] < doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]) ? 5.0 : 0.0;
        if (this.ParMenTrendDn)
          num6 = AdvFazyRynkaYedinyy.GetResADX(false, this.ParMenTrendDn, closePrices[index] < doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]) ? 6.0 : 0.0;
        doubleList3[index] = num1 > 0.0 ? num1 : (num2 > 0.0 ? num2 : (num3 > 0.0 ? num3 : (num4 > 0.0 ? num4 : (num5 > 0.0 ? num5 : (num6 > 0.0 ? num6 : 0.0)))));
      }
      return doubleList3;
    }

    public static bool GetResADX(
      bool ParBolTrend,
      bool ParMenTrend,
      bool ResADX,
      bool ADXPrevUp,
      bool ADXPrevDn)
    {
      if (ParBolTrend && !ParMenTrend)
        return ResADX && ADXPrevUp;
      if (!ParBolTrend && ParMenTrend)
        return ResADX && ADXPrevDn;
      if (ParBolTrend && ParMenTrend)
        return ResADX && ADXPrevUp || ResADX && ADXPrevDn;
      return !ParBolTrend && !ParMenTrend ? false : false;
    }
  }
}
