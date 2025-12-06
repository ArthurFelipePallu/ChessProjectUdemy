using Chess_Console_Project.Board.Exceptions;

namespace Chess_Console_Project.Board;

public class Position
{
    
    private const int MaxChessBoardSize = 8;
    public int Row { get; private set; }
    public int Column { get; private set; }


    public Position(int row, int column)
    {
        ValidateRow(row);
        ValidateColumn(column);
    }
    
    private void ValidateRow(int row)
    {
        if(row is < 0 or > MaxChessBoardSize -1)
            throw new BoardException($"[POSITION] Row {row} is out of range [0 - 7] ");
        Row = row;
    }
    private void ValidateColumn(int col)
    {
        if(col is < 0 or > MaxChessBoardSize-1)
            throw new BoardException($"[POSITION] Column {col} is out of range [0 - 7] ");
        Column = col;
    }
    public override string ToString()
    {
        return "L: " + Row + ", " + "C: " + Column;
    }
}