using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dictionary
{
    public class SqlConnector
    {
        private const string db = "Dictionary";

        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
        public List<Word> GetPolishWord_All()
        {
            List<Word> output = new List<Word>();

            try
            {
                using (IDbConnection connection = new SqlConnection(CnnString(db)))
                {
                    output = connection.Query<Word>("dbo.spPolishWords_GetAll").ToList();
                }
            }
            catch (Exception)
            {
                if (MessageBox.Show("Błąd połączenia z bazą danych", "Błąd", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }

            output.ForEach(w => w.Language = SelectedLanguage.Polish);

            return output;
        }

        public List<Word> GetEnglishWord_All()
        {
            List<Word> output = new List<Word>();
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(CnnString(db)))
                {
                    output = connection.Query<Word>("dbo.spEnglishWords_GetAll").ToList();
                }
            }
            catch (Exception)
            {
                if (MessageBox.Show("Błąd połączenia z bazą danych", "Błąd", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }

            output.ForEach(w => w.Language = SelectedLanguage.English);

            return output;
        }

        public List<Translations> GetTranslation_All()
        {
            List<Translations> output = new List<Translations>();
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(CnnString(db)))
                {
                    output = connection.Query<Translations>("dbo.spTranslations_GetAll").ToList();
                }
            }
            catch (Exception)
            {
                if (MessageBox.Show("Błąd połączenia z bazą danych", "Błąd", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }

            return output;
        }
    }
}
