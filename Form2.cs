using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Av3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
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
                var strsql = "SELECT * FROM produtos";
                var comando = new MySqlCommand(strsql, conexao);
                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);
                dataGridView1.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro ao listar3");
            }
            listar();
        }

        private void listar()
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                var strsql = "SELECT * FROM produtos";
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

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sql = "SELECT * FROM produtos WHERE Nome LIKE @nome";

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

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Por favor, selecione um produto na tabela antes de excluir.");
                return;
            }
            DialogResult confirmacao = MessageBox.Show("Tem certeza que deseja excluir o produto: " + textBox4.Text + "?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                    string sql = "DELETE FROM produtos WHERE Nome = @nome";
                    var comando = new MySqlCommand(sql, conexao);
                    comando.Parameters.AddWithValue("@nome", textBox4.Text);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                    conexao.Close();

                    MessageBox.Show("Produto excluído com sucesso!");
                    textBox6.Clear();
                    textBox4.Clear();
                    textBox3.Clear();
                    textBox2.Clear();
                    textBox5.Clear();

                    listar();
                }
                catch
                {
                    MessageBox.Show("Erro ao excluir: ");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sql = "UPDATE Produtos SET Nome=@nome, Descricao=@desc, Preco=@preco, QuantidadeEstoque=@QuantEs WHERE Nome=@nome";

                var comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@nome", textBox4.Text);
                comando.Parameters.AddWithValue("@desc", textBox3.Text);
                comando.Parameters.AddWithValue("@preco", textBox2.Text);
                comando.Parameters.AddWithValue("@QuantEs", textBox5.Text);

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();

                MessageBox.Show("Atualizado com sucesso!");

                listar();
            }
            catch
            {
                MessageBox.Show("Erro ao atualizar: ");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox3.Text) ||
                string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos antes de cadastrar!");
                return;
            }
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                var strsql = "INSERT INTO produtos (Nome, Descricao, Preco, QuantidadeEstoque) VALUES (@nome, @desc, @preco, @quantEs)";
                var comando = new MySqlCommand(strsql, conexao);
                comando.Parameters.AddWithValue("@nome", textBox4.Text);
                comando.Parameters.AddWithValue("@desc", textBox3.Text);
                comando.Parameters.AddWithValue("@preco", Convert.ToDecimal(textBox2.Text));
                comando.Parameters.AddWithValue("@quantEs", Convert.ToInt32(textBox5.Text));

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();

                MessageBox.Show("Produto cadastrado com sucesso!");
                textBox4.Clear();
                textBox3.Clear();
                textBox2.Clear();
                textBox5.Clear();
                textBox6.Clear();

                listar();
            }
            catch
            {
                MessageBox.Show("Erro ao cadastrar: ");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells["IdProduto"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["Descricao"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Preco"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["QuantidadeEstoque"].Value.ToString();
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                {
                    textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells["IdProduto"].Value.ToString();
                    textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
                    textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["Descricao"].Value.ToString();
                    textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Preco"].Value.ToString();
                    textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["QuantidadeEstoque"].Value.ToString();
                }
        }
    }
}
