using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace data_save_in_database_with_image
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        string reg;
        string session1;
        string session2;

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //login button code
        private void button1_Click(object sender, EventArgs e)
        {
            My_DB db = new My_DB();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `student` WHERE `reg_no`=@reg_no AND `password`=@password", db.getConnection);
            
            if (!(textBox1.Text.Trim() == "") && !(textBox2.Text.Trim() == ""))
            {
                command.Parameters.Add("@reg_no", MySqlDbType.VarChar).Value = textBox1.Text;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = textBox2.Text;

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if(table.Rows.Count > 0)
                {
                    if (table.Rows[0][10].ToString() == "0")
                    {
                        session1 = table.Rows[0][1].ToString();
                        session2 = table.Rows[0][2].ToString();
                        reg = table.Rows[0][0].ToString();

                        this.Close();
                        admindashboard admin = new admindashboard(session1, session2, reg);
                        admin.Show();
                    }
                    if (table.Rows[0][10].ToString() == "1")
                    {
                        reg = table.Rows[0][0].ToString();

                        this.Close();
                        userdashboard user = new userdashboard(reg);
                        user.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid User..! Please Check Your Register No & Password");
                }
                
            }
            else
            {
                if (textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Register Number");
                }
                else
                {
                    MessageBox.Show("Please Enter Password");
                }
                
            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            //set loging panel image
            pictureBox1.Image = Image.FromFile("../../image/profile1.jpg");
        }

        //signup button code
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            register regForm = new register();
            regForm.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
