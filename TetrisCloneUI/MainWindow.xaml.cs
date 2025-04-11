
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TetrisCloneLibrary.GameLogic;

namespace TetrisCloneUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ImageSource[] tileImages =
    [
        new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
    ];

    private readonly ImageSource[] blockImages =
    [
        new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
    ];

    private readonly Image[,] imageControls;
    private readonly int maxDelay = 1000;
    private readonly int minDelay = 75;
    private readonly int delayDecrease = 25;

    private GameState gameState = new();

    public MainWindow()
    {
        InitializeComponent();
        imageControls = SetupGameCanvas(gameState.GameGrid);
    }

    private Image[,] SetupGameCanvas(GameGrid grid)
    {
        Image[,] imageControls = new Image[grid.Rows, grid.Columns];
        int cellSize = 25;

        for (int r = 0; r < grid.Rows; r++)
        {
            for (int c = 0; c < grid.Columns; c++)
            {
                Image imageControl = new()
                {
                    Width = cellSize,
                    Height = cellSize
                };

                Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                Canvas.SetLeft(imageControl, c * cellSize);
                GameCanvas.Children.Add(imageControl);
                imageControls[r, c] = imageControl;
            }
        }

        return imageControls;
    }

    private void DrawGrid(GameGrid grid)
    {
        for (int r = 0; r < grid.Rows; r++)
        {
            for (int c = 0; c < grid.Columns; c++)
            {
                int id = grid[r, c];
                imageControls[r, c].Opacity = 1;
                imageControls[r, c].Source = tileImages[id];
            }
        }
    }

    private void DrawBlock(BlockFormat block)
    {
        foreach (Position p in block.TilePositions())
        {
            imageControls[p.Row, p.Column].Opacity = 1;
            imageControls[p.Row, p.Column].Source = tileImages[block.BlockID];
        }
    }

    private void DrawNextBlock(BlockQueue blockQueue)
    {
        BlockFormat next = blockQueue.NextBlock;
        NextImage.Source = blockImages[next.BlockID];
    }

    private void DrawHeldBlock(BlockFormat? heldBlock)
    {
        if (heldBlock == null)
        {
            HoldImage.Source = blockImages[0];
        }
        else
        {
            HoldImage.Source = blockImages[heldBlock.BlockID];
        }
    }

    private void DrawGhostBlock(BlockFormat block)
    {
        int dropDistance = gameState.BlockDropDistance();

        foreach (Position p in block.TilePositions())
        {
            imageControls[p.Row + dropDistance, p.Column].Opacity = 0.15;
            imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.BlockID];
        }
    }

    private void Draw(GameState gState)
    {
        DrawGrid(gState.GameGrid);
        DrawGhostBlock(gState.CurrentBlock);
        DrawBlock(gState.CurrentBlock);
        DrawNextBlock(gState.BlockQueue);
        DrawHeldBlock(gState.HeldBlock);
        ScoreText.Text = $"Score: {gState.Score}";
    }

    private async Task GameLoop()
    {
        Draw(gameState);

        while (!gameState.GameOver)
        {
            int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
            await Task.Delay(delay);
            gameState.MoveBlockDown();
            Draw(gameState);
        }

        GameOverMenu.Visibility = Visibility.Visible;
        FinalScoreText.Text = $"Score: {gameState.Score}";
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (gameState.GameOver)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Left:
                gameState.MoveBlockLeft();
                break;
            case Key.Right:
                gameState.MoveBlockRight();
                break;
            case Key.Down:
                gameState.MoveBlockDown();
                break;
            case Key.Up:
                gameState.RotateBlockCW();
                break;
            case Key.Z:
                gameState.RotateBlockCCW();
                break;
            case Key.C:
                gameState.HoldBlock();
                break;
            case Key.Space:
                gameState.DropBlock();
                break;
            default:
                return;
        }

        Draw(gameState);
    }

    private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
    {
        await GameLoop();
    }

    private async void PlayAgain_Click(object sender, RoutedEventArgs e)
    {
        gameState = new GameState();
        GameOverMenu.Visibility = Visibility.Hidden;
        await GameLoop();
    }
}