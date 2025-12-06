namespace Chess_Console_Project.Board.Pieces;

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
    protected PieceType _pieceType;
    public int TimesMoved {get; protected set;}



    protected Piece(ChessBoard board, PieceColor pieceColor)
    {
        PieceColor = pieceColor;
        Board = board;
        Position = null;
        TimesMoved = 0;
    }

    public void SetPiecePosition(Position position)
    {
        Position = position;
    }

    public PieceType GetPieceType()
    {
        return _pieceType;
    }
    public string GetPieceTypeAsString()
    {
        return _pieceType.ToString();
    }
    public override string ToString()
    {
        return $" {_chessNotation.ToString()} ";
    }
}