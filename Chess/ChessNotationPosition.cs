using Chess_Console_Project.Board.Exceptions;
using Chess_Console_Project.Chess.Exceptions;

namespace Chess_Console_Project.Board;

public struct ChessNotationPosition
{
    private const int MaxChessBoardSize = 8;
    private int _row;  // 1 - 8
    private char _col;// A - H

    public int Row { get => _row; set => _row = value; }
    public char Col { get => _col; set => _col = char.ToUpper(value); }
    public int ColAsInt => (int)_col;
    
    // Array index properties (0-7) for accessing board arrays
    public int RowIndex => MaxChessBoardSize - _row;  // Converts 1-8 to 0-7
    public int ColumnIndex => _col - 65;  // Converts A-H to 0-7
    
    public ChessNotationPosition(int row, char col) 
    {
        ValidateRow(row);
        ValidateColumn(col);
    }
    
    // Constructor from array indices (0-7)
    public ChessNotationPosition(int rowIndex, int columnIndex, bool fromArrayIndices = true)
    {
        if (fromArrayIndices)
        {
            if (rowIndex < 0 || rowIndex >= MaxChessBoardSize || columnIndex < 0 || columnIndex >= MaxChessBoardSize)
                throw new ChessException(
                    message: $"[CHESS NOTATION POSITION] Array indices out of range: row={rowIndex}, col={columnIndex}",
                    ChessErrorCode.ParseError);
            
            _row = MaxChessBoardSize - rowIndex;  // Convert 0-7 to 1-8
            _col = (char)(columnIndex + 65);  // Convert 0-7 to A-H
            ValidateRow(_row);
            ValidateColumn(_col);
        }
        else
        {
            // Regular constructor behavior
            _row = rowIndex;
            _col = (char)columnIndex;
            ValidateRow(_row);
            ValidateColumn(_col);
        }
    }

    private void ValidateRow(int row)
    {
        if(row is < 1 or > MaxChessBoardSize)
            throw new ChessException(
                message: $"[CHESS NOTATION POSITION] Row {row} is out of range [1 - 8]",
                ChessErrorCode.ParseError);
        _row = row;
    }
    private void ValidateColumn(char col)
    {
        col = char.ToUpper(col);
        if(col is < 'A' or > 'H')
            throw new ChessException(
                message: $"[CHESS NOTATION POSITION] Column {col} is out of range [a - h]",
                ChessErrorCode.ParseError);
        _col = col;
    }
    
    // Helper method to create from array indices
    public static ChessNotationPosition FromArrayIndices(int rowIndex, int columnIndex)
    {
        return new ChessNotationPosition(rowIndex, columnIndex, fromArrayIndices: true);
    }
    
    // Helper method to offset position (useful for movement calculations)
    public ChessNotationPosition Offset(int rowOffset, int columnOffset)
    {
        var newRowIndex = RowIndex + rowOffset;
        var newColumnIndex = ColumnIndex + columnOffset;
        
        if (newRowIndex < 0 || newRowIndex >= MaxChessBoardSize || 
            newColumnIndex < 0 || newColumnIndex >= MaxChessBoardSize)
        {
            throw new MovementException(
                $"Position offset out of bounds: ({rowOffset}, {columnOffset})",
                fromSquare: ToString());
        }
        
        return FromArrayIndices(newRowIndex, newColumnIndex);
    }

    public override string ToString()
    {
        return $"{_col}{_row}".ToLower();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is ChessNotationPosition other)
        {
            return Row == other.Row && Col == other.Col;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col);
    }
}