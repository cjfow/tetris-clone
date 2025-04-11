using TetrisCloneLibrary.BlockTypes;

namespace TetrisCloneLibrary.GameLogic;

public class BlockQueue
{
    private readonly BlockFormat[] blocks =
    [
        new IBlock(),
        new JBlock(),
        new LBlock(),
        new OBlock(),
        new SBlock(),
        new TBlock(),
        new ZBlock()
    ];

    private readonly Random _random = new();

    public BlockFormat NextBlock { get; private set; }

    public BlockQueue()
    {
        NextBlock = RandomBlock();
    }

    private BlockFormat RandomBlock()
    {
        return blocks[_random.Next(blocks.Length)];
    }

    public BlockFormat GetAndUpdate()
    {
        BlockFormat block = NextBlock;

        do
        {
            NextBlock = RandomBlock();
        }
        while (block.BlockID == NextBlock.BlockID);

        return block;
    }
}
