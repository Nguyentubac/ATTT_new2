using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ATTT
{   
    public partial class FormPlayFair : Form
    {
        DataTable table = new DataTable();
        class PlayFair
        {

            
            public char[,] CreateTable(string key)
            {
                

                key = key.ToUpper().Replace("J", "I"); // thay J bằng I
                HashSet<char> usedChars = new HashSet<char>();
                List<char> chars = new List<char>();

                foreach (char c in key)
                {
                    if (char.IsLetter(c) && !usedChars.Contains(c))
                    {
                        usedChars.Add(c);
                        chars.Add(c);
                    }
                }

                for (char c = 'A'; c <= 'Z'; c++)
                {
                    if (c != 'J' && !usedChars.Contains(c))
                    {
                        usedChars.Add(c); // kí tự đang xét đã sử dụng chưa
                        chars.Add(c);
                    }
                }
                // tạo bảng playfair
                char[,] table = new char[5, 5];
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        table[i, j] = chars[i * 5 + j];
                        
                    }
                }
                return table;
            }
            static int[] FindCharacter(char[,] table, char target)
            {
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        if (table[i, j] == target)
                        {
                            return new int[] { i, j };
                        }
                    }
                }
                return null;
            }
            public string Decrypt(string message, char[,] table)
            {
                message = message.ToUpper().Replace("J", "I").Replace(" ", "");
                if (message.Length % 2 != 0)
                {
                    message += "X"; // Thêm ký tự nếu lẻ
                }
                string result = "";

                for (int i = 0; i < message.Length; i += 2)
                {
                    char first = message[i];
                    char second = message[i + 1];
                    int[] pos1 = FindCharacter(table, first);
                    int[] pos2 = FindCharacter(table, second);

                    if (pos1[0] == pos2[0]) // Cùng hàng
                    {
                        result += table[pos1[0], (pos1[1] +4 ) % 5];
                        result += table[pos2[0], (pos2[1] + 4) % 5];
                    }
                    else if (pos1[1] == pos2[1]) // Cùng cột  
                    {
                        result += table[(pos1[0] +4) % 5, pos1[1]];
                        result += table[(pos2[0] +4) % 5, pos2[1]];
                    }
                    else // Hình chữ nhật
                    {
                        result += table[pos1[0], pos2[1]];
                        result += table[pos2[0], pos1[1]];
                    }
                }
                return result;
            }

            
            public string Encrypt(string message, char[,] table)
            {
                
                message = message.ToUpper().Replace("J", "I").Replace(" ", "");
                if (message.Length % 2 != 0)
                {
                    message += "X"; // Thêm ký tự nếu lẻ
                }

                string result = "";

                for (int i = 0; i < message.Length; i += 2)
                {
                    char first = message[i];
                    char second = message[i + 1];
                    int[] pos1 = FindCharacter(table, first);
                    int[] pos2 = FindCharacter(table, second);

                    if (pos1[0] == pos2[0]) // Cùng hàng
                    {
                        result += table[pos1[0], (pos1[1] + 1) % 5];
                        result += table[pos2[0], (pos2[1] + 1) % 5];
                    }
                    else if (pos1[1] == pos2[1]) // Cùng cột
                    {
                        result += table[(pos1[0] + 1) % 5, pos1[1]];
                        result += table[(pos2[0] + 1) % 5, pos2[1]];
                    }
                    else // Hình chữ nhật
                    {
                        result += table[pos1[0], pos2[1]];
                        result += table[pos2[0], pos1[1]];
                    }
                }
                return result; 
            }
        }
        public FormPlayFair()
        {
            InitializeComponent();
            LoadData();
        }
        public DataTable ConvertArrayToDataTable(char[,] array2D)
        {
            DataTable dt = new DataTable();

            for (int col = 0; col < array2D.GetLength(1); col++)
            {
                dt.Columns.Add("Column " + (col + 1));
            }

            // Thêm các hàng vào DataTable
            for (int row = 0; row < array2D.GetLength(0); row++)
            {
                DataRow dr = dt.NewRow();
                for (int col = 0; col < array2D.GetLength(1); col++)
                {
                    dr[col] = array2D[row, col];
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public bool kiemtranumber(string num)
        {
            foreach (char c in num)
            {
                if (char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length==0 || Key.Text.Length==0)
            {
                MessageBox.Show("Các trường thông tin chưa nhập đầy đủ");
            }
            else
            {
                if(kiemtranumber(textBox1.Text.ToString()))
                {
                    if (comboBox1.SelectedIndex != -1)
                    {
                        PlayFair playfair = new PlayFair();
                        char[,] table = playfair.CreateTable(Key.Text);
                        string cb = comboBox1.SelectedItem.ToString();
                        if (cb.Equals("Mã Hoá"))
                        {
                            string encryptedMessage = playfair.Encrypt(textBox1.Text, table);
                            textkq.Text = encryptedMessage;
                        }
                        if (cb.Equals("Giải Mã"))
                        {
                            string descryptedMessage = playfair.Decrypt(textBox1.Text, table);
                            textkq.Text = descryptedMessage;
                        }

                        DataTable dt = ConvertArrayToDataTable(table);
                        dataGridView12.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Chưa chọn phương thức giải mã hoặc mã hoá");
                    }
                }
                else
                {
                    MessageBox.Show("Các trường thông tin chỉ nhận chữ cái");
                }
                
            }    
           
            
        }
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Key_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadData()
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}

