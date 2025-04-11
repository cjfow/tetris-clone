namespace TetrisCloneLibrary.GameLogic;

public class GameState
{
    private BlockFormat _currentBlock;

    public BlockFormat CurrentBlock
    {
        get => _currentBlock;
        private set
        {
            _currentBlock = value;
            _currentBlock.ResetBlock();

            for (int i = 0; i < 2; i++)
            {
                _currentBlock.MoveByOffset(1, 0);

                if (!BlockFits())
                {
                    _currentBlock.MoveByOffset(-1, 0);
                }
            }
        }
    }

    public GameGrid GameGrid { get; }
    public BlockQueue BlockQueue { get; }
    public bool GameOver { get; private set; }
    public int Score { get; private set; }
    public BlockFormat? HeldBlock { get; private set; }
    public bool CanHold { get; private set; }

    public GameState()
    {
        GameGrid = new GameGrid(22, 10);
        BlockQueue = new BlockQueue();
        _currentBlock = BlockQueue.GetAndUpdate();
        CurrentBlock = _currentBlock;
        CanHold = true;
    }

    private bool BlockFits()
    {
        var tilePositions = CurrentBlock.TilePositions();

        foreach (Position p in tilePositions)
        {
            if (!GameGrid.IsEmpty(p.Row, p.Column))
            {
                return false;
            }
        }

        return true;
    }

    public void HoldBlock()
    {
        if (!CanHold)
        {
            return;
        }

        if (HeldBlock == null)
        {
            HeldBlock = CurrentBlock;
            CurrentBlock = BlockQueue.GetAndUpdate();
        }
        else
        {
            (HeldBlock, CurrentBlock) = (CurrentBlock, HeldBlock);
        }

        CanHold = false;
    }

    public void RotateBlockCW()
    {
        CurrentBlock.RotateClockwise();

        if (!BlockFits())
        {
            CurrentBlock.RotateCounterClockwise();
        }
    }

    public void RotateBlockCCW()
    {
        CurrentBlock.RotateCounterClockwise();

        if (!BlockFits())
        {
            CurrentBlock.RotateClockwise();
        }
    }

    public void MoveBlockLeft()
    {
        CurrentBlock.MoveByOffset(0, -1);

        if (!BlockFits())
        {
            CurrentBlock.MoveByOffset(0, 1);
        }
    }

    public void MoveBlockRight()
    {
        CurrentBlock.MoveByOffset(0, 1);

        if (!BlockFits())
        {
            CurrentBlock.MoveByOffset(0, -1);
        }
    }

    private bool IsGameOver()
    {
        return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
    }

    private void PlaceBlock()
    {
        foreach (Position p in CurrentBlock.TilePositions())
        {
            GameGrid[p.Row, p.Column] = CurrentBlock.BlockID;
        }

        Score += GameGrid.ClearFullRows();

        if (IsGameOver())
        {
            GameOver = true;
        }
        else
        {
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }
    }

    public void MoveBlockDown()
    {
        CurrentBlock.MoveByOffset(1, 0);

        if (!BlockFits())
        {
            CurrentBlock.MoveByOffset(-1, 0);
            PlaceBlock();
        }
    }

    private int TileDropDistance(Position p)
    {
        int drop = 0;

        while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
        {
            drop++;
        }

        return drop;
    }

    public int BlockDropDistance()
    {
        int drop = GameGrid.Rows;

        foreach (Position p in CurrentBlock.TilePositions())
        {
            drop = Math.Min(drop, TileDropDistance(p));
        }

        return drop;
    }

    public void DropBlock()
    {
        CurrentBlock.MoveByOffset(BlockDropDistance(), 0);
        PlaceBlock();
    }
}
