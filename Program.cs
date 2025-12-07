// See https://aka.ms/new-console-template for more information


using Chess_Console_Project.Chess.Match;

var match = new ChessMatch();


while(!match.IsMatchFinished())
{
    match.UpdateMatch();
}