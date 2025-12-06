using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Board;

public static class PieceExtensions
{
    public static ConsoleColor GetPieceConsoleColor(this Piece piece)
    {
        return piece.GetPieceColor() == PieceColor.Black ? ConsoleColor.Yellow : ConsoleColor.White;
    }
}