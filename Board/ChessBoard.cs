using Chess_Console_Project.Board.Exceptions;
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

    
    /// <summary>
    /// ADD PIECE METHODS
    /// </summary>
    public void AddWhitePieceOfTypeAtPosition(PieceType pieceType, Position position)
    {
        ValidateNewPiecePositionNotTaken(position);
        AddPieceOfTypeAndColorAtPosition(pieceType,PieceColor.White,position);
    }
    public void AddBlackPieceOfTypeAtPosition(PieceType pieceType, Position position)
    {
        ValidateNewPiecePositionNotTaken(position);
        AddPieceOfTypeAndColorAtPosition(pieceType,PieceColor.Black,position);
    }
    
    private void AddPieceOfTypeAndColorAtPosition(PieceType pieceType,PieceColor color, Position position) 
    {
        var piece = CreateNewPieceOfTypeAndColorAtPosition(pieceType,color,position);
        piece.SetPiecePosition(position);
        Board[position.Row, position.Column] = piece;
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
        throw new BoardException("[CHESS BOARD] Invalid piece type");
    }


    /// <summary>
    /// VERIFICATION METHODS
    /// </summary>
    public bool HasPieceAtChessNotationPosition(ChessNotationPosition notationPosition)
    {
        return HasPieceAtPosition(notationPosition.ToPosition());
    }
    public bool HasPieceAtPosition(Position position)
    {
        return HasPieceAtCoordinate(position.Row, position.Column);
    }
    public bool HasPieceAtCoordinate(int row, int col)
    {
        ValidateBoardCoordinates(row,col);
        
        return Board[row, col] is not null;
    }

    


    /// <summary>
    /// ACCESS METHODS
    /// </summary>
    public Piece AccessPieceAtChessNotationPosition(ChessNotationPosition notationPosition)
    {
        return AccessPieceAtPosition(notationPosition.ToPosition());
    }
    public Piece AccessPieceAtPosition(Position position)
    {
        return AccessPieceAtCoordinates(position.Row, position.Column);
    }
    public Piece AccessPieceAtCoordinates(int row, int col)
    {
        ValidateBoardCoordinates(row,col);
        
        return Board[row, col];
    }




    /// <summary>
    /// VALIDATION METHODS
    /// </summary>
    private void ValidateNewPiecePositionNotTaken(Position pos)
    {
        if (HasPieceAtPosition(pos))
        {
            throw new BoardException($"[CHESS BOARD] Tried to Put a New Piece at {pos.ToString()} but there already is a piece {AccessPieceAtPosition(pos).GetPieceTypeAsString()} at this position");
        }
    }
    private void ValidateNewPiecePositionNotTaken(int row, int column)
    {
        if (HasPieceAtCoordinate(row, column))
        {
            throw new BoardException($"[CHESS BOARD] There already is a piece{AccessPieceAtCoordinates(row,column).GetPieceTypeAsString()} at this position");
        }
    }
    private void ValidateBoardPosition(Position position)
    {
        ValidateBoardCoordinates(position.Row, position.Column);
    }
    private void ValidateBoardCoordinates(int row, int column)
    {
        if(row < 0 || column < 0)
            throw new BoardException("[CHESSBOARD] Position can not have negative values.");
        
        if ( row >= MaxChessBoardSize  || column >= MaxChessBoardSize)
            throw new BoardException("[CHESSBOARD] Position can not be greater than the board's size.");
    }
    
    
    
}