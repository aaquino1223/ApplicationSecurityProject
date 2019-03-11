using System;
using System.Collections.Generic;
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
using Microsoft.Win32;

namespace SpellChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //File path chosen
        private string _FileName;

        //TextFileButton Click Handler
        private void TextFileButton_Click(object sender, RoutedEventArgs e)
        {
            //Set FileDialog filters to only show .txt files
            OpenFileDialog win = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text Files (*.txt)|*.txt"
            };

            //Open File Dialog
            bool? result = win.ShowDialog();
            
            //File was chosen
            if (result == true)
            {
                _FileName = win.FileName;
                TextFileButton.Content = _FileName; //Button text displays path and file name

                string[] lines = System.IO.File.ReadAllLines(_FileName);

                DisplayText.Blocks.Clear();
                foreach (string line in lines)
                {
                    DisplayText.Blocks.Add(new Paragraph(new Run(line)) { Margin = new Thickness(0) }); //Add a paragraph to FlowDocument for each line
                }

                Save.IsEnabled = true; //Turn on save button to save corrections made
            }
        }

        //Save Button Click EventHandler
        //Get all paragraphs, extract a line per paragraph, and write back to file
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            List<Paragraph> paragraphs = DisplayText.Blocks.OfType<Paragraph>().ToList();
            List<string> lines = new List<string>();

            foreach (Paragraph paragraph in paragraphs)
            {
                TextRange line = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
                lines.Add(line.Text);
            }

            System.IO.File.WriteAllLines(_FileName, lines.ToArray());
        }
    }
}
