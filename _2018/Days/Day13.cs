using System;
using System.Collections.Generic;
using System.Drawing;
using _2018.Utils;

namespace _2018.Days
{
    public class Day13 : Day
    {
        private readonly IDictionary<int, Dictionary<int, MapFeature>> _map = new Dictionary<int, Dictionary<int, MapFeature>>();

        private List<Cart> InitialiseMapAndCarts()
        {
            var lines = QuestionLoader.Load(13).Split(Environment.NewLine);
            var carts = new List<Cart>();

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                char? lastPiece = null;

                for (var x = 0; x < line.Length; x++)
                {
                    if (!this._map.ContainsKey(x))
                    {
                        this._map.Add(x, new Dictionary<int, MapFeature>());
                    }
                    
                    var mapPiece = line[x];
                    var curPos = new Point(x, y);

                    switch (mapPiece)
                    {
                        case '/':
                            if (lastPiece.HasValue && (lastPiece == '-' || lastPiece == '+' || lastPiece == '/' || lastPiece == '\\'))
                            {
                                this._map[x].Add(y, new Corner
                                {
                                    Left = true,
                                    Top = true
                                });
                            }
                            else
                            {
                                this._map[x].Add(y, new Corner
                                {
                                    Right = true,
                                    Bottom = true
                                });
                            }
                            break;
                        case '\\':
                            if (lastPiece.HasValue && (lastPiece == '-' || lastPiece == '+' || lastPiece == '/' || lastPiece == '\\'))
                            {
                                this._map[x].Add(y, new Corner
                                {
                                    Left = true,
                                    Bottom = true
                                });
                            }
                            else
                            {
                                this._map[x].Add(y, new Corner
                                {
                                    Right = true,
                                    Top = true
                                });
                            }
                            break;
                        case '+':
                            this._map[x].Add(y, new Intersection());
                            break;
                        case '^':
                            carts.Add(new Cart(curPos, new Point(0, -1)));
                            break;
                        case 'v':
                            carts.Add(new Cart(curPos, new Point(0, 1)));
                            break;
                        case '>':
                            carts.Add(new Cart(curPos, new Point(1, 0)));
                            break;
                        case '<':
                            carts.Add(new Cart(curPos, new Point(-1, 0)));
                            break;
                    }

                    lastPiece = mapPiece;
                }
            }

            return carts;
        }
        
        protected override void DoPart1()
        {
            var carts = this.InitialiseMapAndCarts();

            Point? collision = null;
            
            // Can be left empty for the first iteration as we know there will be no collisions at time t = 0
            var cartPositions = new Dictionary<int, HashSet<int>>();

            while (!collision.HasValue)
            {
                foreach (var cart in carts)
                {
                    if (cartPositions.ContainsKey(cart.X))
                    {
                        cartPositions[cart.X].Remove(cart.Y);
                    }

                    cart.Position = new Point(cart.X + cart.Velocity.X, cart.Y + cart.Velocity.Y);

                    if (this._map[cart.X].ContainsKey(cart.Y))
                    {
                        var feature = this._map[cart.X][cart.Y];

                        if (feature.GetType() == typeof(Corner))
                        {
                            var corner = (Corner) feature;

                            if (corner.Right)
                            {
                                cart.Velocity = cart.Velocity.X != 0 ? new Point(0, corner.Bottom ? 1 : -1) : new Point(1, 0);
                            }
                            else
                            {
                                // Must be Corner.Left
                                cart.Velocity = cart.Velocity.X != 0 ? new Point(0, corner.Bottom ? 1 : -1) : new Point(-1, 0);
                            }
                        }
                        else if (feature.GetType() == typeof(Intersection))
                        {
                            switch (cart.NumTurns % 3)
                            {
                                case 0:
                                    // Left
                                    cart.Velocity = cart.Velocity.X != 0 ? new Point(0, cart.Velocity.X == 1 ? -1 : 1) : new Point(cart.Velocity.Y == 1 ? 1 : -1, 0);
                                    break;
                                case 1:
                                    // Straight
                                    break;
                                case 2:
                                    // Right
                                    cart.Velocity = cart.Velocity.X != 0 ? new Point(0, cart.Velocity.X == 1 ? 1 : -1) : new Point(cart.Velocity.Y == 1 ? -1 : 1, 0);
                                    break;
                            }

                            cart.NumTurns++;
                        }
                    }

                    if (!cartPositions.ContainsKey(cart.X))
                    {
                        cartPositions.Add(cart.X, new HashSet<int>());
                    }

                    if (cartPositions[cart.X].Contains(cart.Y))
                    {
                        collision = new Point(cart.X, cart.Y);
                    }
                    else
                    {
                        cartPositions[cart.X].Add(cart.Y);    
                    }
                }

                carts.Sort((cart1, cart2) => cart1.Y != cart2.Y ? cart1.Y - cart2.Y : cart1.X - cart2.X);
            }
            
            ConsoleUtils.WriteColouredLine($"Collision at {collision}", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            this._map.Clear();
            
            var carts = this.InitialiseMapAndCarts();

            // Can be left empty for the first iteration as we know there will be no collisions at time t = 0
            var cartPositions = new Dictionary<int, Dictionary<int, Cart>>();

            while (carts.Count > 1)
            {   
                var newCarts = new List<Cart>();
                
                foreach (var cart in carts)
                {
                    if (cartPositions.ContainsKey(cart.X))
                    {
                        cartPositions[cart.X].Remove(cart.Y);
                    }

                    if (cart.HasCollided)
                    {
                        // Been hit - do nothing
                        continue;
                    }

                    cart.Position = new Point(cart.X + cart.Velocity.X, cart.Y + cart.Velocity.Y);

                    if (this._map[cart.X].ContainsKey(cart.Y))
                    {
                        var feature = this._map[cart.X][cart.Y];

                        if (feature.GetType() == typeof(Corner))
                        {
                            var corner = (Corner) feature;

                            if (corner.Right)
                            {
                                cart.Velocity = cart.Velocity.X != 0 ? new Point(0, corner.Bottom ? 1 : -1) : new Point(1, 0);
                            }
                            else
                            {
                                // Must be Corner.Left
                                cart.Velocity = cart.Velocity.X != 0 ? new Point(0, corner.Bottom ? 1 : -1) : new Point(-1, 0);
                            }
                        }
                        else if (feature.GetType() == typeof(Intersection))
                        {
                            switch (cart.NumTurns % 3)
                            {
                                case 0:
                                    // Left
                                    cart.Velocity = cart.Velocity.X != 0 ? new Point(0, cart.Velocity.X == 1 ? -1 : 1) : new Point(cart.Velocity.Y == 1 ? 1 : -1, 0);
                                    break;
                                case 1:
                                    // Straight
                                    break;
                                case 2:
                                    // Right
                                    cart.Velocity = cart.Velocity.X != 0 ? new Point(0, cart.Velocity.X == 1 ? 1 : -1) : new Point(cart.Velocity.Y == 1 ? -1 : 1, 0);
                                    break;
                            }

                            cart.NumTurns++;
                        }
                    }

                    if (!cartPositions.ContainsKey(cart.X))
                    {
                        cartPositions.Add(cart.X, new Dictionary<int, Cart>());
                    }

                    if (cartPositions[cart.X].ContainsKey(cart.Y))
                    {
                        // Already a cart here - collision. Remove the collided-with cart
                        newCarts.Remove(cartPositions[cart.X][cart.Y]);

                        // If the collided-with cart has yet to move we will still check it this tick. Set hasCollided
                        // so we don't move it
                        cartPositions[cart.X][cart.Y].HasCollided = true;

                        // We've removed this cart, so get rid of it from cart positions
                        cartPositions[cart.X].Remove(cart.Y);
                    }
                    else
                    {
                        newCarts.Add(cart);
                        
                        cartPositions[cart.X].Add(cart.Y, cart);
                    }
                }

                if (newCarts.Count < carts.Count)
                {
                    ConsoleUtils.WriteColouredLine($"Collision, had {carts.Count} carts, now have {newCarts.Count} carts", ConsoleColor.Magenta);
                }
                
                carts = newCarts;

                carts.Sort((cart1, cart2) => cart1.Y != cart2.Y ? cart1.Y - cart2.Y : cart1.X - cart2.X);
            }
            
            ConsoleUtils.WriteColouredLine($"Last cart at {carts[0].Position}", ConsoleColor.Cyan);
        }

        private class Cart
        {
            public Cart(Point pos, Point vel)
            {
                this.Position = pos;
                this.Velocity = vel;
                this.OriginalPosition = pos;
            }
            
            public Point Velocity { get; set; }
            
            public Point Position { get; set; }

            public Point OriginalPosition;

            public int NumTurns { get; set; }

            public int X => this.Position.X;

            public int Y => this.Position.Y;
            
            public bool HasCollided { get; set; }
        }

        private class MapFeature
        {
            
        }

        private class Corner : MapFeature
        {
            public bool Top { get; set; }
            public bool Bottom { get; set; }
            public bool Left { get; set; }
            public bool Right { get; set; }
        }

        private class Intersection : MapFeature
        {
            
        }
    }
}