using System.Drawing;
using System.Runtime.Intrinsics.X86;
using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Chess.Player;

namespace Chess_Console_Project.Chess.Match;

public class ChessMatch
{
    private int _turn;
    ChessBoard _chessBoard;
    MatchStatus _matchStatus;
    
    private PieceColor _toPlay = PieceColor.White;
    
    private ChessPlayer _playerWhite;
    private ChessPlayer _playerBlack;

    public ChessMatch()
    {
        _matchStatus = MatchStatus.WaitingForPlayers;
    }

    /// <summary>
    /// MATCH METHODS
    /// </summary>
    public void UpdateMatch()
    {
        switch (_matchStatus)
        {
            case MatchStatus.WaitingForPlayers:
                WaitForPlayers();
                break;
            case MatchStatus.WaitingForPlayer:
                Console.Clear();
                break;
            case MatchStatus.Starting:
                Console.Clear();
                CreateChessBoard();

                _matchStatus = MatchStatus.Playing;
                break;
            case MatchStatus.Playing:
                Console.Clear();
                PrintBoard();
                AnnouncePlayerToMove();
                var chessNotationPosition = AskForChessNotationPosition();
                _chessBoard.RemovePieceFromBoardAt(chessNotationPosition.ToPosition());
                ChangePlayerToMove();
                break;
            case MatchStatus.ExitingGame:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void WaitForPlayers()
    {
        //Not gonna wait 
        Console.WriteLine("Entering Players Magnum and Hikaru");

        EnterPlayer(new ChessPlayer("Magnus",2839,PieceColor.White));
        EnterPlayer(new ChessPlayer("Hikaru",2813,PieceColor.Black));
        
        if(CanStartGame())
            _matchStatus = MatchStatus.Starting;

    }
    


    /// <summary>
    /// BOARD METHODS
    /// </summary>
    private void CreateChessBoard()
    {
        _chessBoard = new ChessBoard();
        _chessBoard.CreateChessBoardInitialPosition();
    }
    private void PrintBoard()
    {
        _chessBoard.PrintBoardExtension();
    }



    /// <summary>
    /// MOVEMENT METHODS
    /// </summary>
    private ChessNotationPosition AskForChessNotationPosition()
    {
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Specify a Column [a - h] or [A - H]");
        var col =  Console.ReadLine();

        Console.WriteLine("Specify a Row [1 - 8]");
        var row = Console.ReadLine();
        
        return new ChessNotationPosition( int.Parse(row),char.Parse(col));
    }

    private void ChangePlayerToMove()
    {
        _toPlay = _toPlay == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }
    
    

    /// <summary>
    /// PLAYER METHODS
    /// </summary>
    private bool CanStartGame()
    {
        return _playerBlack is not null && _playerWhite is not null;
    }
    public void EnterPlayer(ChessPlayer player)
    {
        if (player.PlayingColor == PieceColor.White)
        {
            TryAddWhitePlayer(player);
            return;
        }
        TryAddBlackPlayer(player);
    }
    private void TryAddWhitePlayer(ChessPlayer player)
    {
        if(_playerWhite != null)
            ColorAlreadyTakenException(player);
        _playerWhite = player;
    }
    private void TryAddBlackPlayer(ChessPlayer player)
    {
        if (_playerBlack != null)
            ColorAlreadyTakenException(player);
        _playerBlack = player;
    }
    private void ColorAlreadyTakenException(ChessPlayer player)
    {
        throw new ChessException($"[CHESS MATCH] AnotHer player already is playing as {player.PlayingAs()}");
    }

    
    private ChessPlayer PlayerToMove()
    {
        return _toPlay == PieceColor.White ? _playerWhite : _playerBlack;
    }

    private void AnnouncePlayerToMove()
    {
        var player = PlayerToMove();
        
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine( $"{player.PlayingAs()} Player: {player.Name}'s turn");
    }
    
}