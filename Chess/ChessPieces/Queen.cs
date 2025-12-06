using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Queen : Piece
{
    
    public Queen(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        Value = 9;
        Name = "Queen";
        _chessNotation = 'Q';
    }
}