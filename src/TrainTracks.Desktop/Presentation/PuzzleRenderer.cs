using Microsoft.Xna.Framework;
using TrainTracks.Desktop.Infrastructure;
using TrainTracks.Engine;

namespace TrainTracks.Desktop.Presentation;

public class PuzzleRenderer : Game
{
    private readonly TileMapper _tileMapper;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private Solver _solver;
    
    public PuzzleRenderer()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Constants.PuzzleMaxWidth * Constants.TileWidth,
            PreferredBackBufferHeight = Constants.PuzzleMaxHeight * Constants.TileHeight
        };

        Content.RootDirectory = "_Content";

        _tileMapper = new TileMapper();
    }

    protected override void LoadContent()
    {
        _tileMapper.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
    }
}