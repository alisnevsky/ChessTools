using System;

namespace Chess
{
    public enum PieceType
    {
        None,
        King,
        Queen,
        Rook,
        Bishop,
        Knight,
        Pawn
    }

    public enum Color
    {
        None,
        White,
        Black
    }

    public struct Piece
    { 
        public PieceType Type { get; set; }
        public Color Color { get; set; }

        public override string ToString()
        {
            string s = string.Empty;

            if (Color != Color.None)
            {
                switch (Type)
                {
                    case PieceType.King:
                        s = "K";
                        break;
                    case PieceType.Queen:
                        s = "Q";
                        break;
                    case PieceType.Rook:
                        s = "R";
                        break;
                    case PieceType.Bishop:
                        s = "B";
                        break;
                    case PieceType.Knight:
                        s = "N";
                        break;
                    case PieceType.Pawn:
                        s = "P";
                        break;
                }

                if (Color == Color.Black)
                    s = s.ToLower();
            }

            return s;
        }

        public static bool operator ==(Piece p1, Piece p2)
        {
            return (p1.Color == p2.Color && p1.Type == p2.Type);
        }

        public static bool operator !=(Piece p1, Piece p2)
        {
            return (p1.Color != p2.Color || p1.Type != p2.Type);
        }
    }

    public class ChessException : Exception
    {
        public ChessException()
        {
        }
        public ChessException(string message)
            : base(message)
        {
        }
        public ChessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}