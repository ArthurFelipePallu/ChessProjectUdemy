namespace Chess_Console_Project.Chess.Exceptions;

public enum ChessErrorCode
{
    Unknown = 0,
    InvalidMove = 1,
    IllegalState = 2,
    PieceNotFound = 3,
    CheckViolation = 4,
    PromotionError = 5,
    ParseError = 6
}

public class ChessException : Exception
{
    public ChessErrorCode ErrorCode { get; }
    public string? FromSquare { get; }
    public string? ToSquare { get; }
    public string? Piece { get; }

    public ChessException(
        string message,
        ChessErrorCode errorCode = ChessErrorCode.Unknown,
        string? fromSquare = null,
        string? toSquare = null,
        string? piece = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        FromSquare = fromSquare;
        ToSquare = toSquare;
        Piece = piece;
    }

    public override string ToString()
    {
        var baseText = base.ToString();
        var context = $"Code: {ErrorCode}";
        
        if (!string.IsNullOrEmpty(FromSquare))
            context += $", From: {FromSquare}";
        if (!string.IsNullOrEmpty(ToSquare))
            context += $", To: {ToSquare}";
        if (!string.IsNullOrEmpty(Piece))
            context += $", Piece: {Piece}";
        
        return $"{baseText} ({context})";
    }
}

public class MovementException : ChessException
{
    public MovementException(
        string message,
        string? fromSquare = null,
        string? toSquare = null,
        string? piece = null,
        Exception? innerException = null)
        : base(message, ChessErrorCode.InvalidMove, fromSquare, toSquare, piece, innerException)
    {
    }
}