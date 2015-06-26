using System;
using System.Collections;
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
using Class_Graph;

namespace Graph_try_OpenGL_Csh
{
	public partial class Form1 : Form
	{
		// размеры окна 
		double ScreenW, ScreenH;

		// отношения сторон окна визуализации
		// для корректного перевода координат мыши в координаты, 
		// принятые в программе 

		private float devX;
		private float devY; // соотношение для перевода пикселей в нормальный разер экрана

		private int Mcoord_x = 0, Mcoord_y = 0;
		private float[] ar_point_to_draw_edg  = new float[4]; // для хранения вершин ребра до занесения в массив рёбер
		private float mouse_draw_line_x0, mouse_draw_line_y0; // початкова точка для відмалювання ребра
		private float mouse_draw_line_x1, mouse_draw_line_y1; // кінцева точка для відмалювання ребра
		private bool draw_line = false; // зараз малюється ребро
		private float mouse_draw_point_x, mouse_draw_point_y;
		private int pSize = 17;
		private int lWidth = 2;
		private int delta = 1; // допустимое "промазывание" по вершине
		private bool with_mouse;
		private const double eps = 1;
		private int int_weigh;
		//  bool some_vertex_is_choosen = false;
		static int choosen_vertex = 0;
		static int choosen_vertex_right = 0;
		private int EPS = 10000;
		private bool B_F = false; // результат Белмана-Форда

		public Form1()
		{
			InitializeComponent();
			AnT.InitializeContexts();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// инициализация бибилиотеки glut 
			Glut.glutInit();
			// инициализация режима экрана 
			Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

			// установка цвета очистки экрана (RGBA) 
			Gl.glClearColor(255, 255, 255, 1);

			// установка порта вывода 
			Gl.glViewport(0, 0, AnT.Width, AnT.Height);

			// активация проекционной матрицы 
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			// очистка матрицы 
			Gl.glLoadIdentity();

			// определение параметров настройки проекции, в зависимости от размеров сторон элемента AnT. 
			if ((float)AnT.Width <= (float)AnT.Height)
			{
				ScreenW = 30.0;
				ScreenH = 30.0 * (float)AnT.Height / (float)AnT.Width;

				Glu.gluOrtho2D(0.0, ScreenW, 0.0, ScreenH);
			}
			else
			{
				ScreenW = 30.0 * (float)AnT.Width / (float)AnT.Height;
				ScreenH = 30.0;

				Glu.gluOrtho2D(0.0, 30.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 30.0);
			}

			// сохранение коэфицентов, которые нам необходимы для перевода координат указателя в оконной системе, в координаты 
			// принятые в нашей OpenGL сцене 
			devX = (float)ScreenW / (float)AnT.Width;
			devY = (float)ScreenH / (float)AnT.Height;

			// установка объектно-видовой матрицы 
			Gl.glMatrixMode(Gl.GL_MODELVIEW);

			// redrawWindow.Start();
		}

		// малювання неорієнтованого графа
		private void Draw_Graph(object sender, EventArgs e)
		{
			// очистка буфера цвета и буфера глубины 
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

			// очищение текущей матрицы 
			Gl.glLoadIdentity();

			draw_edg();
			draw_vertex();
			draw_numbers_of_vertex();
			Gl.glFlush();
			AnT.Invalidate();

		}

		private void draw_numbers_of_vertex()
		{
			int n_ = G.get_n();
			Gl.glColor3d(0, 0, 0);
			for (int i = 0; i < n_; ++i)
			{

				Gl.glRasterPos2f(G.ar_graph_point_position[i, 0] - 3*devX, G.ar_graph_point_position[i, 1] - 3*devY);
				string text = (i+1).ToString();
				// в цикле foreach перебираем значения из массива text, 
				// который содержит значение строки для визуализации 
				foreach (char char_for_draw in text)
				{
					// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
					Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
				}

			}
			if (G.isWeighed)
			{
				Gl.glColor3d(1, 0, 0);
				float x0, y0;
				for (int i = 0; i < n_; ++i)
				{
					for (int j = i; j < n_; ++j)
						if (G.ar_sumiznist[i, j, 0] == 1)
						// вычислить координаты центра ребра
						{
							x0 = (G.ar_graph_point_position[i, 0] + G.ar_graph_point_position[j, 0]) / 2;
							y0 = (G.ar_graph_point_position[i, 1] + G.ar_graph_point_position[j, 1]) / 2;

							Gl.glRasterPos3f(x0 + 5 * devX, y0 + 5 * devY, 1);
							string text = (G.ar_sumiznist[i, j, 1]).ToString();
							// в цикле foreach перебираем значения из массива text, 
							// который содержит значение строки для визуализации 
							foreach (char char_for_draw in text)
							{
								// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
								Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
							}
						}
				}
			}


		}



		public Graph G = new Graph();
		public int sposob_vvoda = 0; // 1 - рисовать, 2 - сгенирировать

		private void пошукВШиринуToolStripMenuItem_Click(object sender, EventArgs e)
		{
			label2.Text = "поле вводу ваги ";
			// выбрать начальную вершину

			int n = G.get_n();
			int[] poradok_obhodu = new int[n];
			int n_poradok = 0;
			bool[] visited = new bool[n];
			for (int v = choosen_vertex; v < n + choosen_vertex; ++v)
			{
				if (v >= n)
				{
					if (!visited[v - choosen_vertex])
						bfs(v - choosen_vertex, ref poradok_obhodu, ref n_poradok, ref visited);
				}
				else
					if (!visited[v])
						bfs(v, ref poradok_obhodu, ref n_poradok, ref visited);
			}

			Draw_Graph(sender, e);

			// пронумеровать порядок прохода вершин
			Gl.glColor3d(1, 0, 0);
			for (int i = 0; i < n; ++i)
			{
				int v = poradok_obhodu[i];
				Gl.glRasterPos2f(G.ar_graph_point_position[v, 0] - 3 * devX, G.ar_graph_point_position[v, 1] + 13 * devY);
				string text = "[ " + (i + 1).ToString() + " ]";

				// в цикле foreach перебираем значения из массива text, 
				// который содержит значение строки для визуализации 
				foreach (char char_for_draw in text)
				{
					// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
					Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
				}
			}


		}

		private void bfs(int v, ref int[] por, ref int n_por, ref bool[] vis)
		{
			Queue<int> Q = new Queue<int>();
			por[n_por++] = v;
			vis[v] = true;
			Q.Enqueue(v);
			while (Q.Count() != 0)
			{
				int f = Q.Dequeue();
				for (int i = 0; i < G.get_n(); ++i)
				{
					if (G.ar_sumiznist[f, i, 0] == 1)
						if (!vis[i])
						{
							por[n_por++] = i;
							vis[i] = true;
							Q.Enqueue(i);
						}
				}
			}
		}

		private void мишкоюToolStripMenuItem_Click(object sender, EventArgs e)
		{
			with_mouse = true;
		}

		private void згенеруватиToolStripMenuItem_Click(object sender, EventArgs e)
		{
			G.del();
			checkBox1.Checked = false;
			checkBox2.Checked = false;
			Draw_Graph(sender, e);
		}

		private bool is_vertex_area(float xx, float yy, ref int ver) // проверка, пренадлежит ли точка вершине
		{
			for (int i = 0; i < G.get_n(); ++i)
			{

				if ((Math.Abs(xx - G.ar_graph_point_position[i, 0]) <= (pSize/2 + delta) * devX) && (Math.Abs(yy - G.ar_graph_point_position[i, 1]) <= (pSize/2 + delta)* devY))
				{
					ver = i; // номер вершини
					return true;
				}
			}

			return false;
		}

		private bool can_be_drawn(float xx, float yy)
		{
			for (int i = 0; i < G.get_n(); ++i)
			{

				if ((Math.Abs(xx - G.ar_graph_point_position[i, 0]) <= (pSize / 2 + 10 * delta) * devX) && (Math.Abs(yy - G.ar_graph_point_position[i, 1]) <= (pSize / 2 + 10 * delta) * devY))
					return false;

			}

			return true;
		}

		private void AnT_MouseDown(object sender, MouseEventArgs e) // добавление вершин в массив
		{
			label2.Text = "поле вводу ваги ";
			if (e.Button == MouseButtons.Right && with_mouse)
			{
				Mcoord_x = e.X;
				Mcoord_y = e.Y;
				// переведення в розмірність екрану
				mouse_draw_line_x0 = Mcoord_x * devX;
				mouse_draw_line_y0 = (float)ScreenH - Mcoord_y * devY;
				//проверка принадлежности вершине             
				int ver = 0;
				draw_line = is_vertex_area(mouse_draw_line_x0, mouse_draw_line_y0, ref ver);
				if (draw_line)
				{
					ar_point_to_draw_edg[0] = G.ar_graph_point_position[ver, 0];
					ar_point_to_draw_edg[1] = G.ar_graph_point_position[ver, 1];
				}
			}

			if (e.Button == MouseButtons.Left && with_mouse)
			{

				Mcoord_x = e.X;
				Mcoord_y = e.Y;
				// переведення в розмірність екрану
				mouse_draw_point_x = Mcoord_x * devX;
				mouse_draw_point_y = (float)ScreenH - Mcoord_y * devY;
				if (can_be_drawn(mouse_draw_point_x, mouse_draw_point_y))
				{
					G.ar_graph_point_position[G.get_n(), 0] = mouse_draw_point_x;
					G.ar_graph_point_position[G.get_n(), 1] = mouse_draw_point_y;
					G.add_n(); // добавление вершины
					Draw_Graph(sender, e);
				}

			}


		}

		private void draw_vertex() // рисование вершин графа
		{
			Gl.glColor3d(1, 0, 0);
			Gl.glPointSize(pSize);
			Gl.glBegin(Gl.GL_POINTS);
			for (int i = 0; i < G.get_n(); ++i)
			{

				mouse_draw_point_x = G.ar_graph_point_position[i, 0];
				mouse_draw_point_y = G.ar_graph_point_position[i, 1];
				Gl.glVertex2f(mouse_draw_point_x, mouse_draw_point_y);
			}
			Gl.glEnd();
			Gl.glColor3d(0.3, 0.5, 0);
			Gl.glBegin(Gl.GL_POINTS);
			Gl.glVertex2d(G.ar_graph_point_position[choosen_vertex, 0], G.ar_graph_point_position[choosen_vertex, 1]);
			Gl.glEnd();
		}


		private void AnT_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && draw_line && with_mouse)
			{
				int ver2 = -1;
				Mcoord_x = e.X;
				Mcoord_y = e.Y;
				// переведення в розмірність екрану
				mouse_draw_line_x1 = Mcoord_x * devX;
				mouse_draw_line_y1 = (float)ScreenH - Mcoord_y * devY;
				if (is_vertex_area(mouse_draw_line_x1, mouse_draw_line_y1, ref ver2))
				{
					// взфть коодинаты центра второй вершины, добавить в массив для рисования ребра
					ar_point_to_draw_edg[2] = G.ar_graph_point_position[ver2, 0];
					ar_point_to_draw_edg[3] = G.ar_graph_point_position[ver2, 1];

					G.ar_coord_reber[G.get_m(), 0] = ar_point_to_draw_edg[0];
					G.ar_coord_reber[G.get_m(), 1] = ar_point_to_draw_edg[1];
					G.ar_coord_reber[G.get_m(), 2] = ar_point_to_draw_edg[2];
					G.ar_coord_reber[G.get_m(), 3] = ar_point_to_draw_edg[3];
					int num_of_vertex_to_check1 = 0, num_of_vertex_to_check2 = 0;

					// проверить, не проведено ли ребро
					is_vertex_area(ar_point_to_draw_edg[0], ar_point_to_draw_edg[1], ref num_of_vertex_to_check1);
					is_vertex_area(ar_point_to_draw_edg[2], ar_point_to_draw_edg[3], ref num_of_vertex_to_check2);
					if (G.ar_sumiznist[num_of_vertex_to_check1, num_of_vertex_to_check2, 0] == 0 && num_of_vertex_to_check1 != num_of_vertex_to_check2)
					{
						G.add_sumiznist(num_of_vertex_to_check1, num_of_vertex_to_check2);
						if (G.isWeighed)
						{
							G.add_mass(num_of_vertex_to_check1, num_of_vertex_to_check2, int_weigh);
							textBox1.Text = "";
							int_weigh = 0;
						}
						G.add_m();
					}
					Draw_Graph(sender, e);
				}
			}
			draw_line = false;

		}

		private void draw_edg() // рисование ребер графа
		{
			Gl.glColor3b(0, 0, 0);
			Gl.glLineWidth(lWidth);

			for (int i = 0; i < G.get_m(); ++i)
			{

				mouse_draw_line_x0 = G.ar_coord_reber[i, 0];
				mouse_draw_line_y0 = G.ar_coord_reber[i, 1];
				mouse_draw_line_x1 = G.ar_coord_reber[i, 2];
				mouse_draw_line_y1 = G.ar_coord_reber[i, 3];
				if (!G.isDir)
				{
					Gl.glBegin(Gl.GL_LINES);
					Gl.glVertex3f(mouse_draw_line_x0, mouse_draw_line_y0, -1);
					Gl.glVertex3f(mouse_draw_line_x1, mouse_draw_line_y1, -1);
					Gl.glEnd();
				}

				else if (G.isDir)
				{
					//double tan_alpha, b, d, alpha, x1, y1, x2, y2;
					//x1 = mouse_draw_line_x0;
					//y1 = mouse_draw_line_y0;
					//x2 = mouse_draw_line_x1;
					//y2 = mouse_draw_line_y1;
					//tan_alpha = (y2 - y1)/(x2 - x1);
					//alpha = Math.Atan(tan_alpha);
					//b = (y1 * x2 - y2 * x1)/(x2 - x1);

					//d = Math.Sqrt(x2 * x2 + (b - y2) * (b - y2));

					//Gl.glColor3d(0, 0, 0);
					//Gl.glPushMatrix();
					// Gl.glTranslated(0, b, 0);
					// Gl.glRotated(Math.PI/2 - alpha, 0, 0, 1);
					// Gl.glTranslated(d, 0, 0);
					// Glut.glutSolidSphere(3, 32, 32);
					// Gl.glBegin(Gl.GL_TRIANGLES);
					//  Gl.glVertex2d(0, 0);
					//  Gl.glVertex2d(2, -6);
					//  Gl.glVertex2d(-2, -6);
					// Gl.glEnd();
					//Gl.glPopMatrix();
					Gl.glBegin(Gl.GL_TRIANGLES);
					Gl.glVertex3f(mouse_draw_line_x0 + 7 * devX, mouse_draw_line_y0, 0);
					Gl.glVertex3f(mouse_draw_line_x0, mouse_draw_line_y0 + 7 * devY, 0);
					Gl.glVertex3f(mouse_draw_line_x1, mouse_draw_line_y1, 0);
					Gl.glEnd();


				}

			}
		}

		private void AnT_MouseMove(object sender, MouseEventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			G.isDir = true;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			G.isWeighed = true;
		}

		private void пошукВГлибинуToolStripMenuItem_Click(object sender, EventArgs e)
		{
			label2.Text = "поле вводу ваги ";
			int n = G.get_n();
			int[] poradok_obhodu = new int[n];
			bool[] visited = new bool[n];
			for (int i = 0; i < n; ++i)
				visited[i] = false;
			int n_poradok = 0;
			for (int v = choosen_vertex; v < n + choosen_vertex; ++v)
			{
				if (v >= n)
				{
					if (!visited[n - choosen_vertex])
						dfsr(n - choosen_vertex, ref poradok_obhodu, ref n_poradok, ref visited);
				}
				else
					if (!visited[v])
						dfsr(v, ref poradok_obhodu, ref n_poradok, ref visited);
			}

			Draw_Graph(sender, e);

			// пронумеровать порядок прохода вершин
			Gl.glColor3d(1, 0, 0);
			for (int i = 0; i < n; ++i)
			{
				int v = poradok_obhodu[i];
				Gl.glRasterPos2f(G.ar_graph_point_position[v, 0] - 3 * devX, G.ar_graph_point_position[v, 1] + 13 * devY);
				string text = "[ " + (i + 1).ToString() + " ]";

				// в цикле foreach перебираем значения из массива text, 
				// который содержит значение строки для визуализации 
				foreach (char char_for_draw in text)
				{
					// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
					Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
				}
			}
		}

		void dfsr(int v, ref int[] por, ref int n_por, ref bool[] vis)
		{
			por[n_por++] = v;
			vis[v] = true;
			for (int w = 0; w < G.get_n(); ++w)
				if (G.ar_sumiznist[v, w, 0] == 1)
					if (!vis[w])
						dfsr(w, ref por, ref n_por, ref vis);
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (G.isWeighed)
			{
				int result;
				string value = textBox1.Text;
				
				try
				{
					result = Convert.ToInt32(value);
					Console.WriteLine("Converted the {0} value '{1}' to the {2} value {3}.",
									  value.GetType().Name, value, result.GetType().Name, result);
					int_weigh = result;
				}
				catch (OverflowException)
				{
					Console.WriteLine("{0} is outside the range of the Int32 type.", value);
				}
				catch (FormatException)
				{
					Console.WriteLine("The {0} value '{1}' is not in a recognizable format.",
									  value.GetType().Name, value);
				}
				if (value == "")
					int_weigh = 0;
			}

		}

		private void AnT_MouseDoubleClick(object sender, MouseEventArgs e)
		{

			label2.Text = "поле вводу ваги ";
			if (e.Button == MouseButtons.Left)
			{
				float xx = e.X * devX;
				float yy = (float)ScreenH - e.Y * devY;
				for (int i = 0; i < G.get_n(); ++i)
					if ((Math.Abs(xx - G.ar_graph_point_position[i, 0]) <= (pSize/2 + delta) * devX) && (Math.Abs(yy - G.ar_graph_point_position[i, 1]) <= (pSize/2 + delta)* devY))
						choosen_vertex = i;
				Draw_Graph(sender, e);
			}


			if (e.Button == MouseButtons.Right)
			{
				float xx = e.X * devX;
				float yy = (float)ScreenH - e.Y * devY;
				for (int i = 0; i < G.get_n(); ++i)
					if ((Math.Abs(xx - G.ar_graph_point_position[i, 0]) <= (pSize/2 + delta) * devX) && (Math.Abs(yy - G.ar_graph_point_position[i, 1]) <= (pSize/2 + delta)* devY))
						choosen_vertex_right = i;
				Draw_Graph(sender, e);
			}
		}

		private void крускалToolStripMenuItem_Click(object sender, EventArgs e)
		{
			label2.Text = "поле вводу ваги ";
			int n = G.get_n();
			bool[,] result = new bool[n, n]; // матрица смежности для результирующего дерева
			int[] color_of_vertex  = new int[n];
			int tree1 = -1, tree2 = -1;
			int sum = 0;
			for (int i = 0; i < n; ++i)
				for (int j = 0; j < n; ++j)
					result[i, j] = false;
			// разукрасить вершины в разные цвета
			for (int i = 0; i < n; ++i)
				color_of_vertex[i] = i;

			for (int m = 0; m < n - 1; ++m)
			{
				int min = EPS; // достаточно большое число
				for (int i = 0; i < n; ++i)
					for (int j = 0; j < n; ++j)

						if (G.ar_sumiznist[i, j, 0] == 1 && min > G.ar_sumiznist[i, j, 1] && color_of_vertex[i] != color_of_vertex[j])
						{
							min = G.ar_sumiznist[i, j, 1];
							tree1 = i;
							tree2 = j;
						}
				sum += min;
				if (color_of_vertex[tree1] != color_of_vertex[tree2]) // если вершины пренадлежат разным деревьям
				{
					result[tree1, tree2] = true;
					result[tree2, tree1] = true; // добавить в результирующую матрицу
					// все вершины дерава 2, добавить (ократись в тот же цвет во избежания циклов) в дерево 1
					int color_tree2 = color_of_vertex[tree2];
					for (int c = 0; c < n; ++c)
						if (color_of_vertex[c] == color_tree2)
							color_of_vertex[c] = color_of_vertex[tree1];

				}

			}
			Draw_Graph(sender, e);
			Gl.glColor3d(0, 0, 1);
			Gl.glBegin(Gl.GL_LINES);
			for (int i = 0; i < n; ++i)
				for (int j = i; j < n; ++j)
				{
					if (result[i, j])
					{
						Gl.glVertex2d(G.ar_graph_point_position[i, 0], G.ar_graph_point_position[i, 1]);
						Gl.glVertex2d(G.ar_graph_point_position[j, 0], G.ar_graph_point_position[j, 1]);
					}
				}
			Gl.glEnd();
			draw_vertex();
			draw_numbers_of_vertex();
			Gl.glFlush();
			AnT.Invalidate();
			// вывести масу
			label2.Text = "Загальна вага:";
			textBox1.Text = sum.ToString();

		}

		private void прімаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			label2.Text = "поле вводу ваги ";
			int n = G.get_n();
			bool[,] result = new bool[n, n]; // матрица смежности для результирующего дерева
			bool[] is_in_tree  = new bool[n];
			int[] T = new int[n];
			int n_in_tree = 0;
			int min;
			int sum = 0;
			int old_vertex = -1, new_vertex = -1;
			for (int i = 0; i < n; ++i)
				is_in_tree[i]  = false;
			for (int i = 0; i < n; ++i)
				for (int j = 0; j < n; ++j)
					result[i, j] = false;
			// начальная вершина
			T[n_in_tree++] = choosen_vertex;
			is_in_tree[choosen_vertex] = true;
			while (n_in_tree < n)
			{
				min = EPS;
				for (int i = 0; i < n_in_tree; ++i)
					for (int j = 0; j < n; ++j)
					{

						if (G.ar_sumiznist[T[i], j, 0] == 1 && min > G.ar_sumiznist[T[i], j, 1] && !is_in_tree[j]) // минисальное смежное не добавд=оеное ребро
						{
							min = G.ar_sumiznist[T[i], j, 1];
							old_vertex = T[i];
							new_vertex = j;
						}
					}
				sum += min;
				result[old_vertex, new_vertex] = true;
				result[new_vertex, old_vertex] = true;
				T[n_in_tree++] = new_vertex;
				is_in_tree[new_vertex] = true;
			}

			// рисовать результат
			Draw_Graph(sender, e);
			Gl.glColor3d(0, 0, 1);
			Gl.glBegin(Gl.GL_LINES);
			for (int i = 0; i < n; ++i)
				for (int j = i; j < n; ++j)
				{
					if (result[i, j])
					{
						Gl.glVertex2d(G.ar_graph_point_position[i, 0], G.ar_graph_point_position[i, 1]);
						Gl.glVertex2d(G.ar_graph_point_position[j, 0], G.ar_graph_point_position[j, 1]);
					}
				}
			Gl.glEnd();
			draw_vertex();
			draw_numbers_of_vertex();
			Gl.glFlush();
			AnT.Invalidate();
			// вывести масу
			label2.Text = "Загальна вага:";
			textBox1.Text = sum.ToString();

		}

		struct edge
		{
			public int a, b, cost;
		}

		private void беллманаФордаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			label2.Text = "поле вводу ваги ";
			int n = G.get_n();
			int[] d  = new int[n];
			b_f(sender, e, ref d);
			
		}

		bool b_f(object sender, EventArgs e, ref int [] d)
		{
			if (!G.isDir)
			{
				label2.Text = "Метод для орієнтованоо графа";
				return false;
			}
			int n = G.get_n(), m = G.get_m();
			
			int[] p = new int[n];
			edge[] ar_E;

			ar_E = new edge[m];

			for (int i = 0, n_in_edge = 0; i < n; ++i)
				for (int j = 0; j < n; ++j)
					if (G.ar_sumiznist[i, j, 0] == 1 && n_in_edge < m)
					{
						ar_E[n_in_edge].a = i;
						ar_E[n_in_edge].b = j;
						ar_E[n_in_edge].cost = G.ar_sumiznist[i, j, 1];
						++n_in_edge;
					}



			for (int i = 0; i < n; ++i)
			{
				p[i] = -1;
				d[i] = EPS;
			}

			d[choosen_vertex] = 0;
			int x = 0;
			for (int i=0; i < n; ++i)
			{
				x = -1;
				for (int j=0; j < m; ++j) // если реброс с - массой, и на парном месте в списке рёбер -  на него не смотреть

					if (d[ar_E[j].a] < EPS)

						if (d[ar_E[j].b] > d[ar_E[j].a] + ar_E[j].cost)
						{
							d[ar_E[j].b] = Math.Max(-EPS, d[ar_E[j].a] + ar_E[j].cost);
							p[ar_E[j].b] = ar_E[j].a;
							x = ar_E[j].b;
						}
			}

			if (x == -1)
			{
				label2.Text = "No negative cycle from " + (choosen_vertex + 1).ToString();
				B_F = true;
				// кротчайший путь к вершинам
				Draw_Graph(sender, e);
				Gl.glColor3d(0, 1, 0);
				for (int i = 0; i < n; ++i)
				{

					Gl.glRasterPos2f(G.ar_graph_point_position[i, 0] - 3 * devX, G.ar_graph_point_position[i, 1] + 13 * devY);
					string text1 = "[ " + (d[i]).ToString() + " ]";

					// в цикле foreach перебираем значения из массива text, 
					// который содержит значение строки для визуализации 
					foreach (char char_for_draw in text1)
					{
						// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
						Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
					}
				}
				Gl.glFlush();
				AnT.Invalidate();
				return true;

			}
			else
			{
				int y = x;
				for (int i=0; i<n; ++i)
					y = p[y];

				int[] path = new int[n];
				int n_in_p = 0;
				for (int cur=y; ; cur=p[cur])
				{
					path[n_in_p++] = cur;
					if (cur == y && n_in_p > 1)
						break;
				}
				//reverse(path.begin(), path.end());
				int[] re_path = new int[n_in_p];
				string text = "";
				for (int k = 0; k < n_in_p; ++k)
				{
					re_path[k] = path[n_in_p - k - 1];
					text += (re_path[k] + 1).ToString();
				}

				label2.Text = "Negative cycle: ";
				textBox1.Text = text;
				B_F = false;
				return false;
			}
		}
		private void дейкстриToolStripMenuItem_Click(object sender, EventArgs e)
		{
			label2.Text = "поле вводу ваги ";
			int n = G.get_n(), m = G.get_m();
			int[] d  = new int[n]; // d[i] = кротчайший путь из вершины choosen в вершину i (доина)
			//int[,] p = new int[n,n]; // p[i] = содержит кратчайший путь из верщины chosen в вершину i
			//int [] n_in_pi = new int [n]; // количество вершин от даной до і той
			int n_in_U = 0; // количество посещённыхх вершин
			for (int i = 0; i < n; ++i)
			{
				d[i] = EPS;
			}
			//n_in_pi[choosen_vertex] = 0;
			int min_v = -1;
			bool[] U = new bool[n]; // принадлежит ли вершина множеству посещённых вершин
			d[choosen_vertex] = 0;
			while (n_in_U <= n)
			{
				int min = EPS;
				for (int v = 0; v < n; ++v) // найдём вершину с минимальным путём
				{
					if (!U[v] && d[v] < min)
					{
						min = d[v];
						min_v = v;
					}
				}
				// нашли такую = min_v
				U[min_v] = true;
				++n_in_U;
				// ищем непосещённую вершину i смежную с v , и с меньшым весом при переходе
				for (int i = 0; i < n; ++i)
				{
					if (!U[i] && G.ar_sumiznist[min_v, i, 0] == 1)
						if (d[i] > d[min_v] + G.ar_sumiznist[min_v, i, 1])
							d[i] = d[min_v] + G.ar_sumiznist[min_v, i, 1];

				}
			}
			Draw_Graph(sender, e);
			// подписать кратчайший путь
			Gl.glColor3d(0, 1, 0);
			for (int i = 0; i < n; ++i)
			{

				Gl.glRasterPos2f(G.ar_graph_point_position[i, 0] - 3 * devX, G.ar_graph_point_position[i, 1] + 13 * devY);
				string text1 = "[ " + (d[i]).ToString() + " ]";

				// в цикле foreach перебираем значения из массива text, 
				// который содержит значение строки для визуализации 
				foreach (char char_for_draw in text1)
				{
					// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
					Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
				}
			}
			Gl.glFlush();
			AnT.Invalidate();
		}

		void deikstra_for_jonson(int[,,] ar_sum, ref int[] d)
		{
			int n = G.get_n(), m = G.get_m();
			//int[] d  = new int[n]; // d[i] = кротчайший путь из вершины choosen в вершину i (доина)
			int n_in_U = 0; // количество посещённыхх вершин
			for (int i = 0; i < n; ++i)
			{
				d[i] = EPS;
			}
			//n_in_pi[choosen_vertex] = 0;
			int min_v = -1;
			bool[] U = new bool[n]; // принадлежит ли вершина множеству посещённых вершин
			d[choosen_vertex] = 0;
			while (n_in_U <= n)
			{
				int min = EPS;
				for (int v = 0; v < n; ++v) // найдём вершину с минимальным путём
				{
					if (!U[v] && d[v] < min)
					{
						min = d[v];
						min_v = v;
					}
				}
				// нашли такую = min_v
				U[min_v] = true;
				++n_in_U;
				// ищем непосещённую вершину i смежную с v , и с меньшым весом при переходе
				for (int i = 0; i < n; ++i)
				{
					if (!U[i] && ar_sum[min_v, i, 0] == 1)
						if (d[i] > d[min_v] + ar_sum[min_v, i, 1])
							d[i] = d[min_v] + ar_sum[min_v, i, 1];

				}
			}

		}	

		private void флойдаВоршеллаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int n = G.get_n();
			int[,] W = new int[n, n];
			for (int i = 0; i < n; ++i)
				for (int j = 0; j < n; ++j)
				{
					if (G.ar_sumiznist[i, j, 0] == 1)
						W[i, j] = G.ar_sumiznist[i, j, 1];
					else
						W[i, j] = EPS;
					if (i == j)
						W[i, i] = 0;
				}
			for (int k = 0; k < n; ++k)
				for (int i = 0; i < n; ++i)
					for (int j = 0; j < n; ++j)
						W[i, j] = Math.Min(W[i, j], W[i, k] + W[k, j]);

			Draw_Graph(sender, e);
			// подписать кратчайший путь
			Gl.glColor3d(0, 1, 0);
			for (int i = 0; i < n; ++i)
			{

				Gl.glRasterPos2f(G.ar_graph_point_position[i, 0] - 3 * devX, G.ar_graph_point_position[i, 1] + 13 * devY);
				string text1 = "[ " + (W[choosen_vertex, i]).ToString() + " ]";

				// в цикле foreach перебираем значения из массива text, 
				// который содержит значение строки для визуализации 
				foreach (char char_for_draw in text1)
				{
					// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
					Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
				}
			}
			Gl.glFlush();
			AnT.Invalidate();

		}

		private void фордаФалкерсонаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int n = G.get_n();
			int[,] f = new int[n, n];//поток
			int[,] c = new int[n, n]; // пропускная способность
			int[,] mark = new int[n, 3]; // номер вершины, 0 = с какой пришли, 1 = +-, 2 = поток
			bool[] is_marked  = new bool[n];
			int[,] used = new int[n, n];
			int mas = 0;
			for (int i = 0; i < n; ++i)
				is_marked[i] = false;

			for (int i = 0; i < n; ++i)
				for (int j = 0; j < n; ++j)
				{
					f[i, j] = 0;
					c[i, j] = G.ar_sumiznist[i, j, 1];
				}
			// метка вершин
			is_marked[choosen_vertex] = true;
			mark[choosen_vertex, 0] = -1;
			mark[choosen_vertex, 1] = 0;
			mark[choosen_vertex, 2] = EPS;
			int last_vertex = choosen_vertex;
			int counter = 0;
			while (counter < 100)
			{
				is_marked[choosen_vertex] = true;
				mark[choosen_vertex, 0] = -1;
				mark[choosen_vertex, 1] = 0;
				mark[choosen_vertex, 2] = EPS;
				for (int k = 0; k < n; ++k)
				{
					for (int i = 0; i < n; ++i)
						if (is_marked[i])
							for (int j = 0; j < n; ++j)
							{
								if (G.ar_sumiznist[i, j, 0] == 1 && !is_marked[j])
								{
									if (f[i, j] < c[i, j])
									{
										is_marked[j] = true;
										mark[j, 0] = i;
										mark[j, 1] = 1;
										mark[j, 2] = Math.Min(mark[i, 2], c[i, j] - f[i, j]);
										last_vertex = j;
									}
								}
								else if (G.ar_sumiznist[j, i, 0] == 1 && !is_marked[j])
									if (f[j, i] > 0)
									{
										is_marked[j] = true;
										mark[j, 0] = i;
										mark[j, 1] = 0;
										mark[j, 2] = Math.Min(mark[i, 2], f[i, j]);
										last_vertex = j;
									}
							}
					++counter;
				} //    2 варианта:
				//	1) Ни одну вершину больше нельзя пометить, но сток не помечен. Это значит, что найденный поток - максимальный и алгоритм останавливается.
				//	2) Помечается сток. В этом случае производится изменение потока.

				if (!is_marked[choosen_vertex_right]) // сток не помечен
				{
					// написать завершение алгоритма
					Draw_Graph(sender, e);
					Gl.glColor3d(0.5, 0, 0.3);
					Gl.glPointSize(pSize);
					Gl.glBegin(Gl.GL_POINTS);
					Gl.glVertex2d(G.ar_graph_point_position[choosen_vertex_right, 0], G.ar_graph_point_position[choosen_vertex_right, 1]);
					Gl.glEnd();
					// пропечатать вершины и метки дуг с пройденым потоком
					Gl.glColor3d(0, 0, 0);
					for (int i = 0; i < n; ++i)
					{

						Gl.glRasterPos2f(G.ar_graph_point_position[i, 0] - 3*devX, G.ar_graph_point_position[i, 1] - 3*devY);
						string text = (i+1).ToString();
						// в цикле foreach перебираем значения из массива text, 
						// который содержит значение строки для визуализации 
						foreach (char char_for_draw in text)
						{
							// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
							Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
						}

					}
					Gl.glColor3d(1, 0, 0);
					float x0, y0;
					for (int i = 0; i < n; ++i)
					{
						for (int j = i; j < n; ++j)
							if (G.ar_sumiznist[i, j, 0] == 1)
							// вычислить координаты центра ребра
							{
								x0 = (G.ar_graph_point_position[i, 0] + G.ar_graph_point_position[j, 0]) / 2;
								y0 = (G.ar_graph_point_position[i, 1] + G.ar_graph_point_position[j, 1]) / 2;

								Gl.glRasterPos2f(x0 + 5 * devX, y0 + 5 * devY);
								string text = (G.ar_sumiznist[i, j, 1]).ToString() + " / " + f[i, j];
								// в цикле foreach перебираем значения из массива text, 
								// который содержит значение строки для визуализации 
								foreach (char char_for_draw in text)
								{
									// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
									Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
								}
							}
					}

					label2.Text = "Максимальний потік  = " + mas.ToString();
					return;
				}
				else // сток помечен
				{
					// изменение потока
					int d = mark[choosen_vertex_right, 2];
					mas += d;
					int i_k = choosen_vertex_right;

					while (i_k != choosen_vertex)
					{
						int j = i_k;
						i_k = mark[j, 0];
						if (is_marked[j] && mark[j, 1] == 1)
							f[i_k, j] += d;

						else if (is_marked[j] && mark[j, 1] == 0)
							f[i_k, j] -= d;

					}
					for (int i = 0; i < n; ++i)
						is_marked[i] = false;
					/* Если сток получил метку (k+,d), то потоки будут изменяться на величину d следующим образом:
	- если мы находимся в вершине j с меткой (i+,x), то прибавляем d к fij и переходим в вершину i.
	- если мы находимся в вершине j с меткой (i-,x), то вычитаем d из fij и переходим в вершину i.

	Изменение потока начинается от стока и продолжается до достижения истока. После этого все метки стираются и заново выполняется процедура помечивания вершин.

	Этот процесс продолжается до тех пор, пока не будет найден максимальный поток (ни одну вершину больше нельзя пометить, но сток не помечен).*/



				}

			}
			label2.Text = "Немає маршруту з " + (choosen_vertex + 1).ToString() + "  до " + (choosen_vertex_right + 1).ToString();


		}

		private void Орієнтований_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
				G.isDir = true;
			else
				G.isDir = false;
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox2.Checked)
				G.isWeighed = true;
			else
				G.isWeighed = false;
		}

		private void джонсонаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int n = G.get_n();
			int m = G.get_m();
			int v_choose = choosen_vertex;
			// добавить вершину и соеденить её со всемя остольными
			G.add_n();
		
			int N = G.get_n(), M = m + n;
			for (int i = 0; i < n; ++i)
			{
				G.ar_sumiznist[N-1, i, 0] = 1;
				G.ar_sumiznist[N-1, i, 1] = 0;
				G.add_m();
			}

			int[] h = new int[N];
			for (int i = 0; i < N; ++i)
				h[i] = 0;
			choosen_vertex = N - 1;
			if (!b_f(sender, e, ref h))
			{
				label2.Text = "Є від'ємний цикл";
				return;
			}
			// else:
			// новая матрица смежности 
			int[, ,] w =new int[N, N, 2];
			for(int i = 0; i < N; ++i)
				for (int j = 0; j < N; ++j)
				{
					w[i, j, 0] = G.ar_sumiznist[i, j, 0];
					w[i, j, 1] = 0;
				}
			int [] d = new int [n];
			for(int i = 0; i < n; ++i)
				d[i] = 0;

			for (int v = 0; v < N; ++v)
					{
						// h уже зполнен в Белмане-Форде
						for(int u = 0; u < N; ++u)
							if(G.ar_sumiznist[u, v, 0] == 1)// для каждой вершины, смежной с i							
							w[u, v, 1] = G.ar_sumiznist[u, v, 1] + h[u] - h[v];
					}

					// возвращаемся к изначальному графу
			for (int r = 0; r < n; ++r)
					{
						G.ar_sumiznist[N, r, 0] = 0;
						G.ar_sumiznist[N, r, 1] = 0;
						G.sub_m();
					}
						G.sub_n();
					// вернулись у изходному графу

			
			choosen_vertex = v_choose;
			int[] res = new int[n];
			deikstra_for_jonson(w, ref d);
					for(int v = 0; v < n; ++v)
						res[v] = d[v] + h[v] - h[choosen_vertex];
			label2.Text = "поле вводу ваги:";
					Draw_Graph(sender, e);
					// подписать кратчайший путь
					Gl.glColor3d(0, 1, 0);
					for (int i = 0; i < n; ++i)
					{

						Gl.glRasterPos2f(G.ar_graph_point_position[i, 0] - 3 * devX, G.ar_graph_point_position[i, 1] + 13 * devY);
						string text1 = "[ " + (res[i]).ToString() + " ]";

						// в цикле foreach перебираем значения из массива text, 
						// который содержит значение строки для визуализации 
						foreach (char char_for_draw in text1)
						{
							// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
							Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
						}
					}
					Gl.glFlush();
					AnT.Invalidate();


					


		}

		private void алгоритмЕдмондсаКарпаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int n = G.get_n();
			int m = G.get_m();
			int[,] f = new int[n, n]; // поток
			int[,] c = new int[n, n]; // остаточная сеть
			Graph C = new Graph();
			int sum = 0;
			//1) < добавим новый граф С для нахождения ниминального пути алгоритмом поиска в ширину
			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < n; ++j)
				{
					if (G.ar_sumiznist[i, j, 0] == 1)
					{
						c[i, j] = G.ar_sumiznist[i, j, 1];
						C.ar_sumiznist[i, j, 0] = 1;
						C.ar_sumiznist[i, j, 1] = c[i, j];
						C.add_m();
					}
					f[i, j] = 0;
				}
				C.add_n();
			}
			//> 1)
			while (true)
			{
				//2) < В остаточной сети находим кратчайший путь из источника в сток. Если такого пути нет, останавливаемся.
				bool[] visited = new bool[n];
				int[] root = new int[n];
				bool[] has_root = new bool[n];
				bool find_way = false;
				
				for (int i = 0; i < n; ++i)
				{
					visited[i] = false;
					has_root[i] = false;
				}
				Queue<int> O = new Queue<int>();
				O.Enqueue(choosen_vertex);
				visited[choosen_vertex] = true;
				while (O.Count() > 0 && !find_way)
				{
					int u = O.Dequeue();
					for (int v = 0; v < n && !find_way; ++v)
						if (C.ar_sumiznist[u, v, 0] == 1)
							if (!visited[v])
							{
								visited[v] = true;
								has_root[v] = true;
								root[v] = u;
								O.Enqueue(v);
								if (v == choosen_vertex_right)
								{
									find_way = true;
									break;
								}
							}
				}

				if (O.Count() == 0)
				{
					label2.Text = "Максимальний потік = " + sum.ToString();
					// написать завершение алгоритма
					#region draw_answer
					Draw_Graph(sender, e);
					Gl.glColor3d(0.5, 0, 0.3);
					Gl.glPointSize(pSize);
					Gl.glBegin(Gl.GL_POINTS);
					Gl.glVertex2d(G.ar_graph_point_position[choosen_vertex_right, 0], G.ar_graph_point_position[choosen_vertex_right, 1]);
					Gl.glEnd();
					// пропечатать вершины и метки дуг с пройденым потоком
					Gl.glColor3d(0, 0, 0);
					for (int i = 0; i < n; ++i)
					{

						Gl.glRasterPos2f(G.ar_graph_point_position[i, 0] - 3*devX, G.ar_graph_point_position[i, 1] - 3*devY);
						string text = (i+1).ToString();
						// в цикле foreach перебираем значения из массива text, 
						// который содержит значение строки для визуализации 
						foreach (char char_for_draw in text)
						{
							// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
							Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
						}

					}
					Gl.glColor3d(1, 0, 0);
					float x0, y0;
					for (int i = 0; i < n; ++i)
					{
						for (int j = i; j < n; ++j)
							if (G.ar_sumiznist[i, j, 0] == 1)
							// вычислить координаты центра ребра
							{
								x0 = (G.ar_graph_point_position[i, 0] + G.ar_graph_point_position[j, 0]) / 2;
								y0 = (G.ar_graph_point_position[i, 1] + G.ar_graph_point_position[j, 1]) / 2;

								Gl.glRasterPos2f(x0 + 5 * devX, y0 + 5 * devY);
								string text = (G.ar_sumiznist[i, j, 1]).ToString() + " / " + f[i, j];
								// в цикле foreach перебираем значения из массива text, 
								// который содержит значение строки для визуализации 
								foreach (char char_for_draw in text)
								{
									// визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
									Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
								}
							}
					}
					#endregion
					return; /// 
				}
				int[] way = new int[n];
				int n_in_way = 0;
				way[n_in_way++] = choosen_vertex_right;
				for (int i = choosen_vertex_right; root[i] != choosen_vertex; )
				{
					i = root[i];
					way[n_in_way++] = i;
				}
				way[n_in_way++] = choosen_vertex;
				int[] wayr = new int[n_in_way];
				for (int i = n_in_way; i > 0; --i)
					wayr[n_in_way - i] = way[i - 1];
					
				// > 2)

				/* 3) <
				 Пускаем через найденный путь  максимально возможный поток:
				 На найденном пути в остаточной сети ищем ребро с минимальной пропускной способностью c_\min.
				 Для каждого ребра на найденном пути увеличиваем поток на c_\min, а в противоположном ему — уменьшаем на c_\min.
				 Модифицируем остаточную сеть. Для всех рёбер на найденном пути, а также для противоположных им рёбер, вычисляем новую пропускную способность. Если она стала ненулевой, добавляем ребро к остаточной сети, а если обнулилась, стираем его.
				> 3) */
				
				int c_min = EPS;
				for (int i = 0; i < n; ++i)
					for (int j = 0; j < n; ++j)
						if (C.ar_sumiznist[i, j, 0] == 1)
							if (c_min > C.ar_sumiznist[i, j, 1])
								c_min = C.ar_sumiznist[i, j, 1];
								
				sum += c_min;
				for (int i = 0; i < n_in_way - 1; ++i)
				{
					f[wayr[i], wayr[i+1]] += c_min;
					f[wayr[i+1], wayr[i]] -= c_min;
					C.ar_sumiznist[wayr[i], wayr[i+1], 1] -= c_min;
					if (C.ar_sumiznist[wayr[i], wayr[i+1], 1] == 0)
					{
						C.ar_sumiznist[wayr[i], wayr[i+1], 0] = 0;
						C.sub_m();
					}
				}

			}
			
			 

		}


		// функция, управляющая визуализацией сцены 
		//private void Draw()
		//{
		//    // очистка буфера цвета и буфера глубины 
		//    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

		//    // очищение текущей матрицы 
		//    Gl.glLoadIdentity();


		//    //Gl.glColor3d(1, 0, 0);
		//    //Gl.glPointSize(pSize);
		//    //Gl.glBegin(Gl.GL_POINTS);
		//    //     for (int i = 0; i < G.get_n(); ++i)
		//    //         Gl.glVertex2f(G.ar_graph_point_position[i, 0], G.ar_graph_point_position[i, 1]);
		//    //Gl.glEnd();

		//    //Gl.glLineWidth(lWidth);
		//    //Gl.glBegin(Gl.GL_LINES);
		//    //    for (int j = 0; j < G.get_n(); ++j)
		//    //       {
		//    //           Gl.glVertex2f(G.ar_coord_reber[j, 0], G.ar_coord_reber[j, 1]);
		//    //           Gl.glVertex2f(G.ar_coord_reber[j, 2], G.ar_coord_reber[j, 3]);
		//    //       }
		//    //Gl.glEnd();



		//    //// выводим подписи осей "x" и "y" 
		//    //PrintText2D(15.5f, 0, "x");
		//    //PrintText2D(0.5f, 14.5f, "y");




		//    // дожидаемся завершения визуализации кадра 
		//    Gl.glFlush();

		//    // сигнал для обновление элемента реализующего визуализацию. 
		//    AnT.Invalidate();
		//}





	}
}
