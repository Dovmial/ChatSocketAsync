

namespace SocketClientWF
{

    public partial class AuthorizationForm : Form
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        private bool IsRegistration { get; set; }

        public AuthorizationForm(bool isRegistration)
        {
            InitializeComponent();
            ResetProperties();
            if (isRegistration == false)
            {
                Height = 182;
            }
            tbRepeatPass.Visible = lblRepeat.Visible = isRegistration;
            IsRegistration = isRegistration;
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbLogin.Text))
            {
                ShowErrorBox("Login is empty");
                return;
            }
            Login = tbLogin.Text;

            if (string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                ShowErrorBox("Password is empty!");
                return;
            }
            Password = tbPassword.Text;
            if (IsRegistration && tbRepeatPass.Text != Password)
            {
                ShowErrorBox("Passwords is not equal!");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            ResetProperties();
        }

        private void ResetProperties()
        {
            Login = Password = string.Empty;
            tbRepeatPass.Text = tbLogin.Text = tbPassword.Text = string.Empty;
        }
        private void ShowErrorBox(string error)
            => MessageBox.Show(error,
                   "Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
    }
}
