using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Flags]
    public enum MoveResults
    {
        Regular = 0x00,
        Promotion = 0x01,
        Capture = 0x02,
        Check = 0x04,
        Checkmate = 0x08,
        Stalemate = 0x10,
        CastlingKingSide = 0x20,
        CastlingQueenSide = 0x40,
        EnPassant = 0x80
    }

    public class Move
    {
        public ushort MoveNo { get; set; }
        public Piece Piece { get; set; }
        public MoveResults Results { get; set; }
        public PieceType PromoteTo { get; set; }
        public Coordinate From { get; set; }
        public Coordinate To { get; set; }
    }
}
