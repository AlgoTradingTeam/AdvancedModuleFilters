// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvOprimAMA
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
  [HandlerName("Оптим. AMA")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [OutputType(TemplateTypes.BOOL)]
  [Description("Проработка точек входа. Может использоваться в качестве фильтра по AMA и в качестве поиска точек входа в режиме отптимизации.")]
  public class AdvOprimAMA : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs
  {
    [HandlerParameter(Default = "Закрытие", IsShown = false, Name = "Тип", NotOptimized = true)]
    public TypeCandles TypeCandles { get; set; }

    [HandlerParameter(Default = "20", IsShown = false, Max = "205", Min = "5", Name = "Период", NotOptimized = false, Step = "100")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "1. AMA > AMA[-i]", NotOptimized = true)]
    public bool Par1 { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "2. AMA < AMA[-i]", NotOptimized = true)]
    public bool Par2 { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "3. AMA[-i-1] > AMA[-i]", NotOptimized = true)]
    public bool Par3 { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "4. AMA[-i-1] < AMA[-i]", NotOptimized = true)]
    public bool Par4 { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "Оптимизация вкл.", NotOptimized = true)]
    public bool ParOprim { get; set; }

    [HandlerParameter(Default = "1", IsShown = false, Max = "4", Min = "1", Name = "Режим ТВХ", NotOptimized = false, Step = "1")]
    public int ParMode { get; set; }

    [HandlerParameter(Default = "1", IsShown = false, Max = "1", Min = "1", Name = "i =", NotOptimized = false, Step = "1")]
    public int PARi { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      int count = source.Bars.Count;
      IList<double> openPrices = source.OpenPrices;
      IList<double> closePrices = source.ClosePrices;
      IList<double> highPrices = source.HighPrices;
      IList<double> lowPrices = source.LowPrices;
      IList<double> doubleList = AMA.Calc(closePrices, this.ParPeriod, (IMemoryContext) this.Context);
      this.Context?.ReleaseArray((Array) doubleList);
      if (this.TypeCandles == TypeCandles.Открытие)
        doubleList = AMA.Calc(openPrices, this.ParPeriod, (IMemoryContext) this.Context);
      else if (this.TypeCandles == TypeCandles.Максимум)
        doubleList = AMA.Calc(highPrices, this.ParPeriod, (IMemoryContext) this.Context);
      else if (this.TypeCandles == TypeCandles.Минимум)
        doubleList = AMA.Calc(lowPrices, this.ParPeriod, (IMemoryContext) this.Context);
      bool[] flagArray = this.Context?.GetArray<bool>(count) ?? new bool[count];
      int paRi1 = this.PARi;
      int num = this.PARi - 1;
      if (!this.ParOprim)
      {
        for (int paRi2 = this.PARi; paRi2 < count; ++paRi2)
        {
          if (this.Par1 && !this.Par2 && !this.Par3 && !this.Par4)
          {
            bool flag = doubleList[paRi2] > doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag;
          }
          else if (!this.Par1 && this.Par2 && !this.Par3 && !this.Par4)
          {
            bool flag = doubleList[paRi2] < doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag;
          }
          else if (!this.Par1 && !this.Par2 && this.Par3 && !this.Par4)
          {
            bool flag = paRi2 >= this.PARi && doubleList[paRi2 - num] > doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag;
          }
          else if (!this.Par1 && !this.Par2 && !this.Par3 && this.Par4)
          {
            bool flag = paRi2 >= this.PARi && doubleList[paRi2 - num] < doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag;
          }
          else if (this.Par1 && this.Par2 && !this.Par3 && !this.Par4)
          {
            bool flag1 = doubleList[paRi2] > doubleList[paRi2 - paRi1];
            bool flag2 = doubleList[paRi2] < doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag1 || flag2;
          }
          else if (this.Par1 && !this.Par2 && this.Par3 && !this.Par4)
          {
            bool flag1 = doubleList[paRi2] > doubleList[paRi2 - paRi1];
            bool flag2 = paRi2 >= this.PARi && doubleList[paRi2 - num] > doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag1 || flag2;
          }
          else if (this.Par1 && !this.Par2 && !this.Par3 && this.Par4)
          {
            bool flag1 = doubleList[paRi2] > doubleList[paRi2 - paRi1];
            bool flag2 = paRi2 >= this.PARi && doubleList[paRi2 - num] < doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag1 || flag2;
          }
          else if (!this.Par1 && this.Par2 && this.Par3 && !this.Par4)
          {
            bool flag1 = doubleList[paRi2] < doubleList[paRi2 - paRi1];
            bool flag2 = paRi2 >= this.PARi && doubleList[paRi2 - num] > doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag1 || flag2;
          }
          else if (!this.Par1 && this.Par2 && !this.Par3 && this.Par4)
          {
            bool flag1 = doubleList[paRi2] < doubleList[paRi2 - paRi1];
            bool flag2 = paRi2 >= this.PARi && doubleList[paRi2 - num] < doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag1 || flag2;
          }
          else if (!this.Par1 && !this.Par2 && this.Par3 && this.Par4)
          {
            bool flag1 = paRi2 >= this.PARi && doubleList[paRi2 - num] > doubleList[paRi2 - paRi1];
            bool flag2 = paRi2 >= this.PARi && doubleList[paRi2 - num] < doubleList[paRi2 - paRi1];
            flagArray[paRi2] = flag1 || flag2;
          }
        }
      }
      else if (this.ParOprim && (!this.Par1 && !this.Par2 && !this.Par3 && !this.Par4))
      {
        for (int paRi2 = this.PARi; paRi2 < count; ++paRi2)
        {
          flagArray[paRi2] = false;
          if (this.ParMode == 1)
            flagArray[paRi2] = doubleList[paRi2] > doubleList[paRi2 - paRi1];
          if (this.ParMode == 2)
            flagArray[paRi2] = doubleList[paRi2] < doubleList[paRi2 - paRi1];
          if (this.ParMode == 3)
            flagArray[paRi2] = paRi2 >= this.PARi && doubleList[paRi2 - num] > doubleList[paRi2 - paRi1];
          if (this.ParMode == 4)
            flagArray[paRi2] = paRi2 >= this.PARi && doubleList[paRi2 - num] < doubleList[paRi2 - paRi1];
        }
      }
      return (IList<bool>) flagArray;
    }
  }
}
