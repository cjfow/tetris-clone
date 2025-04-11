namespace TetrisCloneLibrary.GameLogic;

public abstract class BlockFormat
{
    protected abstract Position[][] Tiles { get; }
    protected abstract Position InitialOffset { get; }
    public abstract int BlockID { get; }

    private int _rotationState;
    private readonly Position _offset;

    public BlockFormat()
    {
        _offset = new Position(InitialOffset.Row, InitialOffset.Column);
    }

    public IEnumerable<Position> TilePositions()
    {
        foreach (Position p in Tiles[_rotationState])
        {
            yield return new Position(p.Row + _offset.Row, p.Column + _offset.Column);
        }
    }

    public void RotateClockwise()
    {
        _rotationState = (_rotationState + 1) % Tiles.Length;
    }

    public void RotateCounterClockwise()
    {
        if (_rotationState == 0)
        {
            _rotationState = Tiles.Length - 1;
        }
        else
        {
            _rotationState--;
        }
    }

    public void MoveByOffset(int rows, int columns)
    {
        _offset.Row += rows;
        _offset.Column += columns;
    }

    public void ResetBlock()
    {
        _rotationState = 0;
        _offset.Row = InitialOffset.Row;
        _offset.Column = InitialOffset.Column;
    }
}
