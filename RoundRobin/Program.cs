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
        var brackets = ReadSpreadsheet();

        Console.WriteLine("Writing Brackets to Spreadsheet");
        BracketsToSpreadsheet(brackets);

        BracketPDF.WriteBrackets(brackets);
    }

    public List<string>[] ReadSpreadsheet()
    {
        Console.WriteLine("Loading Spreadsheet");
        var rr = new PoolReader();
        var brackets = rr.ReadSheet();
        return brackets;
    }

    public void BracketsToSpreadsheet(List<string>[] brackets)
    {

        Console.WriteLine("Calculating Matches");
        string sheetName = "not assigned";
        var bracketName = string.Empty;
        foreach (var bracket in brackets)
        {
            var pools = PoolDistribution.CalcMatches(bracket, ref sheetName, ref bracketName);

            var dataToWrite = new List<IList<object>>();
            dataToWrite.Add(new List<object>() { "" });
            dataToWrite.Add(new List<object>() { bracketName });
            Console.WriteLine($"Bracket {bracketName}");
            foreach (var pool in pools)
            {
                foreach (var round in pool)
                {
                    var row = new List<object>();
                    foreach (var match in round)
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