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
            PreferredBackBufferWidth = 400,
            PreferredBackBufferHeight = 400
        };

        Content.RootDirectory = "_Content";

        _tileMapper = new TileMapper();
    }

    protected override void LoadContent()
    {
        _tileMapper.LoadContent(Content);
    }
}