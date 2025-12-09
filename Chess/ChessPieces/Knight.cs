using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Knight : Piece
{
    
    public Knight(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        Value = 3;
        Name = "Knight";
        ChessNotation = 'N';
        PieceType = PieceType.Knight;
    }
    public override void CalculatePossibleMoves()
    {
        throw new NotImplementedException();
    }
}