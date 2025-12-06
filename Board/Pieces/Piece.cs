namespace Chess_Console_Project.Board.Pieces;

public abstract class Piece
{
    /// <summary>
    /// PRIVATE
    /// </summary>
    
    public int Value;
    public string Name;
    protected char ChessNotation;
    public Position Position;
    public ChessBoard Board;
    private PieceColor _pieceColor;
    protected PieceType PieceType;
    public int TimesMoved {get; protected set;}



    protected Piece(ChessBoard board, PieceColor pieceColor)
    {
        _pieceColor = pieceColor;
        Board = board;
        TimesMoved = 0;
    }

    public void SetPiecePosition(Position position)
    {
        Position = position;
    }

    /// <summary>
    /// PIECE TYPE
    /// </summary>
    /// <returns></returns>
    public PieceType GetPieceType()
    {
        return PieceType;
    }
    public string GetPieceTypeAsString()
    {
        return PieceType.ToString();
    }
    
    /// <summary>
    /// PIECE COLOR
    /// </summary>
    /// <returns></returns>
    public PieceColor GetPieceColor()
    {
        return _pieceColor;
    }
    public string GetPieceColroAsString()
    {
        return _pieceColor.ToString();
    }
    
    
    public override string ToString()
    {
        return $" {ChessNotation.ToString()} ";
    }
}