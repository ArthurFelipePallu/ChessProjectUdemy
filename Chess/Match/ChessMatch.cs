using System.Drawing;
using System.Runtime.Intrinsics.X86;
using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Exceptions;
using Chess_Console_Project.Chess.Player;

namespace Chess_Console_Project.Chess.Match;

public class ChessMatch
{
    private int _movesCount = 0;
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
                PressEnterToContinue();
                break;
            case MatchStatus.WaitingForPlayer:
                break;
            case MatchStatus.Starting:
                CreateChessBoard();

                _matchStatus = MatchStatus.Playing;
                break;
            case MatchStatus.Playing:
                Console.Clear();
                try
                {
                    PrintBoard();
                    AnnouncePlayerToMove();
                    var originChessNotationPositionPosition = AskPlayerForPieceInBoard();
                    var piece = _chessBoard.AccessPieceAtChessNotationPosition(originChessNotationPositionPosition);
                    piece.CalculatePossibleMoves();
                    
                    Console.Clear();
                    PrintBoardWithPiecePossibleMovements(piece.GetAllPossibleMoves());
                    // piece.PrintPiecePossibleMovesExtension();
                    
                    var destinationChessNotationPositionPosition = AskPlayerForPieceDestinationInBoard(piece);
                    ExecuteMovement(originChessNotationPositionPosition.ToPosition(), destinationChessNotationPositionPosition.ToPosition());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                PressEnterToContinue();

                
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

    public bool IsMatchFinished()
    {
        return _matchStatus == MatchStatus.Finished;
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
        Console.Clear();
        _chessBoard.PrintBoardExtension();
    }
    private void PrintBoardWithPiecePossibleMovements(bool[,] possibleMoves)
    {
        Console.Clear();
        _chessBoard.PrintBoardExtension(possibleMoves);
    }


    /// <summary>
    /// MOVEMENT METHODS
    /// </summary>
    private void IncreaseTurnCount()
    {
        _movesCount++;
    }

    private ChessNotationPosition AskPlayerForPieceInBoard()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Choose a Piece to move");
        return GetChessNotationPosition();
    }
    private ChessNotationPosition AskPlayerForPieceDestinationInBoard(Piece piece)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Choose a Position to move your {piece} ");
        return GetChessNotationPosition();
    }
    private ChessNotationPosition GetChessNotationPosition()
    {

        Console.WriteLine($"Specify a Column [a - h] or [A - H] followed by a Row [1 - 8]. {Environment.NewLine} EX : 'A2' or 'a2'    ");
        var notation =  Console.ReadLine();

        if (notation == null || notation.Length != 2) throw new ChessException("[ CHESS MATCH] Notation specified by player is in wrong format");
        var col = notation[0];
        var row = (int)notation[1] - '0';

        return new ChessNotationPosition( row,col);
    }

    private void ExecuteMovement(Position origin, Position destination)
    {
        try
        {
            _chessBoard.ValidateBoardPosition(origin);
            _chessBoard.ValidateBoardPosition(destination);
            
            
            PieceAtPositionExistsAndBelongsToPlayer(origin);
            DestinationPositionCanBeMovedToOrTaken(destination);
    
            var action = MovePieceFromTo(origin,destination);
            
            PrintBoard();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(action);
            
        }
        catch (BoardException e)
        {
            Console.WriteLine("[CHESS MATCH] [CHESS BOARD] Problem with Board");
            Console.WriteLine(e.Message);
        }
        catch (MovementException e)
        {
            Console.WriteLine("[CHESS MATCH] [ MOVEMENT ] Problem with movement ");
            Console.WriteLine(e.Message);
        }
    }

    private void PieceAtPositionExistsAndBelongsToPlayer(Position pos)
    {
        var piece = _chessBoard.AccessPieceAtPosition(pos);
        if (piece == null)
            throw new MovementException($"[ CHESS MATCH ] Piece at position {pos.ToString()} does not exist");
        
        
        // If piece does not exists or is a piece that does not belongs to the current player
        if (piece.GetPieceColor() != _toPlay)
            throw new MovementException($"[ CHESS MATCH ] Piece at position {pos.ToString()} does not belong to player {_toPlay.ToString()}");
            
    }

    private void DestinationPositionCanBeMovedToOrTaken(Position destination)
    {
        //Verify if destination position has piece
        // if it does not , it can be moved to
        var piece = _chessBoard.AccessPieceAtPosition(destination);
        if (piece == null)
            return;

        // If there is piece at Destination position , it cannot be moved to destination
        // if piece at destination also belongs to current Player
        // if it belongs to opponent , it can be taken
        if (piece.GetPieceColor() == _toPlay)
            throw new MovementException($"[ CHESS MATCH ] Piece at destination {destination.ToString()} belongs to player {_toPlay.ToString()}, it can not be taken.");

    }

    private string MovePieceFromTo(Position origin, Position destination)
    {
        var originPiece = _chessBoard.RemovePieceFromBoardAt(origin);
        originPiece.IncreaseTimesMoved();
        
        var destinationPiece = _chessBoard.AccessPieceAtPosition(destination);
        _chessBoard.PutPieceAtDestinationPosition(originPiece, destination);
        
        return destinationPiece == null
            ? $"[ CHESS MATCH ] Piece {originPiece} moved to destination {destination.ToString()}"
            : $"[ CHESS MATCH ] Piece [{originPiece}] took [{destinationPiece}] at destination [{destination}]";

    }
    
    
    
    private void ChangePlayerToMove()
    {
        _toPlay = _toPlay == PieceColor.White ? PieceColor.Black : PieceColor.White;
        
        //If Player to move is white again , 1 turn has passed
        if(_toPlay == PieceColor.White)
            IncreaseTurnCount();
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