using System;

namespace Chess
{
    public interface IReadOnlyPosition
    {
        Piece this[Coordinate c] { get; }
        byte EPFile { get; }
        bool CanWhiteCastlingKingSide { get; }
        bool CanWhiteCastlingQueenSide { get; }
        bool CanBlackCastlingKingSide { get; }
        bool CanBlackCastlingQueenSide { get; }
        Color SideToMove { get; }
    }

    public class Position : IReadOnlyPosition
    {
        private Piece[] _fields;
        public byte EPFile { get; set; }
        public bool CanWhiteCastlingKingSide { get; set; }
        public bool CanWhiteCastlingQueenSide { get; set; }
        public bool CanBlackCastlingKingSide { get; set; }
        public bool CanBlackCastlingQueenSide { get; set; }
        public Color SideToMove { get; set; }

        public Position()
        {
            _fields = new Piece[64];
            EPFile = 0;
        }

        public Position(IReadOnlyPosition p)
        {
            EPFile = p.EPFile;
            CanWhiteCastlingKingSide = p.CanWhiteCastlingKingSide;
            CanWhiteCastlingQueenSide = p.CanWhiteCastlingQueenSide;
            CanBlackCastlingKingSide = p.CanBlackCastlingKingSide;
            CanBlackCastlingQueenSide = p.CanBlackCastlingQueenSide;
            SideToMove = p.SideToMove;
           
            _fields = new Piece[64];
            for (byte i=0; i<64; i++)
            {
                _fields[i] = p[i];
            }
        }

        public void SetInitial()
        {
            EPFile = 0;
            SideToMove = Color.White;
            CanWhiteCastlingKingSide = true;
            CanWhiteCastlingQueenSide = true;
            CanBlackCastlingKingSide = true;
            CanBlackCastlingQueenSide = true;

            this["a1"] = new Piece { Type = PieceType.Rook, Color = Color.White };
            this["b1"] = new Piece { Type = PieceType.Knight, Color = Color.White };
            this["c1"] = new Piece { Type = PieceType.Bishop, Color = Color.White };
            this["d1"] = new Piece { Type = PieceType.Queen, Color = Color.White };
            this["e1"] = new Piece { Type = PieceType.King, Color = Color.White };
            this["f1"] = new Piece { Type = PieceType.Bishop, Color = Color.White };
            this["g1"] = new Piece { Type = PieceType.Knight, Color = Color.White };
            this["h1"] = new Piece { Type = PieceType.Rook, Color = Color.White };

            this["a2"] = new Piece { Type = PieceType.Pawn, Color = Color.White };
            this["b2"] = new Piece { Type = PieceType.Pawn, Color = Color.White };
            this["c2"] = new Piece { Type = PieceType.Pawn, Color = Color.White };
            this["d2"] = new Piece { Type = PieceType.Pawn, Color = Color.White };
            this["e2"] = new Piece { Type = PieceType.Pawn, Color = Color.White };
            this["f2"] = new Piece { Type = PieceType.Pawn, Color = Color.White };
            this["g2"] = new Piece { Type = PieceType.Pawn, Color = Color.White };
            this["h2"] = new Piece { Type = PieceType.Pawn, Color = Color.White };

            this["a7"] = new Piece { Type = PieceType.Pawn, Color = Color.Black };
            this["b7"] = new Piece { Type = PieceType.Pawn, Color = Color.Black };
            this["c7"] = new Piece { Type = PieceType.Pawn, Color = Color.Black };
            this["d7"] = new Piece { Type = PieceType.Pawn, Color = Color.Black };
            this["e7"] = new Piece { Type = PieceType.Pawn, Color = Color.Black };
            this["f7"] = new Piece { Type = PieceType.Pawn, Color = Color.Black };
            this["g7"] = new Piece { Type = PieceType.Pawn, Color = Color.Black };
            this["h7"] = new Piece { Type = PieceType.Pawn, Color = Color.Black };

            this["a8"] = new Piece { Type = PieceType.Rook, Color = Color.Black };
            this["b8"] = new Piece { Type = PieceType.Knight, Color = Color.Black };
            this["c8"] = new Piece { Type = PieceType.Bishop, Color = Color.Black };
            this["d8"] = new Piece { Type = PieceType.Queen, Color = Color.Black };
            this["e8"] = new Piece { Type = PieceType.King, Color = Color.Black };
            this["f8"] = new Piece { Type = PieceType.Bishop, Color = Color.Black };
            this["g8"] = new Piece { Type = PieceType.Knight, Color = Color.Black };
            this["h8"] = new Piece { Type = PieceType.Rook, Color = Color.Black };
        }

        public Piece this[Coordinate c]
        {
            get
            {
                return _fields[c];
            }
            set
            {
                _fields[c] = value;
            }
        }

        public void PerformMove(Move m)
        {
            throw new NotImplementedException();
        }
    }
}
