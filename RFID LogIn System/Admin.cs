using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO.Ports;

namespace RFID_LogIn_System
{
    public partial class Admin : Form
    {
        private string user;
        private string pass;
        private string cardid;

        public Admin()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            user = UsernameBox.Text;
            pass = PassBox.Text;
            cardid = CardIdBox.Text;
            
            string query = $"INSERT INTO users (username,  password, idcard) VALUES ('{user}','{pass}','{cardid}');";

            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, Form1.connect);
                cmd.ExecuteNonQuery();
                Form1.connect.Close();
                MessageBox.Show("User added");
            }
            else
            {
                Form1.connect.Close();
            }
        }
        private void AllUsersButton_Click(object sender, EventArgs e)
        {
            string query = $"SELECT * FROM users ;";
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, Form1.connect);
                cmd.ExecuteNonQuery();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(table);
                DatabaseTable.DataSource = table;

                Form1.connect.Close();
                //MessageBox.Show("User added");
            }
            else
            {
                Form1.connect.Close();
            }

        }
        private void DisplayButton_Click(object sender, EventArgs e)
        {
            user = UsernameBox.Text;
            string query = $"SELECT * FROM dates WHERE username = '{user}';";
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, Form1.connect);
                cmd.ExecuteNonQuery();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(table);
                DatabaseTable.DataSource = table;

                Form1.connect.Close();
                //MessageBox.Show("User added");
            }
            else
            {
                Form1.connect.Close();
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool OpenConnection()
        {
            try
            {
                Form1.connect.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("No connection to the server!");
                        break;
                }
                return false;
            }
        }
    }
}
