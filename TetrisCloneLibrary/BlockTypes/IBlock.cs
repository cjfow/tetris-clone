
using TetrisCloneLibrary.GameLogic;

namespace TetrisCloneLibrary.BlockTypes;

public class IBlock : BlockFormat
{
    public override int BlockID => 1;
    protected override Position InitialOffset => new(-1, 3);
    protected override Position[][] Tiles => tiles;

    private readonly Position[][] tiles =
    [
        [new(1,0), new(1,1), new(1,2), new(1,3)],
        [new(0,2), new(1,2), new(2,2), new(3,2)],
        [new(2,0), new(2,1), new(2,2), new(2,3)],
        [new(0,1), new(1,1), new(2,1), new(3,1)]
    ];
}
