using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RepeatingQuantal
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch _stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate(this)) return;
            MessageBox.Show(((Data.Main) DataContext).Base.ToString());
            ((Data.Main) DataContext).UpdateFractions(this,10000);
            //var worker = new BackgroundWorker();
            //worker.DoWork += ((Data.Main) DataContext).UpdateFractions;
            //worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
            //worker.WorkerReportsProgress = true;
            //worker.ProgressChanged += WorkerOnProgressChanged;

            //worker.RunWorkerAsync(new List<object>(2) { worker, 10000 });
        }

        private static bool Validate(DependencyObject obj)
        {
            if (obj == null) throw new ArgumentNullException();

            var isValid = !Validation.GetHasError(obj);
            if (isValid) return LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(Validate);

            var element = obj as IInputElement;
            if (element != null) Keyboard.Focus(element);
            return false;
        }

        ////private static string FormatFactorDict(KeyValuePair<int, IEnumerable<KeyValuePair<int, int>>> kv)
        ////{
        ////    var sb = new StringBuilder();
        ////    if (kv.Value.First().Value == 1) sb.AppendFormat("{0} = {1}", kv.Key, kv.Value.First().Key);
        ////    else sb.AppendFormat("{0} = {1}^{2}", kv.Key, kv.Value.First().Key, kv.Value.First().Value);
        ////    foreach (var v in kv.Value.Skip(1))
        ////    {
        ////        if (v.Value == 1) sb.AppendFormat("*{0}", v.Key);
        ////        else sb.AppendFormat("*{0}^{1}", v.Key, v.Value);
        ////    }
        ////    return sb.ToString();
        ////}

    }
}