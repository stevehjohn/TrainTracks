using System;
using System.Collections.Concurrent;
using System.Diagnostics;
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
    private const int SkipFrames = 1;
    
    private readonly TileMapper _tileMapper;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    private readonly int _width;
    
    private readonly int _height;

    private readonly Solver _solver;
    
    private readonly ConcurrentQueue<Grid> _stepQueue = [];
    
    private readonly Stopwatch _stopwatch = new();
    
    private bool _isSolving;

    private bool _isComplete;

    private SpriteFont _font;

    private long _stepCount;

    private long _frameCount;
    
    private Task _task;

    private Grid _grid;
    
    public Grid Grid 
    {
        private get => _grid;
        set => _grid = value;
    }
    
    public PuzzleRenderer()
    {
        _width = Constants.PuzzleMaxWidth * Constants.TileWidth;

        _height = Constants.PuzzleMaxHeight * Constants.TileHeight / 2 + Constants.TileHeight * 6;
            
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = _width,
            PreferredBackBufferHeight = _height
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
            _task = new Task(() =>
            {
                Thread.Sleep(1_000);
                
                _stopwatch.Restart();

                try
                {
                    _solver.Solve(Grid);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                
                _isComplete = true;
            });

            _isSolving = true;
            
            _task.Start();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

        if (_stepQueue.Count > 0)
        {
            if (_stepQueue.TryDequeue(out _grid))
            {
                _frameCount++;
            }
        }
        else
        {
            if (_isComplete)
            {
                _stopwatch.Stop();
            }
        }

        var originX = _width / 2 - (Grid.Width - Grid.Height) * Constants.TileWidth / 2 / 2 - Constants.TileWidth / 2;

        const int originY = Constants.TileHeight * 2;

        for (var y = 0; y < Grid.Height; y++)
        {
            for (var x = 0; x < Grid.Width; x++)
            {
                var tile = _tileMapper.GetTile(_grid[x, y]);

                var isometricX = (x - y) * Constants.TileWidth / 2 + originX;

                var isometricY = (x + y) * Constants.TileCentre + originY;

                var colour = Grid.IsFixed(new Point(x, y)) ? Color.Gray : Color.White;

                _spriteBatch.Draw(tile, new Rectangle(isometricX, isometricY, Constants.TileWidth, Constants.TileHeight),
                    new Rectangle(0, 0, Constants.TileWidth, Constants.TileHeight), colour, 0, Vector2.Zero, SpriteEffects.None, (x + y) / 100f);
            }
        }

        var padding = (int) _font.MeasureString("0").X / 2;
        
        var fontHeight = (int) _font.MeasureString("0").Y;

        string text;
        
        for (var y = 0; y < Grid.Height; y++)
        {
            var count = _grid.GetRowCount(y);
            
            var target = Grid.RowConstraints[y];
            
            text = $"{target}";

            var color = Color.White;

            var isometricX = (-1 - y) * Constants.TileWidth / 2f + originX + Constants.TileWidth *.65 - padding;

            if (target < 10)
            {
                isometricX += padding;
            }

            var isometricY = (-1 + y) * Constants.TileCentre + originY - Constants.TileHeight * .7;

            _spriteBatch.DrawString(_font, text, new Vector2((int) isometricX, (int) isometricY), color);

            color = count == target ? Color.LightGreen : Color.Gray;

            isometricX = (-1 - y) * Constants.TileWidth / 2f + originX + Constants.TileWidth *.65 - padding;

            if (count < 10)
            {
                isometricX += padding;
            }

            isometricY += fontHeight;
            
            text = $"{count}";

            _spriteBatch.DrawString(_font, text, new Vector2((int) isometricX, (int) isometricY), color);
        }

        for (var x = 0; x < Grid.Width; x++)
        {
            var count = _grid.GetColumnCount(x);
            
            var target = Grid.ColumnConstraints[x];
            
            text = $"{target}";

            var color = Color.White;

            var isometricX = (x - -1) * Constants.TileWidth / 2f + originX + Constants.TileWidth *.25 - padding;

            if (target < 10)
            {
                isometricX += padding;
            }

            var isometricY = (x + -1) * Constants.TileCentre + originY - Constants.TileHeight * .7;

            _spriteBatch.DrawString(_font, text, new Vector2((int) isometricX, (int) isometricY), color);

            color = count == target ? Color.LightGreen : Color.Gray;

            isometricX = (x - -1) * Constants.TileWidth / 2f + originX + Constants.TileWidth *.25 - padding;

            if (count < 10)
            {
                isometricX += padding;
            }

            isometricY += fontHeight;
            
            text = $"{count}";

            _spriteBatch.DrawString(_font, text, new Vector2((int) isometricX, (int) isometricY), color);
        }

        text = $"{_frameCount:N0} / {_stepCount:N0}";

        _spriteBatch.DrawString(_font, text, new Vector2(padding * 4, _height - fontHeight * 2), Color.White);

        text = @$"{_stopwatch.Elapsed:h\:mm\:ss\.fff}";

        _spriteBatch.DrawString(_font, text, new Vector2(padding * 4, _height - fontHeight * 3), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void EnqueueStep(Grid grid)
    {
        _stepCount++;

        if (_stepCount % SkipFrames != 0)
        {
            return;
        }

        _stepQueue.Enqueue(grid.Clone());
    }
}