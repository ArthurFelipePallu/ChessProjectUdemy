using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Board;

public static class PieceExtensions
{
    public static ConsoleColor GetPieceConsoleColor(this Piece piece)
    {
        return piece.PieceColor == PieceColor.Black ? ConsoleColor.Yellow : ConsoleColor.White;
    }
}