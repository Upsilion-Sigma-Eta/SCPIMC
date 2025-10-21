using System.Windows;
using SCPIMCMain.ViewModel.Controls;

namespace SCPIMCMain.View.Controls
{
    /// <summary>
    /// Interaction logic for MacroEditorDialog.xaml
    /// </summary>
    public partial class MacroEditorDialog : Window
    {
        public MacroEditorViewModel ViewModel { get; private set; }
        public bool DialogResultSuccess { get; private set; }

        public MacroEditorDialog(MacroEditorViewModel __viewModel)
        {
            InitializeComponent();

            if (__viewModel == null)
            {
                throw new ArgumentNullException(nameof(__viewModel));
            }

            ViewModel = __viewModel;
            DataContext = ViewModel;
            DialogResultSuccess = false;
        }

        private void SaveButton_Click(object __sender, RoutedEventArgs __e)
        {
            try
            {
                // Validate macro name
                if (string.IsNullOrWhiteSpace(ViewModel.MacroName))
                {
                    MessageBox.Show("매크로 이름을 입력해주세요.", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validate at least one command
                if (ViewModel.Commands.Count == 0)
                {
                    MessageBox.Show("최소 하나 이상의 명령어를 추가해주세요.", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DialogResultSuccess = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"저장 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object __sender, RoutedEventArgs __e)
        {
            DialogResultSuccess = false;
            this.Close();
        }
    }
}
