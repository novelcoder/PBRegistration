using SheetServices;

namespace RoundRobin
{
	public class PoolReader
	{
        private SheetManager _spreadsheetManager = new SheetManager();

        public PoolReader()
        {
            _spreadsheetManager = new SheetManager();
            _spreadsheetManager.ConnectGoogle();
        }
            
        public List<string>[] ReadSheet(string sheetId)
        {
            string range = "Pool List!A1:Y32";
            var result = _spreadsheetManager.SheetsService.Spreadsheets.Values.Get(sheetId, range).Execute();
            List<string>[] brackets = new List<string>[1];

            for (int row = 0; row < result.Values.Count; row++)
            {
                var cols = result.Values[row];
                if (row == 0)
                {
                    brackets = new List<string>[cols.Count];
                }

                for (int col = 0; col < cols.Count(); col++)
                {
                    if (row == 0)
                    {
                        brackets[col] = new List<string>();
                    }

                    var val = cols[col].ToString() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(val))
                        brackets[col].Add(val);
                }
            }

            return brackets;
        }
    }
}

