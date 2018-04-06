using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace Result_of_run
{
    public class Result
    {
        public string Marathon { get; set; }
        public string Type { get; set; }
        public string Time { get; set; }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataTable ResultDT;
            string Email = "ester@gmail.com";
            SqlConnection con;
            List<Result> ResultData;
            ResultDT = new DataTable();
            ResultData = new List<Result>();
            string strcon = ConfigurationManager.ConnectionStrings["DB_ALL"].ConnectionString;
            con = null;
            try
            {
                con = new SqlConnection(strcon);
                using (SqlCommand GetResults = new SqlCommand())
                {
                    GetResults.Connection = con;
                    GetResults.CommandText = "SELECT c.CountryName, m.YearHeld, et.EventTypeName, re.RaceTime FROM Event e JOIN EventType et ON et.EventTypeId = e.EventTypeId JOIN Marathon m ON m.MarathonId = e.MarathonId JOIN Country c ON c.CountryCode = m.CountryCode JOIN RegistrationEvent re ON e.EventId = re.EventId JOIN Registration r ON r.RegistrationId = re.RegistrationId JOIN  Runner ru ON ru.RunnerId = r.RunnerId WHERE ru.Email = @Email";
                    GetResults.Parameters.AddWithValue("@Email", Email);
                    using (SqlDataAdapter sda = new SqlDataAdapter(GetResults))
                    {
                        sda.Fill(ResultDT);
                    }
                }

                foreach(DataRow row in ResultDT.Rows)
                {

                    ResultData.Add(new Result
                    {
                        Marathon = row["YearHeld"].ToString() + " " + row["CountryName"].ToString(),
                        Type = row["EventTypeName"].ToString(),
                        Time = (Convert.ToInt32(row["RaceTime"]) / 3600).ToString() + " h " + (Convert.ToInt32(row["RaceTime"]) / 60 %60).ToString() + " m " + (Convert.ToInt32(row["RaceTime"]) % 60).ToString() + " s "
                    }
                 );
                }
                ResultTable.ItemsSource = ResultData;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
            }

        }


    }
}
