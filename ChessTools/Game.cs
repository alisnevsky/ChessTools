using System.Collections.Generic;

namespace Chess
{
    public enum GameResult
    {
        None,
        InProgress,
        WhiteWins,
        BlackWins,
        Draw
    }

    public class Game
    {
        private List<Move> _moves;
        private Position _initialPosition;
        private Position _currentPosition;
        private int _currentMoveIdx;
        private GameResult _result;
        private ushort _firstMoveNo;
        private Dictionary<string, string> _properties;

        public Game()
        {
            _properties = new Dictionary<string, string>();
            _moves = new List<Move>();
            _firstMoveNo = 1;
            _currentMoveIdx = 0;
            _initialPosition = new Position();
            _currentPosition = new Position();
        }

        public void SetInitialPosition()
        {
            _initialPosition.SetInitial();
            _currentPosition.SetInitial();
            _currentMoveIdx = 0;
            _moves.Clear();

            FirstMoveNo = 1;
            Result = GameResult.InProgress;
        }

        public IDictionary<string, string> Properties
        {
            get
            {
                return _properties;
            }
        }

        public GameResult Result
        {
            get
            {
                return _result;
            }

            set
            {
                _result = value;
            }
        }

        public IReadOnlyList<Move> Moves
        {
            get
            {
                return _moves;
            }
        }

        public void AddMove(Move move)
        {
            _moves.Add(move);
        }

        public ushort FirstMoveNo
        {
            get
            {
                return _firstMoveNo;
            }

            set
            {
                _firstMoveNo = value;
            }
        }

        public IReadOnlyPosition GetCurrentPosition()
        {
            return _currentPosition;
        }
    }
}
