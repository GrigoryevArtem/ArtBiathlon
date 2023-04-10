namespace ArtBiathlon.Models;

public class MovingAveragesViewModel
{
    public uint Id { get; set; }
    public double  Value { get; set; }
    public double? IntervalLengthThree { get; set; }
    public double? IntervalLengthFive { get; set; }
    public double? IntervalLengthSeven { get; set; }
}