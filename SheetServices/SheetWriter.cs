using Newtonsoft.Json;

namespace SheetServices
{
	public class SheetWriter
    {
        private SheetManager _sheetManager = new SheetManager();

        public SheetWriter()
        {
            _sheetManager = new SheetManager();
            _sheetManager.ConnectGoogle();
        }

        public string BulkWriteRange(string range, List<IList<object>> data, string sheetId)
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

            var request = _sheetManager.SheetsService.Spreadsheets
                                .Values.BatchUpdate(requestBody, sheetId);

            Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            return JsonConvert.SerializeObject(response);
        }

        public void EraseSheetData(string range, string sheetId)
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
            BulkWriteRange(range, data, sheetId);
        }
    }
}

