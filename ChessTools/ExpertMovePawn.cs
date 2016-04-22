using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class PawnMoveExpert : BaseMoveExpert
    {
        public override Coordinate[] GetAttackedFields(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override Move[] GetPossibleMoves(Position position, Coordinate fromField)
        {
            List<Move> moves = new List<Move>();
            Color color = position.SideToMove;
            Piece piece = new Piece { Type = PieceType.Pawn, Color = color };
            Move move;
            Coordinate toField = fromField;
            int initialRank;
            int step;
            int newRank;

            //TODO

            if (color == Color.White)
            {
                initialRank = 2;
                step = 1;
            }
            else
            {
                initialRank = 7;
                step = -1;
            }

            newRank = fromField.Rank + step;

            if (newRank >= 1 && newRank <= 8)
            {
                toField.Rank = (byte)newRank;
                if (position[toField].Type == PieceType.None)
                {
                    move = new Move();
                    move.From = fromField;
                    move.To = toField;
                    move.Piece = piece;
                    move.Results = MoveResults.Regular;

                    moves.Add(move);
                }

                if (fromField.Rank == initialRank)
                {
                    newRank = fromField.Rank + 2 * step;

                    if (newRank >= 1 && newRank <= 8)
                    {
                        toField.Rank = (byte)newRank;
                        if (position[toField].Type == PieceType.None)
                        {
                            move = new Move();
                            move.From = fromField;
                            move.To = toField;
                            move.Piece = piece;
                            move.Results = MoveResults.Regular;

                            moves.Add(move);
                        }
                    }
                }
            }

            return moves.ToArray();
        }

        public override void PerformMove(Position position, Move move)
        {
            throw new NotImplementedException();
        }
    }
}
