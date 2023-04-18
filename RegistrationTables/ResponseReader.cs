using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RegistrationTables
{
	public class ResponseReader
    {
        private Spreadsheet _spreadsheet = new Spreadsheet();

        public ResponseReader()
        {
            _spreadsheet = new Spreadsheet();
            _spreadsheet.ConnectGoogle();
        }

        public List<Registration> ReadSheet()
        {
            string range = "Form Responses 1!A1:Y200";
            var result = _spreadsheet.SheetsService.Spreadsheets.Values.Get(Spreadsheet.FormResponsesSheetId, range).Execute();
            var values = result.Values;
            var records = new List<Registration>();

            for (int row = 1; row < result.Values.Count; row++)
            {
                var record = ParseRow(result.Values[row]);
                if (!string.IsNullOrEmpty(record.Timestamp))
                    records.Add(record);
            }

            return records;
        }

        private Registration ParseRow(IList<object> list)
        {
            int col = 0;
            var registration = new Registration();
            foreach ( var ll in list)
            {
                switch (col)
                {
                    case 0: //timestamp
                        registration.Timestamp = ll.ToString() ?? string.Empty;
                        break;
                    case 1:
                        registration.Email = ll.ToString() ?? string.Empty;
                        break;
                    case 2:
                        registration.Name = ll.ToString() ?? string.Empty;
                        break;
                    case 3:
                        registration.NotEmail = ll.ToString() ?? string.Empty;
                        break;
                    case 4:
                        registration.PhoneNumber = ll.ToString() ?? string.Empty;
                        break;
                    case 5:
                        registration.MensBracket = ll.ToString() ?? string.Empty;
                        break;
                    case 6:
                        registration.MixedPartnerName = ll.ToString() ?? string.Empty;
                        break;
                    case 7:
                        registration.WomensPartnerName = ll.ToString() ?? string.Empty;
                        break;
                    case 8:
                        registration.ShirtSize = ll.ToString() ?? string.Empty;
                        break;
                    case 9:

                        registration.WomensBracket = ll.ToString() ?? string.Empty;
                        break;
                    case 10:

                        registration.MixedBracket = ll.ToString() ?? string.Empty;
                        break;
                    case 11:

                        registration.MixedPartnerShirtSize = ll.ToString() ?? string.Empty;
                        break;
                    case 12:
                        registration.GenderPartnerShirtSize = ll.ToString() ?? string.Empty;
                        break;
                    case 13:

                        registration.MensPartnerName = ll.ToString() ?? string.Empty;
                        break;
                    case 14:

                        registration.MixedPartnerPhone = ll.ToString() ?? string.Empty;
                        break;
                    case 15:

                        registration.WomensPartnerPhone = ll.ToString() ?? string.Empty;
                        break;
                    case 16:

                        registration.MensPartnerPhone = ll.ToString() ?? string.Empty;
                        break;
                    case 17:

                        registration.MixedPartnerEmail = ll.ToString() ?? string.Empty;
                        break;
                    case 18:

                        registration.WomensPartnerEmail = ll.ToString() ?? string.Empty;
                        break;
                    case 19:
                        registration.MensPartnerEmail = ll.ToString() ?? string.Empty;
                        break;
                }
                col++;
            }
            return registration;
        }
    }
}

