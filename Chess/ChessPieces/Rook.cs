using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Rook : Piece
{
    
    public Rook(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        Value = 5;
        Name = "Rook";
        ChessNotation = 'R';
        PieceType = PieceType.Rook;
    }
    
}