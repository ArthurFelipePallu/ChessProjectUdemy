using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Exceptions;
using Chess_Console_Project.Chess.Player;

namespace Chess_Console_Project.Chess;

public class Screen
{
    private string _emptySquare = "   ";
    private const int MaxChessBoardSize = 8;
    private ConsoleColor _currentForeGroundColor = ConsoleColor.White;
    
    
    /// <summary>
    /// BOARD COLORS
    /// </summary>
    private const  ConsoleColor BoardLightColor = ConsoleColor.DarkGray;
    private const  ConsoleColor BoardDarkColor = ConsoleColor.Gray;
    private const  ConsoleColor PossibleMoveLightColor = ConsoleColor.DarkRed;
    private const  ConsoleColor PossibleMoveDarkColor = ConsoleColor.Red;

    
    /// <summary>
    /// BOARD COLORS
    /// </summary>
    private const  ConsoleColor PiecesLightColor = ConsoleColor.Yellow;
    private const  ConsoleColor PiecesDarkColor = ConsoleColor.Black;
    
    public Screen()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }
    
    
     
    /// <summary>
    /// PRINT BOARD ON SCREEN
    /// </summary>
    public void PrintBoard(ChessBoard board)
    {
        PrintBoardExtension(board);
    }
    public void PrintBoardWithPiecePossibleMovements(ChessBoard board,bool[,] possibleMoves)
    {
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
                ChangeForeGroundColorTo(GetPieceConsoleColor(piece));
                var toWrite = GetPieceUnicodeOrEmptySquare(piece);
                Console.Write(toWrite);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(); // QUEBRA LINHA
        }
    
        WriteSeparationLine();
        WriteColumnNames();
    }
    private  void BoardBackGroundColor( int i, int j, bool pieceMovementIsPossible = false)
    {
        Console.BackgroundColor = ((i+j) %2) != 0 ?  
                                    pieceMovementIsPossible ? PossibleMoveLightColor: BoardLightColor :
                                    pieceMovementIsPossible ? PossibleMoveDarkColor: BoardDarkColor; ;   
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
    /// PIECE METHODS
    /// </summary>
    private ConsoleColor GetPieceConsoleColor(Piece? piece)
    {
        if(piece == null) return BoardLightColor;
        return piece.GetPieceColor() == PieceColor.Black ? PiecesDarkColor : PiecesLightColor;
    }

    private string GetPieceUnicodeOrEmptySquare(Piece? piece)
    {
        if (piece == null) return _emptySquare;
        
        return piece.GetPieceColor() == PieceColor.White ? $" {GetWhitePieceUnicode(piece)} " : $" {GetBlackPieceUnicode(piece)} ";
        
    }

    private char GetWhitePieceUnicode(Piece piece)
    {
        switch (piece.GetPieceType())
        {
            case PieceType.King:
                return '\u2654';
            case PieceType.Queen:
                return '\u2655';
            case PieceType.Rook:
                return '\u2656';
            case PieceType.Knight:
                return '\u2658';
            case PieceType.Bishop:
                return '\u2657';
            case PieceType.Pawn:
                return '\u2659';
        }

        return ' ';
    }
    private string GetBlackPieceUnicode(Piece piece)
    {
        return piece.GetPieceType() switch
        {
            PieceType.King => "\u265a",
            PieceType.Queen => "\u265b",
            PieceType.Rook => "\u265c",
            PieceType.Knight => "\u265e",
            PieceType.Bishop => "\u265d",
            PieceType.Pawn => "\u265f",
            _ => _emptySquare
        };
    }


    /// <summary>
    /// PLAYER PRINT METHODS
    /// </summary>


    public void PrintPlayerDetailedInformation(ChessPlayer player, HashSet<Piece> capturedPieces)
    {
        Console.WriteLine($"{player.Name}  ELO: {player.Elo}");
        Console.WriteLine($"Playing with {player.PlayingColor.ToString()}");

        Console.Write("Captured: ");
        foreach (var piece in capturedPieces)
        {
            Console.Write($" {GetPieceUnicodeOrEmptySquare(piece)} ");
        }
        Console.WriteLine();
    }
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
    public static void ClearScreen()
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


    


    /// <summary>
    /// MAIN MENU METHODS
    /// </summary>
    public void PrintMainMenu()
    {        
        Console.WriteLine("-+#################################################################+#####.");
        Console.WriteLine("-+############......##....##.....#........####......####......#####+#####.");
        Console.WriteLine("-+##########..#####.###..####+..###..-####.##..####.###..####.#########+#.");
        Console.WriteLine(".+#########...#########..####-..###...##.####....######....##########++##.");
        Console.WriteLine("-+#########..+#########.....-...###...#..#######....#####-....###########.");
        Console.WriteLine("-+#########-..#########..####-..###...####+##.#####..##.####+..########+#.");
        Console.WriteLine(".+###########...-+..###..####...###...#-...##...+#..###...#+..#########+#.");
        Console.WriteLine("-+#################################################################+#####.");
    }
    
}