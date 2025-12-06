using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Chess.ChessPieces;

public class King : Piece
{
    
    public King(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        Value = 999;
        Name = "King";
        _chessNotation = 'K';
    }
    
}