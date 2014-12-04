using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace RepeatingQuantal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<int> Primes { get; set; }

        private ObservableCollection<Fraction> Fractions { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //MessageBox.Show(AdditionalMath.MultiplicativeOrder(10, 21).ToString());
            Primes = new ObservableCollection<int>(AdditionalMath.PrimesTo((int)Math.Pow(2, 16)));
            GraphListBox.ItemsSource = Primes;
        }
    }
}