using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;

namespace ClienteAlerta;

public partial class Form1 : Form
{
    private HubConnection connection = null!;
    private Label lblMensagem = null!;
    private System.Windows.Forms.Timer timerPiscar = null!;
    private NotifyIcon notifyIcon = null!;

    public Form1()
    {
        InitializeComponent();
        //RegistrarInicializacaoWindows();
        ConectarServidor();
    }

    // Configuração do formulário que aparecerá na tela da loja
    private void InitializeComponent()
    {
        {
            // Configuração do formulário
            this.Text = "Cliente Alerta";
            this.Size = new Size(1200, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Travar redimensionamento
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Tamanho fixo garantido
            this.MinimumSize = new Size(1200, 600);
            this.MaximumSize = new Size(1200, 600);

            // Cor de fundo para destaque
            this.BackColor = Color.DarkRed;

            // Esconder a janela ao iniciar
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();

            // Label da mensagem
            lblMensagem = new Label();
            lblMensagem.Dock = DockStyle.Fill;
            lblMensagem.Font = new Font("Segoe UI", 48, FontStyle.Bold);
            lblMensagem.ForeColor = Color.White;
            lblMensagem.TextAlign = ContentAlignment.MiddleCenter;
            lblMensagem.Text = "AGUARDANDO ALERTAS...";

            this.Controls.Add(lblMensagem);

            // Timer para efeito piscando
            timerPiscar = new System.Windows.Forms.Timer();
            timerPiscar.Interval = 500;
            timerPiscar.Tick += TimerPiscar_Tick;

            // Ícone de bandeja
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Warning;
            notifyIcon.Visible = true;
            notifyIcon.Text = "Cliente Alerta";
        }
    }

    // Ação que faz a mensagem de POP-UP piscar
    private void TimerPiscar_Tick(object? sender, EventArgs e)
    {
        lblMensagem.Visible = !lblMensagem.Visible;
    }

    // Efetua a conexão com o servidor
    private async void ConectarServidor()
    {
        try
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/alertaHub")
                .WithAutomaticReconnect()
                .Build();

            connection.On<string>("ReceberMensagem", mensagem =>
            {
                this.Invoke(() =>
                {
                    // mostra a janela
                    this.Show();
                    this.WindowState = FormWindowState.Normal;

                    lblMensagem.Text = mensagem;
                    timerPiscar.Start();
                });
            });

            await connection.StartAsync();

            lblMensagem.Text = "Conectado ao servidor de alertas.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Erro ao conectar ao servidor:\n" + ex.Message,
                "Erro",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    // Fazer o executável rodar em segundo plano

/*    private void RegistrarInicializacaoWindows()
    {
        string caminhoApp = Application.ExecutablePath;

        RegistryKey chave = Registry.CurrentUser.OpenSubKey(
            @"SOFTAWRE\Microsoft\Windows\CurrentVersion\Run", true);

        chave.SetValue("ClienteAlerta", caminhoApp); 
    } */
}