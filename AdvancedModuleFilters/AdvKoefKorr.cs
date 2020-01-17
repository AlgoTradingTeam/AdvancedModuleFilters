
using System.Collections.Generic;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("KoefKorr")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Коэффициент корреляции. Позволяет узнать макимальное количество одновременно открытых сделок по портфелю. Подключается к Источнику.")]
  public class AdvKoefKorr : IValuesHandler, IHandler, IOneSourceHandler, IPositionInputs, IDoubleReturns
  {
    public double Execute(ISecurity source, int barNum)
    {
      double num = 0.0;
      if (source != null)
      {
        foreach (IPosition position in (IEnumerable<IPosition>) source.Positions)
        {
          if (position.IsActive)
          {
            if (position.IsLong)
              ++num;
            else
              --num;
            if (position.IsLong && position.EntryBarNum > barNum)
              --num;
            else if (position.IsLong && position.ExitBarNum <= barNum)
              ++num;
            else if (position.IsShort && position.EntryBarNum > barNum)
              ++num;
            else if (position.IsShort && position.ExitBarNum <= barNum)
              --num;
          }
        }
      }
      source.Positions.GetLastLongPositionActive(source.Positions.BarsCount);
      source.Positions.GetLastShortPositionActive(source.Positions.BarsCount);
      IPosition longPositionClosed = source.Positions.GetLastLongPositionClosed(source.Positions.BarsCount);
      if (longPositionClosed != null && longPositionClosed.ExitBarNum > barNum)
        ++num;
      IPosition shortPositionClosed = source.Positions.GetLastShortPositionClosed(source.Positions.BarsCount);
      if (shortPositionClosed != null && shortPositionClosed.ExitBarNum > barNum)
        --num;
      return num;
    }
  }
}
