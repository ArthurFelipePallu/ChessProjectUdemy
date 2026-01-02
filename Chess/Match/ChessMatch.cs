using Chess_Console_Project.Board;
using Chess_Console_Project.Chess.Player;
using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Chess_Movement;
using Chess_Console_Project.Chess.Enums;
using Chess_Console_Project.Chess.Exceptions;

namespace Chess_Console_Project.Chess.Match;

public class ChessMatch
{
    private int _movesCount = 0;
    private Piece _lastCapturedPiece = null;
    ChessBoard _chessBoard;
    MatchStatus _matchStatus;
    private PieceColor _toPlay = PieceColor.White;
    private Screen _screen;
    private ChessPlayer _playerWhite;
    private ChessPlayer _playerBlack;
    private ChessMovement _chessMovement;
    

    public ChessMatch()
    {
        _screen = new Screen();
        _matchStatus = MatchStatus.MainMenu;
        _chessMovement = new ChessMovement();
    }

    /// <summary>
    /// MATCH METHODS
    /// </summary>
    public void UpdateMatch()
    {
        switch (_matchStatus)
        {
            case MatchStatus.MainMenu:
                _screen.PrintMainMenu();
                _screen.ScreenWriteAndWaitForEnterToContinue("Welcome to chess console!");
                _matchStatus = MatchStatus.WaitingForPlayers;
                break;
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
                   _screen.PrintBoardAndPlayers(_playerWhite,_playerBlack,_chessBoard,_toPlay);
                    _screen.AnnouncePlayerToMove(GetPlayingPlayer());

                    var originChessNotationPositionPosition = _screen.AskPlayerForPieceInBoard();
                    var piece = _chessBoard.AccessPieceAtChessNotationPosition(originChessNotationPositionPosition);
                    
                    if (piece == null)
                    {
                        _screen.ScreenWriteAndWaitForEnterToContinue($"No piece at {originChessNotationPositionPosition}");
                        break;
                    }
                    if (piece.GetPieceColor() != _toPlay)
                    {
                        _screen.ScreenWriteAndWaitForEnterToContinue($"Piece {piece} does not belong to {_toPlay} player");
                        break;
                    }
                    piece.CalculatePossibleMoves();
                    // _screen.ScreenWriteAndWaitForEnterToContinue("");
                    if( !piece.HasAtLeastOnePossibleMove())
                    {
                        
                        _screen.ScreenWriteAndWaitForEnterToContinue($"The Selected {piece} has no legal moves , please select another piece");
                        break;
                    }
                    _screen.PrintBoardWithPiecePossibleMovesAndPlayers(piece,_playerWhite,_playerBlack,_chessBoard,_toPlay);

                    var destinationChessNotationPosition = _screen.AskPlayerForPieceDestinationInBoard(piece);
                    
                    if (!piece.ChessNotationPositionIsInPossibleMoves(destinationChessNotationPosition))
                    {
                        _screen.ScreenWriteAndWaitForEnterToContinue($"The {piece} can not move to the {destinationChessNotationPosition} square");
                        break;
                    }
                    ExecuteMovement(piece, destinationChessNotationPosition.ToPosition());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (IsKingOfColorInCheckMate(_toPlay))
                {
                    _screen.PrintBoardAndPlayers(_playerWhite,_playerBlack,_chessBoard,_toPlay);
                    _screen.ScreenWriteAndWaitForEnterToContinue($"Player {GetPlayingPlayer().Name} received a checkmate");
                    _matchStatus = MatchStatus.Finished;
                }
                break;
            case MatchStatus.Finished:
                _screen.PrintCrown();
                _screen.ScreenWriteAndWaitForEnterToContinue($"Player {GetNotPlayingPlayer().Name} won the match");
                _matchStatus = MatchStatus.ExitingGame;
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

    public bool IsExitingGame()
    {
        return _matchStatus == MatchStatus.ExitingGame;
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
    ///   MOVEMENT METHODS
    /// </summary>
    private void IncreaseTurnCount()
    {
        _movesCount++;
    }
    private void ExecuteMovement(Piece piece, Position destination)
    {
        var actionMessage = "";
        var movementIsSuccessful = false;
        
        if (IsMovementCastles(piece, destination))
        {
            var castlesDir = GetCastleDirection(piece, destination);
            movementIsSuccessful = MovePieceTo(piece,destination,out actionMessage);
            MoveRookInCastles(piece,castlesDir);
        }
        else if (IsMovementEnPassent(piece, destination))
        {
            var positionToTake = new Position(piece.GetPiecePosition().Row, destination.Column);
            var pieceToTake = _chessBoard.AccessPieceAtPosition(positionToTake);
            _chessBoard.RemovePieceFromPlay(pieceToTake);
            
            movementIsSuccessful = MovePieceTo(piece,destination,out actionMessage);
        }
        else 
            movementIsSuccessful = MovePieceTo(piece,destination,out actionMessage);

        _screen.PrintBoardAndPlayers(_playerWhite,_playerBlack,_chessBoard,_toPlay);
        _chessBoard.SetLastMovedPiece(piece);
        _screen.ScreenWriteAndWaitForEnterToContinue(actionMessage);
        
        
        if(movementIsSuccessful)
        {
            ChangePlayerToMove();
            // _chessMovement.SaveMovement(piece , MovementType.Move,destinationChessNotationPosition);
            
        }
    }
    private bool MovePieceTo(Piece piece, Position destination, out string message)
    {
        var originalPiecePosition = piece.GetPiecePosition();
        _chessBoard.RemovePieceFromBoardAt(originalPiecePosition);
        
        var destinationPiece = _chessBoard.AccessPieceAtPosition(destination);
        if(destinationPiece != null)
        {
            _lastCapturedPiece = destinationPiece;
            _chessBoard.RemovePieceFromPlay(destinationPiece);
        }
        
        _chessBoard.PutPieceAtDestinationPosition(piece, destination);
        
        if (IsKingOfColorInCheck(piece.GetPieceColor()))
        {
            _chessBoard.RemovePieceFromBoardAt(destination);
            _chessBoard.PutPieceAtDestinationPosition(piece,originalPiecePosition);
            if(destinationPiece != null)
                _chessBoard.PutPieceAtDestinationPosition(destinationPiece,destination);
            message = $"[ CHESS MATCH ] Movement is Invalid. The movement {originalPiecePosition.ToChessNotationPosition()} to {destination.ToChessNotationPosition()} left the {piece.GetPieceColor().ToString()} King In Check.";
            return false;
        }    
        piece.IncreaseTimesMoved();
        
        message = destinationPiece == null
            ? $"[ CHESS MATCH ] Piece {piece} moved to destination {destination.ToChessNotationPosition()}"
            : $"[ CHESS MATCH ] Piece [{piece}] took [{destinationPiece}] at destination [{destination.ToChessNotationPosition()}]";


        return true;
    }
    
    /// <summary>
    /// EN PASSENT
    /// </summary>
    private bool IsMovementEnPassent(Piece piece, Position destination)
    {
        if (piece.GetPieceType() != PieceType.Pawn) return false;
        if (piece.GetPiecePosition().Column == destination.Column) return false;
        return _chessBoard.AccessPieceAtPosition(destination) == null;
    }

    /// <summary>
    /// CASTLES
    /// </summary>
    private bool IsMovementCastles(Piece piece, Position destination)
    {
        if (piece.GetPieceType() != PieceType.King) return false;
        var x = destination.Column - piece.GetPiecePosition().Column;
        return Math.Abs(x) == 2;
    }
    private HorizontalDirections GetCastleDirection(Piece king, Position destination)
    {
        var x = destination.Column - king.GetPiecePosition().Column;
        return x < 0 ? HorizontalDirections.Left : HorizontalDirections.Right;
    }
    private void MoveRookInCastles(Piece king , HorizontalDirections castlesDir)
    {
        var rookCol = castlesDir == HorizontalDirections.Left ? 'a' : 'h';
        var rookOriginalChessNotationPosition = new ChessNotationPosition(king.GetPiecePosition().ToChessNotationPosition().Row , rookCol);
        var rook = _chessBoard.AccessPieceAtChessNotationPosition(rookOriginalChessNotationPosition);
        var rookDestinationPosition = new Position(king.GetPiecePosition().Row, king.GetPiecePosition().Column - (int)castlesDir );
        MovePieceTo(rook,rookDestinationPosition,out var actionMessage);

        _screen.ScreenWriteAndWaitForEnterToContinue(actionMessage);
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
    private void ChangePlayerToMove()
    {
        _toPlay = _toPlay == PieceColor.White ? PieceColor.Black : PieceColor.White;
        
        //If Player to move is white again , 1 turn has passed
        if(_toPlay == PieceColor.White)
            IncreaseTurnCount();
    }
    private ChessPlayer GetPlayingPlayer()
    {
        return _toPlay == PieceColor.White ? _playerWhite : _playerBlack;
    }
    private ChessPlayer GetNotPlayingPlayer()
    {
        return _toPlay == PieceColor.White ? _playerBlack : _playerWhite;
    }

    private bool IsKingOfColorInCheck(PieceColor color)
    {
        return _chessBoard.IsKingInCheck(color);
    }
    private bool IsKingOfColorInCheckMate(PieceColor color)
    {
        if(!IsKingOfColorInCheck(color)) return false;

        var lastPieceCapturedHolder = _lastCapturedPiece;
        foreach (var piece in _chessBoard.GetChessPiecesInPlay(color))
        {
            var originPosition = piece.GetPiecePosition();
            var mat = piece.GetAllPossibleMoves();
            for (var i = 0; i < mat.GetLength(0); i++)
            {
                for (var j = 0; j < mat.GetLength(1); j++)
                {
                    if (mat[i, j])
                    {
                        _lastCapturedPiece = null;
                        var destination = new Position(i, j);
                        var moveIsPossible = MovePieceTo(piece, destination, out var msg);
                        
                        if (moveIsPossible)
                        {
                            _chessBoard.RemovePieceFromBoardAt(destination);
                            _chessBoard.PutPieceAtDestinationPosition(piece, originPosition);
                            if(_lastCapturedPiece != null)
                                _chessBoard.ReturnPieceToPlay(_lastCapturedPiece);
                            
                            _lastCapturedPiece = lastPieceCapturedHolder;
                            piece.DecreaseTimesMoved(); 
                            return false;
                        }
                        if(_lastCapturedPiece != null)
                            _chessBoard.ReturnPieceToPlay(_lastCapturedPiece);
                    }
                }   
            }

        }
        _lastCapturedPiece = lastPieceCapturedHolder;
        return true;
    }
}