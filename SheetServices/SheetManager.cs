using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace SheetServices
{
	public class SheetManager
	{
		private static string GoogleApplicationName { get; } = "PBTournament";       
        private static string CredentialsPath { get; } = "/users/jamesgreenwood/Projects/PBRegistration/RegistrationTables/pbtournament-e5c050a7d4a5.json";
        
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

