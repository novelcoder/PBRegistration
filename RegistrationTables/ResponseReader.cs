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
            string range = "Form Responses 1!A1:Y300";
            var result = _spreadsheet.SheetsService.Spreadsheets.Values.Get(Spreadsheet.FormResponsesSheetId, range).Execute();
            var values = result.Values;
            var records = new List<Registration>();

            for (int row = 1; row < result.Values.Count; row++)
            {
                var record = ParseRow(result.Values[row]);
                if (!string.IsNullOrEmpty(record.Timestamp)
                  && record.Remove.ToLower().Trim() != "withdraw"
                  && record.Remove.ToLower().Trim() != "remove")
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
                        registration.MixedSkillLevel = ll.ToString() ?? string.Empty;
                        break;
                    case 6:
                        registration.MixedPartnerName = ll.ToString() ?? string.Empty;
                        break;
                    case 7:
                        registration.MixedPartnerPhoneNumber = ll.ToString() ?? string.Empty;
                        break;
                    case 8:
                        registration.MensWomensSkillLevel = ll.ToString() ?? string.Empty;
                        break;
                    case 9:
                        registration.MensWomensPartnerName = ll.ToString() ?? string.Empty;
                        break;
                    case 10:
                        registration.MensWomensPhoneNumber = ll.ToString() ?? string.Empty;
                        break;
                    case 11:
                        registration.NeedAPartner = ll.ToString() ?? string.Empty;
                        break;
                    case 12:
                        registration.NumberOfEvents = ll.ToString() ?? string.Empty;
                        break;
                    case 13:
                        registration.Remove = ll.ToString() ?? string.Empty;
                        break;
                }
                col++;
            }
            return registration;
        }
    }
}

