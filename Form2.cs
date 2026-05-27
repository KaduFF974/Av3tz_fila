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
        int idFornecedorSelecionado = 0;
        int idProdutoSelecionado = 0;
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
            listar();
            RecarregarFornecedoresVinculados();
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
            if (idProdutoSelecionado == 0)
            {
                MessageBox.Show("Por favor, selecione um produto na tabela antes de excluir.");
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sqlCheck = "SELECT COUNT(*) FROM FornecedorProduto WHERE IdProduto = @id";
                var comandoCheck = new MySqlCommand(sqlCheck, conexao);
                comandoCheck.Parameters.AddWithValue("@id", idProdutoSelecionado);

                conexao.Open();
                int possuiVinculos = Convert.ToInt32(comandoCheck.ExecuteScalar());
                conexao.Close();

                string mensagemConfirmacao = "Tem certeza que deseja excluir o produto: " + textBox4.Text + "?";
                if (possuiVinculos > 0)
                {
                    mensagemConfirmacao = "ATENÇÃO: Este produto está vinculado a fornecedores!\n" +
                                          "A exclusão removerá esses vínculos automaticamente.\n\n" +
                                          "Deseja prosseguir?";
                }

                DialogResult confirmacao = MessageBox.Show(mensagemConfirmacao, "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmacao == DialogResult.Yes)
                {
                    if (possuiVinculos > 0)
                    {
                        string sqlDelVinculo = "DELETE FROM FornecedorProduto WHERE IdProduto = @id";
                        var cmdDelV = new MySqlCommand(sqlDelVinculo, conexao);
                        cmdDelV.Parameters.AddWithValue("@id", idProdutoSelecionado);
                        conexao.Open();
                        cmdDelV.ExecuteNonQuery();
                        conexao.Close();
                    }

                    string sql = "DELETE FROM produtos WHERE IdProduto = @id";
                    var comando = new MySqlCommand(sql, conexao);
                    comando.Parameters.AddWithValue("@id", idProdutoSelecionado);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                    conexao.Close();

                    MessageBox.Show("Produto excluído com sucesso!");

                    textBox4.Clear();
                    textBox3.Clear();
                    textBox2.Clear();
                    textBox5.Clear();
                    if (textBox6 != null) textBox6.Clear();

                    idProdutoSelecionado = 0;
                    listar();
                }
            }
            catch
            {
                MessageBox.Show("Erro ao excluir: ");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idProdutoSelecionado == 0)
            {
                MessageBox.Show("Selecione um produto clicando na tabela antes de atualizar.");
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                string sql = "UPDATE Produtos SET Nome=@nome, Descricao=@desc, Preco=@preco, QuantidadeEstoque=@QuantEs WHERE IdProduto=@id";

                var comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@id", idProdutoSelecionado);
                comando.Parameters.AddWithValue("@nome", textBox4.Text);
                comando.Parameters.AddWithValue("@desc", textBox3.Text);
                comando.Parameters.AddWithValue("@preco", Convert.ToDecimal(textBox2.Text));
                comando.Parameters.AddWithValue("@QuantEs", Convert.ToInt32(textBox5.Text));

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();

                MessageBox.Show("Produto atualizado com sucesso!");
                idProdutoSelecionado = 0;
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

            decimal preco;
            if (!decimal.TryParse(textBox2.Text, out preco))
            {
                MessageBox.Show("Por favor, insira um valor numérico válido para o campo Preço (Ex: 29,90)!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int estoque;
            if (!int.TryParse(textBox5.Text, out estoque) || estoque < 0)
            {
                MessageBox.Show("A quantidade em estoque deve ser um número inteiro maior ou igual a zero!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                var strsql = "INSERT INTO produtos (Nome, Descricao, Preco, QuantidadeEstoque) VALUES (@nome, @desc, @preco, @quantEs)";
                var comando = new MySqlCommand(strsql, conexao);

                comando.Parameters.AddWithValue("@nome", textBox4.Text);
                comando.Parameters.AddWithValue("@desc", textBox3.Text);
                comando.Parameters.AddWithValue("@preco", preco);
                comando.Parameters.AddWithValue("@quantEs", estoque);

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();

                MessageBox.Show("Produto cadastrado com sucesso!");

                textBox4.Clear();
                textBox3.Clear();
                textBox2.Clear();
                textBox5.Clear();
                if (textBox6 != null) textBox6.Clear();

                listar();
            }
            catch
            {
                MessageBox.Show("Erro ao cadastrar: ");
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idProdutoSelecionado = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["IdProduto"].Value);

                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells["IdProduto"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["Descricao"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Preco"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["QuantidadeEstoque"].Value.ToString();

                ListarFornecedoresVinculados(textBox4.Text);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (idFornecedorSelecionado == 0 || idProdutoSelecionado == 0)
            {
                MessageBox.Show("Selecione um Produto na tabela de cima E um Fornecedor na tabela de baixo para vincular!");
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

                MessageBox.Show("Fornecedor vinculado com sucesso!");

                ListarFornecedoresVinculados(textBox4.Text);

                idFornecedorSelecionado = 0;
            }
            catch
            {
                MessageBox.Show("Erro ao vincular: ");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idFornecedorSelecionado = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["IdFornecedor"].Value);
            }
        }

        private void RecarregarFornecedoresVinculados()
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sql = "SELECT IdFornecedor, Nome, CNPJ, Telefone FROM fornecedores";

                var comando = new MySqlCommand(sql, conexao);
                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);

                dataGridView2.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro ao carregar lista de fornecedores: ");
            }
        }

        private void ListarFornecedoresVinculados(string nomeProduto)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sql = @"SELECT f.IdFornecedor, f.Nome, f.Telefone, f.Email 
                       FROM FornecedorProduto fp
                       INNER JOIN produtos p ON fp.IdProduto = p.IdProduto
                       INNER JOIN fornecedores f ON fp.IdFornecedor = f.IdFornecedor
                       WHERE p.Nome = @nome";

                var comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@nome", nomeProduto);

                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);

                dataGridView2.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro ao listar fornecedores vinculados: ");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                string sql = "SELECT * FROM produtos ORDER BY Nome ASC";
                var comando = new MySqlCommand(sql, conexao);
                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);
                dataGridView1.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Erro: ");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Por favor, digite um preço limite no campo 'Preço' (textBox2) para aplicar o filtro!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");

                string sql = "SELECT * FROM produtos WHERE Preco <= @preco";

                var comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@preco", Convert.ToDecimal(textBox2.Text));

                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);

                dataGridView1.DataSource = tabela;
            }
            catch
            {
                MessageBox.Show("Certifique-se de que o campo 'Preço' contém um número válido. Erro: ");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                var conexao = new MySqlConnection("Server=Localhost;Database=gestaodeprodutos;Userid=root");
                string sql = "SELECT * FROM produtos WHERE QuantidadeEstoque < 10";
                var comando = new MySqlCommand(sql, conexao);
                var adapter = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                adapter.Fill(tabela);
                dataGridView1.DataSource = tabela;

                MessageBox.Show("Lista filtrada! Exibindo apenas produtos com estoque crítico (menos de 10 unidades).");
            }
            catch
            {
                MessageBox.Show("Erro: ");
            }
        }
    }
}