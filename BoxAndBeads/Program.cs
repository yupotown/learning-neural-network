using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxAndBeads
{
    class Program
    {
        static void Main(string[] args)
        {
            var boxes = new ThinkingBoxes();

            while (true)
            {
                var mb = new MaruBatsu();
                var turn = MaruBatsu.State.Maru;

                while (!mb.Finished)
                {
                    Show(mb);
                    Console.Write("{0} >", ToStr(turn));

                    // player
                    while (turn == MaruBatsu.State.Maru)
                    {
                        var line = Console.ReadLine().Select(v => (int)(v - '1')).ToList();
                        var x = line[0];
                        var y = line[1];
                        if (x < 0 || y < 0 || x >= 3 || y >= 3 || mb[x, y] != MaruBatsu.State.None)
                        {
                            Console.WriteLine("そこにはおけないよ");
                            continue;
                        }
                        mb[x, y] = turn;
                        break;
                    }

                    // boxes & beads
                    if (turn == MaruBatsu.State.Batsu)
                    {
                        var bead = boxes.Think(mb);
                        Console.WriteLine("{0}{1}", bead.X + 1, bead.Y + 1);
                        mb[bead.X, bead.Y] = turn;
                    }

                    turn = (turn == MaruBatsu.State.Maru) ? MaruBatsu.State.Batsu : MaruBatsu.State.Maru;
                }

                // result
                Show(mb);
                if (mb.MaruWon)
                {
                    Console.WriteLine("あなたの勝ち");
                    boxes.OnLost();
                }
                else if (mb.BatsuWon)
                {
                    Console.WriteLine("箱とビーズの勝ち");
                    boxes.OnWon();
                }
                else
                {
                    Console.WriteLine("引き分け");
                    boxes.OnDrawn();
                }
            }
        }

        static void Show(MaruBatsu mb)
        {
            for (var y = 0; y < 3; ++y)
            {
                for (var x = 0; x < 3; ++x)
                {
                    Console.Write(ToStr(mb[x, y]));
                }
                Console.WriteLine();
            }
        }

        static string ToStr(MaruBatsu.State s)
        {
            switch (s)
            {
                case MaruBatsu.State.None: return "　";
                case MaruBatsu.State.Maru: return "○";
                case MaruBatsu.State.Batsu: return "×";
            }
            throw new ArgumentException();
        }
    }
}
