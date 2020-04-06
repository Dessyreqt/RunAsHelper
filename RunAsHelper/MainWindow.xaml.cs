using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using RunAsHelper.Properties;

namespace RunAsHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> _usernamePasswords;
        private ObservableCollection<string> _usernameKeys;

        private Dictionary<string, string> _programs;
        private ObservableCollection<string> _programsKeys;

        public MainWindow()
        {
            InitializeComponent();
            _usernamePasswords = JsonConvert.DeserializeObject<Dictionary<string, string>>(Settings.Default.UsernamePasswords) ?? new Dictionary<string, string>();
            _programs = JsonConvert.DeserializeObject<Dictionary<string, string>>(Settings.Default.Programs) ?? new Dictionary<string, string>();
            
            UpdateUsernameKeys();
            UpdateProgramsKeys();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();

            Clipboard.SetData(DataFormats.Text, $"{PasswordTextBox.Text}{Environment.NewLine}");

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "C:\\Windows\\System32\\runas.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = $"/netonly /user:\"{UsernameComboBox.Text}\" {PathTextBox.Text}";

            Process.Start(startInfo);
        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveSettings();
        }

        private void PathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (!string.IsNullOrWhiteSpace(UsernameComboBox.Text))
            {
                _usernamePasswords[UsernameComboBox.Text] = PasswordTextBox.Text;
            }

            if (!string.IsNullOrWhiteSpace(ApplicationComboBox.Text))
            {
                _programs[ApplicationComboBox.Text] = PathTextBox.Text;
            }

            Settings.Default.UsernamePasswords = JsonConvert.SerializeObject(_usernamePasswords);
            UpdateUsernameKeys();

            Settings.Default.Programs = JsonConvert.SerializeObject(_programs);
            UpdateProgramsKeys();

            Settings.Default.Save();
        }

        private void UsernameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsernameComboBox.SelectedItem == null)
            {
                return;
            }

            UsernameComboBox.Text = UsernameComboBox.SelectedItem.ToString();
            PasswordTextBox.Text = _usernamePasswords[UsernameComboBox.Text];
        }

        private void ApplicationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ApplicationComboBox.SelectedItem == null)
            {
                return;
            }

            ApplicationComboBox.Text = ApplicationComboBox.SelectedItem.ToString();
            PathTextBox.Text = _programs[ApplicationComboBox.Text];
        }

        private void RemoveUsernameButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameComboBox.Text))
            {
                return;
            }

            var valueToRemove = UsernameComboBox.Text;

            if (!_usernamePasswords.ContainsKey(valueToRemove))
            {
                return;
            }

            UsernameComboBox.Text = string.Empty;
            PasswordTextBox.Text = string.Empty;

            _usernamePasswords.Remove(valueToRemove);
            SaveSettings();
        }

        private void UpdateUsernameKeys()
        {
            _usernameKeys = new ObservableCollection<string>(_usernamePasswords.Keys.OrderBy(x => x));
            UsernameComboBox.ItemsSource = _usernameKeys;
        }

        private void UpdateProgramsKeys()
        {
            _programsKeys = new ObservableCollection<string>(_programs.Keys.OrderBy(x => x));
            ApplicationComboBox.ItemsSource = _programsKeys;
        }

        private void RemoveApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ApplicationComboBox.Text))
            {
                return;
            }

            var valueToRemove = ApplicationComboBox.Text;

            if (!_programs.ContainsKey(valueToRemove))
            {
                return;
            }

            ApplicationComboBox.Text = string.Empty;
            PathTextBox.Text = string.Empty;

            _programs.Remove(valueToRemove);
            SaveSettings();
        }
    }
}
