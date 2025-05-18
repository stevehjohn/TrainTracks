using Microsoft.Xna.Framework;
using TrainTracks.Desktop.Infrastructure;

namespace TrainTracks.Desktop.Presentation;

public class PuzzleRenderer : Game
{
    private readonly TileMapper _tileMapper;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

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
}