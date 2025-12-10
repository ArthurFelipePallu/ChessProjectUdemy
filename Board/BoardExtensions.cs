using Chess_Console_Project.Board.Exceptions;

namespace Chess_Console_Project.Board;

public static class BoardExtensions
{
    
    
    
    /// <summary>
    /// CREATE BOARD
    /// </summary>
    public static void CreateChessBoardInitialPosition(this ChessBoard board)
    {
        try
        {
            board.CreateBlackPiecesInitialPosition();
            board.CreateWhitePiecesInitialPosition();
        }
        catch (BoardException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void CreateBlackPiecesInitialPosition(this ChessBoard board)
    {
        board.AddBlackPieceOfTypeAtPosition(PieceType.Rook,new Position(0,0));
        board.AddBlackPieceOfTypeAtPosition(PieceType.Knight,new Position(0,1));
        board.AddBlackPieceOfTypeAtPosition(PieceType.Bishop,new Position(0,2));
        board.AddBlackPieceOfTypeAtPosition(PieceType.Queen,new Position(0,3));
        board.AddBlackPieceOfTypeAtPosition(PieceType.King,new Position(0,4));
        board.AddBlackPieceOfTypeAtPosition(PieceType.Bishop,new Position(0,5));
        board.AddBlackPieceOfTypeAtPosition(PieceType.Knight,new Position(0,6));
        board.AddBlackPieceOfTypeAtPosition(PieceType.Rook,new Position(0,7));

        for (var i = 0; i < board.MaxChessBoardSize; i++)
        {
            board.AddBlackPieceOfTypeAtPosition(PieceType.Pawn,new Position(1,i));
        }
    }

    private static void CreateWhitePiecesInitialPosition(this ChessBoard board)
    {
        board.AddWhitePieceOfTypeAtPosition(PieceType.Rook,new Position(7,0));
        board.AddWhitePieceOfTypeAtPosition(PieceType.Knight,new Position(7,1));
        board.AddWhitePieceOfTypeAtPosition(PieceType.Bishop,new Position(7,2));
        board.AddWhitePieceOfTypeAtPosition(PieceType.Queen,new Position(7,3));
        board.AddWhitePieceOfTypeAtPosition(PieceType.King,new Position(7,4));
        board.AddWhitePieceOfTypeAtPosition(PieceType.Bishop,new Position(7,5));
        board.AddWhitePieceOfTypeAtPosition(PieceType.Knight,new Position(7,6));
        board.AddWhitePieceOfTypeAtPosition(PieceType.Rook,new Position(7,7));

        for (var i = 0; i < board.MaxChessBoardSize; i++)
        {
            board.AddWhitePieceOfTypeAtPosition(PieceType.Pawn,new Position(6,i));
        }
    }
    
    
   
    
    
    
    
}