using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
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
            List<Move> _moves = new List<Move>();

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

        public void CalcPositionState(Position position)
        {
            if (!IsLegalPosition(position))
                position.State = PositionState.Invalid;
            else
            {
                if (IsInCheck(position))
                {
                    if (IsCheckmate(position))
                        position.State = PositionState.Checkmate;
                    else
                        position.State = PositionState.Check;
                }
                else if (IsStalemate(position))
                {
                    position.State = PositionState.Stalemate;
                }
                else
                    position.State = PositionState.Valid;
            }
        }

        public bool IsLegalPosition(Position position)
        {
            int whiteKings = 0, blackKings = 0;
            bool isPawnOnEdge = false;

            for (byte i = 0; i < 64; i++)
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

            return isLegal;
        }

        public void PerformMove(Position position, Move move)
        {
            BaseMoveExpert moveExpert = GetMoveExpert(move.Piece.Type);
            moveExpert.PerformMove(position, move);
        }

        private BaseMoveExpert GetMoveExpert(PieceType pieceType)
        {
            BaseMoveExpert moveExpert = null;

            switch (pieceType)
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
