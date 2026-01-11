# Chess Game♟️

A **console-based Chess application written in C#**, created to practice object-oriented programming, game logic, and C# fundamentals taught throughout a Udemy course.

This project lets two players play a full game of chess in the console, with **legal move validation, turn management, and core chess rules implemented**.

---

## 📌 What This Project Is

**ChessProjectUdemy** is a learning project demonstrating how to model a real game (chess) using C#.  
It includes:

✔ Chess board representation  
✔ Piece movement rules  
✔ Turn-based gameplay  
✔ Captures, check, and checkmate detection  
✔ Console user input and board rendering  

It’s designed to strengthen understanding of:
- OOP (classes, inheritance, encapsulation)
- Collections (`List`, arrays, enums)
- Game loops and command parsing  
- Logic and state validation

----------------------------------------------------------------------------

## 📁 Project Structure
ChessProjectUdemy/
- ├── Board/
- │ └── Board.cs # Chess board representation & draw logic
- ├── Chess/
- │ ├── Piece.cs # Base class for chess pieces
- │ ├── Pawn.cs # Pawn logic
- │ ├── Rook.cs
- │ ├── Knight.cs
- │ ├── Bishop.cs
- │ ├── Queen.cs
- │ ├── King.cs
- │ └── MoveValidator.cs # Valid moves computation
- ├── Program.cs # Entry point + game loop
- ├── Chess Console Project.csproj # .NET project file
- ├── ChessProjectUdemy.sln # Solution file


----------------------------------------------------------------------------

## 🚀 How to Install & Run

### 📌 Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) **.NET 6.0 or above**
- A code editor (Visual Studio / Visual Studio Code)

### 📥 Clone the Repo

```bash
git clone https://github.com/ArthurFelipePallu/ChessProjectUdemy.git
cd ChessProjectUdemy 
```

### 📦 Restore & Build

Visual Studio
- Open the .sln file
- Press F5 to build and run

.NET CLI
- dotnet restore
- dotnet build
- dotnet run --project "Chess Console Project.csproj"


----------------------------------------------------------------------------

## 🎮 How to Play

This is a two-player game at the terminal/console.

### ♟️ Chessboard Overview

The chessboard is displayed using ASCII characters with a clear coordinate system:

- **Columns (files)** are labeled from **A to H** (shown at the bottom)
- **Rows (ranks)** are labeled from **1 to 8** (shown on the left)
- Each square alternates color to represent a standard chessboard
- Pieces are represented by symbols/icons in their starting positions

### Example board layout:
![Example board layout](Screenshots/initialBoard.png)
  
- Top side (rank 8) belongs to Black
- Bottom side (rank 1) belongs to White

### 🎯 Move Format

Enter moves using algebraic board coordinates:

<from> <to>
-   e2  -> Selects the piece at e2
-   e4  -> Moves the piece of e2 to e4

### 🔍 Piece Selection & Move Highlighting

When a player selects a piece, the game visually highlights all **valid moves**
available for that piece directly on the board.

![Example board layout](Screenshots/d2PawnChosen.png)
![Example board layout](Screenshots/f6KnightChosen.png)
![Example board layout](Screenshots/e8KingPossibleMoves.png)

### 🟥 Highlighted Squares
- Highlighted squares indicate **legal destinations** for the selected piece
- Only moves that follow **standard chess rules** are shown
- This helps players quickly understand their available options

### ♟️ How It Works
1. The player selects a piece by entering its position (as part of a move)
2. The board updates and displays highlighted squares
3. The player chooses one of the highlighted destinations to complete the move

## ❗ Invalid Destination Handling
- If the destination square is **not highlighted**, the move is rejected
- The player is notified that the move is invalid
- The game then waits for a new input

## ♟️ Captured Pieces Display
The game keeps track of all captured pieces and displays them on the screen
for easy reference during gameplay.

- Captured pieces are shown grouped by player
- The display updates immediately after a capture
- This allows players to quickly see the material advantage at any point

----------------------------------------------------------------------------

## 🧩 Features Implemented

- ✔ Chess board display in console
- ✔ Turn-based gameplay
- ✔ Legal move validation for all pieces
- ✔ Capture logic
- ✔ Show Captured Pieces
- ✔ Check and checkmate detection
- ✔ En passant capture
- ✔ Pawn promotion
- ✔ Castling (king- and queen-side)
- ✔ Structured using OOP best practices

This project models most standard chess rules and enforces legal play throughout.

----------------------------------------------------------------------------

## 📌 Future Improvements

### 🤖 AI Opponent
### 🔁 Move History & Undo
### 🧠 Save / Load Game States
### 🖼 GUI Interface (Windows Forms / WPF / Unity)











