using System.Diagnostics;
using Chess_Console_Project.Board;
using Chess_Console_Project.Board.Pieces;
using Chess_Console_Project.Chess.Enums;

namespace Chess_Console_Project.Chess.Chess_Movement;

public class ChessMovement
{
    private List<string> _movesWhite;
    private List<string> _movesBlack;

    public ChessMovement()
    {
        _movesWhite = [];
        _movesBlack = [];
    }
    
    public void SaveMovement(Piece piece,MovementType move, ChessNotationPosition destination)
    {
        try
        {
            var toSaveNotation = "";
            switch (move)
            {
                case MovementType.Any:
                    ThrowMovementNotationException();
                    break;
                case MovementType.Move:
                    toSaveNotation = GetMovementChessNotation(piece,destination);
                    break;
                case MovementType.Take:
                   // toSaveNotation = GetTakeChessNotation();
                    break;
                case MovementType.Check:
                    //toSaveNotation = GetMovementChessNotation();
                    break;
                case MovementType.IllegalMove:
                    ThrowMovementNotationException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(move), move, null);
            }
            
            SaveChessMovement(piece.GetPieceColor(),toSaveNotation);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private string GetMovementChessNotation(Piece piece , ChessNotationPosition destination)
    {
        if (piece.GetPieceType() == PieceType.Pawn)
        {
            return destination.ToString().ToLower();
        }

        if (piece.GetPieceType() == PieceType.Rook || 
            piece.GetPieceType() == PieceType.Bishop ||
            piece.GetPieceType() == PieceType.Knight )
        {
            //CHECK IF OTHER PIECE OF THE SAME TYPE CAN ALSO REACH 
            
        }
        return $"{piece.GetPieceNotation()}{destination.ToString()}";
    }
    // private string GetTakeChessNotation()
    // {
    //     
    // }
    // private string GetCheckChessNotation()
    // {
    //     
    // }
    private void SaveChessMovement(PieceColor pieceColor,string toSaveMovement)
    {
        if(pieceColor == PieceColor.White)
            _movesWhite.Add(toSaveMovement);
        else
            _movesBlack.Add(toSaveMovement);
        
        Console.WriteLine($"[CHESS MOVEMENT]:{ toSaveMovement}");
    }
    private void ThrowMovementNotationException()
    {
        
    }
}