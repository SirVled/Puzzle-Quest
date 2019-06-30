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
using System.Windows.Shapes;

namespace Puzzle_Quest__AnimeEdition_
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public  partial class MainMenu : Window
    {

        
        public MainMenu()
        {
            InitializeComponent();
            Main.Background = new ImageBrush(new BitmapImage(new Uri("images/Pazzle.png", UriKind.Relative)));
            
        }

        

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FastFight_Click(object sender, RoutedEventArgs e)
        {
            FastFight a = new FastFight();

            a.setCreatingForm = this;

            a.ShowDialog();
        }
        
      
    }
}
