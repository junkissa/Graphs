using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Class_Graph
{
   public class Graph
    {
        public const int max = 200;
        private  int n;
        private  int m;
        public float [ , ] ar_graph_point_position;
        public int[, ,] ar_sumiznist;
        public float[,] ar_coord_reber;
        public bool isDir = false;
        public bool isWeighed;

        public Graph()
        {
            n = 0;
            m = 0;
            isDir = false;
            isWeighed = false;
            ar_graph_point_position = new float [max, 2]; // номер вершины, координаты
            ar_sumiznist = new int[max, max, 2]; //  наявність і вага ребра 
            for (int i = 0; i < max; ++i)
                for (int j = 0; j < max; ++j)
                {
                    ar_sumiznist[i, j, 0] = 0;
                    ar_sumiznist[i, j, 1] = 0;
                }
            ar_coord_reber = new float[(max * (max - 1) / 2), 4];
        }
        public int get_n() {return n;}
        public int get_m() { return m;}
        public void add_sumiznist(int i, int j)
        {
            ar_sumiznist[i, j, 0] = 1;
            if (!isDir)
                ar_sumiznist[j, i, 0] = 1;
        }
        public void add_mass(int i, int j, int mass)
        {
            ar_sumiznist[i, j, 1] = mass;
            if (!isDir)
                ar_sumiznist[j, i, 1] = mass;
        }
        public void add_n() { ++n; }
        public void add_m() { ++m; }
		public void sub_n() { --n; }
		public void sub_m() { --m; }
		public void del() {
			for (int i = 0; i < max; ++i)
				for (int j = 0; j < max; ++j)
				{
					ar_sumiznist[i, j, 0] = 0;
					ar_sumiznist[i, j, 1] = 0;
				}
			n = 0; m = 0; isDir = false; isWeighed = false;}
    }
}
