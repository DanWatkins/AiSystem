﻿using DanWatkins.AiSystem.Model;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.Linq;

namespace DanWatkins.AiSystem.Views
{
    public class BoardView : Drawable
    {
        private Board _board;
        public Board Board
        {
            get { return _board; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _board = value;

                Paint += BoardControl_Paint;
            }
        }

        public Color BaseColor { get; set; } = Colors.Black;

        public Color GroundColor { get; set; } = Colors.SandyBrown;

        public Color BlockColor { get; set; } = Colors.Black;

        public Color GrooveColor { get; set; } = Colors.Gray;

        private void BoardControl_Paint(object sender, PaintEventArgs args)
        {
            int boardWidthPx = Board.Size.Width * (Board.TileSize.Width + Board.GrooveThickness) - Board.GrooveThickness;
            int boardHeightPx = Board.Size.Height * (Board.TileSize.Height + Board.GrooveThickness) - Board.GrooveThickness;

            int baseWidthPx = boardWidthPx + Board.BorderThickness * 2;
            int baseHeightPx = boardHeightPx + Board.BorderThickness * 2;

            var rect = new RectangleF(ClientSize);

            // draw the base
            args.Graphics.SaveTransform();
            args.Graphics.SetClip(new RectangleF(
                    0, 0,
                    baseWidthPx,
                    baseHeightPx));
            args.Graphics.FillRectangle(BaseColor, rect);

            // draw the grooves
            args.Graphics.SaveTransform();
            args.Graphics.SetClip(new RectangleF(
                    Board.BorderThickness, Board.BorderThickness,
                    boardWidthPx,
                    boardHeightPx));
            args.Graphics.FillRectangle(GrooveColor, rect);

            // draw the tiles
            for (int x = 0; x < Board.Size.Width; x++)
            {
                for (int y = 0; y < Board.Size.Height; y++)
                {
                    var tile = Board.Tiles[x + y * Board.Size.Width];

                    args.Graphics.SetClip(new RectangleF(
                            x * (Board.TileSize.Width + Board.GrooveThickness) + Board.BorderThickness,
                            y * (Board.TileSize.Height + Board.GrooveThickness) + Board.BorderThickness,
                            Board.TileSize.Width,
                            Board.TileSize.Height));

                    switch (tile)
                    {
                        case Tile.Ground:
                            args.Graphics.FillRectangle(GroundColor, rect);
                            break;
                        case Tile.Block:
                            args.Graphics.FillRectangle(BlockColor, rect);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            // draw the path
            if (Board.Path != null)
            {
                foreach (var part in Board.Path.Parts)
                {
                    const double scale = 0.55;
                    int centeringX = (int)(Board.TileSize.Width * scale);
                    int centeringY = (int)(Board.TileSize.Height * scale);

                    args.Graphics.SetClip(new RectangleF(
                            part.X * (Board.TileSize.Width + Board.GrooveThickness) + Board.BorderThickness + centeringX,
                            part.Y * (Board.TileSize.Height + Board.GrooveThickness) + Board.BorderThickness + centeringY,
                            Board.TileSize.Width - 2 * centeringX,
                            Board.TileSize.Height - 2 * centeringY));

                    if (part == Board.Path.Parts.First())
                        args.Graphics.FillRectangle(Colors.LightGreen, rect);
                    else if (part == Board.Path.Parts.Last())
                        args.Graphics.FillRectangle(Colors.Red, rect);
                    else
                        args.Graphics.FillRectangle(Colors.LightSlateGray, rect);
                }
            }

            args.Graphics.RestoreTransform();
        }
    }
}