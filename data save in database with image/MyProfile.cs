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
using System.IO;

namespace data_save_in_database_with_image
{
    public partial class MyProfile : Form
    {
        public MyProfile()
        {
            InitializeComponent();
        }

        STUDENT student = new STUDENT();
        string reg_no;

        public MyProfile(string reg_number)
        {
            InitializeComponent();

            try
            {
                this.reg_no = reg_number.ToString();
                MySqlCommand command = new MySqlCommand("SELECT `reg_no`, `first_name`, `last_name`, `email`, `password`, `dept`, `batch_year`, `nic_no`, `mobile`, `gender`, `town`, `image` FROM `student` WHERE `reg_no`=" + reg_no);

                DataTable table = student.getStudent(command);

                if (table.Rows.Count > 0)
                {
                    this.label1.Text = table.Rows[0]["reg_no"].ToString();
                    this.label2.Text = table.Rows[0]["first_name"].ToString();
                    this.label3.Text = table.Rows[0]["last_name"].ToString();
                    this.label4.Text = table.Rows[0]["email"].ToString();
                    this.label5.Text = table.Rows[0]["dept"].ToString();
                    this.label6.Text = table.Rows[0]["batch_year"].ToString();
                    this.label7.Text = table.Rows[0]["nic_no"].ToString();
                    this.label8.Text = table.Rows[0]["mobile"].ToString();
                    this.label9.Text = table.Rows[0]["gender"].ToString();
                    this.label10.Text = table.Rows[0]["town"].ToString();

                    //for image
                    byte[] pic = (byte[])table.Rows[0]["image"];
                    MemoryStream picture = new MemoryStream(pic);
                    pictureBox1.Image = Image.FromStream(picture);
                }
            }
            catch
            {
                MessageBox.Show("Enter Valid Info", "Invalid Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
