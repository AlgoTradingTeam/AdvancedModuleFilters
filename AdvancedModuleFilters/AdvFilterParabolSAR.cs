
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter ParabolSAR")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Цена закрытия свечи больше или меньше значения индикатора ParabolSAR.")]
  public class AdvFilterParabolSAR : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "Close > ParabolSAR", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "0.02", IsShown = false, Max = "0.03", Min = "0.01", Name = "Нач. ускорение", NotOptimized = false, Step = "0.01")]
    public double ParStart { get; set; }

    [HandlerParameter(Default = "0.02", IsShown = false, Max = "0.2", Min = "0.01", Name = "Шаг увел. ускорения", NotOptimized = false, Step = "0.01")]
    public double ParStep { get; set; }

    [HandlerParameter(Default = "0.2", IsShown = false, Max = "0.6", Min = "0.1", Name = "Макс. ускорение", NotOptimized = false, Step = "0.1")]
    public double ParMax { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      IReadOnlyList<IDataBar> bars = source.Bars;
      int count = bars.Count;
      double[] numArray = this.Context?.GetArray<double>(count) ?? new double[count];
      IList<bool> boolList = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      double parStart = this.ParStart;
      double parStep = this.ParStep;
      double parMax = this.ParMax;
      if (count > 1)
      {
        bool flag = bars[1].High > bars[0].High || bars[1].Low > bars[0].Low;
        numArray[0] = numArray[1] = flag ? bars[0].Low : bars[0].High;
        double num1 = parStart;
        double num2 = bars[1].Low;
        double num3 = bars[1].High;
        double num4 = flag ? num3 : num2;
        for (int index = 2; index < count; ++index)
        {
          double low1 = bars[index].Low;
          double high1 = bars[index].High;
          if (flag && low1 < numArray[index - 1])
          {
            num1 = parStart;
            flag = false;
            num4 = low1;
            num2 = low1;
            numArray[index] = num3;
          }
          else if (!flag && high1 > numArray[index - 1])
          {
            num1 = parStart;
            flag = true;
            num4 = high1;
            num3 = high1;
            numArray[index] = num2;
          }
          else
          {
            double num5 = numArray[index - 1];
            double num6 = num5 + num1 * (num4 - num5);
            if (flag)
            {
              if (num4 < high1 && num1 + parStep <= parMax)
                num1 += parStep;
              if (high1 < bars[index - 1].High && index == 2)
                num6 = numArray[index - 1];
              double low2 = bars[index - 1].Low;
              if (num6 > low2)
                num6 = low2;
              double low3 = bars[index - 2].Low;
              if (num6 > low3)
                num6 = low3;
              if (num6 > low1)
              {
                num1 = parStart;
                flag = false;
                num4 = low1;
                num2 = low1;
                numArray[index] = num3;
                continue;
              }
              if (num4 < high1)
              {
                num3 = high1;
                num4 = high1;
              }
            }
            else
            {
              if (num4 > low1 && num1 + parStep <= parMax)
                num1 += parStep;
              if (low1 < bars[index - 1].Low && index == 2)
                num6 = numArray[index - 1];
              double high2 = bars[index - 1].High;
              if (num6 < high2)
                num6 = high2;
              double high3 = bars[index - 2].High;
              if (num6 < high3)
                num6 = high3;
              if (num6 < high1)
              {
                num1 = parStart;
                flag = true;
                num4 = high1;
                num3 = high1;
                numArray[index] = num2;
                continue;
              }
              if (num4 > low1)
              {
                num2 = low1;
                num4 = low1;
              }
            }
            numArray[index] = num6;
          }
        }
      }
      for (int index = 0; index < count; ++index)
        boolList[index] = this.ParBol ? bars[index].Close > numArray[index] : bars[index].Close < numArray[index];
      return boolList;
    }
  }
}
