using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Chess.Exceptions;

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
        if (row is < 0 or >= MaxChessBoardSize)
            throw new MovementException("Invalid position Row");
        Row = row;
    }
    private void ValidateColumn(int col)
    {
        if (col is < 0 or >= MaxChessBoardSize)
            throw new MovementException("Invalid position Column");
        Column = col;
    }
    public override string ToString()
    {
        return $"L:{Row}, C:{Column}";
    }
    
    public ChessNotationPosition ToChessNotationPosition()
    {
        //VALOR ASCII de A = 65 e H = 72
        //Subtraindo 65  A = 0  e H = 7
        
        //Notação de Tabuleiro vai de 1 - 8
        //Subtraindo 1 para acessar
        //posições da matriz de 0 a 7
        
        return new ChessNotationPosition( int.Abs(Row -MaxChessBoardSize) ,(char)(Column + 65) );
    }
}