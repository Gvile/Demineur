namespace Demineur.Client;

public class Cell
{
    public int Row { get; set; }
    public int Column { get; set; }
    public bool IsBomb { get; set; }
    public int Number { get; set; }
    public bool IsDiscovered { get; set; }

    public Cell(int row, int column)
    {
        Row = row;
        Column = column;
    }
}