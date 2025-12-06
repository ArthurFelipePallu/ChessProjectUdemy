using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Pawn : Piece
{
    
    public Pawn(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        Value = 1;
        Name = "Pawn";
        ChessNotation = 'P';
        PieceType = PieceType.Pawn;
    }
}