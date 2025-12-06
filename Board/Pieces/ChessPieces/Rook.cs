namespace Chess_Console_Project.Board.ChessPieces;

public class Rook : Piece
{
    
    public Rook(ChessBoard board, PieceColor pieceColor, Position pos) : base(board, pieceColor, pos)
    {
        Value = 5;
        Name = "Rook";
        _chessNotation = 'R';
    }
    
}