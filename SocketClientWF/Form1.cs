using SocketClientServerLib;
using SocketClientServerLib.DTOs;
using SocketClientServerLib.Helpers;
using SocketClientWF.Helpers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SocketClientWF
{
    public partial class Form1 : Form
    {
        private Client _client;
        private CancellationTokenSource _cts;
        private UserDTO? _selectedClient;
        private Dictionary<int, int> _rtbSelectedTextPositions;
        private Options _options;
        public Form1()
        {
            InitializeComponent();
            Init();
            LoadOptions();
        }
        private void LoadOptions()
        {
            OptionsManager opManager = new();
            (bool result, string error, Options? option) = opManager.GetOptions();
            if (result == false)
            {
                ShowErrorBox(error);
                return;
            }
            _options = option!;
            if (Guid.TryParse(_options.Guid, out Guid guid))
                _client.Guid = guid;

        }
        private void Init()
        {
            _cts = new CancellationTokenSource();

            ResponseServerHandler responseServerHandler = new(
                inviteAction: (UserDTO user) => Invoke(() => cbClients.Items.Add(user)),
                logoutAction: (UserDTO? user) =>
                {
                    if (user is not null)
                        Invoke(() => cbClients.Items.Remove(user));
                },
                loginAccessAction: (UserDTO[] usersOnline) =>
                {
                    if (usersOnline.Any())
                        Invoke(() => cbClients.Items.AddRange(usersOnline));
                },
                registration: async (string guid) =>
                {
                    _options.Guid = guid;
                    OptionsManager opManager = new();
                    return await opManager.SaveOptions(_options);
                });

            _client = new(
                Encoding.Unicode,
                new(WriteMessage, WriteLineMessage, WriteLineError),
                responseServerHandler,
                _cts.Token);
            _rtbSelectedTextPositions = new();
        }

        #region События формы
        private async void btnSend_Click(object sender, EventArgs e) 
            => await _client.SendToAsync(_selectedClient, rtbMessage.Text);
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            bool result = await _client.ConnectAsync(_options.ServerIP, _options.ServerPort);
            if (!result)
                return;
            if (_client.UserList!.Any())
            {
                cbClients.Items.Clear();
                cbClients.Items.Add(_client.UserList![0]);
            }
            _ = Task.Run(async () => await _client.ReceivingMessageStart());
        }
        private void cbClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedClient = (UserDTO)cbClients.SelectedItem;
        }

        private async void LogInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_client.IsConnected == false)
            {
                WriteLineError("Client is not connected!");
                return;
            }

            using AuthorizationForm authorizationForm = new(false);
            if (authorizationForm.ShowDialog() is not DialogResult.OK)
                return;
            await _client.LogIn(authorizationForm.Login, authorizationForm.Password, _options.ClientName);
        }
        private async void tsmiRegistration_Click(object sender, EventArgs e)
        {
            if (_client.IsConnected == false)
            {
                WriteLineError("Client is not connected!");
                return;
            }

            using AuthorizationForm authorizationForm = new(true);
            if (authorizationForm.ShowDialog() is not DialogResult.OK)
                return;
            await _client.Registration(authorizationForm.Login, authorizationForm.Password, _options.ClientName);
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OptionsForm optionsForm = new OptionsForm();
            if (optionsForm.ShowDialog() is not DialogResult.OK)
                return;
            _options = optionsForm.Options;
            if (_client.Guid == Guid.Empty && Guid.TryParse(_options.Guid, out Guid guid))
                _client.Guid = guid;
        }
        #endregion

        #region helpers methods
        private void ShowErrorBox(string error)
           => MessageBox.Show(error,
                  "Error",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
        private void WriteMessage(string message) => Invoke(() => rtbMain.Text += message);
        private void WriteLineMessage(string message) => Invoke(() => rtbMain.Text += $"{message}\n");
        private void WriteLineError(string error) => Invoke(() =>
        {
            _rtbSelectedTextPositions.Add(rtbMain.Text.Length, error.Length);
            rtbMain.Text += $"{error}\n";
            foreach (var item in _rtbSelectedTextPositions)
            {
                rtbMain.Select(item.Key, item.Value);
                rtbMain.SelectionColor = Color.Red;
            }
            rtbMain.Select(0, 0);
        });
        #endregion
    }
}