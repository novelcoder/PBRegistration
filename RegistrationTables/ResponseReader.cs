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
                        registration.PhoneNumber = ll.ToString() ?? string.Empty;
                        break;
                    case 4:
                        registration.ShirtSize = ll.ToString() ?? string.Empty;
                        break;
                    case 5:
                        registration.RNR_MixedSkillLevel = ll.ToString() ?? string.Empty;
                        break;
                    case 6:
                        registration.RNR_MixedPartnerName = ll.ToString() ?? string.Empty;
                        break;
                    case 7:
                        registration.RNR_MixedPartnerPhoneNumber = ll.ToString() ?? string.Empty;
                        break;
                    case 8:
                        registration.RNR_MensWomensSkillLevel = ll.ToString() ?? string.Empty;
                        break;
                    case 9:
                        registration.RNR_MensWomensPartnerName = ll.ToString() ?? string.Empty;
                        break;
                    case 10:
                        registration.RNR_MensWomensPhoneNumber = ll.ToString() ?? string.Empty;
                        break;
                    case 11:
                        registration.PINKED_MixedSkillLevel = ll.ToString() ?? string.Empty;
                        break;
                    case 12:
                        registration.PINKED_MixedPartnerName = ll.ToString() ?? string.Empty;
                        break;
                    case 13:
                        registration.PINKED_MixedPartnerPhoneNumber = ll.ToString() ?? string.Empty;
                        break;
                    case 14:
                        registration.PINKED_MensWomensSkillLevel = ll.ToString() ?? string.Empty;
                        break;
                    case 15:
                        registration.PINKED_MensWomensPartnerName = ll.ToString() ?? string.Empty;
                        break;
                    case 16:
                        registration.PINKED_MensWomensPhoneNumber = ll.ToString() ?? string.Empty;
                        break;
                    case 17:
                        registration.NumberOfEvents = ll.ToString() ?? string.Empty;
                        break;
                }
                col++;
            }
            return registration;
        }
    }
}

