using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Enums;

namespace Chess_Console_Project.Chess.ChessPieces;

public class King : Piece
{
    
    public King(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        Value = 999;
        Name = "King";
        ChessNotation = 'K';
        PieceType = PieceType.King;
    }
    public override void CalculatePossibleMoves()
    {
        ClearPossibleMoves();

        //Posição de Cima
        CheckPossibleMovesInDirection(HorizontalDirections.None,VerticalDirections.Up,1);
        
        //Posição de Cima e Esquerda
        CheckPossibleMovesInDirection(HorizontalDirections.Left,VerticalDirections.Up,1);

        //Posição de Cima e Direita
        CheckPossibleMovesInDirection(HorizontalDirections.Right,VerticalDirections.Up,1);
        
        //Posição Esquerda
        CheckPossibleMovesInDirection(HorizontalDirections.Left,VerticalDirections.None,1);

        //Posição Direita
        CheckPossibleMovesInDirection(HorizontalDirections.Right,VerticalDirections.None,1);

        //Posição de Baixo
        CheckPossibleMovesInDirection(HorizontalDirections.None,VerticalDirections.Down,1);
        
        //Posição de Baixo e Esquerda
        CheckPossibleMovesInDirection(HorizontalDirections.Left,VerticalDirections.Down,1);
        
        //Posição de Baixo e Direita
        CheckPossibleMovesInDirection(HorizontalDirections.Right,VerticalDirections.Down,1);
        
        
    }
    
    
}