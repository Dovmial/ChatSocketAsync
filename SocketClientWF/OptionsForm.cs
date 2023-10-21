using SocketClientServerLib;
using SocketClientWF.Helpers;


namespace SocketClientWF
{
    public partial class OptionsForm : Form
    {
        internal Options Options { get; set; }
        private readonly OptionsManager _opManager;
        internal OptionsForm()
        {
            InitializeComponent();
            _opManager = new OptionsManager();
            LoadOptions();
        }
        private void LoadOptions()
        {
            (bool result, string error, Options? option) = _opManager.GetOptions();
            if (result == false)
            {
                ShowErrorBox(error);
                return;
            }
            Options = option!;
            tbName.Text = Options.ClientName;
            lblGuid.Text = Options.Guid!.ToString();
            tbHost.Text = Options.ServerIP;
            tbPort.Text = Options.ServerPort;
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            Options = new()
            {
                ClientName = tbName.Text,
                Guid = lblGuid.Text,
                ServerPort = tbPort.Text,
                ServerIP = tbHost.Text,
            };
            (bool okResult, string error) = await _opManager.SaveOptions(Options);
            if (okResult == false)
            {
                ShowErrorBox(error);
                return;
            }
            DialogResult = DialogResult.OK;
        }
        private void ShowErrorBox(string error)
           => MessageBox.Show(error,
                  "Error",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
    }
}
