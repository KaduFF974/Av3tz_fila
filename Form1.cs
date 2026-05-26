using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Av3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 tela3 = new Form3();
            tela3.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                var strsql = "SELECT * FROM fornecedores";
                var comando = new MySqlCommand(strsql, conexao);
                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);
                dataGridView1.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro ao listar1");
            }
            listar();
        }


        private void listar()
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                var strsql = "SELECT * FROM fornecedores";
                var comando = new MySqlCommand(strsql, conexao);
                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);
                dataGridView1.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro ao listar1");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sql = "SELECT * FROM contato WHERE nome LIKE @nome";

                var comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@nome", "%" + textBox1.Text + "%");

                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);

                dataGridView1.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro ao buscar!");
            }
        }
    }
}
