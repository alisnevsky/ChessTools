using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    abstract class BaseMoveExpert
    {
        public virtual bool IsLegalMove(Position position, Move move)
        {
            Piece piece = position[move.From];
            return piece == move.Piece;
        }

        public abstract void PerformMove(Position position, Move move);
        public abstract Move[] GetPossibleMoves(Position position, Coordinate fromField);
        public abstract Coordinate[] GetAttackedFields(Position position, Coordinate fromField);
    }
}
