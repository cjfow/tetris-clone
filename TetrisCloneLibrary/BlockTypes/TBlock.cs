
using TetrisCloneLibrary.GameLogic;

namespace TetrisCloneLibrary.BlockTypes;

public class TBlock : BlockFormat
{
    public override int BlockID => 6;
    protected override Position InitialOffset => new(0, 3);

    protected override Position[][] Tiles => [
        [new(0,1), new(1,0), new(1,1), new(1,2)],
        [new(0,1), new(1,1), new(1,2), new(2,1)],
        [new(1,0), new(1,1), new(1,2), new(2,1)],
        [new(0,1), new(1,0), new(1,1), new(2,1)]
    ];
}
