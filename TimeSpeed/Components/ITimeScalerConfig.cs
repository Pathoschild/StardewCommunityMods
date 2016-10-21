using Microsoft.Xna.Framework.Input;

namespace TimeSpeed.Components
{
    public interface ITimeScalerConfig
    {
        int OutdoorTickLength { get; set; }

        int IndoorTickLength { get; set; }
        
        int MineTickLength { get; set; }

        bool ChangeTimeSpeedOnFestivalDays { get; set; }

        Keys IncreaseTickLengthKey { get; set; }

        Keys DecreaseTickLengthKey { get; set; }
    }
}
