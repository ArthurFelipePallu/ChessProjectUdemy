using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Enums;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Rook : Piece
{
    
    public Rook(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        _value = 5;
        _name = "Rook";
        ChessNotation = 'R';
        PieceType = PieceType.Rook;
    }

    public override void AfterMoveVerification()
    {
        throw new NotImplementedException();
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
    }
}