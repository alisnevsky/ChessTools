using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;
using Chess.Formatters;
using System.Collections.Specialized;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = File.OpenText(@"c:\Work\PGN\sample.txt"))
            {

                Dictionary<string,string> c = new Dictionary<string,string>();
                c.Add("Nam", "Val1");
                c.Add("A", "B");
                c["B"] = "C";
                c["A"] = "B2";
   
                foreach(var k in c)
                {
                    string key = k.Key;
                    string val = k.Value;
                }

                PGNFormatter parser = new PGNFormatter();
                Game game = parser.Parse(reader);

                Move m = game.Moves[2];
                //game.Moves[3] = m;

                Coordinate cr = new Coordinate("a2");
                int[] a = new int[64];
                a[cr] = 100;

                //Coordinate t = 64;

                IReadOnlyPosition rp = game.GetCurrentPosition();

                //Position p2 = rp as Position;
                //p2.SetInitial();
            }
        }
    }
}
