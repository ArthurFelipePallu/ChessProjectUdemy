namespace Chess_Console_Project.Board;

public abstract class Piece
{
    /// <summary>
    /// PRIVATE
    /// </summary>
    
    public int Value;
    public PieceColor PieceColor;
    public string Name;
    protected char _chessNotation;
    public Position Position;
    public ChessBoard Board;
    public int TimesMoved {get; protected set;}



    protected Piece(ChessBoard board, PieceColor pieceColor,Position pos)
    {
        PieceColor = pieceColor;
        Board = board;
        Position = pos;
        TimesMoved = 0;
    }

    public override string ToString()
    {
        return $" {_chessNotation.ToString()} ";
    }
}