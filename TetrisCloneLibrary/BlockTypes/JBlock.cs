
using TetrisCloneLibrary.GameLogic;

namespace TetrisCloneLibrary.BlockTypes;

public class JBlock : BlockFormat
{
    public override int BlockID => 2;
    protected override Position InitialOffset => new(0, 3);

    protected override Position[][] Tiles => [
        [new(0, 0), new(1, 0), new(1, 1), new(1, 2)],
        [new(0, 1), new(0, 2), new(1, 1), new(2, 1)],
        [new(1, 0), new(1, 1), new(1, 2), new(2, 2)],
        [new(0, 1), new(1, 1), new(2, 1), new(2, 0)]
    ];
}
