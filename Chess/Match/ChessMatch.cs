using Chess_Console_Project.Board;
using Chess_Console_Project.Chess.Player;
using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Chess_Movement;
using Chess_Console_Project.Chess.ChessPieces;
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
                AlterMatchStatus(MatchStatus.WaitingForPlayers);
                break;
            case MatchStatus.WaitingForPlayers:
                WaitForPlayers();
                _screen.ScreenWriteAndWaitForEnterToContinue("Players have gathered, Starting Game");
                break;
            case MatchStatus.WaitingForPlayer:
                //Implement Later with FrontEnd
                break;
            case MatchStatus.Starting:
                CreateChessBoard();
                AlterMatchStatus(MatchStatus.Playing);
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
                    ExecuteMovement(piece, destinationChessNotationPosition);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (IsKingOfColorInCheckMate(_toPlay))
                {
                    _screen.PrintBoardAndPlayers(_playerWhite,_playerBlack,_chessBoard,_toPlay);
                    _screen.ScreenWriteAndWaitForEnterToContinue($"Player {GetPlayingPlayer().Name} received a checkmate");
                    AlterMatchStatus(MatchStatus.Finished);
                }
                break;
            case MatchStatus.Finished:
                _screen.PrintCrown();
                _screen.ScreenWriteAndWaitForEnterToContinue($"Player {GetNotPlayingPlayer().Name} won the match");
                AlterMatchStatus(MatchStatus.ExitingGame);
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
        EnterPlayer(new ChessPlayer("Elysia",2839,PieceColor.White));
        EnterPlayer(new ChessPlayer("Valeron",2813,PieceColor.Black));
        if(CanStartGame())
            _matchStatus = MatchStatus.Starting;
    }

    public bool IsExitingGame()
    {
        return _matchStatus == MatchStatus.ExitingGame;
    }

    
    
    /// <summary>
    /// MATCH METHODS
    /// </summary>
    private void AlterMatchStatus(MatchStatus status)
    {
        _matchStatus = status;
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
    private void ExecuteMovement(Piece piece, ChessNotationPosition destination)
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
            var positionToTake = new ChessNotationPosition(piece.GetPiecePosition().Row, destination.Col);
            var pieceToTake = _chessBoard.AccessPieceAtChessNotationPosition(positionToTake);
            _chessBoard.RemovePieceFromPlay(pieceToTake);
            
            movementIsSuccessful = MovePieceTo(piece,destination,out actionMessage);
        }
        else 
            movementIsSuccessful = MovePieceTo(piece,destination,out actionMessage);

        _screen.PrintBoardAndPlayers(_playerWhite,_playerBlack,_chessBoard,_toPlay);
        _chessBoard.SetLastMovedPiece(piece);
        _screen.ScreenWriteAndWaitForEnterToContinue(actionMessage);
        
        if (piece is Pawn pawn)
        {
            if (pawn.CanPromote())
            {
                var pawnPromotesTo = _screen.AskForPawnPromotion();
                var pawnChessNotationPos = pawn.GetPiecePosition();
                _chessBoard.RemovePieceFromBoardAt(pawnChessNotationPos);
                _chessBoard.AddPlayingPiece(pawn.GetPieceColor(),pawnPromotesTo,pawnChessNotationPos.Col,pawnChessNotationPos.Row);
                
                _screen.PrintBoardAndPlayers(_playerWhite,_playerBlack,_chessBoard,_toPlay);
                _screen.ScreenWriteAndWaitForEnterToContinue($"{pawn} promoted to {pawn.GetPieceColor()} {pawnPromotesTo} ");
            }
        }
        
        if(movementIsSuccessful)
        {
            ChangePlayerToMove();
            // _chessMovement.SaveMovement(piece , MovementType.Move,destinationChessNotationPosition);
            
        }
    }
    private bool MovePieceTo(Piece piece, ChessNotationPosition destination, out string message)
    {
        var originalPiecePosition = piece.GetPiecePosition();
        _chessBoard.RemovePieceFromBoardAt(originalPiecePosition);
        
        var destinationPiece = _chessBoard.AccessPieceAtChessNotationPosition(destination);
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

            throw new ChessException(
                message: $"Movement is Invalid. The movement left the {piece.GetPieceColor()} King In Check.",
                ChessErrorCode.CheckViolation,
                fromSquare: originalPiecePosition.ToString(),
                toSquare: destination.ToString(),
                piece: piece.ToString());
        }    
        piece.IncreaseTimesMoved();
        
        message = destinationPiece == null
            ? $"[ CHESS MATCH ] Piece {piece} moved to destination {destination}"
            : $"[ CHESS MATCH ] Piece [{piece}] took [{destinationPiece}] at destination [{destination}]";


        return true;
    }
    
    /// <summary>
    /// EN PASSENT
    /// </summary>
    private bool IsMovementEnPassent(Piece piece, ChessNotationPosition destination)
    {
        if (piece.GetPieceType() != PieceType.Pawn) return false;
        if (piece.GetPiecePosition().Col == destination.Col) return false;
        return _chessBoard.AccessPieceAtChessNotationPosition(destination) == null;
    }

    /// <summary>
    /// CASTLES
    /// </summary>
    private bool IsMovementCastles(Piece piece, ChessNotationPosition destination)
    {
        if (piece.GetPieceType() != PieceType.King) return false;
        var x = destination.ColumnIndex - piece.GetPiecePosition().ColumnIndex;
        return Math.Abs(x) == 2;
    }
    private HorizontalDirections GetCastleDirection(Piece king, ChessNotationPosition destination)
    {
        var x = destination.ColumnIndex - king.GetPiecePosition().ColumnIndex;
        return x < 0 ? HorizontalDirections.Left : HorizontalDirections.Right;
    }
    private void MoveRookInCastles(Piece king , HorizontalDirections castlesDir)
    {
        var rookCol = castlesDir == HorizontalDirections.Left ? 'a' : 'h';
        var kingPos = king.GetPiecePosition();
        var rookOriginalChessNotationPosition = new ChessNotationPosition(kingPos.Row, rookCol);
        var rook = _chessBoard.AccessPieceAtChessNotationPosition(rookOriginalChessNotationPosition);
        var rookDestinationColumnIndex = kingPos.ColumnIndex - (int)castlesDir;
        var rookDestinationPosition = ChessNotationPosition.FromArrayIndices(kingPos.RowIndex, rookDestinationColumnIndex);
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
            throw new ChessException(
                message: $"[CHESS MATCH] Another player already is playing as {player.PlayingAs()}",
                ChessErrorCode.IllegalState);
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
                        var destination = ChessNotationPosition.FromArrayIndices(i, j);
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