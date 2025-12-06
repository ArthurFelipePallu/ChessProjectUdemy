namespace Chess_Console_Project.Board;

public struct ChessNotationPosition
{
    private const int MaxChessBoardSize = 8;
    public char Col;// A - H
    public int Row;  // 1 - 8

    public ChessNotationPosition(int row, char col) 
    {
        ValidateRow(row);
        ValidateColumn(col);
    }

    private void ValidateRow(int row)
    {
        if(row is < 1 or > MaxChessBoardSize)
            throw new ArgumentOutOfRangeException($"[CHESS NOTRIFICATION POSITION] Row {row} is out of range [1 - 8] ");
        this.Row = row;
    }
    private void ValidateColumn(char col)
    {
        col = char.ToUpper(col);
        if(col is < 'A' or > 'H')
            throw new ArgumentOutOfRangeException($"[CHESS NOTRIFICATION POSITION] Column {col} is out of range [a - h] ");
        this.Col = col;
    }

    public Position ToPosition()
    {
        //VALOR ASCII de A = 65 e H = 72
        //Subtraindo 65  A = 0  e H = 7
        
        //Notação de Tabuleiro vai de 1 - 8
        //Subtraindo 1 para acessar
        //posições da matriz de 0 a 7
        return new Position(Col - 65,Row - 1 );
    }

    public override string ToString()
    {
        return $"[CHESS NOTATION POSITION[:  [{Col}{Row}]";
    }
}