namespace Chess_Console_Project.Board.ChessPieces;

public class Queen : Piece
{
    
    public Queen(ChessBoard board, PieceColor pieceColor, Position pos) : base(board, pieceColor, pos)
    {
        Value = 9;
        Name = "Queen";
        _chessNotation = 'Q';
    }
}