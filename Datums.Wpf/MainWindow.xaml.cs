using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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

namespace Datums.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime opstartMoment;
        DateTime opstartDatum;
        int interval = 1000;
        Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            opstartMoment = DateTime.Now;
            opstartDatum = new DateTime(opstartMoment.Year, opstartMoment.Month, opstartMoment.Day);
            dtDatum.SelectedDate = opstartDatum;
            ToonTijd(opstartMoment);
            StartTimer();
        }

        #region Gebruik van de timer
        void StartTimer()
        {
            timer = new Timer();
            timer.Interval = interval;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ToonTijd(DateTime.Now);
                ToonActief(DateTime.Now);
            });
        }

        void ToggleTimer()
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
            }
            else
            {
                timer.Enabled = true;
            }
        } 
        #endregion

        void ToonTijd(DateTime moment)
        {
            lblTijdsAanduiding.Content = moment.ToString("d MMMM yyyy HH:mm:ss");
        }

        void ToonActief(DateTime moment)
        {
            TimeSpan verschil = moment - opstartMoment;
            //In onderstaand statement worden de backslashes gebruikt om aan te duiden dat 
            //het volgende character letterlijk weergegeven moet worden
            string tijdVerlopen = verschil.ToString(@"d\ \d\a\g\e\n\ hh\:mm\:ss");
            lblTimer.Content = "Het programma is reeds " + tijdVerlopen + " actief";
        }

        string Leeftijd(DateTime van, DateTime tot)
        {
            string verschil = "";

            //Hieronder zie je een manier om het verschil tussen twee datums te berekenen
            int getalVan = int.Parse(van.ToString("yyyyMMdd"));
            int getalTot = int.Parse(tot.ToString("yyyyMMdd"));
            //In de gehele deling hieronder verdwijnen de cijfers na de komma.
            int jarenVerschil = (getalTot - getalVan) / 10000;

            if (jarenVerschil > 0)
            {
                verschil = jarenVerschil + " jaar geleden";
            }
            else if (jarenVerschil == 0)
            {
                if (getalVan > getalTot)
                {
                    verschil = "binnen het jaar";
                }
                else if (getalVan == getalTot)
                {
                    verschil = "vandaag";
                }
                else
                {
                    verschil = "minder dan een jaar geleden";
                }
            }
            else
            {
                verschil = "Over " + jarenVerschil * -1 + " jaar";
            }

            return verschil;
        }

        string DagenVerschil(DateTime van, DateTime tot)
        {
            string verschil = "";

            //Hieronder een manier om het verschil in dagen te berekenen.
            int dagenVerschil;
            van = new DateTime(van.Year, van.Month, van.Day);
            tot = new DateTime(tot.Year, tot.Month, tot.Day);
            TimeSpan tijdsVerschil = tot - van;
            dagenVerschil = tijdsVerschil.Days;

            //Je zou ook de methode kunnen gebruiken zoals bij de jaren.
            //int dagenVerschil = (int.Parse(tot.ToString("yyyyMMdd")) - int.Parse(van.ToString("yyyyMMdd")));

            if (dagenVerschil > 0)
            {
                verschil = dagenVerschil + " dag(en) geleden";
            }
            else if (dagenVerschil == 0)
            {
                verschil = "zelfde dag";
            }
            else
            {
                verschil = "Binnen " + dagenVerschil * -1 + " dag(en)";
            }

            return verschil;
        }

        private void dtDatum_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime geselecteerdeDag = (DateTime)dtDatum.SelectedDate;
            tbFeedback.Text = DagenVerschil(geselecteerdeDag, opstartMoment) + "\n";
            tbFeedback.Text += Leeftijd(geselecteerdeDag, opstartMoment);
        }

        private void btnStartStopTimer_Click(object sender, RoutedEventArgs e)
        {
            ToggleTimer();
        }

    }
}
