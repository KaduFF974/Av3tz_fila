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

        // --- BOTÃO VOLTAR ---
        private void button4_Click(object sender, EventArgs e)
        {
            Form3 tela3 = new Form3();
            tela3.Show();
            this.Close();
        }

        // --- BOTÃO LISTAR SUPERIOR ---
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

        // --- MÉTODO AUXILIAR LISTAR ---
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

        // --- BOTÃO CADASTRAR FORNECEDOR ---
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox3.Text) ||
                string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox5.Text) ||
                string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos antes de cadastrar!");
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                var strsql = "INSERT INTO fornecedores (Nome, CNPJ, Telefone, Email, Endereco) VALUES (@nome, @cnpj, @telefone, @email, @endereco)";
                var comando = new MySqlCommand(strsql, conexao);

                comando.Parameters.AddWithValue("@nome", textBox4.Text);
                comando.Parameters.AddWithValue("@cnpj", textBox3.Text);
                comando.Parameters.AddWithValue("@telefone", textBox2.Text);
                comando.Parameters.AddWithValue("@email", textBox5.Text);
                comando.Parameters.AddWithValue("@endereco", textBox6.Text);

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();

                MessageBox.Show("Fornecedor cadastrado com sucesso!");

                LimparCampos();
                listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar fornecedor: " + ex.Message);
            }
        }

        // --- BOTÃO EXCLUIR FORNECEDOR ---
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Por favor, selecione um fornecedor na tabela antes de excluir.");
                return;
            }

            DialogResult confirmacao = MessageBox.Show("Tem certeza que deseja excluir o fornecedor: " + textBox4.Text + "?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                    string sql = "DELETE FROM fornecedores WHERE Nome = @nome";
                    var comando = new MySqlCommand(sql, conexao);
                    comando.Parameters.AddWithValue("@nome", textBox4.Text);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                    conexao.Close();

                    MessageBox.Show("Fornecedor excluído com sucesso!");

                    LimparCampos();
                    listar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao excluir fornecedor: " + ex.Message);
                }
            }
        }

        // --- BOTÃO BUSCAR FORNECEDOR ---
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                // CORREÇÃO: Alterado de 'contato' para 'fornecedores'
                string sql = "SELECT * FROM fornecedores WHERE Nome LIKE @nome";

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

        // --- EVENTOS DE CLIQUE NO DATAGRIDVIEW ---
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LimparCampos()
        {
            textBox4.Clear();
            textBox3.Clear();
            textBox2.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Selecione um fornecedor clicando na tabela antes de atualizar.");
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                string sql = "UPDATE fornecedores SET Nome=@nome, CNPJ=@cnpj, Telefone=@telefone, Email=@email, Endereco=@endereco WHERE Nome=@nome";

                var comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@nome", textBox4.Text);
                comando.Parameters.AddWithValue("@cnpj", textBox3.Text);
                comando.Parameters.AddWithValue("@telefone", textBox2.Text);
                comando.Parameters.AddWithValue("@email", textBox5.Text);
                comando.Parameters.AddWithValue("@endereco", textBox6.Text);

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();

                MessageBox.Show("Fornecedor atualizado com sucesso!");

                LimparCampos();
                listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar fornecedor: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["CNPJ"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Telefone"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells["Endereco"].Value.ToString();
            }
        }
    }
}