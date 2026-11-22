using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Kuzmenko_KPR4
{
    public partial class Form1 : Form
    {
        private DataTable table;
        private DataSet dataSet;

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;
            btnCalculate.Click += btnCalculate_Click;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            table = new DataTable("Results");

            table.Columns.Add("i", typeof(int));
            table.Columns.Add("x", typeof(double));
            table.Columns.Add("y", typeof(double));
            table.Columns.Add("F(x,y)", typeof(double));

            dataSet = new DataSet();
            dataSet.Tables.Add(table);

            dataGridView1.DataSource = table;

            comboBox1.Items.Add("Point");
            comboBox1.Items.Add("Line");
            comboBox1.Items.Add("Spline");
            comboBox1.Items.Add("Pie");
            comboBox1.Items.Add("Radar");
            comboBox1.Items.Add("Bar");
            comboBox1.Items.Add("Column");

            comboBox1.SelectedIndex = 1;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            table.Rows.Clear();

            int n = 14;

            double x = 2.8;
            double y = 1.7;

            double dx = 2.1;
            double dy = 0.3;

            double sum = 0;

            for (int i = 1; i <= n; i++)
            {
                double z =
                    Math.Sin(x * x)
                    - Math.Pow(Math.Cos(y * y), 3)
                    + i / Math.Cos(x);

                table.Rows.Add(
                  i,
                  Math.Round(x, 2),
                  Math.Round(y, 2),
                  Math.Round(z, 3));

                sum += z;

                x += dx;
                y += dy;
            }

            BuildChart();

            MessageBox.Show(
                $"Сума значень = {sum:F3}",
                "Результат");
        }

        private void BuildChart()
        {
            chart1.Series.Clear();

            Series series = new Series("F(x,y)");

            series.ChartType = SeriesChartType.Line;

            foreach (DataRow row in table.Rows)
            {
                series.Points.AddXY(
                    row["i"],
                    row["F(x,y)"]);
            }

            chart1.Series.Add(series);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chart1.Series.Count == 0)
                return;

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    chart1.Series[0].ChartType = SeriesChartType.Point;
                    break;

                case 1:
                    chart1.Series[0].ChartType = SeriesChartType.Line;
                    break;

                case 2:
                    chart1.Series[0].ChartType = SeriesChartType.Spline;
                    break;

                case 3:
                    chart1.Series[0].ChartType = SeriesChartType.Pie;
                    break;

                case 4:
                    chart1.Series[0].ChartType = SeriesChartType.Radar;
                    break;

                case 5:
                    chart1.Series[0].ChartType = SeriesChartType.Bar;
                    break;

                default:
                    chart1.Series[0].ChartType = SeriesChartType.Column;
                    break;
            }
        }

        private void створитиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            table.Rows.Clear();

            chart1.Series.Clear();
        }

        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "XML files (*.xml)|*.xml";

            if (open.ShowDialog() == DialogResult.OK)
            {
                dataSet.Clear();
                dataSet.ReadXml(open.FileName);

                table = dataSet.Tables[0];

                dataGridView1.DataSource = table;

                BuildChart();
            }
        }

        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();

            save.Filter = "XML files (*.xml)|*.xml";

            if (save.ShowDialog() == DialogResult.OK)
            {
                dataSet.WriteXml(save.FileName);

                MessageBox.Show(
                    "Файл успішно збережено",
                    "Інформація");
            }
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}