using CrudBasico_Windows_Form.src.dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CrudBasico_Windows_Form
{
    public partial class Main : Form
    {
        List<TextBox> campos = new List<TextBox>();
        string idSelected;
        OpenFileDialog ofSelecionaImagem;

        public void adicionaCampos()
        {
            campos.Add(txtHostname);
            campos.Add(txtModelo);
            campos.Add(txtSetor);
            campos.Add(txtIP);
        }

        public Main()
        {
            InitializeComponent();
            atualizarMaquinas();
            adicionaCampos();
            limparCelulas();

            ofSelecionaImagem = new OpenFileDialog()
            {
                FileName = "",
                Filter = "Arquivos de Imagem (*.jpg)|*.jpg",
                Title = "Selecione uma Imagem"
            };
        }

        private void btnCadastrar_Click(object sender, System.EventArgs e)
        {

            try
            {
                if (validaCampos())
                {
                    string efetuado = "";
                    if (btnCadastrar.Text == "Cadastrar")
                    {
                        ConexaoOracle.ExecutaComando($"insert into maquina values ('', '{txtHostname.Text}', '{txtIP.Text}', '{txtModelo.Text}', '{txtSetor.Text}', '', sysdate)");
                        efetuado = "cadastrada";
                    }

                    else if (btnCadastrar.Text == "Editar")
                    {
                        ConexaoOracle.ExecutaComando($"update maquina set hostname = '{txtHostname.Text}', ip = '{txtIP.Text}', modelo = '{txtModelo.Text}', setor = '{txtSetor.Text}' where id = {idSelected}");
                        efetuado = "editada";
                        btnVoltar.PerformClick();
                    }

                    MessageBox.Show("Máquina " + efetuado + " com sucesso", "Cadastrado!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limparCampos();
                    atualizarMaquinas();
                    limparCelulas();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro: " + ex.Message);
            }

        }

        private void atualizarMaquinas()
        {
            dgDados.DataSource = ConexaoOracle.RetornaDados("SELECT M.ID, M.HOSTNAME, M.IP, M.MODELO, M.SETOR, M.DATA_CADASTRO FROM MAQUINA M");
        }

        private Boolean validaCampos()
        {
            for (int i = 0; i < campos.Count; i++)
            {
                if (String.IsNullOrEmpty(campos[i].Text))
                {
                    MessageBox.Show($"O campo {campos[i].Tag.ToString()} deve ser preenchido!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private void limparCampos()
        {
            for (int i = 0; i < campos.Count; i++)
            {
                campos[i].Clear();
            }
        }

        private void dgDados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIP.Text = dgDados.Rows[dgDados.SelectedCells[0].RowIndex].Cells["IP"].Value.ToString();
            txtHostname.Text = dgDados.Rows[dgDados.SelectedCells[0].RowIndex].Cells["HOSTNAME"].Value.ToString();
            txtSetor.Text = dgDados.Rows[dgDados.SelectedCells[0].RowIndex].Cells["SETOR"].Value.ToString();
            txtModelo.Text = dgDados.Rows[dgDados.SelectedCells[0].RowIndex].Cells["MODELO"].Value.ToString();
            idSelected = dgDados.Rows[dgDados.SelectedCells[0].RowIndex].Cells["ID"].Value.ToString();
            btnCadastrar.Text = "Editar";
            btnDeletar.Visible = true;
            btnVoltar.Visible = true;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            btnDeletar.Visible = false;
            btnVoltar.Visible = false;
            btnCadastrar.Text = "Cadastrar";
            limparCampos();
            dgDados.CurrentCell = null;
            limparCelulas();

        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            ConexaoOracle.ExecutaComando($"delete from maquina where ID =" + idSelected);
            MessageBox.Show("Máquina Deletada com sucesso", "Deletada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            limparCampos();
            atualizarMaquinas();
            btnVoltar.PerformClick();
            limparCelulas();
        }

        private void limparCelulas()
        {
            dgDados.CurrentCell = null;
            dgDados.ClearSelection();
        }

        private void lblCarregaFoto_Click(object sender, EventArgs e)
        {         
            if (ofSelecionaImagem.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var caminhoImagem = ofSelecionaImagem.FileName;
                    pbFoto.Image = Image.FromFile(caminhoImagem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar arquivo:" + ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
