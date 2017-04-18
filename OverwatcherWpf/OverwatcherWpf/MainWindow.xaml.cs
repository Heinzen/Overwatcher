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
using LiveCharts;
using LiveCharts.Wpf;
using Overwatcher;
using System.ComponentModel;

namespace Overwatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        BackgroundWorker bgWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            SpinnerIcon.Visibility = Visibility.Visible;
            PlayerInformation initializer = new PlayerInformation(  PlatformInputDropDown.SelectionBoxItem.ToString(), RegionInputDropDown.SelectionBoxItem.ToString(), BattleTagInputBox.Text);
            runBackgroundWorkerForSpinner();
        }

        private void runBackgroundWorkerForSpinner()
        {
            bgWorker.DoWork += bgWorker_DoWork;
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.RunWorkerAsync();
        }

        private void bgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            PlayerDataHandler.UpdateAll();    
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                MessageBox.Show("Canceled.");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else
            {
                SpinnerIcon.Visibility = Visibility.Hidden;
                string rankImagePath = "/Assets/Images/OW_" + PlayerInformation.PlayerRank + ".png";
                PlayerNicknameText.Text = PlayerInformation.Nickname;
                CurrentSrText.Text = PlayerInformation.CurrentSkillRating.ToString();
                PlayerLevelRank.Source = new BitmapImage(new Uri(PlayerInformation.PrestigeRankUrl, UriKind.Absolute));
                PlayerLevelBorder.Source = new BitmapImage(new Uri(PlayerInformation.PrestigeBorderUrl, UriKind.Absolute));
                CurrentLevelText.Text = PlayerInformation.CurrentLevel.ToString();
            }
        }
    }
}
