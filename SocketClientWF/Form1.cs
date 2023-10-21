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
            _client = new(
                Encoding.Unicode,
                new(WriteMessage, WriteLineMessage, WriteLineError),
                _cts.Token);
            _client.MessageReceived += OnMessageReceived;
            _rtbSelectedTextPositions = new();
        }

        private async void OnMessageReceived(object? sender, MessageRecevedEventArgs e)
        {
            Task task = e.Message.commandType switch
            {
                Enum_CommandType.NEW_GUID => RegistrationResponseHandle(e.Message.message),
                (Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.LOG_IN) => LoginResponseErrorHandle(e.Message.ToString()),
                Enum_CommandType.LOG_IN => LoginAccessHandle(e.Message),
                Enum_CommandType.SEND_TO => SendToHandle(e.Message),
                Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.CLIENT_INVITE => ClientInviteHandle(e.Message),
                Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.LOG_OUT => ClientLogOutHandle(e.Message),
                _ => throw new NotImplementedException("Неизвестный тип команды")
            };
            await task;
        }

        #region обработчики сообщений сервера
        private Task ClientLogOutHandle(MessageRecord message)
        {
            WriteLineMessage(message.ToString());
            if (Guid.TryParse(message.message, out Guid guid) == false)
                return Task.CompletedTask;
            UserDTO? user = _client.UserList?.FirstOrDefault(u => u.guid == guid);
            if (user is not null)
                Invoke(() => cbClients.Items.Remove(user));
            return Task.CompletedTask;
        }

        private Task ClientInviteHandle(MessageRecord message)
        {
            string[] parts = message.message.Split((char)29); //name + guid
            if (Guid.TryParse(parts[1], out Guid guid))
            {
                UserDTO user = new(guid, parts[0], true);
                _client.UserList!.Add(user);
                Invoke(() => cbClients.Items.Add(user));
            }
            else
                WriteLineError("Unknown user");
            return Task.CompletedTask;
        }

        private Task LoginAccessHandle(MessageRecord message)
        {
            WriteLineMessage((message with { message = "Authentication success" }).ToString());
            //список онлайн клиентов
            ReadOnlySpan<byte> buffer = new(Convert.FromBase64String(message.message));
            List<UserDTO> usersOnline = JsonSerializer.Deserialize<List<UserDTO>>(buffer)!;
            foreach (UserDTO user in usersOnline)
                _client.UserList!.Add(user);
            if (usersOnline.Any())
                Invoke(() => cbClients.Items.AddRange(usersOnline.ToArray()));
            return Task.CompletedTask;
        }

        private Task LoginResponseErrorHandle(string error)
        {
            WriteLineError(error);
            return Task.CompletedTask;
        }

        private Task SendToHandle(MessageRecord message)
        {
            WriteLineMessage(message.ToString());
            return Task.CompletedTask;
        }

        private async Task RegistrationResponseHandle(string guid)
        {
            _options.Guid = guid;
            OptionsManager opManager = new();
            if (Guid.TryParse(guid, out Guid g))
                _client.Guid = g;
            (bool result, string error) = await opManager.SaveOptions(_options);
            if (result == false)
            {
                WriteLineError(error);
                return;
            }
            WriteLineMessage("Registry is successfull");
        }
        #endregion

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (_client.IsConnected == false)
            {
                WriteLineError("Клиент не подключен к серверу");
                return;
            }
            if (_selectedClient is null)
            {
                WriteLineError("Адресат не выбран");
                return;
            }
            try
            {
                await _client.SendAsync(new(
                    DateTime.Now,
                    _client.Guid,
                    new List<Guid>() { (Guid)_selectedClient?.guid! },
                    Enum_CommandType.SEND_TO,
                    rtbMessage.Text));
            }
            catch (SocketException ex)
            {
                WriteLineError($"socket: {ex.Message}");
            }
            catch (Exception ex)
            {
                WriteLineError(ex.Message);
            }
        }
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (_client.IsConnected)
            {
                WriteLineError("Уже подключен");
                return;
            }
            if (IPEndPoint.TryParse($"{_options.ServerIP}:{_options.ServerPort}", out IPEndPoint? endPoint) == false)
            {
                WriteLineError("Некорректный адрес или порт сокета");
                return;
            }
            await _client.ConnectAsync(endPoint!);
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

            WriteLineMessage("try authentification..");
            await _client.SendAsync(new(
                DateTime.Now,
                _client.Guid,
                null,
                Enum_CommandType.LOG_IN,
                $"{authorizationForm.Login}{(char)29}{authorizationForm.Password}{(char)29}{_options.ClientName}"
                ));
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
            WriteLineMessage("try registration..");
            await _client.SendAsync(new(
                 DateTime.Now,
                _client.Guid,
                null,
                Enum_CommandType.REGISTRATION,
                $"{authorizationForm.Login}{(char)29}{authorizationForm.Password}{(char)29}{_options.ClientName}"
                ));
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