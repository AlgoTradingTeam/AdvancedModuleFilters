using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("FazyRynka3 (cж.)")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Фильтр фазы рынка с использованием данных индикатора ADX. Подключается к сжатию.")]
  public class AdvFazyRynkaThree : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "false", IsShown = false, Name = "Вверх + ADXup", NotOptimized = true)]
    public bool ParBolTrendUp { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "Вверх + ADXdn", NotOptimized = true)]
    public bool ParMenTrendUp { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "Флет + ADXup", NotOptimized = true)]
    public bool ParBolFlet { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "Флет + ADXdn", NotOptimized = true)]
    public bool ParMenFlet { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "Вниз + ADXup", NotOptimized = true)]
    public bool ParBolTrendDn { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "Вниз + ADXdn", NotOptimized = true)]
    public bool ParMenTrendDn { get; set; }

    [HandlerParameter(Default = "14", IsShown = false, Max = "20", Min = "1", Name = "Период", NotOptimized = false, Step = "1")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "30", IsShown = false, Max = "30", Min = "5", Name = "Коэф. ADXD1", NotOptimized = false, Step = "5")]
    public double KoefADXD1 { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
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
      IList<bool> boolList3 = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      for (int index = 1; index < count; ++index)
      {
        boolList3[index] = false;
        boolList1[index] = doubleList1[index] > doubleList1[index - 1];
        boolList2[index] = doubleList1[index] < doubleList1[index - 1];
        if ((this.ParBolTrendUp || this.ParMenTrendUp) && (!this.ParBolFlet && !this.ParMenFlet) && (!this.ParBolTrendDn && !this.ParMenTrendDn))
        {
          bool resAdx = AdvFazyRynkaThree.GetResADX(this.ParBolTrendUp, this.ParMenTrendUp, closePrices[index] > doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]);
          boolList3[index] = resAdx;
        }
        else if (!this.ParBolTrendUp && !this.ParMenTrendUp && (this.ParBolFlet || this.ParMenFlet) && (!this.ParBolTrendDn && !this.ParMenTrendDn))
        {
          bool resAdx = AdvFazyRynkaThree.GetResADX(this.ParBolFlet, this.ParMenFlet, doubleList1[index] < this.KoefADXD1, boolList1[index], boolList2[index]);
          boolList3[index] = resAdx;
        }
        else if (!this.ParBolTrendUp && !this.ParMenTrendUp && (!this.ParBolFlet && !this.ParMenFlet) && (this.ParBolTrendDn || this.ParMenTrendDn))
        {
          bool resAdx = AdvFazyRynkaThree.GetResADX(this.ParBolTrendDn, this.ParMenTrendDn, closePrices[index] < doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]);
          boolList3[index] = resAdx;
        }
        if ((this.ParBolTrendUp || this.ParMenTrendUp) && (this.ParBolFlet || this.ParMenFlet) && (!this.ParBolTrendDn && !this.ParMenTrendDn))
        {
          bool resAdx1 = AdvFazyRynkaThree.GetResADX(this.ParBolTrendUp, this.ParMenTrendUp, closePrices[index] > doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]);
          bool resAdx2 = AdvFazyRynkaThree.GetResADX(this.ParBolFlet, this.ParMenFlet, doubleList1[index] < this.KoefADXD1, boolList1[index], boolList2[index]);
          boolList3[index] = resAdx1 || resAdx2;
        }
        else if ((this.ParBolTrendUp || this.ParMenTrendUp) && (!this.ParBolFlet && !this.ParMenFlet) && (this.ParBolTrendDn || this.ParMenTrendDn))
        {
          bool resAdx1 = AdvFazyRynkaThree.GetResADX(this.ParBolTrendUp, this.ParMenTrendUp, closePrices[index] > doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]);
          bool resAdx2 = AdvFazyRynkaThree.GetResADX(this.ParBolTrendDn, this.ParMenTrendDn, closePrices[index] < doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]);
          boolList3[index] = resAdx1 || resAdx2;
        }
        else if (!this.ParBolTrendUp && !this.ParMenTrendUp && (this.ParBolFlet || this.ParMenFlet) && (this.ParBolTrendDn || this.ParBolTrendDn))
        {
          bool resAdx1 = AdvFazyRynkaThree.GetResADX(this.ParBolFlet, this.ParMenFlet, doubleList1[index] < this.KoefADXD1, boolList1[index], boolList2[index]);
          bool resAdx2 = AdvFazyRynkaThree.GetResADX(this.ParBolTrendDn, this.ParMenTrendDn, closePrices[index] < doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]);
          boolList3[index] = resAdx1 || resAdx2;
        }
        else if ((this.ParBolTrendUp || this.ParMenTrendUp) && (this.ParBolFlet || this.ParMenFlet) && (this.ParBolTrendDn || this.ParMenTrendDn))
        {
          bool resAdx1 = AdvFazyRynkaThree.GetResADX(this.ParBolTrendUp, this.ParMenTrendUp, closePrices[index] > doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]);
          bool resAdx2 = AdvFazyRynkaThree.GetResADX(this.ParBolFlet, this.ParMenFlet, doubleList1[index] < this.KoefADXD1, boolList1[index], boolList2[index]);
          bool resAdx3 = AdvFazyRynkaThree.GetResADX(this.ParBolTrendDn, this.ParMenTrendDn, closePrices[index] < doubleList2[index] && doubleList1[index] > this.KoefADXD1, boolList1[index], boolList2[index]);
          boolList3[index] = resAdx1 || resAdx2 || resAdx3;
        }
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
