﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFID_LogIn_System
{
    public partial class User : Form
    {
        public User()
        {
            InitializeComponent();
        }

        private void User_Load(object sender, EventArgs e)
        {
            string temp = Form1.tempid;
            label1.Text = "Welcome " + temp.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.logouttime = DateTime.Now;
            this.Close();
        }
    }
}
