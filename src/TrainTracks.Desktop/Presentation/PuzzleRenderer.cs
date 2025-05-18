using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TrainTracks.Desktop.Infrastructure;
using TrainTracks.Engine;
using TrainTracks.Engine.Board;
using Point = TrainTracks.Engine.Board.Point;

namespace TrainTracks.Desktop.Presentation;

public class PuzzleRenderer : Game
{
    private readonly TileMapper _tileMapper;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    private readonly int _width;

    private readonly Solver _solver;
    
    private bool _isSolving;

    private readonly Queue<Piece[,]> _stepQueue = [];
    
    public Grid Grid { get; set; }
    
    public PuzzleRenderer()
    {
        _width = Constants.PuzzleMaxWidth * Constants.TileWidth;

        var height = Constants.PuzzleMaxHeight * Constants.TileHeight / 2 + Constants.TileHeight * 4;
            
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = _width,
            PreferredBackBufferHeight = height
        };

        Content.RootDirectory = "_Content";

        _tileMapper = new TileMapper();
        
        IsMouseVisible = true;
        
        _solver = new Solver
        {
            StepCallback = EnqueueStep
        };
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _tileMapper.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (! _isSolving)
        {
            var task = new Task(() =>
            {
                Thread.Sleep(1_000);

                _solver.Solve(Grid);
            });

            _isSolving = true;
            
            task.Start();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

        Piece[,] grid = null;

        if (_stepQueue.Count > 0)
        {
            grid = _stepQueue.Dequeue();
        }

        var originX = _width / 2 - (Grid.Width - Grid.Height) * Constants.TileWidth / 2 / 2 - Constants.TileWidth / 2;

        var originY = Constants.TileHeight; // padding from top

        for (var y = 0; y < Grid.Height; y++)
        {
            for (var x = 0; x < Grid.Width; x++)
            {
                var tile = _tileMapper.GetTile(grid == null ? Grid[x, y] : grid[x, y]);

                var isometricX = (x - y) * Constants.TileWidth / 2 + originX;

                // isometricX += Constants.TileWidth / 2;
                
                // if (Grid.Width % 2 == 1)
                // {
                //     isometricX -= Constants.TileWidth / 2;
                // }

                var isometricY = (x + y) * Constants.TileCentre + originY;

                var colour = Grid.IsFixed(new Point(x, y)) ? Color.Gray : Color.White;

                _spriteBatch.Draw(tile, new Rectangle(isometricX, isometricY, Constants.TileWidth, Constants.TileHeight),
                    new Rectangle(0, 0, Constants.TileWidth, Constants.TileHeight), colour, 0, Vector2.Zero, SpriteEffects.None, (x + y) / 100f);
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void EnqueueStep(Grid grid)
    {
        _stepQueue.Enqueue(grid.ShallowClone());
    }
}