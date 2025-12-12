using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Enums;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Bishop : Piece
{
    
    public Bishop(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        _value = 3;
        _name = "Bishop";
        ChessNotation = 'B';
        PieceType = PieceType.Bishop;
    }

    public override void AfterMoveVerification()
    {
        throw new NotImplementedException();
    }

    public override void CalculatePossibleMoves()
    {
        ClearPossibleMoves();
        
        //Direção Diagonal Esquerda para Cima
        CheckPossibleMovesInDirection(HorizontalDirections.Left,VerticalDirections.Up);

        //Direção Diagonal Direita para Cima
        CheckPossibleMovesInDirection(HorizontalDirections.Right,VerticalDirections.Up);
        
        //Direção Diagonal Esquerda para Baixo
        CheckPossibleMovesInDirection(HorizontalDirections.Left,VerticalDirections.Down);
        
        //Direção Diagonal Direita para Baixo
        CheckPossibleMovesInDirection(HorizontalDirections.Right,VerticalDirections.Down);
    }
}