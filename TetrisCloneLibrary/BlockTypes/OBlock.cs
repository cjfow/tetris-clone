
using TetrisCloneLibrary.GameLogic;

namespace TetrisCloneLibrary.BlockTypes;

public class OBlock : BlockFormat
{
    public override int BlockID => 4;
    protected override Position InitialOffset => new(0, 4);
    protected override Position[][] Tiles => tiles;

    private readonly Position[][] tiles =
    [
        [new(0,0), new(0,1), new(1,0), new(1,1)]
    ];
}
