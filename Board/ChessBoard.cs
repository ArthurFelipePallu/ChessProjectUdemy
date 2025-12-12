using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.ChessPieces;

namespace Chess_Console_Project.Board;

public class ChessBoard
{
    private Piece[,] Board { get; }
    private HashSet<Piece> _inPlayPieces;
    private HashSet<Piece> _capturedPieces;
    public int MaxChessBoardSize { get; } = 8;

    

    public ChessBoard()
    {
        _inPlayPieces = new HashSet<Piece>();
        _capturedPieces = new HashSet<Piece>();
        Board = new Piece[MaxChessBoardSize, MaxChessBoardSize];
    }
    
    /// <summary>
    /// ADD PIECE TO BOARD METHODS
    /// </summary>

    private Piece CreatePieceOfTypeAndColorAtPosition(PieceType pieceType,PieceColor color, Position position) 
    {
        var piece = CreateNewPieceOfTypeAndColorAtPosition(pieceType,color,position);
        piece.SetPiecePosition(position);
        return piece;
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

    public void AddPlayingPiece(PieceColor pieceColor , PieceType pieceType,char column,int row)
    {
        var notationPosition = new ChessNotationPosition(row, column);
        var pos = notationPosition.ToPosition();
        ValidateNewPiecePositionNotTaken(notationPosition.ToPosition());
        var piece = CreatePieceOfTypeAndColorAtPosition(pieceType,pieceColor,pos);
        _inPlayPieces.Add(piece);
        Board[pos.Row, pos.Column] = piece;
    }




    /// <summary>
    /// REMOVE PIECE METHODS
    /// </summary>
    public Piece RemovePieceFromBoardAt(Position pos)
    {
        return RemovePieceFromBoardAtCoordinates(pos.Row, pos.Column);
    }
    public Piece RemovePieceFromBoardAtCoordinates(int row,int col)
    {
        if (!HasPieceAtCoordinate(row, col))
        {
            throw new BoardException($"[CHESS BOARD] No Piece to remove at [ {row} , {col} ]");
        }

        var removedPiece = Board[row, col];
        Board[row, col] = null;
        return removedPiece;
    }

    public void RemovePieceFromPlay(Piece piece)
    {
        _inPlayPieces.Remove(piece);
        _capturedPieces.Add(piece);
    }
    
    

    /// <summary>
    /// PUT PIECES METHOD
    /// </summary>
    public void PutPieceAtDestinationPosition(Piece piece , Position destination)
    {
        PutPieceAtDestinationCoordinates(piece,destination.Row,destination.Column);
        piece.SetPiecePosition(destination);
    }
    private void PutPieceAtDestinationCoordinates(Piece piece , int row , int col)
    {
        Board[row, col] = piece;
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
    public PieceColor BoardPositionHasPieceOfColor(Position position)
    {
        var piece = AccessPieceAtPosition(position);
        return piece.GetPieceColor();
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
    public void ValidateNewPiecePositionNotTaken(Position pos)
    {
        if (HasPieceAtPosition(pos))
        {
            throw new BoardException($"[CHESS BOARD] Tried to Put a New Piece at {pos.ToString()} but there already is a piece {AccessPieceAtPosition(pos).GetPieceTypeAsString()} at this position");
        }
    }
    public void ValidateNewPiecePositionNotTaken(int row, int column)
    {
        if (HasPieceAtCoordinate(row, column))
        {
            throw new BoardException($"[CHESS BOARD] There already is a piece{AccessPieceAtCoordinates(row,column).GetPieceTypeAsString()} at this position");
        }
    }
    public void ValidateBoardPosition(Position position)
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