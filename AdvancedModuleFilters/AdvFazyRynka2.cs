// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvFazyRynka2
// Assembly: AdvancedModuleFilters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2D333C3-D24A-49AB-A5A2-ABA962EADE8F
// Assembly location: C:\Users\zuzuka\AppData\Local\TSLab\TSLab 2.0\Handlers\AdvancedModuleFilters.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("FazyRynka2 (D1)")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Фильтр фазы рынка с использованием данных индикатора ADX. Использует сжатие до дневных данных внутри кубика. Подключается к источнику.")]
  public class AdvFazyRynka2 : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "Тренд вверх", NotOptimized = true)]
    public bool ParTrendUp { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "ADX > ADX[-1]", NotOptimized = true)]
    public bool ParBolTrendUp { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "ADX < ADX[-1]", NotOptimized = true)]
    public bool ParMenTrendUp { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "Флет", NotOptimized = true)]
    public bool ParFlet { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "ADX > ADX[-1]", NotOptimized = true)]
    public bool ParBolFlet { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "ADX < ADX[-1]", NotOptimized = true)]
    public bool ParMenFlet { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "Тренд вниз", NotOptimized = true)]
    public bool ParTrendDn { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "ADX > ADX[-1]", NotOptimized = true)]
    public bool ParBolTrendDn { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "ADX < ADX[-1]", NotOptimized = true)]
    public bool ParMenTrendDn { get; set; }

    [HandlerParameter(Default = "14", IsShown = false, Max = "20", Min = "1", Name = "Период", NotOptimized = false, Step = "1")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "30", IsShown = false, Max = "30", Min = "5", Name = "Коэф. ADXD1", NotOptimized = false, Step = "5")]
    public double KoefADXD1 { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      ISecurity dailySec = source.CompressTo(Interval.D1, 0, 1440, 600);
      IList<double> highPrices = dailySec.HighPrices;
      IList<double> lowPrices = dailySec.LowPrices;
      IList<double> closePrices = dailySec.ClosePrices;
      int count1 = source.Bars.Count;
      int count2 = dailySec.Bars.Count;
      IList<double> doubleList1 = (IList<double>) new double[count2];
      IList<double> doubleList2 = (IList<double>) new double[count2];
      IList<double> candles1 = (IList<double>) new double[count2];
      IList<double> candles2 = (IList<double>) new double[count2];
      IList<double> candles3 = (IList<double>) new double[count2];
      IList<double> data1 = this.Context.GetData("ATR", new string[1]
      {
        this.ParPeriod.ToString()
      }, (CacheObjectMaker<IList<double>>) (() => Series.AverageTrueRange(dailySec.Bars, this.ParPeriod)));
      IList<double> data2 = this.Context.GetData("EMA", new string[1]
      {
        this.ParPeriod.ToString()
      }, (CacheObjectMaker<IList<double>>) (() => Series.EMA(dailySec.ClosePrices, this.ParPeriod)));
      IList<double> doubleList3 = (IList<double>) new double[count1];
      IList<double> doubleList4 = (IList<double>) new double[count1];
      IList<double> doubleList5 = (IList<double>) new double[count1];
      IList<bool> boolList1 = (IList<bool>) new bool[count1];
      IList<bool> boolList2 = (IList<bool>) new bool[count1];
      IList<bool> boolList3 = (IList<bool>) new bool[count1];
      for (int index = 1; index < count2; ++index)
      {
        candles1[index] = 0.0;
        candles2[index] = 0.0;
        if (highPrices[index] > highPrices[index - 1])
          candles1[index] = highPrices[index] - highPrices[index - 1];
        if (lowPrices[index] < lowPrices[index - 1])
          candles2[index] = lowPrices[index - 1] - lowPrices[index];
        if (candles1[index] > candles2[index])
          candles2[index] = 0.0;
        else if (candles2[index] > candles1[index])
          candles1[index] = 0.0;
        else if (candles1[index] == candles2[index])
        {
          candles1[index] = 0.0;
          candles2[index] = 0.0;
        }
      }
      IList<double> doubleList6 = Series.EMA(candles1, this.ParPeriod);
      IList<double> doubleList7 = Series.EMA(candles2, this.ParPeriod);
      for (int index = 0; index < count2; ++index)
      {
        doubleList6[index] = data1[index] == 0.0 ? 0.0 : doubleList6[index] / data1[index];
        doubleList7[index] = data1[index] == 0.0 ? 0.0 : doubleList7[index] / data1[index];
        candles3[index] = doubleList6[index] != 0.0 || doubleList7[index] != 0.0 ? Math.Abs(doubleList6[index] - doubleList7[index]) / (doubleList6[index] + doubleList7[index]) * 100.0 : 0.0;
      }
      IList<double> doubleList8 = Series.EMA(candles3, this.ParPeriod);
      int index1 = 0;
      for (int index2 = 0; index2 < count1; ++index2)
      {
        DateTime date1 = source.Bars[index2].Date;
        DateTime date2 = date1.Date;
        boolList1[index2] = false;
        boolList2[index2] = false;
        while (true)
        {
          int num;
          if (index1 < count2)
          {
            date1 = dailySec.Bars[index1].Date;
            num = date1.Date < date2 ? 1 : 0;
          }
          else
            num = 0;
          if (num != 0)
          {
            doubleList3[index2] = doubleList8[index1];
            doubleList4[index2] = data2[index1];
            doubleList5[index2] = closePrices[index1];
            if ((uint) index1 > 0U)
            {
              if (doubleList8[index1] > doubleList8[index1 - 1])
                boolList1[index2] = true;
              else if (doubleList8[index1] < doubleList8[index1 - 1])
                boolList2[index2] = true;
            }
            else
            {
              boolList1[index2] = false;
              boolList2[index2] = false;
            }
            ++index1;
          }
          else
            break;
        }
        int num1;
        if (index2 + 1 < count1)
        {
          date1 = source.Bars[index2 + 1].Date;
          if (date1.Date == date2)
          {
            num1 = 1;
            goto label_33;
          }
        }
        num1 = index1 == count2 ? 1 : 0;
label_33:
        if (num1 != 0)
          --index1;
        if (index1 <= 0)
        {
          index1 = 0;
          doubleList3[index2] = doubleList8[index1];
          doubleList4[index2] = data2[index1];
          doubleList5[index2] = closePrices[index1];
          boolList1[index2] = false;
          boolList2[index2] = false;
        }
      }
      for (int index2 = 0; index2 < count1; ++index2)
      {
        if (this.ParTrendUp && !this.ParFlet && !this.ParTrendDn)
        {
          bool ResADX = doubleList5[index2] > doubleList4[index2] && doubleList3[index2] > this.KoefADXD1;
          int num = this.ParBolTrendUp ? 0 : (!this.ParMenTrendUp ? 1 : 0);
          boolList3[index2] = num == 0 ? AdvFazyRynka2.GetResADX(this.ParBolTrendUp, this.ParMenTrendUp, ResADX, boolList1[index2], boolList2[index2]) : ResADX;
        }
        else if (!this.ParTrendUp && this.ParFlet && !this.ParTrendDn)
        {
          bool ResADX = doubleList3[index2] < this.KoefADXD1;
          int num = this.ParBolFlet ? 0 : (!this.ParMenFlet ? 1 : 0);
          boolList3[index2] = num == 0 ? AdvFazyRynka2.GetResADX(this.ParBolFlet, this.ParMenFlet, ResADX, boolList1[index2], boolList2[index2]) : ResADX;
        }
        else if (!this.ParTrendUp && !this.ParFlet && this.ParTrendDn)
        {
          bool ResADX = doubleList5[index2] < doubleList4[index2] && doubleList3[index2] > this.KoefADXD1;
          int num = this.ParBolTrendDn ? 0 : (!this.ParMenTrendDn ? 1 : 0);
          boolList3[index2] = num == 0 ? AdvFazyRynka2.GetResADX(this.ParBolTrendDn, this.ParMenTrendDn, ResADX, boolList1[index2], boolList2[index2]) : ResADX;
        }
        else if (this.ParTrendUp && this.ParFlet && !this.ParTrendDn)
        {
          bool ResADX1 = doubleList5[index2] > doubleList4[index2] && doubleList3[index2] > this.KoefADXD1;
          bool flag1 = this.ParBolTrendUp || this.ParMenTrendUp ? AdvFazyRynka2.GetResADX(this.ParBolTrendUp, this.ParMenTrendUp, ResADX1, boolList1[index2], boolList2[index2]) : ResADX1;
          bool ResADX2 = doubleList3[index2] < this.KoefADXD1;
          bool flag2 = this.ParBolFlet || this.ParMenFlet ? AdvFazyRynka2.GetResADX(this.ParBolFlet, this.ParMenFlet, ResADX2, boolList1[index2], boolList2[index2]) : ResADX2;
          boolList3[index2] = flag1 || flag2;
        }
        else if (this.ParTrendUp && !this.ParFlet && this.ParTrendDn)
        {
          bool ResADX1 = doubleList5[index2] > doubleList4[index2] && doubleList3[index2] > this.KoefADXD1;
          bool flag1 = this.ParBolTrendUp || this.ParMenTrendUp ? AdvFazyRynka2.GetResADX(this.ParBolTrendUp, this.ParMenTrendUp, ResADX1, boolList1[index2], boolList2[index2]) : ResADX1;
          bool ResADX2 = doubleList5[index2] < doubleList4[index2] && doubleList3[index2] > this.KoefADXD1;
          bool flag2 = this.ParBolTrendDn || this.ParMenTrendDn ? AdvFazyRynka2.GetResADX(this.ParBolTrendDn, this.ParMenTrendDn, ResADX2, boolList1[index2], boolList2[index2]) : ResADX2;
          boolList3[index2] = flag1 || flag2;
        }
        else if (!this.ParTrendUp && this.ParFlet && this.ParTrendDn)
        {
          bool ResADX1 = doubleList3[index2] < this.KoefADXD1;
          bool flag1 = this.ParBolFlet || this.ParMenFlet ? AdvFazyRynka2.GetResADX(this.ParBolFlet, this.ParMenFlet, ResADX1, boolList1[index2], boolList2[index2]) : ResADX1;
          bool ResADX2 = doubleList5[index2] < doubleList4[index2] && doubleList3[index2] > this.KoefADXD1;
          bool flag2 = this.ParBolTrendDn || this.ParMenTrendDn ? AdvFazyRynka2.GetResADX(this.ParBolTrendDn, this.ParMenTrendDn, ResADX2, boolList1[index2], boolList2[index2]) : ResADX2;
          boolList3[index2] = flag1 || flag2;
        }
        else if (this.ParTrendUp && this.ParFlet && this.ParTrendDn)
        {
          bool ResADX1 = doubleList5[index2] > doubleList4[index2] && doubleList3[index2] > this.KoefADXD1;
          bool flag1 = this.ParBolTrendUp || this.ParMenTrendUp ? AdvFazyRynka2.GetResADX(this.ParBolTrendUp, this.ParMenTrendUp, ResADX1, boolList1[index2], boolList2[index2]) : ResADX1;
          bool ResADX2 = doubleList3[index2] < this.KoefADXD1;
          bool flag2 = this.ParBolFlet || this.ParMenFlet ? AdvFazyRynka2.GetResADX(this.ParBolFlet, this.ParMenFlet, ResADX2, boolList1[index2], boolList2[index2]) : ResADX2;
          bool ResADX3 = doubleList5[index2] < doubleList4[index2] && doubleList3[index2] > this.KoefADXD1;
          bool flag3 = this.ParBolTrendDn || this.ParMenTrendDn ? AdvFazyRynka2.GetResADX(this.ParBolTrendDn, this.ParMenTrendDn, ResADX3, boolList1[index2], boolList2[index2]) : ResADX3;
          boolList3[index2] = flag1 || flag2 || flag3;
        }
        else
          boolList3[index2] = false;
      }
      return boolList3;
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
