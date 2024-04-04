using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace RegistrationTables
{
	public class Spreadsheet
	{
		private static string GoogleApplicationName { get; } = "PBTournament";       
        private static string CredentialsPath { get; } = "/users/jamesgreenwood/Projects/PBRegistration/RegistrationTables/pbtournament-e5c050a7d4a5.json";

        public static string FormResponsesSheetId { get; } = "1-eCVYkybOakTBVGiroHBLlHPJ8HEWYlp9EdScpCFiAE";
        //public static string FormResponsesSheetId { get;  } = "1LqUrcyHJneAeFVVsYGZVPflTPwWG-ar7r8HTyc5Po8Y";
        //public static string RockNRollRallySheetId { get; } = "14jINMg4mNaV-Vwv4QmkBOqOjuDpjG3pOR6T6mvkBXhI";
        //public static string PinkedSheetId { get; set; } = "1fl-wn4q1VqUFd0sTdirsfUt0RDgm6D4sC9iLgm5PnUk";
        public static string StrokeOfLuckSheetId { get; set; } = "174GuLEliHUlIw2gpUFE8JbrUZK03NiJPvSbzh613hRk";

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

