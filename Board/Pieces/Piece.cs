namespace Chess_Console_Project.Board;

public class Piece
{
    /// <summary>
    /// PRIVATE
    /// </summary>
    
    public int Value;
    public Color Color;
    public string Name;
    public char Symbol;
    public Position Position;
    public ChessBoard Board;
    public int TimesMoved {get; protected set;}



    public Piece(ChessBoard board, int value, Color color, string name, char symbol,Position pos)
    {
        Name = name;
        Symbol = symbol;
        Value = value;
        Color = color;
        Board = board;
        Position = pos;
        TimesMoved = 0;
    }
}