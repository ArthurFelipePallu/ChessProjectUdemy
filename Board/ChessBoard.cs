using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.ChessPieces;

namespace Chess_Console_Project.Board;

public class ChessBoard
{
    public int MaxChessBoardSize { get; } = 8;

    private Piece[,] Board { get; }

    public ChessBoard()
    {
        Board = new Piece[MaxChessBoardSize, MaxChessBoardSize];

    }

    
    
    public void AddWhitePieceOfTypeAtPosition(PieceType pieceType, Position position)
    {
        AddPieceOfTypeAndColorAtPosition(pieceType,PieceColor.White,position);
    }
    public void AddBlackPieceOfTypeAtPosition(PieceType pieceType, Position position)
    {
        AddPieceOfTypeAndColorAtPosition(pieceType,PieceColor.Black,position);
    }
    
    private void AddPieceOfTypeAndColorAtPosition(PieceType pieceType,PieceColor color, Position position)
    {
        var piece = CreateNewPieceOfTypeAndColorAtPosition(pieceType,color,position);
        piece.SetPiecePosition(position);
        Board[position.Line, position.Column] = piece;
    }

    private Piece CreateNewPieceOfTypeAndColorAtPosition(PieceType pieceType,PieceColor pieceColor,Position position)
    {
        switch (pieceType)
        {
            case PieceType.King:
                return new King(this, pieceColor);
            case PieceType.Queen:
                return new Queen(this, pieceColor);
            case PieceType.Bishop:
                return new Bishop(this, pieceColor);
            case PieceType.Knight:
                return new Knight(this, pieceColor);
            case PieceType.Rook:
                return new Rook(this, pieceColor);
            case PieceType.Pawn:
                return new Pawn(this, pieceColor);
        }
        throw new Exception("Invalid piece type");
    }

    public Piece AccessPieceAtPosition(int x, int y)
    {
        if(x < 0 || y < 0)
            throw new IndexOutOfRangeException("[CHESSBOARD] Position can not have negative values.");
        
        if ( x >= MaxChessBoardSize  || y >= MaxChessBoardSize)
            throw new IndexOutOfRangeException("[CHESSBOARD] Position can not be greater than the board's size.");
        
        return Board[x, y];
    }
}