using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Exceptions;
using Chess_Console_Project.Chess.Player;

namespace Chess_Console_Project.Chess;

public class Screen
{
    private const int MaxChessBoardSize = 8;
    private ConsoleColor _currentForeGroundColor = ConsoleColor.White;
    public Screen()
    {
        
    }
    
    
     
    /// <summary>
    /// PRINT BOARD ON SCREEN
    /// </summary>
    public void PrintBoard(ChessBoard board)
    {
        ClearScreen();
        PrintBoardExtension(board);
    }
    public void PrintBoardWithPiecePossibleMovements(ChessBoard board,bool[,] possibleMoves)
    {
        ClearScreen();
        PrintBoardExtension(board,possibleMoves);
    }
    private  void PrintBoardExtension( ChessBoard board)
    {
        var possibleMoves = new bool[MaxChessBoardSize, MaxChessBoardSize];
        PrintBoardExtension(board,possibleMoves);
    }
    private  void PrintBoardExtension( ChessBoard board , bool[,] possiblePieceMoves )
    {
        for (var i = 0; i < board.MaxChessBoardSize; i++)
        {
            WriteRowName(i);
            for (var j = 0; j < board.MaxChessBoardSize; j++)
            {
                BoardBackGroundColor(i,j , possiblePieceMoves[i,j]);
                var piece = board.AccessPieceAtCoordinates(i, j);
                ChangeForeGroundColorTo(piece?.GetPieceConsoleColor() ?? ConsoleColor.DarkGray);
                var toWrite = piece?.GetPieceNotation() ?? "   ";
                Console.Write(toWrite);
            }
            ChangeForeGroundColorTo(ConsoleColor.Black);
            Console.WriteLine(); // QUEBRA LINHA
        }
    
        WriteSeparationLine();
        WriteColumnNames();
    }
    private  void BoardBackGroundColor( int i, int j, bool pieceMovementIsPossible = false)
    {
        Console.BackgroundColor = ((i+j) %2) != 0 ?  
                                    pieceMovementIsPossible ? ChessBoard.PossibleMoveLightColor: ChessBoard.BoardLightColor :
                                    pieceMovementIsPossible ? ChessBoard.PossibleMoveDarkColor: ChessBoard.BoardDarkColor; ;   
    }
    private void WriteRowName(  int row)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(8 - row + " |");
    }
    private  void WriteSeparationLine()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        Console.Write("   ");
        for (var i = 0; i < 8; i++)
        {
            Console.Write($"___");
        }
    }
    private void WriteColumnNames()
    {
        Console.WriteLine(); // QUEBRA LINHA
        
        Console.Write("   ");
        for (var i = 0; i < MaxChessBoardSize; i++)
        {
            char letter = Convert.ToChar(i + 65);
            Console.Write($" {letter} ");
        }
        Console.WriteLine(); // QUEBRA LINHA
    }
    
    
    
    /// <summary>
    /// PLAYER PRINT METHODS
    /// </summary>
    
    public void PrintBoardAndPlayerToMove(ChessBoard board,ChessPlayer player)
    {
        PrintBoard(board);
        AnnouncePlayerToMove(player);
    }
    public ChessNotationPosition AskPlayerForPieceInBoard()
    {
        ScreenWrite($"Choose a Piece to move");
        return GetChessNotationPosition();
    }
    public ChessNotationPosition AskPlayerForPieceDestinationInBoard(Piece piece)
    {
        ScreenWrite($"Choose a Position to move your {piece} ");
        return GetChessNotationPosition();
    }
    private ChessNotationPosition GetChessNotationPosition()
    {

        ScreenWrite($"Specify a Column [a - h] or [A - H] followed by a Row [1 - 8]. {Environment.NewLine} EX : 'A2' or 'a2'    ");
        var notation =  Console.ReadLine();

        if (notation == null || notation.Length != 2) throw new ChessException("[ CHESS MATCH] Notation specified by player is in wrong format");
        var col = notation[0];
        var row = (int)notation[1] - '0';

        return new ChessNotationPosition( row,col);
    }
    public void AnnouncePlayerToMove(ChessPlayer player)
    {
        ScreenWrite( $"{player.PlayingAs()} Player: {player.Name}'s turn");
    }
    
    
    
    
    /// <summary>
    /// SCREEN METHODS
    /// </summary>
    private void ClearScreen()
    {
        Console.Clear();
    }
    public void ScreenWriteAndWaitForEnterToContinue(string message)
    {
        ScreenWrite(message);
        PressEnterToContinue();
    }
    private void ScreenWrite(string message, ConsoleColor color = ConsoleColor.White)
    {
        if(_currentForeGroundColor != color)
            ChangeForeGroundColorTo(color);

        Console.WriteLine( message);
    }
    private void PressEnterToContinue()
    {
        ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
        
        while (keyInfo.Key != ConsoleKey.Enter)
        {
            Console.WriteLine("Press Enter To Continue");
            keyInfo = Console.ReadKey(true);
            Console.Clear();
        }
    }
    private void ChangeForeGroundColorTo(ConsoleColor color)
    {
        _currentForeGroundColor = color;
        Console.ForegroundColor = color;
    }
}