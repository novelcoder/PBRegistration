using Google.Apis.Sheets.v4.Data;
using RoundRobin;
using SheetServices;

Program program = new();
program.Start();

partial class Program
{
    public void Start()
    {
        Console.WriteLine("start reader");

        var rr = new PoolReader();
        var brackets = rr.ReadSheet();

        Console.WriteLine("Calculating Matches");
        string sheetName = "not assigned";
        var bracketName = string.Empty;
        foreach (var bracket in brackets)
        {
            var pools = PoolDistribution.CalcMatches(bracket, ref sheetName, ref bracketName);

            var dataToWrite = new List<IList<object>>();
            dataToWrite.Add(new List<object>() { "" });
            dataToWrite.Add(new List<object>() { bracketName });
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
        }
    }
}