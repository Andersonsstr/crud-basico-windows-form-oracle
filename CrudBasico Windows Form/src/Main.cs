using CrudBasico_Windows_Form.src.dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CrudBasico_Windows_Form
{
    public partial class Main : Form
    {
        List<TextBox> campos = new List<TextBox>();

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
        }

        private void btnCadastrar_Click(object sender, System.EventArgs e)
        {

            try
            {
                if (validaCampos())
                {
                    ConexaoOracle.ExecutaComando($"insert into maquina values ('', '{txtHostname.Text}', '{txtIP.Text}', '{txtModelo.Text}', '{txtSetor.Text}', '', sysdate)");
                    MessageBox.Show("Máquina cadastrada com sucesso", "Cadastrado!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limparCampos();
                    atualizarMaquinas();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro: " + ex.Message);
            }

        }

        private void atualizarMaquinas()
        {
            dgDados.DataSource = ConexaoOracle.RetornaDados("SELECT * FROM MAQUINA");
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
    }
}
