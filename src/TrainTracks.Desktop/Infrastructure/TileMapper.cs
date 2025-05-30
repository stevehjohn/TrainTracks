using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TrainTracks.Engine.Board;

namespace TrainTracks.Desktop.Infrastructure;

public class TileMapper
{
    private readonly Dictionary<Piece, Texture2D> _tiles = new();

    public Texture2D GetTile(Piece piece)
    {
        return _tiles[piece];
    }

    public void LoadContent(ContentManager contentManager)
    {
        _tiles.Add(Piece.NorthEast, contentManager.Load<Texture2D>("north-east"));
        
        _tiles.Add(Piece.SouthEast, contentManager.Load<Texture2D>("south-east"));
        
        _tiles.Add(Piece.SouthWest, contentManager.Load<Texture2D>("south-west"));
        
        _tiles.Add(Piece.NorthWest, contentManager.Load<Texture2D>("north-west"));
        
        _tiles.Add(Piece.Vertical, contentManager.Load<Texture2D>("vertical"));
        
        _tiles.Add(Piece.Horizontal, contentManager.Load<Texture2D>("horizontal"));
        
        _tiles.Add(Piece.Empty, contentManager.Load<Texture2D>("grass"));
        
        _tiles.Add(Piece.Placeholder, contentManager.Load<Texture2D>("grass"));
        
        _tiles.Add(Piece.Cross, contentManager.Load<Texture2D>("grass"));
    }
}