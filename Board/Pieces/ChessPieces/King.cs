namespace Chess_Console_Project.Board.ChessPieces;

public class King : Piece
{
    
    public King(ChessBoard board, PieceColor pieceColor, Position pos) : base(board, pieceColor, pos)
    {
        Value = 999;
        Name = "King";
        _chessNotation = 'K';
    }
    
}