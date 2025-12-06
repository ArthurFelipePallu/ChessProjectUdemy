namespace Chess_Console_Project.Board.ChessPieces;

public class Bishop : Piece
{
    
    public Bishop(ChessBoard board, PieceColor pieceColor, Position pos) : base(board, pieceColor, pos)
    {
        Value = 3;
        Name = "Bishop";
        _chessNotation = 'B';
    }
}