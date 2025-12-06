namespace Chess_Console_Project.Board;

public class ChessBoard
{
    private readonly int _maxChessBoardSize = 8;
    private int[][] _board;
    

    public ChessBoard()
    {
        _board = new int[_maxChessBoardSize][];
        for (var i = 0; i < _maxChessBoardSize; i++)
        {
            
            for (var j = 0; j < _maxChessBoardSize; j++)
            {
                
            }
            
        }
        
    }
}