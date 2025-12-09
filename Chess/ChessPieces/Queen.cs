using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Enums;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Queen : Piece
{
    
    public Queen(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        Value = 9;
        Name = "Queen";
        ChessNotation = 'Q';
        PieceType = PieceType.Queen;
    }
    public override void CalculatePossibleMoves()
    {
        ClearPossibleMoves();
        
        //Direção para Cima
        CheckPossibleMovesInDirection(HorizontalDirections.None,VerticalDirections.Up);

        //Direção para Direita 
        CheckPossibleMovesInDirection(HorizontalDirections.Right,VerticalDirections.None);
        
        //Direção para Baixo
        CheckPossibleMovesInDirection(HorizontalDirections.None,VerticalDirections.Down);
        
        //Direção para Esquerda
        CheckPossibleMovesInDirection(HorizontalDirections.Left,VerticalDirections.None);
        
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