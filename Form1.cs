using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace ControleEstoque
{
    public partial class FormControle : Form
    {

        public FormControle()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet();
        private MySqlConnectionStringBuilder conexaoBanco()
        {
            MySqlConnectionStringBuilder conectionDB = new MySqlConnectionStringBuilder();
            conectionDB.Server = "localhost";
            conectionDB.Database = "estoque";
            conectionDB.UserID = "root";
            conectionDB.Password = "";
            conectionDB.SslMode = 0;
            return conectionDB;
        }

        private void FormControle_Load(object sender, EventArgs e)
        {
            atualizar();

            tbAlfaTam.SelectedIndex = 0;
            cbSexo.SelectedIndex = 0;
        }

        private void atualizar()
        {
            MySqlConnectionStringBuilder conectionDB = conexaoBanco();
            MySqlConnection doConection = new MySqlConnection(conectionDB.ToString());
            try
            {
                doConection.Open();

                MySqlCommand commandMysql = doConection.CreateCommand();
                commandMysql.CommandText = "SELECT * FROM produto WHERE ativoProduto = 1";
                MySqlDataReader reader = commandMysql.ExecuteReader();

                gridEstoque.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)gridEstoque.Rows[0].Clone();
                    row.Cells[0].Value = reader.GetInt32(0); //id
                    row.Cells[1].Value = reader.GetString(1); //prouto
                    row.Cells[2].Value = reader.GetString(2); //sexo
                    row.Cells[3].Value = reader.GetString(3); //tamanho
                    row.Cells[4].Value = reader.GetString(4); //cor
                    row.Cells[5].Value = reader.GetInt32(5); //quantidade
                    gridEstoque.Rows.Add(row); //Add linha na tabela    
                }
                
                doConection.Close();


            }

            catch (Exception ex)
            {
                MessageBox.Show("Can't open connetion");
                Console.WriteLine(ex.Message);
            }
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();
        }
        private void limparCampos()
        {
            tbProduto.Clear();
            tbNumTam.Value = 0;
            tbAlfaTam.SelectedIndex = 0;
            tbCor.Clear();
            tbQtd.Value = 0;
            cbSexo.SelectedIndex = 0;
            tbID.Clear();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conectionDB = conexaoBanco();
            MySqlConnection doConection = new MySqlConnection(conectionDB.ToString());

            string tamanho;

            if (tbNumTam.Value > 0 && tbAlfaTam.SelectedIndex != 0)
            {
                MessageBox.Show("Valor de Tamanho incorreto");
                tamanho = "";
            }
            else if (tbNumTam.Value > 0 && tbAlfaTam.SelectedIndex == 0 && tbProduto.Text != "" && tbCor.Text != "")
            {
                tamanho = Convert.ToString(tbNumTam.Value);
                try
                {
                    doConection.Open();

                    MySqlCommand commandMysql = doConection.CreateCommand();


                    commandMysql.CommandText = "INSERT INTO produto (nomeProduto,sexoProduto,tamanhoProduto,corProduto, quantidadeProduto)" +
                        "VALUES('" + tbProduto.Text + "','" + cbSexo.GetItemText(cbSexo.SelectedItem) + "','" + tamanho + "','" + tbCor.Text + "','" + Convert.ToInt16(tbQtd.Value) + "')";
                    commandMysql.ExecuteNonQuery();
                    doConection.Close();
                    MessageBox.Show("Item cadastrado com sucesso");
                    atualizar();
                    limparCampos();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (tbNumTam.Value == 0 && tbAlfaTam.GetItemText(tbAlfaTam.SelectedIndex) != "" && tbProduto.Text != "" && tbCor.Text != "")
            {
                tamanho = tbAlfaTam.GetItemText(tbAlfaTam.SelectedItem);
                try
                {
                    doConection.Open();

                    MySqlCommand commandMysql = doConection.CreateCommand();


                    commandMysql.CommandText = "INSERT INTO produto (nomeProduto,sexoProduto,tamanhoProduto,corProduto, quantidadeProduto)" +
                        "VALUES('" + tbProduto.Text + "','" + cbSexo.GetItemText(cbSexo.SelectedItem) + "','" + tamanho + "','" + tbCor.Text + "','" + Convert.ToInt16(tbQtd.Value) + "')";
                    commandMysql.ExecuteNonQuery();
                    doConection.Close();
                    MessageBox.Show("Item cadastrado com sucesso");
                    atualizar();
                    limparCampos();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Preencha corretamente os campos");
            }
        }

        private void tbProduto_KeyPress(object sender, KeyPressEventArgs e)
        {
            string searchQuery = tbProduto.Text;
            MySqlConnectionStringBuilder conectionDB = conexaoBanco();
            MySqlConnection doConection = new MySqlConnection(conectionDB.ToString());
            try
            {
                doConection.Open();

                MySqlCommand commandMysql = doConection.CreateCommand();
                commandMysql.CommandText = "SELECT * FROM produto WHERE nomeProduto LIKE '"+searchQuery+"%'";
                MySqlDataReader reader = commandMysql.ExecuteReader();

                gridEstoque.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)gridEstoque.Rows[0].Clone();
                    row.Cells[0].Value = reader.GetInt32(0); //id
                    row.Cells[1].Value = reader.GetString(1); //prouto
                    row.Cells[2].Value = reader.GetString(2); //sexo
                    row.Cells[3].Value = reader.GetString(3); //tamanho
                    row.Cells[4].Value = reader.GetString(4); //cor
                    row.Cells[5].Value = reader.GetString(5); //quantidade
                    gridEstoque.Rows.Add(row); //Add linha na tabela
                }
                doConection.Close();


            }

            catch (Exception ex)
            {
                MessageBox.Show("Can't open connetion");
                Console.WriteLine(ex.Message);
            }
        }

        private void gridEstoque_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(gridEstoque.Rows[e.RowIndex].Cells[e.ColumnIndex ].Value != null)
            {
                gridEstoque.CurrentRow.Selected = true;
                tbID.Text = gridEstoque.Rows[e.RowIndex].Cells["colID"].FormattedValue.ToString();
                tbProduto.Text = gridEstoque.Rows[e.RowIndex].Cells["colProduto"].FormattedValue.ToString();
                cbSexo.Text= gridEstoque.Rows[e.RowIndex].Cells["colSexo"].FormattedValue.ToString();
                tbAlfaTam.Text = gridEstoque.Rows[e.RowIndex].Cells["colTamanho"].FormattedValue.ToString();
                tbCor.Text = gridEstoque.Rows[e.RowIndex].Cells["colCor"].FormattedValue.ToString();
                tbQtd.Text = gridEstoque.Rows[e.RowIndex].Cells["colQuantidade"].FormattedValue.ToString();
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conectionDB = conexaoBanco();
            MySqlConnection doConection = new MySqlConnection(conectionDB.ToString());
            string tamanho;

            if (tbNumTam.Value > 0 && tbAlfaTam.SelectedIndex != 0)
            {
                MessageBox.Show("Valor de Tamanho incorreto");
                tamanho = "";
            }
            else if (tbNumTam.Value > 0 && tbAlfaTam.SelectedIndex == 0 && tbProduto.Text != "" && tbCor.Text != "")
            {
                tamanho = Convert.ToString(tbNumTam.Value);
                try
                {
                    doConection.Open();

                    MySqlCommand commandMysql = doConection.CreateCommand();


                    commandMysql.CommandText = "UPDATE produto SET nomeProduto = '" + tbProduto.Text + "'," +
                        "sexoProduto = '" + cbSexo.GetItemText(cbSexo.SelectedItem) + "'," +
                        "tamanhoProduto = '" + tamanho + "'," +
                        "corProduto = '" + tbCor.Text + "'," +
                        "quantidadeProduto = '" + Convert.ToInt16(tbQtd.Value) + "' " +
                        "WHERE idProduto = " + tbID.Text + "";
                   commandMysql.ExecuteNonQuery();
                    doConection.Close();
                    MessageBox.Show("Item cadastrado com sucesso");
                    atualizar();
                    limparCampos();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (tbNumTam.Value == 0 && tbAlfaTam.GetItemText(tbAlfaTam.SelectedIndex) != "" && tbProduto.Text != "" && tbCor.Text != "")
            {
                tamanho = tbAlfaTam.GetItemText(tbAlfaTam.SelectedItem);
                try
                {
                    doConection.Open();

                    MySqlCommand commandMysql = doConection.CreateCommand();
                    commandMysql.CommandText = "UPDATE produto SET nomeProduto = '" + tbProduto.Text + "'," +
                        "sexoProduto = '" + cbSexo.GetItemText(cbSexo.SelectedItem) + "'," +
                        "tamanhoProduto = '" + tamanho + "'," +
                        "corProduto = '" + tbCor.Text + "'," +
                        "quantidadeProduto = '" + Convert.ToInt16(tbQtd.Value) + "' " +
                        "WHERE idProduto = " + tbID.Text + "";

                    commandMysql.ExecuteNonQuery();
                    doConection.Close();
                    MessageBox.Show("Item Atualizado com  com sucesso");
                    atualizar();
                    limparCampos();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Preencha corretamente os campos");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conectionDB = conexaoBanco();
            MySqlConnection doConection = new MySqlConnection(conectionDB.ToString());

                try
                {
                    doConection.Open();

                    MySqlCommand commandMysql = doConection.CreateCommand();


                    commandMysql.CommandText = "UPDATE produto SET ativoProduto = 0 WHERE idProduto = " + tbID.Text + "";
                    commandMysql.ExecuteNonQuery();
                    MessageBox.Show("Item removido com sucesso");
                    atualizar();
                    limparCampos();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
    }
    
}
