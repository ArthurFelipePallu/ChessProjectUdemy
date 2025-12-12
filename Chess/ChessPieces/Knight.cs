using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;

namespace Chess_Console_Project.Chess.ChessPieces;

public class Knight : Piece
{
    
    public Knight(ChessBoard board, PieceColor pieceColor) : base(board, pieceColor)
    {
        _value = 3;
        _name = "Knight";
        ChessNotation = 'N';
        PieceType = PieceType.Knight;
    }

    public override void AfterMoveVerification()
    {
        throw new NotImplementedException();
    }

    public override void CalculatePossibleMoves()
    {
        ClearPossibleMoves();

        HorizontalLMovements();
        VerticalLMovements();
    }
    
    
    private void HorizontalLMovements()
    {
        // L para Esquerda e para cima
        var pos = new Position(PiecePosition.Row - 1, PiecePosition.Column -2);
        TryPositionPossibleMove(pos);
        
        // L para Esquerda e para baixo
        pos.SetPosition(PiecePosition.Row + 1, PiecePosition.Column -2);
        TryPositionPossibleMove(pos);
        
        // L para Direita e para cima
        pos.SetPosition(PiecePosition.Row - 1, PiecePosition.Column +2);
        TryPositionPossibleMove(pos);
        
        // L para Direita e para baixo
        pos.SetPosition(PiecePosition.Row + 1, PiecePosition.Column +2);
        TryPositionPossibleMove(pos);
        
    }
    private void VerticalLMovements()
    {
        // L para Cima e para Esquerda
        var pos = new Position(PiecePosition.Row - 2, PiecePosition.Column -1);
        TryPositionPossibleMove(pos);
        
        // L para Baixo e para Esquerda
        pos.SetPosition(PiecePosition.Row + 2, PiecePosition.Column -1);
        TryPositionPossibleMove(pos);
        
        // L para Cima e para Direita
        pos.SetPosition(PiecePosition.Row - 2, PiecePosition.Column +1);
        TryPositionPossibleMove(pos);
        
        // L para Baixo e para Direita
        pos.SetPosition(PiecePosition.Row + 2, PiecePosition.Column +1);
        TryPositionPossibleMove(pos);
    }
}