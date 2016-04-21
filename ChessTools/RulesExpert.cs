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

    class KingMoveExpert : BaseMoveExpert
    {
        public override Coordinate[] GetAttackedFields(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override Move[] GetPossibleMoves(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override void PerformMove(Position position, Move move)
        {
            throw new NotImplementedException();
        }
    }

    class QueenMoveExpert : BaseMoveExpert
    {
        public override Coordinate[] GetAttackedFields(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override Move[] GetPossibleMoves(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override void PerformMove(Position position, Move move)
        {
            throw new NotImplementedException();
        }
    }

    class RookMoveExpert : BaseMoveExpert
    {
        public override Coordinate[] GetAttackedFields(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override Move[] GetPossibleMoves(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override void PerformMove(Position position, Move move)
        {
            throw new NotImplementedException();
        }
    }

    class BishopMoveExpert : BaseMoveExpert
    {
        public override Coordinate[] GetAttackedFields(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override Move[] GetPossibleMoves(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override void PerformMove(Position position, Move move)
        {
            throw new NotImplementedException();
        }
    }

    class KnightMoveExpert : BaseMoveExpert
    {
        public override Coordinate[] GetAttackedFields(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override Move[] GetPossibleMoves(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override void PerformMove(Position position, Move move)
        {
            throw new NotImplementedException();
        }
    }

    class PawnMoveExpert : BaseMoveExpert
    {
        public override Coordinate[] GetAttackedFields(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override Move[] GetPossibleMoves(Position position, Coordinate fromField)
        {
            throw new NotImplementedException();
        }

        public override void PerformMove(Position position, Move move)
        {
            throw new NotImplementedException();
        }
    }

    class RulesExpert
    {
        private BaseMoveExpert _kingMoveExpert;
        private BaseMoveExpert _queenMoveExpert;
        private BaseMoveExpert _rookMoveExpert;
        private BaseMoveExpert _bishopMoveExpert;
        private BaseMoveExpert _knightMoveExpert;
        private BaseMoveExpert _pawnMoveExpert;

        public RulesExpert()
        {
            _kingMoveExpert = new KingMoveExpert();
            _queenMoveExpert = new QueenMoveExpert();
            _rookMoveExpert = new RookMoveExpert();
            _bishopMoveExpert = new BishopMoveExpert();
            _knightMoveExpert = new KnightMoveExpert();
            _pawnMoveExpert = new PawnMoveExpert();
        }

        public Move[] GetPossibleMoves(Position position)
        {
            // TODO

            return null;
        }

        public Move[] GetPossibleMoves(Position position, Coordinate[] fromFields)
        {
            // TODO

            return null;
        }

        public Move[] GetPossibleMoves(Position position, Coordinate fromField)
        {
            // TODO

            return null;
        }

        public Coordinate[] GetAttackedFields(Position position, Color side = Color.None)
        {
            // TODO
            return null;
        }

        public bool IsStalemate(Position position)
        {
            throw new NotImplementedException();
        }

        public bool IsCheckmate(Position position)
        {
            throw new NotImplementedException();
        }

        public bool IsInCheck(Position position, Color side = Color.None)
        {
            throw new NotImplementedException();
        }

        public bool IsLegalPosition(Position position)
        {
            int whiteKings = 0, blackKings = 0;
            bool isPawnOnEdge = false;

            for(byte i=0; i<64; i++)
            {
                Coordinate c = i;
                Piece p = position[c];

                if (p.Type == PieceType.King)
                {
                    if (p.Color == Color.White)
                        whiteKings++;
                    else if (p.Color == Color.Black)
                        blackKings++;
                }

                if (!isPawnOnEdge && p.Type == PieceType.Pawn && (c.Rank == 1 || c.Rank == 8))
                    isPawnOnEdge = true;
            }

            return whiteKings == 1 && blackKings == 1 && !isPawnOnEdge;
        }

        public bool IsLegalMove(Position position, Move move)
        {
            bool isLegal;
            BaseMoveExpert moveExpert = GetMoveExpert(move.Piece.Type);

            if (moveExpert != null)
                isLegal = moveExpert.IsLegalMove(position, move);
            else
                isLegal = false;

            return  isLegal;
        }

        public void PerformMove(Position position, Move move)
        {
            BaseMoveExpert moveExpert = GetMoveExpert(move.Piece.Type);
            moveExpert.PerformMove(position, move);
        }

        private BaseMoveExpert GetMoveExpert(PieceType pieceType)
        {
            BaseMoveExpert moveExpert = null;

            switch(pieceType)
            {
                case PieceType.King:
                    moveExpert = _kingMoveExpert;
                    break;
                case PieceType.Queen:
                    moveExpert = _queenMoveExpert;
                    break;
                case PieceType.Rook:
                    moveExpert = _rookMoveExpert;
                    break;
                case PieceType.Bishop:
                    moveExpert = _bishopMoveExpert;
                    break;
                case PieceType.Knight:
                    moveExpert = _knightMoveExpert;
                    break;
                case PieceType.Pawn:
                    moveExpert = _pawnMoveExpert;
                    break;
            }

            return moveExpert;
        }
    }
}
