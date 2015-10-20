using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxAndBeads
{
    class MaruBatsu
    {
        public MaruBatsu()
        {
            Init();
        }

        public enum State
        {
            None,
            Maru,
            Batsu,
        }

        public void Init()
        {
            for (var y = 0; y < 3; ++y)
            {
                for (var x = 0; x < 3; ++x)
                {
                    state[x, y] = State.None;
                }
            }
        }

        public bool Finished
        {
            get
            {
                return state.Cast<State>().All(v => v != State.None) || MaruWon || BatsuWon;
            }
        }

        public bool MaruWon { get { return won(State.Maru); } }
        public bool BatsuWon { get { return won(State.Batsu); } }

        public State this[int x, int y]
        {
            get { return state[x, y]; }
            set { state[x, y] = value; }
        }

        public MaruBatsu Clone()
        {
            var res = new MaruBatsu();
            for (var y = 0; y < 3; ++y)
            {
                for (var x = 0; x < 3; ++x)
                {
                    res[x, y] = this[x, y];
                }
            }
            return res;
        }

        private bool won(State player)
        {
            return
                   (state[0, 0] == player && state[0, 1] == player && state[0, 2] == player)
                || (state[1, 0] == player && state[1, 1] == player && state[1, 2] == player)
                || (state[2, 0] == player && state[2, 1] == player && state[2, 2] == player)
                || (state[0, 0] == player && state[1, 0] == player && state[2, 0] == player)
                || (state[0, 1] == player && state[1, 1] == player && state[2, 1] == player)
                || (state[0, 2] == player && state[1, 2] == player && state[2, 2] == player)
                || (state[0, 0] == player && state[1, 1] == player && state[2, 2] == player)
                || (state[0, 2] == player && state[1, 1] == player && state[2, 0] == player);
        }

        private State[,] state = new State[3, 3];
    }
}
