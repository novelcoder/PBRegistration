
using System;
namespace RegistrationTables
{
	public class PartnerEmailReader
    {
        private Spreadsheet _spreadsheet = new Spreadsheet();

        public PartnerEmailReader()
        {
            _spreadsheet = new Spreadsheet();
            _spreadsheet.ConnectGoogle();
        }

        public List<PartnerEmail> ReadSpreadsheet()
        {
            var result = new List<PartnerEmail>();

            string range = "PartnerEmail Save!A1:Y200";
            var data = _spreadsheet.SheetsService.Spreadsheets.Values.Get(Spreadsheet.MDMUploadSheetId, range).Execute();

            for (int iii = 0; iii < data.Values.Count; iii++)
            {
                var cols = data.Values[iii];
                var emailRow = new PartnerEmail
                {
                    CombinedName = cols[0].ToString() ?? string.Empty,
                    Division = cols[1].ToString() ?? string.Empty,
                    PlayerDue = cols[2].ToString() ?? string.Empty,
                    PartnerDue = cols[3].ToString() ?? string.Empty,
                    Emails  = cols[4].ToString() ?? string.Empty
                };

                if (cols.Count() > 5)
                    emailRow.PlayerPaid = cols[5].ToString() ?? string.Empty;
                if (cols.Count() > 6)
                    emailRow.PartnerPaid = cols[6].ToString() ?? string.Empty;
                
                result.Add(emailRow);
            }

            return result;
        }
    }
}

