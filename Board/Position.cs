using Chess_Console_Project.Board.Exceptions;

namespace Chess_Console_Project.Board;

public class Position
{
    
    private const int MaxChessBoardSize = 8;
    public int Row { get; private set; }
    public int Column { get; private set; }


    public Position(int row, int column)
    {
        SetPosition(row,column);
    }

    public void SetPosition(int row, int column)
    {
        ValidateRow(row);
        ValidateColumn(column);
    }
    
    private void ValidateRow(int row)
    {
        Row = row;
    }
    private void ValidateColumn(int col)
    {
        Column = col;
    }
    public override string ToString()
    {
        return $"L:{Row}, C:{Column}";
    }
}