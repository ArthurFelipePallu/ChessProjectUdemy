using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Chess.Exceptions;
using Chess_Console_Project.Chess.ChessPieces;

namespace Chess_Console_Project.Board;

public class ChessBoard
{

    private Piece[,] Board { get; }
    private HashSet<Piece> _chessPieces;
    private HashSet<Piece> _capturedPieces;
    

    private bool[,] AllTargetedSquares;
    public int MaxChessBoardSize { get; } = 8;
    public Piece LastMovedPiece { get; private set; }

    public ChessBoard()
    {
        _chessPieces = [];
        _capturedPieces = [];
        Board = new Piece[MaxChessBoardSize, MaxChessBoardSize];
        AllTargetedSquares = new bool[MaxChessBoardSize, MaxChessBoardSize];
    }
    
    /// <summary>
    /// ADD PIECE TO BOARD METHODS
    /// </summary>
    public void AddPlayingPiece(PieceColor pieceColor , PieceType pieceType,char column,int row)
    {
        var notationPosition = new ChessNotationPosition(row, column);
        ValidateNewPieceChessNotationPositionNotTaken(notationPosition);
        var piece = CreatePieceOfTypeAndColorAtPosition(pieceType,pieceColor,notationPosition);
        _chessPieces.Add(piece);
        Board[notationPosition.RowIndex, notationPosition.ColumnIndex] = piece;
    }
    private Piece CreatePieceOfTypeAndColorAtPosition(PieceType pieceType,PieceColor color, ChessNotationPosition position) 
    {
        var piece = CreateNewPieceOfTypeAndColorAtPosition(pieceType,color,position);
        piece.SetPiecePosition(position);
        return piece;
    }
    private Piece CreateNewPieceOfTypeAndColorAtPosition(PieceType pieceType,PieceColor pieceColor,ChessNotationPosition position)
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
    ///  UPDATES
    /// </summary>
    public void UpdateAllTargetedSquaresInBoardWithAdversaryTargets(PieceColor pieceColor)
    {
        AllTargetedSquares = GetAllTargetedSquaresInBoardByPlayer(AdversaryPieceColor(pieceColor));
    }
    public void SetLastMovedPiece(Piece piece)
    {
        LastMovedPiece = piece;
    }
    

    /// <summary>
    /// REMOVE PIECE METHODS
    /// </summary>
    public Piece RemovePieceFromBoardAt(ChessNotationPosition pos)
    {
        return RemovePieceFromBoardAtCoordinates(pos.RowIndex, pos.ColumnIndex);
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
        var pos = piece.GetPiecePosition();
        Board[pos.RowIndex, pos.ColumnIndex] = null;
        _capturedPieces.Add(piece);
    }
    
    /// <summary>
    /// PUT PIECES METHOD
    /// </summary>
    public void PutPieceAtDestinationPosition(Piece piece , ChessNotationPosition destination)
    {
        PutPieceAtDestinationCoordinates(piece,destination.RowIndex,destination.ColumnIndex);
        piece.SetPiecePosition(destination);
    }
    private void PutPieceAtDestinationCoordinates(Piece piece , int row , int col)
    {
        Board[row, col] = piece;
    }
    public void ReturnPieceToPlay(Piece piece)
    {
        var pos = piece.GetPiecePosition();
        Board[pos.RowIndex, pos.ColumnIndex] = piece;
        _capturedPieces.Remove(piece);
    }
    
    /// <summary>
    ///  VERIFICATIONS
    /// </summary>
    public bool IsKingInCheck(PieceColor color)
    {
        var king = GetKing(color);
        UpdateAllTargetedSquaresInBoardWithAdversaryTargets(color);
        return IsSquareInPositionTargetedByOpponent(king.GetPiecePosition());
    }
    public bool IsSquareInPositionTargetedByOpponent(ChessNotationPosition position)
    {
        return IsSquareInCoordinatesTargetedByOpponent(position.RowIndex, position.ColumnIndex);
    }
    public bool IsSquareInCoordinatesTargetedByOpponent(int row, int column)
    {
        try
        {
            ValidateBoardCoordinates(row, column);
            return AllTargetedSquares[row, column];
        }
        catch (Exception e)
        {
            return true;
        }
    }
    public bool HasPieceAtChessNotationPosition(ChessNotationPosition notationPosition)
    {
        return HasPieceAtCoordinate(notationPosition.RowIndex, notationPosition.ColumnIndex);
    }
    public bool HasPieceAtCoordinate(int row, int col)
    {
        ValidateBoardCoordinates(row,col);
        
        return Board[row, col] is not null;
    }
    
    
    /// <summary>
    /// ACCESS METHODS
    /// </summary>
    public Piece GetKing(PieceColor color)
    {
        var piecesOfColor = GetChessPiecesInPlay(color);
        return piecesOfColor.FirstOrDefault(piece => piece.GetPieceType() == PieceType.King) ?? throw new ChessException("[CHESS PIECES] No king was found");
    }
    public bool[,] GetAllTargetedSquaresInBoardByPlayer(PieceColor pieceColor)
    {
        var allTargetedSquaresInBoard = new bool[MaxChessBoardSize, MaxChessBoardSize];
        var piecesOfColor = GetChessPiecesInPlay(pieceColor);
        foreach (var piece in piecesOfColor)
        {
            piece.CalculatePossibleAttackMoves();
            for (var i = 0; i < MaxChessBoardSize; i++)
            {
                for(var j = 0; j < MaxChessBoardSize; j++)
                    allTargetedSquaresInBoard[i, j] |= piece.GetAllPossibleMoves()[i,j];
            }
            
        }

        return allTargetedSquaresInBoard;
    }
    public HashSet<Piece> GetChessPiecesInPlay(PieceColor pieceColor)
    {
        var aux = new HashSet<Piece>();
        foreach (var piece in _chessPieces.Where(piece => piece.GetPieceColor() == pieceColor))
        {
            aux.Add(piece);
        }
        aux.ExceptWith(GetCapturedPieces(pieceColor));
        return aux;
    }
    public HashSet<Piece> GetCapturedPieces(PieceColor pieceColor)
    {
        var aux = new HashSet<Piece>();
        foreach (var piece in _capturedPieces.Where(piece => piece.GetPieceColor() == pieceColor))
        {
            aux.Add(piece);
        }
        return aux;
    }
    public Piece AccessPieceAtChessNotationPosition(ChessNotationPosition notationPosition)
    {
        return AccessPieceAtCoordinates(notationPosition.RowIndex, notationPosition.ColumnIndex);
    }
    public Piece AccessPieceAtCoordinates(int row, int col)
    {
        try
        {
            ValidateBoardCoordinates(row,col);
        
            return Board[row, col];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
           
        }

        return null;
    }
    private PieceColor AdversaryPieceColor(PieceColor pieceColor)
    {
        return  pieceColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }


    /// <summary>
    /// VALIDATION METHODS
    /// </summary>
    public void ValidateNewPieceChessNotationPositionNotTaken(ChessNotationPosition pos)
    {
        if (HasPieceAtChessNotationPosition(pos))
        {
            throw new BoardException($"[CHESS BOARD] Tried to Put a New Piece at {pos.ToString()} but there already is a piece {AccessPieceAtChessNotationPosition(pos).GetPieceTypeAsString()} at this position");
        }
    }
    public void ValidateBoardChessNotationPosition(ChessNotationPosition position)
    {
        ValidateBoardCoordinates(position.RowIndex, position.ColumnIndex);
    }
    private void ValidateBoardCoordinates(int row, int column)
    {
        if(row < 0 || column < 0)
            throw new BoardException("[CHESSBOARD] Position can not have negative values.");
        
        if ( row >= MaxChessBoardSize  || column >= MaxChessBoardSize)
            throw new BoardException("[CHESSBOARD] Position can not be greater than the board's size.");
    }
    
    
    
}