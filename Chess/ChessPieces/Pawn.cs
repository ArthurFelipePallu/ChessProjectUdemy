using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Enums;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Pawn : Piece
{
    public Pawn(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        _value = 1;
        _name  = "Pawn";
        ChessNotation = 'P';
        PieceType = PieceType.Pawn;
    }
    
    public override void CalculatePossibleMoves()
    {
        CalculatePossibleAttackMoves();

        var firstPawnMove = TimesMoved == 0 ? 2 : 1;

        var vDir = GetPawnDirectionInBoardByColor();
        
        for (var i = 1; i <= firstPawnMove; i++)
        {
            var pos = PiecePosition.Offset((int)vDir * i, 0);
            if (PossibleMoveAtPositionIsOfAllowedTypes(pos,MovementType.Move))
                SetPositionAsPossibleMove(pos);
        }
        
        VerifyEnPassantMovementIsPossibleInDirection(HorizontalDirections.Left);
        
        VerifyEnPassantMovementIsPossibleInDirection(HorizontalDirections.Right);
    }


    private void VerifyEnPassantMovementIsPossibleInDirection(HorizontalDirections hDir)
    {
        //is in 5th rank
        if(!PawnIsIn5ThRank()) return;
        //has opponent pawn by its side
        var sidePosition = PiecePosition.Offset(0, (int)hDir);
        var enemyPawn  = Board.AccessPieceAtChessNotationPosition(sidePosition);
        if (enemyPawn == null) return;

        //opponents pawn moved only once
        if(enemyPawn.TimesMoved > 1) return;
        //opponents pawn was last piece moved
        if (Board.LastMovedPiece != enemyPawn) return;

        var vDir = GetPawnDirectionInBoardByColor();
        SetPositionAsPossibleMove(PiecePosition.Offset((int)vDir, (int)hDir));
    }

    private bool PawnIsIn5ThRank()
    {
        var isPawnWhite = PieceColor == PieceColor.White;
        if (isPawnWhite)
        {
            return PiecePosition.RowIndex == 3;
        }
        return PiecePosition.RowIndex == 4;
    }
    
    

    public override void CalculatePossibleAttackMoves()
    {
        ClearPossibleMoves();
        var vDir = GetPawnDirectionInBoardByColor();
        //Posição de Cima e Esquerda e precisa ser Movimento de Captura
        PossibleMovementAtPositionWithModifiersIsOfMoveType((int)vDir, (int)HorizontalDirections.Left,MovementType.TakeEnemyPiece);
        
        //Posição de Cima e Direita e precisa ser Movimento de Captura
        PossibleMovementAtPositionWithModifiersIsOfMoveType((int)vDir, (int)HorizontalDirections.Right,MovementType.TakeEnemyPiece);
    }

    public override void AfterMoveVerification()
    {
        throw new NotImplementedException();
    }
    
    private void PossibleMovementAtPositionWithModifiersIsOfMoveType(int rowModifier, int columnModifier,MovementType movementType)
    {
        try
        {
            var pos = PiecePosition.Offset(rowModifier, columnModifier);
            if (PossibleMoveAtPositionIsOfAllowedTypes(pos, movementType))
            {
                SetPositionAsPossibleMove(pos);
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    private VerticalDirections GetPawnDirectionInBoardByColor()
    {
        return GetPieceColor() == PieceColor.White ? VerticalDirections.Up : VerticalDirections.Down;
    }


    public bool CanPromote()
    {
        var rowToPromote = GetPieceColor() == PieceColor.White ? 0 : 7;
        
        return PiecePosition.RowIndex == rowToPromote;
    }
}