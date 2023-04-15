using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace RegistrationTables
{
	public class ReadPayments
    {
        private Spreadsheet _spreadsheet = new Spreadsheet();

        public ReadPayments()
        {
            _spreadsheet = new Spreadsheet();
            _spreadsheet.ConnectGoogle();
        }

        public List<Payment> ReadSpreadsheet()
        {
            var result = new List<Payment>();

            string range = "Payments!A1:Y200";
            var data = _spreadsheet.SheetsService.Spreadsheets.Values.Get(Spreadsheet.MDMUploadSheetId, range).Execute();

            for (int iii = 0; iii < data.Values.Count; iii++)
            {
                var cols = data.Values[iii];
                var paymentRow = new Payment
                {
                    Name = cols[0].ToString() ?? string.Empty,
                    Division = cols[1].ToString() ?? string.Empty,
                    Due = cols[2].ToString() ?? string.Empty,
                    Paid = cols[3].ToString() ?? string.Empty,
                    ShirtSize = cols[4].ToString() ?? string.Empty,
                    Email = cols[5].ToString() ?? string.Empty
                };
                result.Add(paymentRow);
            }

            return result;
        }
    }
}

