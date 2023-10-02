using BBL;
using DAL;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ControleDeEstoque
{
    public partial class frmCadastroProduto : ControleDeEstoque.frmModeloDeFormularioDeCadastro
    {
        public string foto = "";
        public frmCadastroProduto()
        {
            InitializeComponent();
        }

        private void btInserir_Click(object sender, EventArgs e)
        {
            this.operacao = "inserir";
            this.alteraBotoes(2);
        }
        private void LimpaTela()
        {
            txtCodigo.Clear();
            txtNome.Clear();
            txtDescricao.Clear();
            txtValorPago.Clear();
            txtValorVenda.Clear();
            txtQtde.Clear();
            this.foto = "";
            pbFoto.Image = null;
        }
        private void btLocalizar_Click(object sender, EventArgs e)
        {
            //frmConsultaProduto f = new frmConsultaProduto();
            //f.ShowDialog();
            //if (f.codigo != 0)
            //{
            //    DALConexao cx = new DALConexao(DadosDaConexao.StringDeConexao);
            //    BLLProduto bll = new BLLProduto(cx);
            //    ModeloProduto modelo = bll.CarregaModeloProduto(f.codigo);
            //    txtCodigo.Text = modelo.CatCod.ToString();
            //    //colocar os dados na tela
            //    txtCodigo.Text = modelo.ProCod.ToString();
            //    txtDescricao.Text = modelo.ProDescricao;
            //    txtNome.Text = modelo.ProNome;
            //    txtQtde.Text = modelo.ProQtde.ToString();
            //    txtValorPago.Text = modelo.ProValorPago.ToString();
            //    txtValorVenda.Text = modelo.ProValorVenda.ToString();
            //    cbCategoria.SelectedValue = modelo.CatCod;
            //    cbSubCategoria.SelectedValue = modelo.ScatCod;
            //    cbUnd.SelectedValue = modelo.UmedCod;
            //    try
            //    {
            //        MemoryStream ms = new MemoryStream(modelo.ProFoto);
            //        pbFoto.Image = Image.FromStream(ms);
            //        this.foto = "Foto Original";
            //    }
            //    catch { }

            //    //txtQtde_Leave(sender, e);
            //    //txtValorPago_Leave(sender, e);
            //    //txtValorVenda_Leave(sender, e);
            //    alteraBotoes(3);
            //}
            //else
            //{
            //    this.LimpaTela();
            //    this.alteraBotoes(1);
            //}
            //f.Dispose();
        }

        private void btAlterar_Click(object sender, EventArgs e)
        {
            this.operacao = "alterar";
            this.alteraBotoes(2);

        }

        private void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult d = MessageBox.Show("Deseja excluir o registro?", "Aviso", MessageBoxButtons.YesNo);
                if (d.ToString() == "Yes")
                {
                    DALConexao cx = new DALConexao(DadosDaConexao.StringDeConexao);
                    BLLProduto bll = new BLLProduto(cx);
                    bll.Excluir(Convert.ToInt32(txtCodigo.Text));
                    this.LimpaTela();
                    this.alteraBotoes(1);
                }
            }
            catch
            {
                MessageBox.Show("Impossível excluir o registro. \n O registro esta sendo utilizado em outro local.");
                this.alteraBotoes(3);
            }
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                //leitura dos dados
                ModeloProduto modelo = new ModeloProduto();
                modelo.ProNome = txtNome.Text;
                modelo.ProDescricao = txtDescricao.Text;
                modelo.ProValorPago = Convert.ToDouble(txtValorPago.Text);
                modelo.ProValorVenda = Convert.ToDouble(txtValorVenda.Text);
                modelo.ProQtde = Convert.ToDouble(txtQtde.Text);
                modelo.UmedCod = Convert.ToInt32(cbUnd.SelectedValue);
                modelo.CatCod = Convert.ToInt32(cbCategoria.SelectedValue);
                modelo.ScatCod = Convert.ToInt32(cbSubCategoria.SelectedValue);

                //obj para gravar os dados no banco
                DALConexao cx = new DALConexao(DadosDaConexao.StringDeConexao);
                BLLProduto bll = new BLLProduto(cx);
                if (this.operacao == "inserir")
                {
                    //cadastrar uma Produto
                    modelo.CarregaImagem(this.foto);
                    bll.Incluir(modelo);
                    MessageBox.Show("Cadastro efetuado: Código " + modelo.ProCod.ToString());

                }
                else
                {
                    modelo.ProCod = Convert.ToInt32(txtCodigo.Text);
                    //alterar um produto
                    if (this.foto == "Foto Original")
                    {
                        ModeloProduto mt = bll.CarregaModeloProduto(modelo.ProCod);
                        modelo.ProFoto = mt.ProFoto;
                    }
                    else
                    {
                        modelo.CarregaImagem(this.foto);
                    }
                    bll.Alterar(modelo);
                    MessageBox.Show("Cadastro alterado");
                }
                this.LimpaTela();
                this.alteraBotoes(1);
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.alteraBotoes(1);
            this.LimpaTela();
        }
    }
}
