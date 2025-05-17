using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TrainTracks.Engine.Board;

namespace TrainTracks.Desktop.Infrastructure;

public class TileMapper
{
    private readonly Dictionary<Piece, Texture2D> _tiles = new();
    
    public TileMapper(ContentManager contentManager)
    {
        LoadContent(contentManager);
    }

    public Texture2D GetTile(Piece piece)
    {
        return _tiles[piece];
    }

    private void LoadContent(ContentManager contentManager)
    {
        _tiles.Add(Piece.NorthEast, contentManager.Load<Texture2D>("roadNE"));
        
        _tiles.Add(Piece.SouthEast, contentManager.Load<Texture2D>("roadES"));
        
        _tiles.Add(Piece.SouthWest, contentManager.Load<Texture2D>("roadSW"));
        
        _tiles.Add(Piece.NorthWest, contentManager.Load<Texture2D>("roadNW"));
        
        _tiles.Add(Piece.Horizontal, contentManager.Load<Texture2D>("roadEW"));
        
        _tiles.Add(Piece.Vertical, contentManager.Load<Texture2D>("roadNS"));
    }
}