using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Enums;
using Chess_Console_Project.Chess.Exceptions;

namespace Chess_Console_Project.Chess.ChessPieces;

public class King : Piece
{
    
    public King(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        _value = 999;
        _name = "King";
        ChessNotation = 'K';
        PieceType = PieceType.King;
    }
    public override void CalculatePossibleMoves()
    {
        ClearPossibleMoves();

        Board.UpdateAllTargetedSquaresInBoardWithAdversaryTargets(PieceColor);
        
        if(CanShortCastle())
            SetShortCastlePossiblePosition();
        if(CanLongCastle())
            SetLongCastlePossiblePosition();
        
        //Posição de Cima
         CheckPossibleMoveIsNotCheck(VerticalDirections.Up,HorizontalDirections.None);
        
         //Posição de Cima e Esquerda
         CheckPossibleMoveIsNotCheck(VerticalDirections.Up,HorizontalDirections.Left);
        
         //Posição de Cima e Direita
         CheckPossibleMoveIsNotCheck(VerticalDirections.Up,HorizontalDirections.Right);
        
         //Posição Esquerda
        CheckPossibleMoveIsNotCheck(VerticalDirections.None,HorizontalDirections.Left);
        
        //Posição Direita
        CheckPossibleMoveIsNotCheck(VerticalDirections.None,HorizontalDirections.Right);
        
         //Posição de Baixo
         CheckPossibleMoveIsNotCheck(VerticalDirections.Down,HorizontalDirections.None);
        
         //Posição de Baixo e Esquerda
         CheckPossibleMoveIsNotCheck(VerticalDirections.Down,HorizontalDirections.Left);
        
         //Posição de Baixo e Direita
         CheckPossibleMoveIsNotCheck(VerticalDirections.Down,HorizontalDirections.Right);
         
    }

    public override void CalculatePossibleAttackMoves()
    {
        ClearPossibleMoves();

        //Posição de Cima
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Up,(int)HorizontalDirections.None))
            SetPositionAsPossibleMove(PiecePosition.Offset((int)VerticalDirections.Up, (int)HorizontalDirections.None));

        //Posição de Cima e Esquerda
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Up,(int)HorizontalDirections.Left))
            SetPositionAsPossibleMove(PiecePosition.Offset((int)VerticalDirections.Up, (int)HorizontalDirections.Left));
        
        //Posição de Cima e Direita
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Up,(int)HorizontalDirections.Right))
            SetPositionAsPossibleMove(PiecePosition.Offset((int)VerticalDirections.Up, (int)HorizontalDirections.Right));
        
        //Posição Esquerda
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.None,(int)HorizontalDirections.Left))
            SetPositionAsPossibleMove(PiecePosition.Offset((int)VerticalDirections.None, (int)HorizontalDirections.Left));
        
        //Posição Direita
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.None,(int)HorizontalDirections.Right))
            SetPositionAsPossibleMove(PiecePosition.Offset((int)VerticalDirections.None, (int)HorizontalDirections.Right));
        
        //Posição de Baixo
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Down,(int)HorizontalDirections.None))
            SetPositionAsPossibleMove(PiecePosition.Offset((int)VerticalDirections.Down, (int)HorizontalDirections.None));
        
        //Posição de Baixo e Esquerda
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Down,(int)HorizontalDirections.Left))
            SetPositionAsPossibleMove(PiecePosition.Offset((int)VerticalDirections.Down, (int)HorizontalDirections.Left));
        
        //Posição de Baixo e Direita
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Down,(int)HorizontalDirections.Right))
            SetPositionAsPossibleMove(PiecePosition.Offset((int)VerticalDirections.Down, (int)HorizontalDirections.Right));
    }

    private void CheckPossibleMoveIsNotCheck(VerticalDirections vDir, HorizontalDirections hDir)
    {
        if (!PossibleMovementAtPositionIsMoveOrTake((int)vDir, (int)hDir)) return;
        //var kingOriginalPos = PiecePosition;


        var auxPosition = PiecePosition.Offset((int)vDir, (int)hDir);

        if(Board.IsSquareInCoordinatesTargetedByOpponent(auxPosition.RowIndex , auxPosition.ColumnIndex ))
        {
            SetPositionAsNotPossibleMove(auxPosition);
        }
        else
            SetPositionAsPossibleMove(auxPosition);

        // try
        // { 
        //     var verifyingPosition = new Position(PiecePosition.Row + (int)vDir, PiecePosition.Column + (int)hDir);
        //     SetPiecePosition(verifyingPosition);
        //     if(IsInCheck())
        //         SetPositionAsNotPossibleMove(verifyingPosition);
        // }
        // catch (MovementException e)
        // {
        //     Console.WriteLine( "[KING IS IN CHECK VERIFICATION] : " + e.Message);
        // }
        // SetPiecePosition(kingOriginalPos);
    }



    private bool CanShortCastle()
    {
        return CanCastle(PieceColor == PieceColor.White ? HorizontalDirections.Left : HorizontalDirections.Right);
    }
    private bool CanLongCastle()
    {
        return CanCastle(PieceColor == PieceColor.White ? HorizontalDirections.Right : HorizontalDirections.Left);
    }
    private bool CanCastle(HorizontalDirections hDir)
    {
        if (TimesMoved > 0) return false;

        if (Board.IsSquareInCoordinatesTargetedByOpponent(PiecePosition.RowIndex, PiecePosition.ColumnIndex)) return false;

        var piece = GetFirstPieceInDirection(hDir, VerticalDirections.None);
        if (piece == null) return false;
        if (piece.GetPieceType() != PieceType.Rook) return false;
        if (piece.GetPieceColor() != PieceColor) return false;
        if(piece.TimesMoved > 0) return false;
        if (Board.IsSquareInCoordinatesTargetedByOpponent(PiecePosition.RowIndex, PiecePosition.ColumnIndex + (int)hDir)) return false;
        if (Board.IsSquareInCoordinatesTargetedByOpponent(PiecePosition.RowIndex, PiecePosition.ColumnIndex + (int)hDir * 2 )) return false;


        return true;
    }


    private void SetShortCastlePossiblePosition()
    {
        var hDir =  PieceColor == PieceColor.White ? HorizontalDirections.Left : HorizontalDirections.Right;
        SetCastlePositionAsPossible(hDir);
    }
    private void SetLongCastlePossiblePosition()
    {
        var hDir =  PieceColor == PieceColor.White ? HorizontalDirections.Right : HorizontalDirections.Left;
        SetCastlePositionAsPossible(hDir);
    }
    
    private void SetCastlePositionAsPossible(HorizontalDirections hDir)
    {
        SetPositionAsPossibleMove(PiecePosition.Offset(0, (int)hDir * 2));
    }
    
    public override void AfterMoveVerification()
    {
        throw new NotImplementedException();
    }
}