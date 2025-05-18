using Microsoft.Xna.Framework;
using TrainTracks.Desktop.Infrastructure;

namespace TrainTracks.Desktop.Presentation;

public class PuzzleRenderer : Game
{
    private TileMapper _tileMapper;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    public PuzzleRenderer()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 100,
            PreferredBackBufferHeight = 100
        };

        Content.RootDirectory = "_Content";

        _tileMapper = new TileMapper(Content);
    }
}