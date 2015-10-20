using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxAndBeads
{
    class ThinkingBoxes
    {
        public ThinkingBoxes()
        {
            for (var i = 0; i < mapCount; ++i)
            {
                boxes[i] = new Box(unmap(i), 3);
            }
        }

        public Bead Think(MaruBatsu mb)
        {
            var box = boxes[map(mb)];
            var bead = box.GetBead();
            log.Add(Tuple.Create(box, bead));
            return bead;
        }

        public void OnWon()
        {
            foreach (var l in log)
            {
                l.Item1.AddBeads(l.Item2, 2);
            }
            log.Clear();
        }

        public void OnLost()
        {
            log.Clear();
        }

        public void OnDrawn()
        {
            foreach (var l in log)
            {
                l.Item1.AddBeads(l.Item2, 1);
            }
            log.Clear();
        }

        private int map(MaruBatsu mb)
        {
            var res = 0;
            for (var y = 0; y < 3; ++y)
            {
                for (var x = 0; x < 3; ++x)
                {
                    res *= 3;
                    res += map(mb[x, y]);
                }
            }
            return res;
        }

        private MaruBatsu unmap(int mapped)
        {
            var res = new MaruBatsu();
            for (var y = 2; y >= 0; --y)
            {
                for (var x = 2; x >= 0; --x)
                {
                    res[x, y] = unmapState(mapped % 3);
                    mapped /= 3;
                }
            }
            return res;
        }

        private int map(MaruBatsu.State s)
        {
            switch (s)
            {
                case MaruBatsu.State.None: return 0;
                case MaruBatsu.State.Maru: return 1;
                case MaruBatsu.State.Batsu: return 2;
            }
            throw new NotImplementedException();
        }

        private MaruBatsu.State unmapState(int mapped)
        {
            if (mapped % 3 == 0)
            {
                return MaruBatsu.State.None;
            }
            else if (mapped % 3 == 1)
            {
                return MaruBatsu.State.Maru;
            }
            else
            {
                return MaruBatsu.State.Batsu;
            }
        }

        private Box[] boxes = new Box[mapCount];
        private List<Tuple<Box, Bead>> log = new List<Tuple<Box,Bead>>();

        // map により得られる値の最大値 + 1
        // 最小値は 0
        private static readonly int mapCount = 19683; // = 3^9
    }
}
