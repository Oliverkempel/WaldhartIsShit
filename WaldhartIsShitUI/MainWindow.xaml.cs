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

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaldhartIsShit.WaldhartRequestHandler ReqHandler;

        public MainWindow()
        {
            InitializeComponent();

            ReqHandler = new WaldhartIsShit.WaldhartRequestHandler();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ReqHandler.login(UserIdInput.Text, UserIdInput.Text);

            string? result = ReqHandler.fetchCoursesFromDateRange(new DateOnly(2025, 2, 9), new DateOnly(2025, 2, 25));
            List<GetCoursesForecastResponse> resp = WaldhartDeserializer.ConvertGetCoursesForecastResponse(result);


            List<GetCourseInfoResponse> courseDataResps = new List<GetCourseInfoResponse>();
            List<Course> courses = new List<Course>();


            foreach (var curResp in resp) {
                GetCourseInfoResponse courseDataResp = new GetCourseInfoResponse();
                if (curResp.CourseId != 0 || curResp.JournalId != 0)
                {
                    string dataResponse = ReqHandler.fetchCourseData(curResp.JournalId, curResp.CourseId, curResp.CourseDate_Converted);
                    courseDataResp = WaldhartDeserializer.ConvertGetCourseDataResponse(dataResponse);
                    courseDataResp.Title = curResp.Data01;
                    courseDataResp.courseDate = curResp.CourseDate;
                }
                else
                {
                    courseDataResp.courseDate = curResp.CourseDate.ToString();
                    courseDataResp.Title = curResp.Data01;
                    courseDataResp.courseTime = curResp.Data04;
                }

                courseDataResps.Add(courseDataResp);
                string[] splittTimeSpanString = courseDataResp.courseTime.Split("-");
                string fromStr = splittTimeSpanString[0];
                string toStr = splittTimeSpanString[1];

                TimeOnly startTimeConverted = TimeOnly.Parse(fromStr.Trim());
                TimeOnly endTimeConverted = TimeOnly.Parse(toStr.Trim());

                courses.Add(new Course { Title = courseDataResp.Title, Date = curResp.CourseDate_Converted, startTime = startTimeConverted, endTime = endTimeConverted});
            }



            ResultsList.ItemsSource = courses;
            //ResultsList.ItemsSource = courseDataResps;

        }
    }
}