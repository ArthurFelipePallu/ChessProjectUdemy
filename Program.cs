// See https://aka.ms/new-console-template for more information


using Chess_Console_Project.Chess.Match;

var match = new ChessMatch();

bool PLAY = true;

while(PLAY)
{
    match.UpdateMatch();
    
    
    Console.WriteLine("DO YOU WISH TO CONTINUE??  [ S , N ] ");
    var s = Console.ReadLine();
    var ss = char.Parse(s);
    if (char.ToLower(ss) == 'n')
        PLAY = false;

}