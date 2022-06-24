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
            double[] randXred = new double[N];
            double[] randYred = new double[N];

            // Расчитываем точки для графика функции
            double max1 = Math.Sin(Xmin);
            double min1 = max1;
            double max2 = Math.Pow(Xmin, 2);
            double min2 = max2;
            double max3=0, min3=0;
            if ((radioButton3.Checked == true) && (Xmin < 0))
            {
                MessageBox.Show("Введите положительное значение левой границы!", "Ошибка ввода", MessageBoxButtons.OK);
                return;
            }
            else
            {
                max3 = Math.Sqrt(Xmin);
                min3 = max3;
            }
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
            double max=0, min=0, temp1, temp2;
            if (radioButton1.Checked == true) { max = max1; min = min1; }
            else if (radioButton2.Checked == true) { max = max2; min = min2; }
            else if (radioButton3.Checked == true) { max = max3; min = min3; }
            if ((max >= 0) && (min >= 0)) min=0;
            if ((max <= 0) && (min <= 0)) max = 0;
            int cnt = 0, cntNeg = 0, Np=0, Nm=0;
            //Генерируем N случайных точек в прямоугольнике Xmin:Xmax/min:max
            for (int i = 0; i < N; i++)
            {
                temp1 = rnd.NextDouble() * (Xmax - Xmin) + Xmin;
                temp2 = rnd.NextDouble() * (max - min) + min;
                if (temp2 >= 0) Np++;
                else Nm++;
                if (radioButton1.Checked == true)
                {
                    if (Math.Sin(temp1) >= 0)
                    {
                        if ((temp2 <= Math.Sin(temp1))&&(temp2>=0))
                        { cnt++; randXred[i] = temp1; randYred[i] = temp2; }
                        else { randX[i] = temp1; randY[i] = temp2; }
                    }
                    else
                    {
                        if ((temp2 >= Math.Sin(temp1)) && (temp2 <= 0))
                        { cntNeg++; randXred[i] = temp1; randYred[i] = temp2; }
                        else { randX[i] = temp1; randY[i] = temp2; }
                    }

                }
                else if (radioButton2.Checked == true)
                {
                    if (temp2 <= Math.Pow(temp1,2))
                    { cnt++; randXred[i] = temp1; randYred[i] = temp2; }
                    else { randX[i] = temp1; randY[i] = temp2; }
                }
                else if (radioButton3.Checked == true)
                {
                    if (temp2 <= Math.Sqrt(temp1))
                    { cnt++; randXred[i] = temp1; randYred[i] = temp2; }
                    else { randX[i] = temp1; randY[i] = temp2; }
                }
            }

            // Настраиваем оси графика
            chart1.ChartAreas[0].AxisX.Minimum = Xmin;
            chart1.ChartAreas[0].AxisX.Maximum = Xmax;
            // Определяем шаг сетки
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = Step;
            // Добавляем вычисленные значения в графики
            chart1.Series[0].Points.DataBindXY(x, y);
            // Добавляем случайные точки выше графика
            chart1.Series[1].Points.DataBindXY(randX, randY);
            // Добавляем случайные точки ниже графика
            chart1.Series[2].Points.DataBindXY(randXred, randYred);
            // Вычисляем значение интеграла
            double itog;
            if ((Np != 0) && (Nm != 0))
                itog = (Xmax - Xmin) * max * cnt / Np + (Xmax - Xmin) * min * cntNeg / Nm;
            else
                itog = (Xmax - Xmin) * (max+min) * (cnt+ cntNeg) / N;

            textBox1.Text = itog.ToString();
        }
    }
}