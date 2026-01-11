# ChessProjectUdemy ♟️

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
Open the .sln file
Press F5 to build and run

.NET CLI
dotnet restore
dotnet build
dotnet run --project "Chess Console Project.csproj"


----------------------------------------------------------------------------

## 🎮 How to Play

This is a two-player game at the terminal/console.

### 🎯 Move Format

Enter moves using algebraic board coordinates:

<from> <to>
  e2    e4
→ Moves the Pawn from e2 to e4.

----------------------------------------------------------------------------

## 🧩 Features Implemented

- ✔ Chess board display in console
- ✔ Turn-based gameplay
- ✔ Legal move validation for all pieces
- ✔ Capture logic
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











