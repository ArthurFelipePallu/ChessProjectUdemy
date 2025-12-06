using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Bishop : Piece
{
    
    public Bishop(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        Value = 3;
        Name = "Bishop";
        ChessNotation = 'B';
        PieceType = PieceType.Bishop;
    }
}