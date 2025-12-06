// See https://aka.ms/new-console-template for more information

using Chess_Console_Project.Board;

var chessBoard = new ChessBoard();

chessBoard.CreateChessBoardInitialPosition();

chessBoard.PrintBoardExtension();

Console.WriteLine("Specify a Column [a - h] or [A - H]");
var col =  Console.ReadLine();

Console.WriteLine("Specify a Row [1 - 8]");
var row = Console.ReadLine();

var newPosition = new ChessNotationPosition( int.Parse(row),char.Parse(col));

Console.WriteLine($"Position {newPosition.ToPosition()}");

var piece = chessBoard.AccessPieceAtChessNotationPosition(newPosition);

Console.WriteLine(piece.GetPieceColor() + " "+  piece.GetPieceTypeAsString());

