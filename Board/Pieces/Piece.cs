using System.Drawing;
using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Chess.Enums;
using Chess_Console_Project.Chess.Exceptions;

namespace Chess_Console_Project.Board.Pieces;

public abstract class Piece
{
    /// <summary>
    /// PRIVATE
    /// </summary>
    protected int _value;
    protected string _name;
    private bool[,] _possibleMoves;
    
    
    
    /// <summary>
    /// PROTECTED
    /// </summary>
    protected char ChessNotation;
    protected PieceType PieceType;
    protected readonly PieceColor PieceColor;
    protected ChessNotationPosition PiecePosition;
    protected readonly ChessBoard Board;
    
    /// <summary>
    /// PUBLIC
    /// </summary>
    public int TimesMoved {get; private set;}
   
    
    
    
    

    /// <summary>
    /// CONSTRUCTOR
    /// </summary>
    protected Piece(ChessBoard board, PieceColor pieceColor)
    {
        PieceColor = pieceColor;
        Board = board;
        TimesMoved = 0;
        _possibleMoves = new bool[Board.MaxChessBoardSize, Board.MaxChessBoardSize];
    }




    /// <summary>
    /// PIECE POSITION METHODS
    /// </summary>
    public bool IsCurrentlyAtCoordinates(int row, int col)
    {
        return PiecePosition.RowIndex == row && PiecePosition.ColumnIndex == col;
    }
    public void SetPiecePosition(ChessNotationPosition position)
    {
        PiecePosition = position;
    }
    public ChessNotationPosition GetPiecePosition()
    {
        return PiecePosition;
    }

    
    
    
    
    
    /// <summary>
    /// PIECE MOVEMENTS
    /// </summary>
    public void IncreaseTimesMoved()
    {
        TimesMoved++;
    }
    public void DecreaseTimesMoved()
    {
        TimesMoved--;
    }
    protected void ClearPossibleMoves()
    {
        for (int i = 0; i < Board.MaxChessBoardSize; i++)
        {
            for (int j = 0; j < Board.MaxChessBoardSize; j++)
            {
                _possibleMoves[i, j] = false;
            }
        }
    } 
    public bool[,] GetAllPossibleMoves()
    {
        return _possibleMoves;
    }
    public bool HasAtLeastOnePossibleMove()
    {
        for (var i = 0; i < _possibleMoves.GetLength(0) ; i++)
        {
            for (var j = 0; j < _possibleMoves.GetLength(1)  ; j++)
            {
                if (_possibleMoves[i, j])
                    return true;
            }
        }
        return false;
    }
    
    public bool ChessNotationPositionIsInPossibleMoves(ChessNotationPosition chessNotationPos)
    {
        return CoordinatesIsInPossibleMoves(chessNotationPos.RowIndex, chessNotationPos.ColumnIndex);
    }
    private bool CoordinatesIsInPossibleMoves(int row,int col)
    {
        return _possibleMoves[row, col];
    }
    
    public abstract void AfterMoveVerification();
    public abstract void CalculatePossibleMoves();
    
    public abstract void CalculatePossibleAttackMoves();
    protected void CheckPossibleMovesInDirection(VerticalDirections vDir, HorizontalDirections hDir,int maxDistantMovesToCheck = 8 )
    {
        var countHelper = 0;
        var keepGoing = true;

        while (keepGoing)
        {
            countHelper++;
            try
            {
                var possiblePosition = PiecePosition.Offset(
                    countHelper * (int)vDir, 
                    countHelper * (int)hDir);

                var move = CheckMovementTypeAt(possiblePosition);
                
                switch (move)
                {
                    case MovementType.Move :
                        SetPositionAsPossibleMove(possiblePosition);
                        break;
                    case MovementType.TakeEnemyPiece:
                        SetPositionAsPossibleMove(possiblePosition);
                        keepGoing = false;
                        break;
                    case MovementType.Illegal:
                        keepGoing = false;
                        break;
                    case MovementType.AllyPiece :
                        keepGoing = false;
                        break;
                    case MovementType.SamePiece :
                        keepGoing = false;
                        break;
                }
            }
            catch (MovementException e)
            {
                keepGoing = false;
                break;
            }
            
            if(countHelper >= maxDistantMovesToCheck)
                keepGoing = false;
        }
    }
    protected bool PositionIsEnemyPieceOfType(int rowModifier, int columnModifier,params PieceType[] piecesToBeAwareOf)
    {
        try
        {
            var pos = PiecePosition.Offset(rowModifier, columnModifier);
            return IsPieceAtPositionIsAnEnemyOfType(pos, piecesToBeAwareOf);
        }
        catch (MovementException e)
        {
            return false;
        }
    }
    protected bool PossibleMovementAtPositionIsMoveOrTake(int rowModifier, int columnModifier)
    {
        try
        {
            var pos = PiecePosition.Offset(rowModifier, columnModifier);
            if (!PossibleMoveAtPositionIsOfAllowedTypes(pos, MovementType.Move, MovementType.TakeEnemyPiece))
                return false;
            SetPositionAsPossibleMove(pos);
            return true;
        }
        catch (MovementException e)
        {
            return false;
        }
       
    }
    protected bool PossibleMoveAtPositionIsOfAllowedTypes(ChessNotationPosition pos,params MovementType[] allowedMovementTypes)
    {
        var move = CheckMovementTypeAt(pos);
        return  allowedMovementTypes.Contains(move);
    }
    protected bool DirectionHasEnemyPieceOfType(HorizontalDirections hDirection, VerticalDirections vDirection,int maxDistantMovesToCheck = 8 ,params PieceType[] piecesToBeAwareOf)
    {
        var countHelper = 0;
        var keepGoing = true;

        while (keepGoing)
        {
            countHelper++;
            try
            {
                var possiblePosition = PiecePosition.Offset(
                    countHelper * (int)vDirection, 
                    countHelper * (int)hDirection);
                
                var move = CheckMovementTypeAt(possiblePosition);
                switch (move)
                {
                    case MovementType.Move:
                        break;
                    case MovementType.Illegal:
                        keepGoing = false;
                        break;
                    case MovementType.AllyPiece:
                        keepGoing = false;
                        break;
                    case MovementType.SamePiece:
                        keepGoing = false;
                        break;
                    case MovementType.TakeEnemyPiece:
                        var piece = Board.AccessPieceAtChessNotationPosition(possiblePosition);
                        return piecesToBeAwareOf.Contains(piece.PieceType);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Was Checking for EnemyPiece { piecesToBeAwareOf[0].ToString()}");
                return false;
            }
            
            if(countHelper >= maxDistantMovesToCheck)
                keepGoing = false;
        }

        return false;
    }

    protected Piece GetFirstPieceInDirection(HorizontalDirections hDirection, VerticalDirections vDirection)
    {
        var countHelper = 0;
        var keepGoing = true;
        var maxDistantMovesToCheck = 8;

        while (keepGoing)
        {
            countHelper++;
            try
            {
                var possiblePosition = PiecePosition.Offset(
                    countHelper * (int)vDirection, 
                    countHelper * (int)hDirection);

                var piece = Board.AccessPieceAtChessNotationPosition(possiblePosition);
                if (piece != null)
                    return piece;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Was Checking for Piece but went out of bounds");
                return null;
            }
            
            if(countHelper >= maxDistantMovesToCheck)
                keepGoing = false;
        }

        return null;
    }
    
    
    protected bool PossibleMoveAtPositionIsLegalAndNotOfNotSpecifiedTypes(ChessNotationPosition pos,params MovementType[] notAllowedMovementTypes)
    {
        var move = CheckMovementTypeAt(pos);
        return move != MovementType.Illegal && !notAllowedMovementTypes.Contains(move);
    }

    private bool IsPieceAtPositionIsAnEnemyOfType(ChessNotationPosition position,params PieceType[] enemyPiecesToBeAware)
    {
        if (PossibleMoveAtPositionIsLegalAndNotOfNotSpecifiedTypes(position,MovementType.TakeEnemyPiece)) return false;
        var piece = Board.AccessPieceAtChessNotationPosition(position);
        return enemyPiecesToBeAware.Contains(piece.PieceType);

    }
    private MovementType CheckMovementTypeAt(ChessNotationPosition position)
    {
        try
        {
            Board.ValidateBoardChessNotationPosition(position);
        }
        catch (BoardException e)
        {
            return MovementType.Illegal;
        }

        var pieceAtPosition = Board.AccessPieceAtChessNotationPosition(position);
        
        if (pieceAtPosition == null)
            return MovementType.Move;

        if (pieceAtPosition.GetPieceColor() != PieceColor) return MovementType.TakeEnemyPiece;
        return pieceAtPosition.PieceType == PieceType ? MovementType.SamePiece : MovementType.AllyPiece;
    }
    protected void SetPositionAsPossibleMove(ChessNotationPosition pos)
    {
        _possibleMoves[pos.RowIndex, pos.ColumnIndex] = true;
    }
    protected void SetPositionAsNotPossibleMove(ChessNotationPosition pos)
    {
        _possibleMoves[pos.RowIndex, pos.ColumnIndex] = false;
    }
    
    
    
    /// <summary>
    /// PIECE TYPE
    /// </summary>
    /// <returns></returns>
    public PieceType GetPieceType()
    {
        return PieceType;
    }
    public string GetPieceTypeAsString()
    {
        return PieceType.ToString();
    }
    
    /// <summary>
    /// PIECE COLOR
    /// </summary>
    /// <returns></returns>
    public PieceColor GetPieceColor()
    {
        return PieceColor;
    }

    /// <summary>
    /// PIECE NOTATION
    /// </summary>
    public string GetPieceNotation()
    {
        return $"{ChessNotation.ToString()}";
    }
    
    
    
    public override string ToString()
    {
        return $" {PieceColor.ToString()} {_name} ";
    }
}