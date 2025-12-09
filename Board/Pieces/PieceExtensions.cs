using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Board;

public static class PieceExtensions
{
    public static ConsoleColor GetPieceConsoleColor(this Piece piece)
    {
        return piece.GetPieceColor() == PieceColor.Black ? ConsoleColor.Black : ConsoleColor.Yellow;
    }
    public static void PrintPiecePossibleMovesExtension(this Piece piece)
    {
        for (var i = 0; i < piece.Board.MaxChessBoardSize; i++)
        {
            piece.Board.WriteRowName(i);
            Console.ForegroundColor = ConsoleColor.White;
            for (var j = 0; j < piece.Board.MaxChessBoardSize; j++)
            {
                if(piece.IsCurrentlyAtCoordinates(i,j))
                    Console.Write(  " 0 " );
                else
                    Console.Write( piece.CoordinatesIsInPossibleMoves(i,j) ? " X " : " - " );
            }
            Console.WriteLine(); // QUEBRA LINHA
        }

        piece.Board.WriteSeparationLine();
        piece.Board.WriteColumnNames();
    }
}