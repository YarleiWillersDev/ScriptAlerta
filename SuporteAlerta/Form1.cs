using System.Reflection.Metadata.Ecma335;
using System.Xml.Serialization;

namespace SuporteAlerta;

public partial class Form1 : Form
{
    private readonly HttpClient client = new HttpClient();

    private TextBox txtMensagem = null!;
    private Button button_Enviar = null!;
    private Label lblStatus = null!;

    public Form1()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        // Configurações do formulário
        this.Text = "Enviar Alerta";
        this.Size = new Size(800, 400);
        this.StartPosition = FormStartPosition.CenterScreen;

        // TextBox
        txtMensagem = new TextBox();
        txtMensagem.Location = new Point(20, 20);
        txtMensagem.Size = new Size(740, 200);
        txtMensagem.Multiline = true;
        this.Controls.Add(txtMensagem);

        // Button
        button_Enviar = new Button();
        button_Enviar.Text = "Enviar";
        button_Enviar.Location = new Point(20, 240);
        button_Enviar.Size = new Size(100, 30);
        button_Enviar.Click += botao_Enviar_Click;
        this.Controls.Add(button_Enviar);

        // Label
        lblStatus = new Label();
        lblStatus.Location = new Point(140, 240);
        lblStatus.Size = new Size(630, 30);
        this.Controls.Add(lblStatus);
    }

    public async void botao_Enviar_Click(object sender, EventArgs e)
    {
        string mensagem = txtMensagem.Text.Trim();
        if (string.IsNullOrEmpty(mensagem))
        {
            MessageBox.Show("Mensagem vazia não pode ser vazia!");
            return;
        }

        try
        {
            var response = await client.PostAsync(
                $"http://localhost:5000/Alerta?mensagem={mensagem}",
                null
            );

            if (response.IsSuccessStatusCode)
            {
                lblStatus.Text = "Mensagem enviada para as lojas!";
                txtMensagem.Clear();
            }
            else
            {
                lblStatus.Text = "erro ao enviar mensagem.";
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Erro " + ex.Message;
        }
    }
}
