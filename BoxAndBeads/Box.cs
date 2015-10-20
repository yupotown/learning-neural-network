using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxAndBeads
{
    class Box
    {
        public Box(int initCount)
        {
            Beads = new List<Bead>();
            for (var y = 0; y < 3; ++y)
            {
                for (var x = 0; x < 3; ++x)
                {
                    for (var i = 0; i < initCount; ++i)
                    {
                        Beads.Add(new Bead(x, y));
                    }
                }
            }
        }

        public Bead GetBead()
        {
            if (Beads.Count == 0)
            {
                return null;
            }
            else
            {
                var i = rnd.Next(Beads.Count);
                var bead = Beads[i];
                Beads.RemoveAt(i);
                return bead;
            }
        }

        public void AddBeads(Bead bead, int count)
        {
            for (var i = 0; i < count; ++i)
            {
                Beads.Add(bead);
            }
        }

        public List<Bead> Beads { get; private set; }

        private Random rnd = new Random();
    }
}
