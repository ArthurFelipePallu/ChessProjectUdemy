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
        board.AddPlayingPiece(PieceColor.Black,PieceType.Rook,'a',8);
        board.AddPlayingPiece(PieceColor.Black,PieceType.Knight,'b',8);
        board.AddPlayingPiece(PieceColor.Black,PieceType.Bishop,'c',8);
        board.AddPlayingPiece(PieceColor.Black,PieceType.Queen,'d',8);
        board.AddPlayingPiece(PieceColor.Black,PieceType.King,'e',8);
        board.AddPlayingPiece(PieceColor.Black,PieceType.Bishop,'f',8);
        board.AddPlayingPiece(PieceColor.Black,PieceType.Knight,'g',8);
        board.AddPlayingPiece(PieceColor.Black,PieceType.Rook,'h',8);

        for (var i = 0; i < board.MaxChessBoardSize; i++)
        {
            board.AddPlayingPiece(PieceColor.Black,PieceType.Pawn,(char)(i+65),7);
        }
    }

    private static void CreateWhitePiecesInitialPosition(this ChessBoard board)
    {
        board.AddPlayingPiece(PieceColor.White,PieceType.Rook,'a',1);
        board.AddPlayingPiece(PieceColor.White,PieceType.Knight,'b',1);
        board.AddPlayingPiece(PieceColor.White,PieceType.Bishop,'c',1);
        board.AddPlayingPiece(PieceColor.White,PieceType.Queen,'d',1);
        board.AddPlayingPiece(PieceColor.White,PieceType.King,'e',1);
        board.AddPlayingPiece(PieceColor.White,PieceType.Bishop,'f',1);
        board.AddPlayingPiece(PieceColor.White,PieceType.Knight,'g',1);
        board.AddPlayingPiece(PieceColor.White,PieceType.Rook,'h',1);

        
        board.AddPlayingPiece(PieceColor.White,PieceType.Bishop,'c',4);
        board.AddPlayingPiece(PieceColor.White,PieceType.Queen,'f',3);
        
        for (var i = 0; i < board.MaxChessBoardSize; i++)
        {
            board.AddPlayingPiece(PieceColor.White,PieceType.Pawn,(char)(i+65),2);
        }
    }
    
    
   
    
    
    
    
}