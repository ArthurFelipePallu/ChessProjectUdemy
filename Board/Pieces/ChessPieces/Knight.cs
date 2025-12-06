namespace Chess_Console_Project.Board.ChessPieces;

public class Knight : Piece
{
    
    public Knight(ChessBoard board, PieceColor pieceColor, Position pos) : base(board, pieceColor, pos)
    {
        Value = 3;
        Name = "Knight";
        _chessNotation = 'N';
    }
    
}