using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Enums;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Pawn : Piece
{
    
    public Pawn(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        Value = 1;
        Name = "Pawn";
        ChessNotation = 'P';
        PieceType = PieceType.Pawn;
    }
    
    public override void CalculatePossibleMoves()
    {
        ClearPossibleMoves();

        var firstPawnMove = TimesMoved == 0 ? 2 : 1;

        var vDir = GetPieceColor() == PieceColor.White ? VerticalDirections.Up : VerticalDirections.Down;
        
        //Posição de Cima
        var hasPieceAhead = Board.HasPieceAtCoordinate( PiecePosition.Row + ((int)vDir * firstPawnMove) , PiecePosition.Column );
        
        if(!hasPieceAhead)
            CheckPossibleMovesInDirection(HorizontalDirections.None,vDir,firstPawnMove);
        
        //Posição de Cima e Esquerda e precisa ser Movimento de Captura
        CheckPossibleMovesInDirection(HorizontalDirections.Left,vDir,1,MovementType.Take);

        //Posição de Cima e Direita e precisa ser Movimento de Captura
        CheckPossibleMovesInDirection(HorizontalDirections.Right,vDir,1,MovementType.Take);
    }


}