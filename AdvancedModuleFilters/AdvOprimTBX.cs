// Decompiled with JetBrains decompiler
// Type: TSLab.AdvancedModuleFilters.AdvOprimTBX
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
  [HandlerName("Оптим. ТВХ")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [OutputType(TemplateTypes.BOOL)]
  [Description("Проработка точек входа. Может использоваться в качестве фильтра по ТВХ и в качестве поиска лучших точек входа в режиме отптимизации.")]
  public class AdvOprimTBX : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs
  {
    [HandlerParameter(Default = "false", IsShown = false, Name = "1. Close > Open", NotOptimized = true)]
    public bool Par1 { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "2. Close < Open", NotOptimized = true)]
    public bool Par2 { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "3. Close[-i] > Open[-i]", NotOptimized = true)]
    public bool Par3 { get; set; }

    [HandlerParameter(Default = "false", IsShown = false, Name = "4. Close[-i] < Open[-i]", NotOptimized = true)]
    public bool Par4 { get; set; }

    [HandlerParameter(Default = "true", IsShown = false, Name = "Оптимизация вкл.", NotOptimized = true)]
    public bool ParOprim { get; set; }

    [HandlerParameter(Default = "4", IsShown = false, Max = "4", Min = "1", Name = "Режим ТВХ", NotOptimized = false, Step = "1")]
    public int ParMode { get; set; }

    [HandlerParameter(Default = "1", IsShown = false, Max = "1", Min = "1", Name = "i =", NotOptimized = false, Step = "1")]
    public int PARi { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      IList<bool> boolList = (IList<bool>) new bool[source.Bars.Count];
      if (!this.ParOprim)
        boolList = AdvOprimTBX.GetFilter(source, this.Par1, this.Par2, this.Par3, this.Par4, this.PARi);
      else if (this.ParOprim && (!this.Par1 && !this.Par2 && !this.Par3 && !this.Par4))
        boolList = AdvOprimTBX.GetOprim(source, this.ParMode, this.PARi);
      return boolList;
    }

    public static IList<bool> GetFilter(
      ISecurity source,
      bool Par1,
      bool Par2,
      bool Par3,
      bool Par4,
      int PARi)
    {
      IList<double> openPrices = source.OpenPrices;
      IList<double> closePrices = source.ClosePrices;
      int count = source.Bars.Count;
      IList<bool> boolList = (IList<bool>) new bool[count];
      int num = PARi;
      for (int index = 0; index < count; ++index)
      {
        if (Par1 && !Par2 && !Par3 && !Par4)
        {
          bool flag = closePrices[index] > openPrices[index];
          boolList[index] = flag;
        }
        else if (!Par1 && Par2 && !Par3 && !Par4)
        {
          bool flag = closePrices[index] < openPrices[index];
          boolList[index] = flag;
        }
        else if (!Par1 && !Par2 && Par3 && !Par4)
        {
          bool flag = index >= PARi && closePrices[index - num] > openPrices[index - num];
          boolList[index] = flag;
        }
        else if (!Par1 && !Par2 && !Par3 && Par4)
        {
          bool flag = index >= PARi && closePrices[index - num] < openPrices[index - num];
          boolList[index] = flag;
        }
        else if (Par1 && Par2 && !Par3 && !Par4)
        {
          bool flag1 = closePrices[index] > openPrices[index];
          bool flag2 = closePrices[index] < openPrices[index];
          boolList[index] = flag1 || flag2;
        }
        else if (Par1 && !Par2 && Par3 && !Par4)
        {
          bool flag1 = closePrices[index] > openPrices[index];
          bool flag2 = index >= PARi && closePrices[index - num] > openPrices[index - num];
          boolList[index] = flag1 || flag2;
        }
        else if (Par1 && !Par2 && !Par3 && Par4)
        {
          bool flag1 = closePrices[index] > openPrices[index];
          bool flag2 = index >= PARi && closePrices[index - num] < openPrices[index - num];
          boolList[index] = flag1 || flag2;
        }
        else if (!Par1 && Par2 && Par3 && !Par4)
        {
          bool flag1 = closePrices[index] < openPrices[index];
          bool flag2 = index >= PARi && closePrices[index - num] > openPrices[index - num];
          boolList[index] = flag1 || flag2;
        }
        else if (!Par1 && Par2 && !Par3 && Par4)
        {
          bool flag1 = closePrices[index] < openPrices[index];
          bool flag2 = index >= PARi && closePrices[index - num] < openPrices[index - num];
          boolList[index] = flag1 || flag2;
        }
        else if (!Par1 && !Par2 && Par3 && Par4)
        {
          bool flag1 = index >= PARi && closePrices[index - num] > openPrices[index - num];
          bool flag2 = index >= PARi && closePrices[index - num] < openPrices[index - num];
          boolList[index] = flag1 || flag2;
        }
      }
      return boolList;
    }

    public static IList<bool> GetOprim(ISecurity source, int ParMode, int PARi)
    {
      IList<double> openPrices = source.OpenPrices;
      IList<double> closePrices = source.ClosePrices;
      int count = source.Bars.Count;
      IList<bool> boolList = (IList<bool>) new bool[count];
      int num = PARi;
      for (int index = 0; index < count; ++index)
      {
        boolList[index] = false;
        if (ParMode == 1)
          boolList[index] = closePrices[index] > openPrices[index];
        if (ParMode == 2)
          boolList[index] = closePrices[index] < openPrices[index];
        if (ParMode == 3)
          boolList[index] = index >= PARi && closePrices[index - num] > openPrices[index - num];
        if (ParMode == 4)
          boolList[index] = index >= PARi && closePrices[index - num] < openPrices[index - num];
      }
      return boolList;
    }
  }
}
