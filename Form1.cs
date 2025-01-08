using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Tanzverein
{
    public partial class Form1 : Form
    {
        Dictionary<long, Taenzer> taenzerById = new Dictionary<long, Taenzer>();

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Verbindung zur lokalen SQL-Datenbank.
            string connectionString = "Server=localhost;Database=TanzvereinDB;User Id=your_username;Password=your_password;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Name, Age FROM Taenzer";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Taenzer t = new Taenzer(reader.GetInt64(0), reader.GetString(1), reader.GetInt32(2));
                        taenzerById[t.ID] = t;
                        listBox1.Items.Add($"{t.ID}: {t.Name} ({t.Age})");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (long.TryParse(textBox1.Text, out long taenzerID) && taenzerById.TryGetValue(taenzerID, out Taenzer t))
            {
                MessageBox.Show($"Name: {t.Name}, Alter: {t.Age}");
            }
            else
            {
                MessageBox.Show("Tänzer nicht gefunden.");
            }
        }
    }

    public class Taenzer
    {
        public long ID { get; }
        public string Name { get; }
        public int Age { get; }

        public Taenzer(long id, string name, int age)
        {
            ID = id;
            Name = name;
            Age = age;
        }
    }
}
