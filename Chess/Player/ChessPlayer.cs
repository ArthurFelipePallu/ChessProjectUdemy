using Chess_Console_Project.Board;

namespace Chess_Console_Project.Chess.Player;

public class ChessPlayer
{
    public string Name { get; set; }
    public int Elo { get; set; }
    public PieceColor PlayingColor { get; set; }

    public ChessPlayer(string name, int elo, PieceColor playingColor)
    {
        Name = name;
        Elo = elo;
        PlayingColor = playingColor;
    }

    public string PlayingAs()
    {
        return PlayingColor.ToString();
    }
}