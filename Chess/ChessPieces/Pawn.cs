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
            
            var pos = new Position(PiecePosition.Row + ((int)vDir * i), PiecePosition.Column);
            if (PossibleMoveAtPositionIsOfAllowedTypes(pos,MovementType.Move))
                SetPositionAsPossibleMove(pos);
            
        }
        
        VerifyEnPassantMovementIsPossibleInDirection(HorizontalDirections.Left);
        
        VerifyEnPassantMovementIsPossibleInDirection(HorizontalDirections.Right);
        
        Board.PrintPiecePossibleMoves(this);
    }


    private void VerifyEnPassantMovementIsPossibleInDirection(HorizontalDirections hDir)
    {
        //is in 5th rank
        if(!PawnIsIn5thRank()) return;
        //has opponent pawn by its side
        var enemyPawn  = Board.AccessPieceAtCoordinates(PiecePosition.Row, PiecePosition.Column + (int)hDir);
        if (enemyPawn == null) return;

        //opponentspawn moved only once
        if(enemyPawn.TimesMoved > 1) return;
        //opponents pawn was last piece moved
        if (Board.LastMovedPiece != enemyPawn) return;

        var vDir = GetPawnDirectionInBoardByColor();
        SetPositionAsPossibleMove(new Position(PiecePosition.Row + (int)vDir, PiecePosition.Column + (int)hDir));
    }

    private bool PawnIsIn5thRank()
    {
        var isPawnWhite = PieceColor == PieceColor.White;
        if (isPawnWhite)
        {
            return PiecePosition.Row == 3;
        }
        return PiecePosition.Row == 4;
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
            var pos = new Position(PiecePosition.Row + rowModifier, PiecePosition.Column + columnModifier);
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
}