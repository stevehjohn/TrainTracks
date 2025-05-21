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
    private const int SkipFrames = 10000;
    
    private readonly TileMapper _tileMapper;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    private readonly int _width;

    private readonly Solver _solver;
    
    private bool _isSolving;

    private readonly Queue<Grid> _stepQueue = [];
    
    private SpriteFont _font;

    private long _frameCount;
    
    public Grid Grid { get; set; }
    
    public PuzzleRenderer()
    {
        _width = Constants.PuzzleMaxWidth * Constants.TileWidth;

        const int height = Constants.PuzzleMaxHeight * Constants.TileHeight / 2 + Constants.TileHeight * 6;
            
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
        
        _font = Content.Load<SpriteFont>("font");
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

        Grid grid = null;

        if (_stepQueue.Count > 0)
        {
            grid = _stepQueue.Dequeue();
        }
        
        grid ??= Grid;

        var originX = _width / 2 - (Grid.Width - Grid.Height) * Constants.TileWidth / 2 / 2 - Constants.TileWidth / 2;

        const int originY = Constants.TileHeight * 2;

        for (var y = 0; y < Grid.Height; y++)
        {
            for (var x = 0; x < Grid.Width; x++)
            {
                var tile = _tileMapper.GetTile(grid[x, y]);

                var isometricX = (x - y) * Constants.TileWidth / 2 + originX;

                var isometricY = (x + y) * Constants.TileCentre + originY;

                var colour = Grid.IsFixed(new Point(x, y)) ? Color.Gray : Color.White;

                _spriteBatch.Draw(tile, new Rectangle(isometricX, isometricY, Constants.TileWidth, Constants.TileHeight),
                    new Rectangle(0, 0, Constants.TileWidth, Constants.TileHeight), colour, 0, Vector2.Zero, SpriteEffects.None, (x + y) / 100f);
            }
        }
        
        for (var y = 0; y < Grid.Height; y++)
        {
            var count = grid.GetRowCount(y);
            
            var target = Grid.RowConstraints[y];
            
            var text = $"{target}";

            var color = Color.White;

            var isometricX = (-1 - y) * Constants.TileWidth / 2f + originX + Constants.TileWidth *.65;

            var isometricY = (-1 + y) * Constants.TileCentre + originY - Constants.TileHeight * .7;

            _spriteBatch.DrawString(_font, text, new Vector2((int) isometricX, (int) isometricY), color);

            color = count == target ? Color.LightGreen : Color.Gray;
            
            isometricY += (int) _font.MeasureString(text).Y;
            
            text = $"{count}";

            _spriteBatch.DrawString(_font, text, new Vector2((int) isometricX, (int) isometricY), color);
        }

        for (var x = 0; x < Grid.Width; x++)
        {
            var count = grid.GetColumnCount(x);
            
            var target = Grid.ColumnConstraints[x];
            
            var text = $"{target}";

            var color = Color.White;

            var isometricX = (x - -1) * Constants.TileWidth / 2f + originX + Constants.TileWidth *.25;

            var isometricY = (x + -1) * Constants.TileCentre + originY - Constants.TileHeight * .7;

            _spriteBatch.DrawString(_font, text, new Vector2((int) isometricX, (int) isometricY), color);

            color = count == target ? Color.LightGreen : Color.Gray;
            
            isometricY += (int) _font.MeasureString(text).Y;
            
            text = $"{count}";

            _spriteBatch.DrawString(_font, text, new Vector2((int) isometricX, (int) isometricY), color);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void EnqueueStep(Grid grid)
    {
        _frameCount++;

        if (_frameCount % SkipFrames != 0)
        {
            return;
        }

        _stepQueue.Enqueue(grid.Clone());
    }
}