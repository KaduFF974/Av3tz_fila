using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Av3
{
    public partial class Form1 : Form
    {
        int idFornecedorSelecionado = 0;
        int idProdutoSelecionado = 0;
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

            listar();
            RecarregarProdutosVinculados();
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
            LimparCampos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox3.Text) ||
                string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox5.Text) ||
                string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos antes de cadastrar!");
                return;
            }

            if (!ValidarCNPJ(textBox3.Text))
            {
                MessageBox.Show("O CNPJ digitado é inválido! Certifique-se de digitar os 14 números.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidarEmail(textBox5.Text))
            {
                MessageBox.Show("O formato do E-mail digitado está incorreto! Exemplo correto: nome@empresa.com", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sqlCheck = "SELECT COUNT(*) FROM fornecedores WHERE CNPJ = @cnpj";
                var comandoCheck = new MySqlCommand(sqlCheck, conexao);
                comandoCheck.Parameters.AddWithValue("@cnpj", textBox3.Text);

                conexao.Open();
                int cnpjExiste = Convert.ToInt32(comandoCheck.ExecuteScalar());
                conexao.Close();

                if (cnpjExiste > 0)
                {
                    MessageBox.Show("Erro: Já existe um fornecedor cadastrado com este CNPJ!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
            catch
            {
                MessageBox.Show("Erro ao cadastrar fornecedor: ");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idFornecedorSelecionado == 0)
            {
                MessageBox.Show("Por favor, selecione um fornecedor na tabela antes de excluir.");
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sqlCheck = "SELECT COUNT(*) FROM FornecedorProduto WHERE IdFornecedor = @id";
                var comandoCheck = new MySqlCommand(sqlCheck, conexao);
                comandoCheck.Parameters.AddWithValue("@id", idFornecedorSelecionado);

                conexao.Open();
                int possuiVinculos = Convert.ToInt32(comandoCheck.ExecuteScalar());
                conexao.Close();

                string mensagemConfirmacao = "Tem certeza que deseja excluir o fornecedor: " + textBox4.Text + "?";
                if (possuiVinculos > 0)
                {
                    mensagemConfirmacao = "ATENÇÃO: Este fornecedor possui VÍNCULOS com produtos no sistema!\n" +
                                          "Se você excluí-lo, os vínculos também serão apagados.\n\n" +
                                          "Deseja mesmo continuar?";
                }

                DialogResult confirmacao = MessageBox.Show(mensagemConfirmacao, "Confirmação de Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmacao == DialogResult.Yes)
                {
                    if (possuiVinculos > 0)
                    {
                        string sqlDelVinculo = "DELETE FROM FornecedorProduto WHERE IdFornecedor = @id";
                        var cmdDelV = new MySqlCommand(sqlDelVinculo, conexao);
                        cmdDelV.Parameters.AddWithValue("@id", idFornecedorSelecionado);
                        conexao.Open();
                        cmdDelV.ExecuteNonQuery();
                        conexao.Close();
                    }

                    string sql = "DELETE FROM fornecedores WHERE IdFornecedor = @id";
                    var comando = new MySqlCommand(sql, conexao);
                    comando.Parameters.AddWithValue("@id", idFornecedorSelecionado);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                    conexao.Close();

                    MessageBox.Show("Fornecedor excluído com sucesso!");

                    LimparCampos();
                    idFornecedorSelecionado = 0;
                    listar();
                }
            }
            catch
            {
                MessageBox.Show("Erro ao excluir fornecedor: ");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

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
            if (idFornecedorSelecionado == 0)
            {
                MessageBox.Show("Selecione um fornecedor clicando na tabela antes de atualizar.");
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                string sql = "UPDATE fornecedores SET Nome=@nome, CNPJ=@cnpj, Telefone=@telefone, Email=@email, Endereco=@endereco WHERE IdFornecedor=@id";

                var comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@id", idFornecedorSelecionado);
                comando.Parameters.AddWithValue("@nome", textBox4.Text);
                comando.Parameters.AddWithValue("@cnpj", textBox3.Text);
                comando.Parameters.AddWithValue("@telefone", textBox2.Text);
                comando.Parameters.AddWithValue("@email", textBox5.Text);
                comando.Parameters.AddWithValue("@endereco", textBox6.Text);

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();

                MessageBox.Show("Fornecedor actualizado com sucesso!");

                LimparCampos();
                idFornecedorSelecionado = 0;
                listar();
            }
            catch
            {
                MessageBox.Show("Erro ao atualizar fornecedor: ");
            }
        }

        private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idFornecedorSelecionado = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["IdFornecedor"].Value);

                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["CNPJ"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Telefone"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells["Endereco"].Value.ToString();

                ListarProdutosVinculados(textBox4.Text);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (idFornecedorSelecionado == 0 || idProdutoSelecionado == 0)
            {
                MessageBox.Show("Por favor, selecione um Fornecedor na tabela de cima E um Produto na tabela de baixo antes de vincular!");
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                string sql = "INSERT INTO FornecedorProduto (IdFornecedor, IdProduto) VALUES (@idFornecedor, @idProduto)";

                var comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@idFornecedor", idFornecedorSelecionado);
                comando.Parameters.AddWithValue("@idProduto", idProdutoSelecionado);

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();

                MessageBox.Show("Produto vinculado com sucesso!");

                ListarProdutosVinculados(textBox4.Text);

                idProdutoSelecionado = 0;
            }
            catch
            {
                MessageBox.Show("Erro ao vincular no banco de dados: ");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idProdutoSelecionado = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["IdProduto"].Value);
            }
        }
        private void RecarregarProdutosVinculados()
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sql = "SELECT IdProduto, Nome, Preco, QuantidadeEstoque FROM produtos";

                var comando = new MySqlCommand(sql, conexao);
                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);

                dataGridView2.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro ao carregar lista de produtos: ");
            }
        }

        private void ListarProdutosVinculados(string nomeFornecedor)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sql = @"SELECT p.IdProduto, p.Nome, p.Descricao, p.Preco 
                       FROM FornecedorProduto fp
                       INNER JOIN fornecedores f ON fp.IdFornecedor = f.IdFornecedor
                       INNER JOIN produtos p ON fp.IdProduto = p.IdProduto
                       WHERE f.Nome = @nome";

                var comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@nome", nomeFornecedor);

                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);

                dataGridView2.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro ao listar produtos vinculados: ");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                string sql = "SELECT * FROM fornecedores ORDER BY Nome ASC";
                var comando = new MySqlCommand(sql, conexao);
                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);
                dataGridView1.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro ao ordenar: ");
            }
        }

        private bool ValidarEmail(string email)
        {
            string modeloEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, modeloEmail);
        }

        private bool ValidarCNPJ(string cnpj)
        {
            string apenasNumeros = Regex.Replace(cnpj, @"[^\d]", "");

            return apenasNumeros.Length == 14;
        }
    }
}