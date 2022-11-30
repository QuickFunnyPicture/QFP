using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QFP.App.Windows;

public partial class CreateProjectWindow : Window
{
    public int ImageWidth { get; set; }

    public int ImageHeight { get; set; }

    public int Dpi { get; set; }

    public bool IsCancelled { get; set; } = true;

    public CreateProjectWindow()
    {
        InitializeComponent();
    }

    private void Button_Cancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
        IsCancelled = true;
    }

    private void Button_Create_Click(object sender, RoutedEventArgs e)
    {
        if (CheckValuesInTextBoxes(TextBox_Width) && CheckValuesInTextBoxes(TextBox_Height))
        {
            ImageWidth = int.Parse(TextBox_Width.Text);
            ImageHeight = int.Parse(TextBox_Height.Text);

            // Temporary value for creating image
            Dpi = 72;

            IsCancelled = false;

            Close();
        }
    }

    private void TextBox_Width_TextChanged(object sender, TextChangedEventArgs e)
    {
        CheckValuesInTextBoxes(TextBox_Width);
    }

    private void TextBox_Height_TextChanged(object sender, TextChangedEventArgs e)
    {
        CheckValuesInTextBoxes(TextBox_Height);
    }

    private bool CheckValuesInTextBoxes(TextBox textBox)
    {
        if (!string.IsNullOrEmpty(textBox.Text) && int.TryParse(textBox.Text, out int _))
        {
            textBox.BorderBrush = Brushes.Black;
            return true;
        }
        else
        {
            textBox.BorderBrush = Brushes.Red;
            return false;
        }
    }
}
