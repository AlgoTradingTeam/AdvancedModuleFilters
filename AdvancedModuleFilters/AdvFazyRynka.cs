// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvFazyRynka
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
  [HandlerName("FazyRynka")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Фильтр фазы рынка. Подключается к сжатию.")]
  public class AdvFazyRynka : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "Тренд вверх", NotOptimized = true)]
    public bool ParTrendUp { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "Флет", NotOptimized = true)]
    public bool ParFlet { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "Тренд вниз", NotOptimized = true)]
    public bool ParTrendDn { get; set; }

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
      IList<bool> boolList = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      for (int index = 0; index < count; ++index)
        boolList[index] = !this.ParTrendUp || this.ParFlet || this.ParTrendDn ? (this.ParTrendUp || !this.ParFlet || this.ParTrendDn ? (this.ParTrendUp || this.ParFlet || !this.ParTrendDn ? (!this.ParTrendUp || !this.ParFlet || this.ParTrendDn ? (!this.ParTrendUp || this.ParFlet || !this.ParTrendDn ? (this.ParTrendUp || !this.ParFlet || !this.ParTrendDn ? this.ParTrendUp && this.ParFlet && this.ParTrendDn && (closePrices[index] > doubleList2[index] && doubleList1[index] > this.KoefADXD1 || doubleList1[index] < this.KoefADXD1 || closePrices[index] < doubleList2[index] && doubleList1[index] > this.KoefADXD1) : doubleList1[index] < this.KoefADXD1 || closePrices[index] < doubleList2[index] && doubleList1[index] > this.KoefADXD1) : doubleList1[index] > this.KoefADXD1) : closePrices[index] > doubleList2[index] && doubleList1[index] > this.KoefADXD1 || doubleList1[index] < this.KoefADXD1) : closePrices[index] < doubleList2[index] && doubleList1[index] > this.KoefADXD1) : doubleList1[index] < this.KoefADXD1) : closePrices[index] > doubleList2[index] && doubleList1[index] > this.KoefADXD1;
      return boolList;
    }
  }
}
