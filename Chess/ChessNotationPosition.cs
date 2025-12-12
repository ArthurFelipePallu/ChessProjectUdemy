using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Chess.Exceptions;

namespace Chess_Console_Project.Board;

public struct ChessNotationPosition
{
    private const int MaxChessBoardSize = 8;
    private int _row;  // 1 - 8
    private char _col;// A - H

    public int Row { get => _row; set => _row = value; }
    public char Col { get => _col; set => _col = char.ToUpper(value); }
    public int ColAsInt => (int)_col;
    public ChessNotationPosition(int row, char col) 
    {
        ValidateRow(row);
        ValidateColumn(col);
    }


    private void ValidateRow(int row)
    {
        if(row is < 1 or > MaxChessBoardSize)
            throw new ChessException($"[CHESS NOTIFICATION POSITION] Row {row} is out of range [1 - 8] ");
        Row = row;
    }
    private void ValidateColumn(char col)
    {
        col = char.ToUpper(col);
        if(col is < 'A' or > 'H')
            throw new ChessException($"[CHESS NOTRIFICATION POSITION] Column {col} is out of range [a - h] ");
        Col = col;
    }

    public Position ToPosition()
    {
        //VALOR ASCII de A = 65 e H = 72
        //Subtraindo 65  A = 0  e H = 7
        
        //Notação de Tabuleiro vai de 1 - 8
        //Subtraindo 1 para acessar
        //posições da matriz de 0 a 7
        
        return new Position(MaxChessBoardSize -_row ,_col - 65 );
    }

    public override string ToString()
    {
        return $"{_col}{_row}".ToLower();
    }
}