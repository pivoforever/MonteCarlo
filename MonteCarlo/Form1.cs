using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonteCarlo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Считываем с формы требуемые значения
            double Xmin = double.Parse(textBoxXmin.Text);
            double Xmax = double.Parse(textBoxXmax.Text);
            int N = int.Parse(textBoxStep.Text);
            // Количество точек графика
            int count = 1000;
            // Массив значений X
            double[] x = new double[count];
            // Массив значений Y
            double[] y = new double[count];
            // Шаг построения функции
            double Step = (Xmax - Xmin) / count;
            // Массивы значений randX и randY
            double[] randX = new double[N];
            double[] randY = new double[N];
            // Расчитываем точки для графика функции
            double max1 = Math.Sin(Xmin);
            double min1 = max1;
            double max2 = Math.Pow(Xmin, 2);
            double min2 = max1;
            double max3 = Math.Sqrt(Xmin);
            double min3 = max1;
            for (int i = 0; i < count; i++)
            {
                // Вычисляем значение X
                x[i] = Xmin + Step * i;
                // Вычисляем значение функций в точке X
                if (radioButton1.Checked == true)
                {
                    y[i] = Math.Sin(x[i]);
                    if (max1 < y[i]) max1 = y[i];
                    if (min1 > y[i]) min1 = y[i];
                }
                else if (radioButton2.Checked == true)
                {
                    y[i] = Math.Pow(x[i], 2);
                    if (max2 < y[i]) max2 = y[i];
                    if (min2 > y[i]) min2 = y[i]; ;
                }
                else if (radioButton3.Checked == true)
                {
                    y[i] = Math.Sqrt(x[i]);
                    if (max3 < y[i]) max3 = y[i];
                    if (min3 > y[i]) min3 = y[i]; ;
                }
            }
            Random rnd = new Random();
            double max=0, min=0;
            if (radioButton1.Checked == true) { max = max1; min = min1; }
            else if (radioButton2.Checked == true) { max = max2; min = min2; }
            else if (radioButton3.Checked == true) { max = max3; min = min3; }
            int cnt = 0;
            //Генерируем N случайных точек в прямоугольнике Xmin:Xmax/min:max
            for (int i = 0; i < N; i++)
            {
                randX[i] = rnd.NextDouble() * (Xmax - Xmin) + Xmin;
                randY[i] = rnd.NextDouble() * (max - min) + min;
                if ((radioButton1.Checked == true) && (randY[i] <= Math.Sin(randX[i]))) cnt++;
                else if ((radioButton2.Checked == true) && (randY[i] <= Math.Pow(randX[i],2))) cnt++;
                else if ((radioButton3.Checked == true) && (randY[i] <= Math.Sqrt(randX[i]))) cnt++;
            }

            // Настраиваем оси графика
            chart1.ChartAreas[0].AxisX.Minimum = Xmin;
            chart1.ChartAreas[0].AxisX.Maximum = Xmax;
            // Определяем шаг сетки
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = Step;
            // Добавляем вычисленные значения в графики
            chart1.Series[0].Points.DataBindXY(x, y);
            // Добавляем случайные точки
            chart1.Series[1].Points.DataBindXY(randX, randY);
            // Вычисляем значение интеграла
            double itog;
            itog = (Xmax - Xmin) * (max - min) * cnt / N;
            textBox1.Text = itog.ToString();
        }
    }
}