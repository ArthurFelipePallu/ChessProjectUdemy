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
            SetPositionAsPossibleMove(new Position(PiecePosition.Row + (int)VerticalDirections.Up, PiecePosition.Column + (int)HorizontalDirections.None));

        //Posição de Cima e Esquerda
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Up,(int)HorizontalDirections.Left))
            SetPositionAsPossibleMove(new Position(PiecePosition.Row + (int)VerticalDirections.Up, PiecePosition.Column + (int)HorizontalDirections.Left));
        
        //Posição de Cima e Direita
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Up,(int)HorizontalDirections.Right))
            SetPositionAsPossibleMove(new Position(PiecePosition.Row + (int)VerticalDirections.Up, PiecePosition.Column + (int)HorizontalDirections.Right));
        
        //Posição Esquerda
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.None,(int)HorizontalDirections.Left))
            SetPositionAsPossibleMove(new Position(PiecePosition.Row + (int)VerticalDirections.None, PiecePosition.Column + (int)HorizontalDirections.Left));
        
        //Posição Direita
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.None,(int)HorizontalDirections.Right))
            SetPositionAsPossibleMove(new Position(PiecePosition.Row + (int)VerticalDirections.None, PiecePosition.Column + (int)HorizontalDirections.Right));
        
        //Posição de Baixo
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Down,(int)HorizontalDirections.None))
            SetPositionAsPossibleMove(new Position(PiecePosition.Row + (int)VerticalDirections.Down, PiecePosition.Column + (int)HorizontalDirections.None));
        
        //Posição de Baixo e Esquerda
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Down,(int)HorizontalDirections.Left))
            SetPositionAsPossibleMove(new Position(PiecePosition.Row + (int)VerticalDirections.Down, PiecePosition.Column + (int)HorizontalDirections.Left));
        
        //Posição de Baixo e Direita
        if (PossibleMovementAtPositionIsMoveOrTake((int)VerticalDirections.Down,(int)HorizontalDirections.Right))
            SetPositionAsPossibleMove(new Position(PiecePosition.Row + (int)VerticalDirections.Down, PiecePosition.Column + (int)HorizontalDirections.Right));
    }

    private void CheckPossibleMoveIsNotCheck(VerticalDirections vDir, HorizontalDirections hDir)
    {
        if (!PossibleMovementAtPositionIsMoveOrTake((int)vDir, (int)hDir)) return;
        //var kingOriginalPos = PiecePosition;


        var auxPosition = new Position(PiecePosition.Row + (int)vDir, PiecePosition.Column + (int)hDir);

        if(Board.IsSquareInCoordinatesTargetedByOpponent(auxPosition.Row , auxPosition.Column ))
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

    private bool IsInCheck()
    {
        return IsInCheckFromPawn() ||
               IsInCheckFromKnight() ||
               IsInCheckFromDiagonalsDirections() ||
               IsInCheckFromHorizontalOrVerticalDirection();
    }

    private bool IsInCheckFromHorizontalOrVerticalDirection()
    {
               //Direção para Cima
        return DirectionHasEnemyPieceOfType(HorizontalDirections.None, VerticalDirections.Up, 8, PieceType.Queen, PieceType.Rook) ||
               //Direção para Direita 
               DirectionHasEnemyPieceOfType(HorizontalDirections.Right, VerticalDirections.None, 8, PieceType.Queen, PieceType.Rook) ||
               //Direção para Baixo
               DirectionHasEnemyPieceOfType(HorizontalDirections.None, VerticalDirections.Down, 8, PieceType.Queen, PieceType.Rook) ||
               //Direção para Esquerda
               DirectionHasEnemyPieceOfType(HorizontalDirections.Left, VerticalDirections.None, 8, PieceType.Queen,PieceType.Rook);
    }

    private bool IsInCheckFromDiagonalsDirections()
    {
        
                //Direção Diagonal Esquerda para Cima
        return DirectionHasEnemyPieceOfType(HorizontalDirections.Left,VerticalDirections.Up,8,PieceType.Queen,PieceType.Bishop) ||
                //Direção Diagonal Direita para Cima
                DirectionHasEnemyPieceOfType(HorizontalDirections.Right,VerticalDirections.Up,8,PieceType.Queen,PieceType.Bishop) ||
                //Direção Diagonal Esquerda para Baixo
                DirectionHasEnemyPieceOfType(HorizontalDirections.Left,VerticalDirections.Down,8,PieceType.Queen,PieceType.Bishop) ||
                //Direção Diagonal Direita para Baixo
                DirectionHasEnemyPieceOfType(HorizontalDirections.Right,VerticalDirections.Down,8,PieceType.Queen,PieceType.Bishop);
    }

    private bool IsInCheckFromKnight()
    {
                // L para Esquerda e para cima
        return PositionIsEnemyPieceOfType(-1 , -2,PieceType.Knight) ||
                // L para Esquerda e para baixo
                PositionIsEnemyPieceOfType(1, -2,PieceType.Knight) ||
                // L para Direita e para cima
                PositionIsEnemyPieceOfType(-1 , 2,PieceType.Knight) ||
                // L para Direita e para baixo
                PositionIsEnemyPieceOfType(1 , 2,PieceType.Knight) ||
                // L para Cima e para Esquerda
                PositionIsEnemyPieceOfType(-2 , -1,PieceType.Knight) ||
                // L para Baixo e para Esquerda
                PositionIsEnemyPieceOfType(2 , -1,PieceType.Knight) ||
                // L para Cima e para Direita
                PositionIsEnemyPieceOfType(-2 , 1,PieceType.Knight) ||
                // L para Baixo e para Direita
                PositionIsEnemyPieceOfType(2 , 1,PieceType.Knight);
    }
    
    private bool IsInCheckFromPawn()
    {
        var vDir = GetPieceColor() == PieceColor.White ? VerticalDirections.Up : VerticalDirections.Down;
        return PositionIsEnemyPieceOfType((int)vDir , 1,PieceType.Pawn) ||
                PositionIsEnemyPieceOfType((int)vDir , -1,PieceType.Pawn);
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

        if (Board.IsSquareInCoordinatesTargetedByOpponent(PiecePosition.Row, PiecePosition.Column)) return false;

        var piece = GetFirstPieceInDirection(hDir, VerticalDirections.None);
        if (piece == null) return false;
        if (piece.GetPieceType() != PieceType.Rook) return false;
        if (piece.GetPieceColor() != PieceColor) return false;
        if(piece.TimesMoved > 0) return false;
        if (Board.IsSquareInCoordinatesTargetedByOpponent(PiecePosition.Row, PiecePosition.Column + (int)hDir)) return false;
        if (Board.IsSquareInCoordinatesTargetedByOpponent(PiecePosition.Row, PiecePosition.Column + (int)hDir * 2 )) return false;


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
        SetPositionAsPossibleMove(new Position(PiecePosition.Row , PiecePosition.Column + (int)hDir * 2  ));
    }
    
    public override void AfterMoveVerification()
    {
        throw new NotImplementedException();
    }
}