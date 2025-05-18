using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TrainTracks.Desktop.Infrastructure;
using TrainTracks.Engine;
using TrainTracks.Engine.Board;

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
    
    private bool _isSolved;
    
    public Grid Grid { get; set; }
    
    public PuzzleRenderer()
    {
        _width = Constants.PuzzleMaxWidth * Constants.TileWidth;

        var height = Constants.PuzzleMaxHeight * Constants.TileHeight;
            
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
            StepCallback = VisualiseStep
        };
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _tileMapper.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (! _isSolved && ! _isSolving)
        {
            Task.Factory.StartNew(() =>
            {
                _isSolving = true;
                
                _solver.Solve(Grid);

                _isSolved = true;
                
                _isSolving = false;
            });
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
        
        for (var y = 0; y < Grid.Height; y++)
        {
            for (var x = 0; x < Grid.Width; x++)
            {
                var isometricX = (x - y) * Constants.TileWidth / 2 + _width / 2 - (Constants.PuzzleMaxWidth - Grid.Width) * Constants.TileWidth / 2;

                var isometricY = (x + y) * Constants.TileCentre;

                _spriteBatch.Draw(_tileMapper.GetTile(Grid[x, y]), new Rectangle(isometricX, isometricY, Constants.TileWidth, Constants.TileHeight),
                    new Rectangle(0, 0, Constants.TileWidth, Constants.TileHeight), Color.White, 0, Vector2.Zero, SpriteEffects.None, (x + y) / 100f);
            }
        }

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
    
    private void VisualiseStep(Grid grid)
    {
    }
}