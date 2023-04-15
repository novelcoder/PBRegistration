using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace RegistrationTables
{
	public class Spreadsheet
	{
		private static string GoogleApplicationName { get; } = "PBTournament";       
        private static string CredentialsPath { get; } = "/users/jamesgreenwood/Projects/RegistrationTables/RegistrationTables/pbtournament-81d8134df6b8.json";
        public static string FormResponsesSheetId { get;  } = "1AA--AFHnwpLzI9LyLI68IE0JMfQrlpjX5nkaiNpTXA4";
        public static string MDMUploadSheetId { get;  } = "1lBVqkPoWgvM1mZBsrU7XWmSi8Q_lwqr1w83HPTVf5R4";
        private static string[] Scopes = { SheetsService.Scope.Spreadsheets };

        public SheetsService SheetsService { get; set; } = new SheetsService();

        public void ConnectGoogle()
        {
            SheetsService = new SheetsService();
            ConnectToGoogle();
        }

        private void ConnectToGoogle()
        {
            GoogleCredential credential;

            // Put your credentials json file in the root of the solution and make sure copy to output dir property is set to always copy 
            using (var stream = new FileStream(CredentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            // Create Google Sheets API service.
            SheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = GoogleApplicationName
            });
        }
    }

}

