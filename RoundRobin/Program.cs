using Google.Apis.Sheets.v4.Data;
using RoundRobin;
using SheetServices;

Program program = new();
program.Start();

partial class Program
{
    public void Start()
    {
        Console.WriteLine("Read Spreadsheet");
        var brackets = ReadSpreadsheet(CurrentTournament.PoolSheetId);

        Console.WriteLine("Writing Brackets to Spreadsheet");
        BracketsToSpreadsheet(brackets);

        BracketPDF.WriteBrackets(brackets);
    }

    public List<string>[] ReadSpreadsheet(string sheetId)
    {
        Console.WriteLine("Loading Spreadsheet");
        var rr = new PoolReader();
        var divisions = rr.ReadSheet(sheetId);
        return divisions;
    }

    public void BracketsToSpreadsheet(List<string>[] divisions)
    {
        Console.WriteLine("Calculating Matches");
        string sheetName = "not assigned";
        var divisionName = string.Empty;
        foreach (var division in divisions)
        {
            var matches = PoolDistribution.CalcMatches(division, ref sheetName, ref divisionName);
            var divisionModel = PoolDistribution.LoadDivisionModel(matches, divisionName);

            var dataToWrite = new List<IList<object>>();
            dataToWrite.Add(new List<object>() { "" });
            dataToWrite.Add(new List<object>() { divisionName });
            Console.WriteLine($"Bracket {divisionName}");
            foreach (var pool in divisionModel.Pools)
            {
                foreach (var round in pool.Rounds)
                {
                    var row = new List<object>();
                    foreach (var match in round.Matches)
                    {
                        row.Add($"{match.LeftTeam} vs \n{match.RightTeam}");
                    }
                    dataToWrite.Add(row);
                }
                dataToWrite.Add(new List<object>() { "-" });
            }

            var sheetWriter = new SheetWriter();
            sheetWriter.EraseSheetData($"{sheetName}!A1:Y", CurrentTournament.PoolSheetId);
            sheetWriter.BulkWriteRange($"{sheetName}!A1:Y", dataToWrite, CurrentTournament.PoolSheetId);
            Console.WriteLine("Brackets Complete");
        }
    }
}