using Chess_Console_Project.Board.ChessPieces;

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
        Board[position.Line, position.Column] = CreateNewPieceOfTypeAndColorAtPosition(pieceType,color,position);
    }

    private Piece CreateNewPieceOfTypeAndColorAtPosition(PieceType pieceType,PieceColor pieceColor,Position position)
    {
        switch (pieceType)
        {
            case PieceType.King:
                return new King(this, pieceColor, position);
            case PieceType.Queen:
                return new Queen(this, pieceColor, position);
            case PieceType.Bishop:
                return new Bishop(this, pieceColor, position);
            case PieceType.Knight:
                return new Knight(this, pieceColor, position);
            case PieceType.Rook:
                return new Rook(this, pieceColor, position);
            case PieceType.Pawn:
                return new Pawn(this, pieceColor, position);
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