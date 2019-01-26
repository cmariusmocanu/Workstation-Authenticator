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
using System.Threading;

namespace RFID_LogIn_System
{
    public partial class Form1 : Form
    {
        User userstate = new User();                   //user state
        Admin adminstate = new Admin();                //admin state
        public static string tempid;                   //save the curent user that it loged in
        public static DateTime logintime;              //get the curent time and date wen user login
        public static DateTime logouttime;             //get the curent time and date wen user logout

        private SerialPort port ;
        //private Thread sThread;

        //define variables for the server conection
        public static MySqlConnection connect;
        private string server;
        private string database;
        private string userid;
        private string password;

        public Form1()
        {
            //create the connection to the server
            server = "localhost";
            database = "rfidlogin";
            userid = "root";
            password = "123456";
            string connectStr = $"SERVER={server};DATABASE={database};UID={userid};PASSWORD={password};";
            connect = new MySqlConnection(connectStr);

            //sThread = new Thread(Read);

            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            port = new SerialPort("COM3", 19200, Parity.None, 8, StopBits.One);
            OpenSerialPort();
        }
        public void OpenSerialPort()
        {
            //port = serialPort1;
            port.Open();
            port.DataReceived += new SerialDataReceivedEventHandler(SerialInput);
        }

        private void SerialInput(object sender, SerialDataReceivedEventArgs e)
        {
            //port.Open();
            SerialPort sp = (SerialPort)sender;

            string input = sp.ReadLine();
            input = input.ToString();
            input = input.Substring(0, 12);
            this.Invoke((MethodInvoker)delegate 
            {
                label4.Text = input.ToString();
                while (Login(userTextBox.Text, passTextBox.Text, true, input) != true)
                {
                    input = sp.ReadLine();
                    input = input.ToString();
                    input = input.Substring(0, 12);
                    label4.Text = input.ToString();
                }
            });
            //if (port.IsOpen == false) { port.Close(); }
        }

        private void button1_Click(object sender, EventArgs e)      //login button
        {
            Login( userTextBox.Text, passTextBox.Text, false, "");
        }
        private bool Login(string user, string pass, bool cardlogin, string carid)                //login function
        {
            string query;
            if (cardlogin) { query = $"SELECT * FROM users WHERE idcard = '{carid}';"; }
            else { query = $"SELECT * FROM users WHERE username = '{user}' AND password = '{pass}';"; }
            
            if(OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connect);
                MySqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    logintime = DateTime.Now;
                    MessageBox.Show("Debug:  " + reader[0].ToString() + " -- " + reader[1].ToString() + " -- " + reader[2].ToString());
                    tempid = reader[1].ToString();
                    reader.Close();
                    connect.Close();
                    ChageForm();
                    return true;
                }
                else {
                    MessageBox.Show("Invalid username or password");
                    connect.Close();
                    return false;
                }
            }
            else
            {
                connect.Close();
                return false;
            }
        }

        private void ChageForm()
        {
            if (tempid == "admin")
            {
                if (port.IsOpen == false) { port.Close(); }
                this.Hide();
                adminstate.ShowDialog();
                this.Show();
            }
            else
            {
                if (port.IsOpen == false) { port.Close(); }
                this.Hide();
                userstate.ShowDialog();
                SaveDate();
                this.Show();
                //if (port.IsOpen == false) { OpenSerialPort(); }
            }
        }
        public void SaveDate()
        {
            logouttime = DateTime.Now;
            string query2 = $"INSERT INTO dates (username,  logindate, logoutdate) VALUES ('{tempid}','{logintime}','{logouttime}');";

            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query2, connect);
                cmd.ExecuteNonQuery();
                connect.Close();
            }
            else
            {
                connect.Close();
            }
        }
        private bool  OpenConnection()
        {
            try
            {
                connect.Open();
                return true;
            }catch (MySqlException ex)
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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
