namespace Chess_Console_Project.Board;

public class Position(int line, int column)
{
    public int Line { get; } = line;
    public int Column { get; } = column;


    public override string ToString()
    {
        return "L: " + Line + ", " + "C: " + Column;
    }
}