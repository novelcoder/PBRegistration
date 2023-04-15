using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Newtonsoft.Json;

namespace RegistrationTables
{
	public class SheetWriter
    {
        private Spreadsheet _spreadsheet = new Spreadsheet();

        public SheetWriter()
        {
            _spreadsheet = new Spreadsheet();
            _spreadsheet.ConnectGoogle();
        }

        public string BulkWriteRange(string range, List<IList<object>> data)
        {
            string valueInputOption = "USER_ENTERED";

            // The new values to apply to the spreadsheet.
            var updateData = new List<Google.Apis.Sheets.v4.Data.ValueRange>();
            var dataValueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
            dataValueRange.Range = range;
            dataValueRange.Values = data;
            updateData.Add(dataValueRange);

            var requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
            requestBody.ValueInputOption = valueInputOption;
            requestBody.Data = updateData;

            var request = _spreadsheet.SheetsService.Spreadsheets
                                .Values.BatchUpdate(requestBody, Spreadsheet.MDMUploadSheetId);

            Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            return JsonConvert.SerializeObject(response);
        }

        internal void EraseSheetData(string range)
        {
            var data = new List<IList<object>>();
            for (int row = 0; row < 200; row++)
            {
                var xxx = new List<object>();
                for (int col = 0; col < 22; col++)
                {
                    xxx.Add(string.Empty);
                }
                data.Add(xxx);
            }
            BulkWriteRange(range, data);
        }
    }
}

