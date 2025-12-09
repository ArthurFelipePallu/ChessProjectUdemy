using System.Drawing;
using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Chess.Enums;

namespace Chess_Console_Project.Board.Pieces;

public abstract class Piece
{
    /// <summary>
    /// PRIVATE
    /// </summary>
    
    public int Value;
    public string Name;
    protected char ChessNotation;
    public Position PiecePosition;
    public ChessBoard Board;
    private PieceColor _pieceColor;
    protected PieceType PieceType;
    public int TimesMoved {get; protected set;}
    protected bool[,] PossibleMoves;


    protected Piece(ChessBoard board, PieceColor pieceColor)
    {
        _pieceColor = pieceColor;
        Board = board;
        TimesMoved = 0;
        PossibleMoves = new bool[Board.MaxChessBoardSize, Board.MaxChessBoardSize];
    }




    /// <summary>
    /// PIECE POSITION METHODS
    /// </summary>

    public bool IsCurrentlyAtCoordinates(int row, int col)
    {
        return PiecePosition.Row == row && PiecePosition.Column == col;
    }
    

    public void SetPiecePosition(Position position)
    {
        PiecePosition = position;
    }

    
    
    
    
    
    /// <summary>
    /// PIECE MOVEMENTS
    /// </summary>
    public bool PositionIsInPossibleMoves(Position pos)
    {
        return CoordinatesIsInPossibleMoves(pos.Row, pos.Column);
    }

    public bool HasAtLeastOnePossibleMove()
    {
        for (var i = 0; i < PossibleMoves.GetLength(0) - 1; i++)
        {
            for (var j = 0; j < PossibleMoves.GetLength(1) -1 ; j++)
            {
                if (PossibleMoves[i, j])
                    return true;
            }
        }

        return false;
    }
    public bool[,] GetAllPossibleMoves()
    {
        return PossibleMoves;
    }
    public bool CoordinatesIsInPossibleMoves(int row,int col)
    {
        return PossibleMoves[row, col];
    }
    public void IncreaseTimesMoved()
    {
        TimesMoved++;
    }
    public abstract void CalculatePossibleMoves();
    protected void ClearPossibleMoves()
    {
        for (int i = 0; i < Board.MaxChessBoardSize; i++)
        {
            for (int j = 0; j < Board.MaxChessBoardSize; j++)
            {
                PossibleMoves[i, j] = false;
            }
        }
    } 
    protected void CheckPossibleMovesInDirection(HorizontalDirections hDirection, VerticalDirections vDirection,int maxDistantMovesToCheck = 99 , MovementType hasToBeOfMovementType = MovementType.Any)
    {
        var countHelper = 0;
        var keepGoing = true;

        var possibleMovePosX = 0;
        var possibleMovePosY = 0;

        var possiblePosition = new Position(possibleMovePosX, possibleMovePosY);
        while (keepGoing)
        {
            countHelper++;
            possibleMovePosX = PiecePosition.Row + (countHelper * (int)vDirection);
            possibleMovePosY = PiecePosition.Column + (countHelper * (int)hDirection);
            possiblePosition.SetPosition(possibleMovePosX, possibleMovePosY);

            var move = CheckMovementTypeAt(possiblePosition);

            if (hasToBeOfMovementType != MovementType.Any)
            {
                if(move == hasToBeOfMovementType)
                    SetPositionAsPossibleMove(possiblePosition);
                return;
            }
            

            switch (move)
            {
                case MovementType.IllegalMove:
                    keepGoing = false;
                    break;
                case MovementType.Take:
                    SetPositionAsPossibleMove(possiblePosition);
                    keepGoing = false;
                    break;
                default:
                    SetPositionAsPossibleMove(possiblePosition);
                    break;
            }
            if(countHelper >= maxDistantMovesToCheck)
                keepGoing = false;
        }
    }
    protected MovementType TryPositionPossibleMove(Position pos)
    {
        var move = CheckMovementTypeAt(pos);
        if(move != MovementType.IllegalMove)
            SetPositionAsPossibleMove(pos);

        return move;
    }
    private MovementType CheckMovementTypeAt(Position position)
    {
        try
        {
            Board.ValidateBoardPosition(position);
        }
        catch (BoardException e)
        {
            return MovementType.IllegalMove;
        }

        var piece = Board.AccessPieceAtPosition(position);
        
        if (piece == null)
            return MovementType.Move;
        
        if (piece.GetPieceColor() == _pieceColor)
            return MovementType.IllegalMove;
        
        return piece.GetPieceType() == PieceType.King ? MovementType.Check : MovementType.Take;
    }
    private void SetPositionAsPossibleMove(Position pos)
    {
        PossibleMoves[pos.Row,pos.Column] = true;
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
        return _pieceColor;
    }
    public string GetPieceColroAsString()
    {
        return _pieceColor.ToString();
    }

    /// <summary>
    /// PIECE NOTATION
    /// </summary>
    public string GetPieceNotation()
    {
        return $" {ChessNotation.ToString()} ";
    }
    
    
    
    public override string ToString()
    {
        return $" {_pieceColor.ToString()} {Name} ";
    }
}