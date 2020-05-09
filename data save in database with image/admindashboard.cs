using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace data_save_in_database_with_image
{
    public partial class admindashboard : Form
    {
        public admindashboard()
        {
            InitializeComponent();
        }

        public admindashboard(string fname, string lname, string no)
        {
            InitializeComponent();
            this.label11.Text = fname;
            this.label12.Text = lname;
            this.reg_no = no;
        }

        STUDENT student = new STUDENT();

        string reg_no;
        string value;
        string imgLocation;

        //to clear fields
        public void clearDataFields()
        {
            findtextBox.Text = null;
            textBoxFn.Text = null;
            textBoxLn.Text = null;
            textBoxMail.Text = null;
            textBoxDept.Text = null;
            textBoxBatch.Text = null;
            textBoxNIC.Text = null;
            textBoxMobi.Text = null;
            textBoxGen.Text = null;
            textBoxTwn.Text = null;
            pictureBox1.Image = null;
        }


        //image select button code
        private void button1_Click(object sender, EventArgs e)
        {
            //image select code
            OpenFileDialog imgOpen = new OpenFileDialog();
            imgOpen.Filter = "Image Files (*.jpg)|*.jpg | All Files (*.*)|*.*";


            if (imgOpen.ShowDialog() == DialogResult.OK)
            {
                imgLocation = imgOpen.FileName.ToString();
                pictureBox1.ImageLocation = imgLocation;


            }
        }



        //verify data fields empty or not
        bool verify()
        {
            if ((textBoxLn.Text.Trim() == "") || 
                (textBoxMail.Text.Trim() == "") ||
                (textBoxFn.Text.Trim() == "") ||
                (textBoxNIC.Text.Trim() == "") ||
                (textBoxDept.Text.Trim() == "") ||
                (textBoxTwn.Text.Trim() == "") ||
                (textBoxMobi.Text.Trim() == "") ||
                (textBoxGen.Text.Trim() == "") ||
                (textBoxBatch.Text.Trim() == "") ||
                (pictureBox1.Image == null))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //when dashboard is loading code
        private void admindashboard_Load(object sender, EventArgs e)
        {
            

            MySqlCommand command = new MySqlCommand("SELECT * FROM `student`");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudent(command);
            picCol=(DataGridViewImageColumn)dataGridView1.Columns[12];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;
        }


        //when dataDridView clicked code
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //set the data to textBoxes
            findtextBox.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBoxFn.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBoxLn.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBoxMail.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBoxNIC.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            textBoxBatch.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            textBoxDept.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBoxTwn.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString();
            textBoxMobi.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            textBoxGen.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
            

            //for image set
            byte[] pic;
            pic = (byte[])dataGridView1.CurrentRow.Cells[12].Value;
            MemoryStream picture = new MemoryStream(pic);
            pictureBox1.Image = Image.FromStream(picture);
            Show();
            
        }


        //update button code
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string reg_no = findtextBox.Text;
                string first_name = textBoxFn.Text;
                string last_name = textBoxLn.Text;
                string email = textBoxMail.Text;
                string dept = textBoxDept.Text;
                string batch_year = textBoxBatch.Text;
                string nic_no = textBoxNIC.Text;
                string mobile = textBoxMobi.Text;
                string gender = textBoxGen.Text;
                string town = textBoxTwn.Text;
                MemoryStream pic = new MemoryStream();

                if (verify())
                {
                    pictureBox1.Image.Save(pic, pictureBox1.Image.RawFormat);

                    if (student.updateStudent(reg_no, first_name, last_name, email, dept, batch_year, nic_no, mobile, gender, town, pic))
                    {
                        MessageBox.Show("Student Updated", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //clear fields
                        clearDataFields();
                    }
                    else
                    {
                        MessageBox.Show("Error", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Empty Field", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch
            {
                MessageBox.Show("Enter_valid_Info", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //delete selected data button code
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string reg_no = findtextBox.Text;

                //messageBox before delete data findtextBox.Text != null
                if (MessageBox.Show("Are You Sure You Want to Delete This Student", "Delete Student", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (student.deleteStudent(reg_no))
                    {
                        MessageBox.Show("Student Deleted", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //clear fields
                        clearDataFields();

                    }
                    else
                    {
                        MessageBox.Show("Enter Valid Deleted", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            catch
            {
                MessageBox.Show("Enter_valid_Info", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }


        //find-search button code
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                string reg_no = findtextBox.Text;
                MySqlCommand command = new MySqlCommand("SELECT `reg_no`, `first_name`, `last_name`, `email`, `password`, `dept`, `batch_year`, `nic_no`, `mobile`, `gender`, `town`, `image` FROM `student` WHERE `reg_no`=" + reg_no);

                DataTable table = student.getStudent(command);

                if (table.Rows.Count > 0)
                {
                    findtextBox.Text = table.Rows[0]["reg_no"].ToString();
                    textBoxFn.Text = table.Rows[0]["first_name"].ToString();
                    textBoxLn.Text = table.Rows[0]["last_name"].ToString();
                    textBoxMail.Text = table.Rows[0]["email"].ToString();
                    textBoxDept.Text = table.Rows[0]["dept"].ToString();
                    textBoxBatch.Text = table.Rows[0]["batch_year"].ToString();
                    textBoxNIC.Text = table.Rows[0]["nic_no"].ToString();
                    textBoxMobi.Text = table.Rows[0]["mobile"].ToString();
                    textBoxGen.Text = table.Rows[0]["gender"].ToString();
                    textBoxTwn.Text = table.Rows[0]["town"].ToString();

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


        //refresh button code 
        private void button9_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM `student`");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudent(command);
            picCol = (DataGridViewImageColumn)dataGridView1.Columns[12];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;
        }


        //validate only numbers when keyPress
        private void textBoxNIC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        //validate only numbers when keyPress
        private void textBoxMobi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clearDataFields();
        }


        //add new button code
        private void button8_Click(object sender, EventArgs e)
        {
            
            MyProfile profile1 = new MyProfile(reg_no);
            profile1.Show();

        }

        private void textBoxMail_Leave(object sender, EventArgs e)
        {
            Regex reg = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            if (reg.IsMatch(textBoxMail.Text.Trim()))
            {
                MessageBox.Show("Email is valid");
            }
            else
            {
                MessageBox.Show("Invalid Email");
                textBoxMail.Text = "";
            }
        }


        
        
    }
}
