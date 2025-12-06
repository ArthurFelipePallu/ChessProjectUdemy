namespace Chess_Console_Project.Board.ChessPieces;

public class Pawn : Piece
{
    
    public Pawn(ChessBoard board, PieceColor pieceColor, Position pos) : base(board, pieceColor, pos)
    {
        Value = 1;
        Name = "Pawn";
        _chessNotation = 'P';
    }
}