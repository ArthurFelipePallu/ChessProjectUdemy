using Chess_Console_Project.Board;
using Chess_Console_Project.Chess.Player;
using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Chess.Exceptions;

namespace Chess_Console_Project.Chess.Match;

public class ChessMatch
{
    private int _movesCount = 0;
    ChessBoard _chessBoard;
    MatchStatus _matchStatus;
    private PieceColor _toPlay = PieceColor.White;
    private Screen _screen;
    private ChessPlayer _playerWhite;
    private ChessPlayer _playerBlack;

    public ChessMatch()
    {
        _screen = new Screen();
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
                _screen.ScreenWriteAndWaitForEnterToContinue("Players have gathered, Starting Game");
                break;
            case MatchStatus.WaitingForPlayer:
                break;
            case MatchStatus.Starting:
                CreateChessBoard();

                _matchStatus = MatchStatus.Playing;
                break;
            case MatchStatus.Playing:
                try
                {
                    _screen.PrintBoardAndPlayerToMove(_chessBoard,PlayerToMove());

                    var originChessNotationPositionPosition = _screen.AskPlayerForPieceInBoard();
                    var piece = _chessBoard.AccessPieceAtChessNotationPosition(originChessNotationPositionPosition);
                    if (piece == null)
                    {
                        _screen.ScreenWriteAndWaitForEnterToContinue($"No piece at {originChessNotationPositionPosition}");
                        break;
                    }
                    piece.CalculatePossibleMoves();
                    if( !piece.HasAtLeastOnePossibleMove())
                    {
                        _screen.ScreenWriteAndWaitForEnterToContinue($"The Selected {piece} has no legal moves , please select another piece");
                        break;
                    }
                    
                    _screen.PrintBoardWithPiecePossibleMovements(_chessBoard,piece.GetAllPossibleMoves());
                    
                    var destinationChessNotationPosition = _screen.AskPlayerForPieceDestinationInBoard(piece);

                    if (!piece.PositionIsInPossibleMoves(destinationChessNotationPosition.ToPosition()))
                    {
                        _screen.ScreenWriteAndWaitForEnterToContinue($"The {piece} can not move to the {destinationChessNotationPosition} square");
                        break;
                    }
                    ExecuteMovement(originChessNotationPositionPosition.ToPosition(), destinationChessNotationPosition.ToPosition());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                _screen.ScreenWriteAndWaitForEnterToContinue($"{PlayerToMove().Name}'s turn is over ");
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



    /// <summary>
    /// BOARD METHODS
    /// </summary>
    private void CreateChessBoard()
    {
        _chessBoard = new ChessBoard();
        _chessBoard.CreateChessBoardInitialPosition();
    }



    /// <summary>
    /// MOVEMENT METHODS
    /// </summary>
    private void IncreaseTurnCount()
    {
        _movesCount++;
    }
    private void ExecuteMovement(Position origin, Position destination)
    {
        try
        {
            _chessBoard.ValidateBoardPosition(origin);
            _chessBoard.ValidateBoardPosition(destination);
            
            
            PieceAtPositionExistsAndBelongsToPlayer(origin);
            DestinationPositionCanBeMovedToOrTaken(destination);
    
            var actionMessage = MovePieceFromTo(origin,destination);
            
            _screen.PrintBoard(_chessBoard);
            _screen.ScreenWriteAndWaitForEnterToContinue(actionMessage);

        }
        catch (BoardException e)
        {
            _screen.ScreenWriteAndWaitForEnterToContinue("[CHESS MATCH] [CHESS BOARD] Problem with Board" + e.Message);
        }
        catch (MovementException e)
        {
            _screen.ScreenWriteAndWaitForEnterToContinue("[CHESS MATCH] [ MOVEMENT ] Problem with movement " + e.Message);
        }
        ChangePlayerToMove();
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
            ? $"[ CHESS MATCH ] Piece {originPiece} moved to destination {destination}"
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
 
    
}