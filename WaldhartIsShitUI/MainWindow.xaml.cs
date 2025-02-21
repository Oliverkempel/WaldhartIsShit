namespace WaldhartIsShitUI
{
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using WaldhartIsShit.Models.Responses;

    using WaldhartIsShit;
    using WaldhartIsShit.Models;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Waldhart Waldhart;
        Capitech Capitech;

        public CollectionViewSource obsColCourses { get; set; }
        private ICollectionView TabColViewCourses { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Waldhart = new Waldhart();
            Capitech = new Capitech();

            Waldhart.excludedCourseNames.Add("off");
            Waldhart.excludedCourseNames.Add("do not book");
            Waldhart.excludedCourseNames.Add("fri");
            Waldhart.excludedCourseNames.Add("ORIGO");

            obsColCourses = new CollectionViewSource() { Source = Waldhart.Courses };
            TabColViewCourses = obsColCourses.View;

            ResultsList.ItemsSource = TabColViewCourses;

            fromDateInput.SelectedDate = new DateTime(2025, 1, 1);
            toDateInput.SelectedDate = new DateTime(2025, 1, 31);

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Waldhart.login(UserIdInput.Text, UserIdInput.Text);
            Capitech.login("oliversbutik@gmail.com", "Okbutik29");

            DateOnly fromdate_tmp = DateOnly.FromDateTime((DateTime)fromDateInput.SelectedDate);
            DateOnly todate_tmp = DateOnly.FromDateTime((DateTime)toDateInput.SelectedDate);

            //Task.Run(() => {
                Waldhart.fetchCourses(fromdate_tmp, todate_tmp);
            //});

            
            //ResultsList.ItemsSource = Waldhart.Courses;
        }

        private void CalculateHours_Click(object sender, RoutedEventArgs e)
        {
            DateOnly fromdate_tmp = DateOnly.FromDateTime((DateTime)fromDateInput.SelectedDate);
            DateOnly todate_tmp = DateOnly.FromDateTime((DateTime)toDateInput.SelectedDate);

            TimeSpan res = Waldhart.summarizeHours(fromdate_tmp, todate_tmp);

            double hours = res.TotalHours;

            CalculatedHoursBox.Text = hours.ToString();
        }
    }
}