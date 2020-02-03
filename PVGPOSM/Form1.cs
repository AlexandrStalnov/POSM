using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace PVGPOSM
{
    public partial class Form1 : Form
    {
        public string row1;
        public string row2;
        public string foto;
        public string strDes;
        public string strDes1;
        public string connectString = @"Data Source=S6-084-FP; Initial Catalog=BSJobs;  User id =admin; Password=admin"; //строка подключения к DB

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.POSM2". При необходимости она может быть перемещена или удалена.
            this.pOSM2TableAdapter.Fill(this.bSJobsDataSet.POSM2);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.POSM1". При необходимости она может быть перемещена или удалена.
            this.pOSM1TableAdapter.Fill(this.bSJobsDataSet.POSM1);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.POSM". При необходимости она может быть перемещена или удалена.
            this.pOSMTableAdapter.Fill(this.bSJobsDataSet.POSM);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //полное выделение строки поле1
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //полное выделение строки поле2
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //полное выделение строки поле3
            textBox3.Text = "0";
            textBox4.Text = "100000";
            textBox5.Text = "0";
            textBox6.Text = "5000";
            textBox7.Text = "0";
            textBox8.Text = "5000";
            textBox9.Text = "0";
            textBox10.Text = "5000";
            textBox11.Text = "0";
            textBox12.Text = "1000";
            richTextBox1.Text = null;
            pictureBox1.Image = null;
        }

        private void button1_Click(object sender, EventArgs e) //кнопка сохранения
        {
            try
            {
                this.pOSMTableAdapter.Update(this.bSJobsDataSet);
                this.pOSM1TableAdapter.Update(this.bSJobsDataSet);
                this.pOSM2TableAdapter.Update(this.bSJobsDataSet);
            }
            catch
            {
                try
                {
                    this.pOSM1TableAdapter.Update(this.bSJobsDataSet);
                }
                catch
                {
                    this.pOSM2TableAdapter.Update(this.bSJobsDataSet);
                    MessageBox.Show("Строка атрибутов не сохранилась!!! \n(При внесени данных следует сначал сохранить строку объекта, а потом создать/сохранять атрибуты!) \nОбновите данные и заполните строку атрибутов. \n(Строка объекта сохранилась!)");
                }
                finally
                {
                    this.pOSM1TableAdapter.Update(this.bSJobsDataSet);
                }
            }
            finally
            {
                try
                {
                    this.pOSMTableAdapter.Update(this.bSJobsDataSet);
                    MessageBox.Show("Изменения сохранены!");
                }
                catch
                {

                    MessageBox.Show("Изменения НЕ сохранены!");
                }
                finally //сохранение описания объекта из textbox13 в DB
                {
                    //string connectString = @"Data Source=S6-084-FP; Initial Catalog=BSJobs;  User id =admin; Password=admin"; //строка подключения к DB
                    SqlConnection sqlConn = new SqlConnection(connectString);
                    strDes1 = richTextBox1.Text;
                    sqlConn.Open();
                    string sql = "UPDATE POSM SET Des = N'" + strDes1 + "' where Code = '" + row1 + "'";
                    SqlCommand cmd = new SqlCommand(sql, sqlConn);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) //кнопка обновления(перезгрузки) данных
        {
            Form1_Load(sender, e);
        }

        private void button3_Click(object sender, EventArgs e) //кнопка удаления объекта
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView2.SelectedRows) //первичный цикл foreach для удаления сначала строки атрибутов и сохранения изменений в таблице атрибутов(POSM1)
                {
                    dataGridView2.Rows.Remove(row);
                    this.pOSM1TableAdapter.Update(this.bSJobsDataSet);
                }
                foreach (DataGridViewRow row in dataGridView3.SelectedRows) //первичный цикл foreach для удаления сначала строки документов и сохранения изменений в таблице документов (POSM2)
                {
                    dataGridView3.Rows.Remove(row);
                    this.pOSM2TableAdapter.Update(this.bSJobsDataSet);
                }
                foreach (DataGridViewRow row in dataGridView1.SelectedRows) //цикл foreach для удаления строки объектов и сохранения изменений в таблице объектов(POSM)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            catch
            {
                MessageBox.Show("Не все атрибуты удалены");
            }
        }

        private void button4_Click(object sender, EventArgs e) //кнопка удаление атрибутов
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove(row);
                //this.pOSM1TableAdapter.Update(this.bSJobsDataSet); //сохранение изменений
            }
        }

        private void button5_Click(object sender, EventArgs e) //кнопка удаления документов
        {
            foreach (DataGridViewRow row in dataGridView3.SelectedRows)
            {
                dataGridView3.Rows.Remove(row);
                //this.pOSM2TableAdapter.Update(this.bSJobsDataSet); //сохранение изменений
            }
        }

        private void button6_Click(object sender, EventArgs e) //кнопка добавления ссылки
        {
            //folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //string connectString = @"Data Source=S6-084-FP; Initial Catalog=BSJobs; User id =admin; Password=admin"; //строка подключения к DB
                    SqlConnection sqlConn = new SqlConnection(connectString);
                    string link = folderBrowserDialog1.SelectedPath; //запись пути к папке в переменную
                    sqlConn.Open();
                    string sql = "insert into POSM2 (Id_Req, Link) VALUES ('" + row2 + "','" + link + "')";
                    SqlCommand cmd = new SqlCommand(sql, sqlConn);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                    MessageBox.Show("Ссылка добавлена.");
                }
                catch
                {
                    MessageBox.Show("Ошибка добавления ссылки!");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e) //кнопка добавления фото
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "(*.JPEG)|*.JPG";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //string connectString = @"Data Source=S6-084-FP; Initial Catalog=BSJobs; User id =admin; Password=admin"; //строка подключения к DB
                    SqlConnection sqlConn = new SqlConnection(connectString);
                    string base64 = Convert.ToBase64String(File.ReadAllBytes(fileDialog.FileName));
                    sqlConn.Open();
                    string sql = "UPDATE POSM SET Photo = '" + base64 + "' where Code = '" + row1 + "'";
                    SqlCommand cmd = new SqlCommand(sql, sqlConn);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                    MessageBox.Show("Фото добавлено.");
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки фото!");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e) //кнопка добавления объекта (новая форма для заполнения данных)
        {
            var frm3 = new Form3();
            frm3.Show();
        }

        private void button9_Click(object sender, EventArgs e) //кнопка применения фильтров
        {
            try
            {
                pOSMBindingSource.Filter = "[Code] Like'" + textBox1.Text + "%' and [Number] Like'" + textBox2.Text + "%' and [Customer] Like'" + comboBox1.Text + "%' and [Type] Like'" + comboBox2.Text + "%' and [TypePrint] Like'" + comboBox3.Text + "%' and [QuantityShelf] Like'" + comboBox4.Text + "%' and [ProfileYN] Like'" + comboBox5.Text + "%' and [AccessProd] Like'" + comboBox6.Text + "%' and [Quantity] >='" + textBox3.Text + "' and [Quantity] <='" + textBox4.Text + "' and [TotalHeight] >='" + textBox5.Text + "' and [TotalHeight] <='" + textBox6.Text + "' and [Width] >='" + textBox7.Text + "' and [Width] <='" + textBox8.Text + "' and [Depth] >='" + textBox9.Text + "' and [Depth] <='" + textBox10.Text + "' and [LoadShelf] >='" + textBox11.Text + "' and [LoadShelf] <='" + textBox12.Text + "'";
            }
            catch
            {
                MessageBox.Show("Поля фильтров с промежутками не заполнены!");
            }
            //pOSMBindingSource.Filter = "[Code] Like'" + textBox1.Text + "%' and [Number] Like'" + textBox2.Text + "%' and [Customer] Like'" + comboBox1.Text + "%' and [Type] Like'" + comboBox2.Text + "%' and [TypePrint] Like'" + comboBox3.Text + "%' and [QuantityShelf] Like'" + comboBox4.Text + "%' and [ProfileYN] Like'" + comboBox5.Text + "%' and [AccessProd] Like'" + comboBox6.Text + "%' and [Quantity] >='" + textBox3.Text + "' and [Quantity] <='" + textBox4.Text + "' and [TotalHeight] >='" + textBox5.Text + "' and [TotalHeight] <='" + textBox6.Text + "' and [Width] >='" + textBox7.Text + "' and [Width] <='" + textBox8.Text + "' and [Depth] >='" + textBox9.Text + "' and [Depth] <='" + textBox10.Text + "' and [LoadShelf] >='" + textBox11.Text + "' and [LoadShelf] <='" + textBox12.Text + "'";
            //pOSMBindingSource.Filter = "[LoadShelf] >= '" + textBox3.Text + "' and [LoadShelf] <='" + textBox4.Text + "'";
        }

        private void button10_Click(object sender, EventArgs e) //кнопка сброса фильтров
        {
            pOSMBindingSource.Filter = "[Code] Like'%'";
            //button2_Click(sender, e); //вызов кнопки обновления формы
            Form1_Load(sender, e); //вызов обновления формы
            textBox1.Text = null;
            textBox2.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;
            comboBox4.Text = null;
            comboBox5.Text = null;
            comboBox6.Text = null;
            //фильтрация для более точного вызова всех данных датасета
            //pOSMBindingSource.Filter = "[Code] Like'%' and [LoadShelf] >= '0' and [LoadShelf] <='100000' and [TotalHeight] >='0' and [TotalHeight] <='5000' and [Width] >='0' and [Width] <='5000' and [Depth] >='0' and [Depth] <='5000' and [LoadShelf] >='0' and [LoadShelf] <='1000'";
            //pOSMBindingSource.Filter = "[Code] Like'%' and [Number] Like'%' and [Customer] Like'%' and [Type] Like'%' and [TypePrint] Like'%' and [QuantityShelf] Like'%' and [ProfileYN] Like'%' and [AccessProd] Like'%' and [LoadShelf] >= '0' and [LoadShelf] <='100000' and [TotalHeight] >='0' and [TotalHeight] <='5000' and [Width] >='0' and [Width] <='5000' and [Depth] >='0' and [Depth] <='5000' and [LoadShelf] >='0' and [LoadShelf] <='1000'";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //событие клика по строке для отображения картинки
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                row1 = row.Cells[codeDataGridViewTextBoxColumn.Name].Value.ToString();
                row2 = row.Cells[idDataGridViewTextBoxColumn.Name].Value.ToString();
                try
                {
                    //DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    foto = row.Cells[photoDataGridViewTextBoxColumn.Name].Value.ToString();
                    //row1 = row.Cells[codeDataGridViewTextBoxColumn.Name].Value.ToString();
                    //row2 = row.Cells[idDataGridViewTextBoxColumn.Name].Value.ToString();
                    //strDes = row.Cells[desDataGridViewTextBoxColumn.Name].Value.ToString();
                    var image = Image.FromStream(new MemoryStream(Convert.FromBase64String(foto)));
                    pictureBox1.Image = image;
                    richTextBox1.Text = strDes;
                }
                catch
                {
                    pictureBox1.Image = null; //очистка pictureBox1 с картинкой объекта
                }
                try
                {
                    //DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    //row1 = row.Cells[codeDataGridViewTextBoxColumn.Name].Value.ToString();
                    //row2 = row.Cells[idDataGridViewTextBoxColumn.Name].Value.ToString();
                    strDes = row.Cells[desDataGridViewTextBoxColumn.Name].Value.ToString();
                    richTextBox1.Text = strDes;
                }
                catch
                {
                    richTextBox1.Text = null; //очистка textbox13 с описанием объекта
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //событие двойного клика по строке и последующего вызова масштабной картинки
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    foto = row.Cells[photoDataGridViewTextBoxColumn.Name].Value.ToString();
                    var image = Image.FromStream(new MemoryStream(Convert.FromBase64String(foto)));
                    var frm2 = new Form2(image);
                    frm2.ShowDialog();
                }
                catch
                {
                    MessageBox.Show("Картинка отсутсвует");
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) //Событие ошибки ввода данных
        {
            MessageBox.Show("Не все обязательные поля заполнены! Либо есть несоответсвие введённых данных типу поля!");
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Process.Start(dataGridView3.SelectedCells[2].Value.ToString());//события клика внутри ячейки (активация гиперссылок)
            }
            catch
            {
                MessageBox.Show("Указанный путь к файлу неактивен или отсутствует подклюючение у сетевому диску!");
            }
        } 
    }
}
