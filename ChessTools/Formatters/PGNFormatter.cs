using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;
using System.IO;
using System.Text.RegularExpressions;

/*
 * TODO 
 * 1. [Tag "Value with \" quote and \\ backslash"]
 * 2. First move is black N... 
 * 3. Comments ; {not nested}
 * 4. Export to PGN format
 * 
 */

namespace Chess.Formatters
{
    public class PGNFormatterException : ChessException
    {
        public PGNFormatterException()
        {
        }
        public PGNFormatterException(string message)
            : base(message)
        {
        }
        public PGNFormatterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class PGNFormatter
    {
        private static RulesExpert _rulesExpert = new RulesExpert();

        private bool _expectedFirstLine;
        private bool _expectedHeader;
        private bool _expectedMoves;
        private bool _expectedMoveNo;
        private bool _foundEndGame;
        private bool _foundAnyHeader;
        private bool _foundAnyMove;
        private ushort _currentMoveNo;
        private string _currentLine;
        private Color _currentColor;
        private Game _game;

        private const string _sCastlingKingSide = "O-O";
        private const string _sCastlingQueenSide = "O-O-O";
        private const string _sDraw = "1/2-1/2";
        private const string _sWhiteWins = "1-0";
        private const string _sBlackWins = "0-1";
        private const string _sInProgress = "*";
        private const char _cKing = 'K';
        private const char _cQueen = 'Q';
        private const char _cRook = 'R';
        private const char _cBishop = 'B';
        private const char _cKnight = 'N';
        private const char _cCapture = 'x';
        private const char _cPromotion = '=';
        private const char _cCheck = '+';
        private const char _cCheckmate = '#';

        public PGNFormatter()
        {
            
        }

        private void InitializeParser()
        {
            _expectedFirstLine = true;
            _expectedHeader = false;
            _expectedMoves = false;
            _expectedMoveNo = false;
            _foundEndGame = false;
            _foundAnyHeader = false;
            _foundAnyMove = false;

            _currentMoveNo = 0;
            _currentLine = string.Empty;
            _currentColor = Color.White;

            _game = new Game();
            _game.SetInitialPosition();
        }

        private bool IsLikeHeaderLine(string line)
        {
            bool isHeader;

            if (string.IsNullOrEmpty(line))
                isHeader = false;
            else
                isHeader = line[0] == '[' && line[line.Length - 1] == ']';

            return isHeader;
        }

        private void ParseLineAsHeader(string line)
        {
            // Expected line format [Name "Value"]

            string header;
            string key;
            string value;

            if (line.Length < 2 || line[0] != '[' || line[line.Length - 1] != ']')
                throw new PGNFormatterException($"Unexpected header line: {line}");

            header = line.Substring(1, line.Length - 2);

            string[] pair = header.Split((char[])null, 2, StringSplitOptions.RemoveEmptyEntries);

            if (pair.Length != 2)
                throw new PGNFormatterException($"Unexpected header format: {line}");

            key = pair[0];
            value = pair[1].TrimEnd();

            if (value.Length < 2 || value[0] != '"' || value[value.Length - 1] != '"')
                throw new PGNFormatterException($"Expected quoted header value: {value}");

            value = value.Substring(1, value.Length - 2).Trim();

            IDictionary<string, string> properties = _game.Properties;

            if (properties.ContainsKey(key))
                throw new PGNFormatterException($"Duplicate header tag [{key}] in line: {line}");

            properties.Add(key, value);
            _foundAnyHeader = true;
        }

        private bool TryParseGameResult(string sResult, out GameResult result)
        {
            bool isParsed = true;

            switch (sResult)
            {
                case _sWhiteWins:
                    result = GameResult.WhiteWins;
                    break;
                case _sBlackWins:
                    result = GameResult.BlackWins;
                    break;
                case _sDraw:
                    result = GameResult.Draw;
                    break;
                case _sInProgress:
                    result = GameResult.InProgress;
                    break;
                default:
                    result = GameResult.None;
                    isParsed = false;
                    break;
            }

            return isParsed;
        }

        private void SetGameResult(GameResult gameResult)
        {
            _game.Result = gameResult;

            _expectedMoveNo = false;
            _expectedMoves = false;
            _foundEndGame = true;
        }

        private PieceType ParsePiece(char cPiece)
        {
            PieceType pieceType;

            switch (cPiece)
            {
                case _cKing:
                    pieceType = PieceType.King;
                    break;
                case _cQueen:
                    pieceType = PieceType.Queen;
                    break;
                case _cRook:
                    pieceType = PieceType.Rook;
                    break;
                case _cBishop:
                    pieceType = PieceType.Bishop;
                    break;
                case _cKnight:
                    pieceType = PieceType.Knight;
                    break;
                default:
                    pieceType = PieceType.None;
                    break;
            }

            return pieceType;
        }

        private void ParseTokenAsMove(string token)
        {
            Move move = new Move();
            Piece piece = new Piece();

            if (!_expectedMoves)
                throw new PGNFormatterException($"Unexpected [{token}] in line: {_currentLine}");

            move.MoveNo = _currentMoveNo;
            move.Results = MoveResults.Regular;

            piece.Color = _currentColor;
            
            Regex rxValidMove = new Regex(@"^(O-O|O-O-O|[KQRBN]?[a-h]?[1-8]?x?[a-h][1-8](=[KQRBN])?)(\+|#)?$");

            if (!rxValidMove.IsMatch(token))
                throw new PGNFormatterException($"Invalid move [{token}] in line: {_currentLine}");

            string sResult = string.Empty;

            if (token.StartsWith(_sCastlingQueenSide))
            {
                // Expected O-O-O{+|#}
                move.Results |= MoveResults.CastlingQueenSide;
                piece.Type = PieceType.King;
                sResult = token.Substring(_sCastlingQueenSide.Length);
            }
            else if (token.StartsWith(_sCastlingKingSide))
            {
                // Expected O-O{+|#}
                move.Results |= MoveResults.CastlingKingSide;
                piece.Type = PieceType.King;
                sResult = token.Substring(_sCastlingKingSide.Length);
            }
            else
            {
                // Expected {F}{f}{r}{x}fr{=F}{+|#}
                //          |1||2   ||3|4 |5 ||6  |

                string s;
                int pos;

                // Field#1 Figure
                PieceType pieceType = ParsePiece(token[0]);

                if (pieceType == PieceType.None)
                {
                    pieceType = PieceType.Pawn;
                    s = token;
                }
                else
                {
                    s = token.Substring(1);
                }

                piece.Type = pieceType;

                // Field#3 Capture
                pos = s.IndexOf(_cCapture);
                if (pos != -1)
                {
                    move.Results |= MoveResults.Capture;
                    s = s.Remove(pos, 1);
                }

                // Field#5 Promotion
                pos = s.IndexOf(_cPromotion);
                if (pos != -1)
                {
                    move.Results |= MoveResults.Promotion;
                    move.PromoteTo = ParsePiece(s[pos + 1]);
                    s = s.Remove(pos, 2);
                }

                // Field#6 Game result
                pos = s.IndexOfAny(new char[] { _cCheck, _cCheckmate });
                if (pos != -1)
                {
                    sResult = s.Substring(pos, 1);
                    s = s.Remove(pos, 1);
                }

                // Field#4 New coordinate
                pos = s.Length - 2;
                move.To = new Coordinate(s.Substring(pos));
                s = s.Remove(pos, 2);

                // Field#2 File or/and rank
                Coordinate fromField;
                if (TryFindFromField(s, piece.Type, move.To, out fromField))
                {
                    move.From = fromField;
                }
                else
                    throw new PGNFormatterException($"Incorrect move [{token}] in line: {_currentLine}");
            }

            if (sResult.Length > 1)
                throw new PGNFormatterException($"Invalid move result [{token}] in line: {_currentLine}");

            if (sResult.Length == 1)
            {
                switch (sResult[0])
                {
                    case _cCheck:
                        move.Results |= MoveResults.Check;
                        break;
                    case _cCheckmate:
                        move.Results |= MoveResults.Checkmate;
                        break;
                }
            }

            move.Piece = piece;

            _foundAnyMove = true;
            _game.AddMove(move);

            if (_currentColor == Color.Black)
            {
                _currentColor = Color.White;
                _expectedMoveNo = true;
            }
            else
            {
                _currentColor = Color.Black;
            }
        }

        private bool TryFindFromField(string sFileRank, PieceType pieceType, Coordinate toField, out Coordinate fromField)
        {
            bool result = false;
            Position position = new Position(_game.GetCurrentPosition());
            Color sideToMove = position.SideToMove;
            List<Coordinate> fields = new List<Coordinate>();
            Move[] possibleMoves;
            Piece piece;
            byte file;
            byte rank;

            fromField = "a1";

            if (sFileRank.Length == 2)
            {
                fromField = sFileRank;

                piece = position[fromField];

                if (piece.Color == sideToMove && piece.Type == pieceType)
                    fields.Add(fromField);
            }
            else if (sFileRank.Length <= 1)
            {
                for (byte c = 0; c < 64; c++)
                {
                    piece = position[c];

                    if (piece.Color == sideToMove && piece.Type == pieceType)
                        fields.Add(c);
                }
            }

            possibleMoves = _rulesExpert.GetPossibleMoves(position, fields.ToArray());
                
            if (sFileRank.Length == 2)
            {
                int cnt = 0;

                foreach (Move m in possibleMoves)
                {
                    if (m.To == toField)
                    {
                        fromField = m.From;
                        if (++cnt > 1) break;
                    }
                }

                result = cnt == 1;
            }
            else if (sFileRank.Length == 1 && Coordinate.TryParseFile(sFileRank[0], out file))
            {
                int cnt = 0;

                foreach(Move m in possibleMoves)
                {
                    if (m.To == toField && m.From.File == file)
                    {
                        fromField = m.From;
                        if (++cnt > 1) break;
                    }
                }

                result = cnt == 1;
            }
            else if (sFileRank.Length == 1 && Coordinate.TryParseRank(sFileRank[0], out rank))
            {
                int cnt = 0;

                foreach (Move m in possibleMoves)
                {
                    if (m.To == toField && m.From.Rank == rank)
                    {
                        fromField = m.From;
                        if (++cnt > 1) break;
                    }
                }

                result = cnt == 1;
            }
            else if (sFileRank.Length == 0)
            {
                int cnt = 0;

                foreach (Move m in possibleMoves)
                {
                    if (m.To == toField)
                    {
                        fromField = m.From;
                        if (++cnt > 1) break;
                    }
                }

                result = cnt == 1;
            }
            return result;
        }

        private void ParseLineAsMoveList(string line)
        {
            string[] tokens = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            foreach(string token in tokens)
            {
                GameResult gameResult;

                if (TryParseGameResult(token, out gameResult))
                    SetGameResult(gameResult);
                else
                {
                    string sMove;

                    if (_expectedMoveNo)
                    {                        
                        int dotPos = token.IndexOf('.');

                        if (dotPos < 1)
                            throw new PGNFormatterException($"Expected move number followed by dot [{token}] in line: {line}");

                        string sMoveNo = token.Substring(0, dotPos);
                        ushort moveNo;

                        if (!ushort.TryParse(sMoveNo, out moveNo))
                            throw new PGNFormatterException($"Expected move number [{sMoveNo}] in line: {line}");

                        if (_currentMoveNo + 1 != moveNo)
                            throw new PGNFormatterException($"Expected move number {_currentMoveNo + 1} instead of {moveNo} in line: {line}");

                        _currentMoveNo = moveNo;
                        _expectedMoveNo = false;

                        sMove = token.Substring(dotPos + 1);
                    }
                    else
                    {
                        sMove = token;
                    }

                    if (sMove.Length != 0)
                        ParseTokenAsMove(sMove);
                }
            }
        }

        public Game Parse(TextReader reader)
        {
            string line;

            InitializeParser();

            while ((line=reader.ReadLine()) != null)
            {
                _currentLine = line;
                line = line.Trim();

                if (line.Length == 0)
                {
                    if (_expectedMoves)
                    {
                        _expectedMoves = false;
                        break;
                    }

                    if (_expectedHeader)
                    {
                        _expectedHeader = false;
                        _expectedMoves = true;
                        _expectedMoveNo = true;
                    }
                }
                else
                {
                    if (_expectedFirstLine)
                    {
                        _expectedFirstLine = false;
                        _expectedHeader = true;
                    }
                    else if (_expectedHeader && !IsLikeHeaderLine(line))
                    {
                        _expectedHeader = false;
                        _expectedMoves = true;
                        _expectedMoveNo = true;
                    }

                    if (_expectedHeader)
                        ParseLineAsHeader(line);
                    else if (_expectedMoves)
                        ParseLineAsMoveList(line);
                }

                if (_foundEndGame)
                    break;
            }

            // TODO Проверить, что распарсили полностью
            // TODO Возвращать null, если ничего не распарсили
            return _game;
        }
    }
}
