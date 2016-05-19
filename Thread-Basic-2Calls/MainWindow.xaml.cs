using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Thread_Basic_2Calls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // We have 2 methods that takes long time to execute 

        // Long Method 1
        public static int LongMethod1()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return 2;
        }

        // Long Method 1
        public static int LongMethod2()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return 4;
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        // =========================================================================================
        private void CallWithoutTPL(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() => {
                Func<int> f1 = new Func<int>(LongMethod1);
                Func<int> f2 = new Func<int>(LongMethod2);
                
                IAsyncResult r1 = f1.BeginInvoke(null, null);
                IAsyncResult r2 = f2.BeginInvoke(null, null);
                
                int i = f1.EndInvoke(r1);
                int j = f2.EndInvoke(r2);
                
                txtResultWithoutTPL.Dispatcher.Invoke(() => { txtResultWithoutTPL.Text = (i + j).ToString(); });

            });
            t.Start();
        }
        // =========================================================================================
        private void CallWithTPL(object sender, RoutedEventArgs e)
        {
            Task t = new Task(() => {
                // Initialize two new tasks
                var t1 = new Task<int>(LongMethod1);
                var t2 = new Task<int>(LongMethod2);

                t1.Start();
                t2.Start();

                int i = t1.Result;
                int j = t2.Result;

                txtResultWithTPL.Dispatcher.Invoke(() => { txtResultWithTPL.Text = (i + j).ToString(); });
            });
            t.Start();
        }
        // =========================================================================================
        int result;
        private void CallBackgroundWorker(object sender, RoutedEventArgs e)
        {
            Action <int> updateUI = (input) => {
                if (result == 0)
                    result = input;
                else
                    txtResultBackgroundWorker.Text = (result + input).ToString();
            };

            BackgroundWorker bw1 = new BackgroundWorker();
            BackgroundWorker bw2 = new BackgroundWorker();
            bw1.DoWork += (s, ea) => ea.Result = LongMethod1();
            bw1.RunWorkerCompleted += (s, ea) => updateUI(int.Parse(ea.Result.ToString()));
            
            bw2.DoWork += (s, ea) => ea.Result = LongMethod2();
            bw2.RunWorkerCompleted += (s, ea) => updateUI(int.Parse(ea.Result.ToString()));

            bw1.RunWorkerAsync();
            bw2.RunWorkerAsync();
        }
        // =========================================================================================
        private async void CallAsyncAwaitTPL(object sender, RoutedEventArgs e)
        {
            var t1 = Task.Factory.StartNew(() => LongMethod1());
            var t2 = Task.Factory.StartNew(() => LongMethod2());

            var i = await t1;
            var j = await t2;

            txtResultAsyncAwait.Text = (i + j).ToString();
        }

    }
}
