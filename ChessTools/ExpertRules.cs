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
            List<Coordinate> fields = new List<Coordinate>();
            Move[] moves;

            for (byte i = 0; i < 64; i++)
            {
                Coordinate c = i;
                Piece p = position[c];

                if (p.Type != PieceType.None && p.Color == position.SideToMove)
                    fields.Add(c);
            }

            moves = GetPossibleMoves(position, fields.ToArray());

            return moves;
        }

        public Move[] GetPossibleMoves(Position position, Coordinate[] fromFields)
        {
            List<Move> allMoves = new List<Move>();
            Move[] moves;

            foreach (Coordinate c in fromFields)
            {
                moves = GetPossibleMoves(position, c);
                allMoves.AddRange(moves);
            }

            return allMoves.ToArray();
        }

        public Move[] GetPossibleMoves(Position position, Coordinate fromField)
        {
            Piece piece = position[fromField];
            BaseMoveExpert moveExpert;

            if (piece.Type == PieceType.None)
                throw new ChessException($"Missing piece at {fromField}");

            if (position.SideToMove != piece.Color || piece.Color == Color.None)
                throw new ChessException("Incorrect piece color");

            moveExpert = GetMoveExpert(piece.Type);

            return moveExpert.GetPossibleMoves(position, fromField);
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
            position.SideToMove = position.SideToMove == Color.White ? Color.Black : Color.White;
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
