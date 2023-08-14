using CsvHelper;
using System.Globalization;
using RegistrationTables;
using System.IO;
using System.Text;

Program program = new();
program.Start();


partial class Program
{
    public void Start()
    {
        Console.WriteLine("start reader");

        var rr = new ResponseReader();
        var sheetWriter = new SheetWriter();

        var records = rr.ReadSheet();
        var people = Registration.Parse(records);
        Console.WriteLine($"{records.Count} records. {people.Count} people.");
        foreach ( var person in people)
        {
            Console.WriteLine($"\t{person.Name} number events {person.Events.Count}");
            foreach ( var evt in person.Events)
            {
                Console.WriteLine($"\t\t{evt.PartnerName}\t{evt.Tournament}\t{evt.DivisionLevel}\t{evt.EventType}");
            }
        }
        Console.WriteLine("parsing complete. [enter] to continue.");
        Console.ReadLine();

        //var paymentReader = new ReadPayments();
        //var payments = paymentReader.ReadSpreadsheet();

        //List<IList<object>> dataToWrite = null;

        //// Division reports
        //Console.WriteLine("Division Reports");
        //IndividualDivisionReports(sheetWriter, people);

        //// Shirts Report
        //Console.WriteLine("Shirts Report");
        //    ShirtReport(sheetWriter, people);

        ////All Division Reports
        //Console.WriteLine("All Division Report");
        //dataToWrite = AllDivisionReport(people);
        //sheetWriter.EraseSheetData("ALL Divisions!A1:Y");
        //sheetWriter.BulkWriteRange("ALL Divisions!A1:Y", dataToWrite);

        //// Full Data
        //Console.WriteLine("Full Data Report");
        //dataToWrite = FullDataReport(people);
        //sheetWriter.EraseSheetData("FullData!A1:Y");
        //sheetWriter.BulkWriteRange("FullData!A1:Y", dataToWrite);

        ////Check In Report
        //Console.WriteLine("Check In Report");
        //dataToWrite = CheckInReport(people);
        //sheetWriter.EraseSheetData("Check In!A1:Y");
        //sheetWriter.BulkWriteRange("Check In!A1:Y", dataToWrite);

        ////Partner Email
        //Console.WriteLine("PartnerEmail Report");
        //dataToWrite = PartnerEmailReport.PartnerEmail(people, payments);
        //sheetWriter.EraseSheetData("PartnerEmail!A1:Y");
        //sheetWriter.BulkWriteRange("PartnerEmail!A1:Y", dataToWrite);

        ////MailChimp Upload
        //Console.WriteLine("MailChimp Upload");
        //dataToWrite = MailChimpUpload(people);
        //sheetWriter.EraseSheetData("MailChimp!A1:Y");
        //sheetWriter.BulkWriteRange("MailChimp!A1:Y", dataToWrite);

        ////Payments
        //Console.WriteLine("Payments Report");
        //dataToWrite = PaymentsReport(people, payments);
        //sheetWriter.BulkWriteRange($"Payments!A{payments.Count() + 1}", dataToWrit
        Console.WriteLine("Done");
    }

    private static List<IList<object>> PaymentsReport(List<Person> people, List<Payment> payments)
    {
        var missingPeople = new List<Person>();
        foreach (var person in people)
        {
            if (payments.FirstOrDefault(x => x.Name == person.Name) == null)
            {
                missingPeople.Add(person);
            }
        }

        return MissingPeopleData(missingPeople);
    }

    private static List<IList<object>> MailChimpUpload(List<Person> people)
    {
        var result = new List<IList<object>>();
        var colData = new List<object>();

        result.Add( new List<object>() { "email","first","last","phone","tags" } );
        foreach (var person in people)
        {
            string[] names = person.Name.Split();
            string firstName=string.Empty;
            string last=string.Empty;
            if (names.Count()>=1)
                firstName = names[0]; 
            if (names.Count() >= 2)
                last = names[1];

            var sb = new StringBuilder();
            var first = true;
            foreach (var evt in person.Events)
            {
                if (first)
                    first = false;
                else
                    sb.Append(",");
                sb.AppendFormat($"MDM23_{evt.DivisionLevel.ToString()}_{evt.EventType.ToString()}");
            }

            colData = new List<object>();
            colData.Add(person.Email);
            colData.Add(firstName);
            colData.Add(last);
            colData.Add($"'{person.PhoneNumber}");
            colData.Add($"MDM23,{sb.ToString()}");
            result.Add(colData);
        }

        return result;
    }

    private static List<IList<object>> MissingPeopleData(List<Person> people)
    {
        var result = new List<IList<object>>();
        var colData = new List<object>();

        foreach ( var person in people)
        {
            colData = new List<object>();
            colData.Add(person.Name);
            colData.Add(Event.DisplayList(person.Events));
            colData.Add(Event.Due(person.Events));
            colData.Add(string.Empty);
            colData.Add(string.Empty);
            colData.Add(string.Empty);
            colData.Add(person.ShirtSize);
            colData.Add(person.Email);
            result.Add(colData);
        }

        return result;
    }

    private static List<IList<object>> CheckInReport(List<Person> people)
    {
        var result = new List<IList<object>>();
        var colData = new List<object>();
        //
        // Check In Report
        //
        people.Sort((x, y) => x.Name.CompareTo(y.Name));
        // day one
        colData.Add("Check In Report");
        result.Add(colData);
        colData = new List<object>() { string.Empty };
        result.Add(colData);

        foreach (var person in people)
        {
                colData = new List<object>();
                colData.Add($"{person.Name}");
                colData.Add(Event.DisplayList(person.Events));
                colData.Add(Event.Due(person.Events));
                colData.Add(person.Email);
                colData.Add(person.ShirtSize);
                result.Add(colData);
        }
        return result;
    }

    private static void ShirtReport(SheetWriter writer, List<Person> people, string sheetId)
    {
        var result = new List<IList<object>>();
        var colData = new List<object>();
        var shirts = new Dictionary<string, int>();

        colData.Add("Shirt Size");
        colData.Add("Count");
        result.Add(colData);

        foreach ( var person in people)
        {
            if (shirts.ContainsKey(person.ShirtSize))
                shirts[person.ShirtSize]++;
            else
                shirts[person.ShirtSize] = 1;
        }
        foreach ( var key in shirts.Keys)
        {
            colData = new List<object>();
            colData.Add(key);
            colData.Add(shirts[key].ToString());
            result.Add(colData);
        }
        colData = new List<object>();
        colData.Add(string.Empty);
        colData.Add($"=sum(B1:B{result.Count()})");
        result.Add(colData);
        writer.EraseSheetData("Shirts!A1:Y", sheetId);
        writer.BulkWriteRange("Shirts!A1:Y", result, sheetId);
    }

    private static void IndividualDivisionReports(SheetWriter writer, List<Person> people, string sheetId)
    {
        foreach (int evtType in Enum.GetValues(typeof(EventType)))
        {
            foreach (int blvl in Enum.GetValues(typeof(DivisionLevel)))
            {
                var list = Person.DivisionList(people, (EventType)evtType, (DivisionLevel)blvl);
                if (list.Count > 0)
                {
                    var eType = (EventType)evtType;
                    var divisionLevel = (DivisionLevel)blvl;
                    var divisionData = Person.PrintDivision(list, eType, divisionLevel);
                    var tabName = Event.DivisionTabName(eType, divisionLevel);
                    writer.EraseSheetData(tabName, sheetId);
                    writer.BulkWriteRange(tabName, divisionData, sheetId);
                }
            }
        }
    }


    //
    // FullData
    //
    private static List<IList<object>> FullDataReport(List<Person> people)
    {
        var result = new List<IList<object>>();

        result.Add(new List<object> { "Name", "Email", "Phone", "Due", "Event 1", "Partner", "Event 2", "Partner" });

        foreach (var person in people)
        {
            var row = new List<object>();

            row.Add(person.Name);
            row.Add(person.Email);
            row.Add(person.PhoneNumber);
            row.Add(Event.Due(person.Events));

            foreach (var evt in person.Events)
            {
                row.Add(Event.DivisionTabName(evt.EventType, evt.DivisionLevel));
                row.Add(evt.PartnerName);
            }
            result.Add(row);
        }

        return result;
    }

    //
    // AllDivisionReport
    //
    private static List<IList<object>> AllDivisionReport( List<Person> people)
    {
        var result = new List<IList<object>>();
        var divisionListsOne = new List<Tuple<string, List<Person>>>();

        foreach (var div in Event.Divisions())
        {

            var list = Person.DivisionList(people, div.Item1, div.Item2);
            var division = Registration.DivisionName(div.Item1, div.Item2);
            divisionListsOne.Add(new Tuple<string, List<Person>>(division, list));
        }

        AllDivisionDetails(result, divisionListsOne);

        return result;
    }

    private static void AllDivisionDetails(List<IList<object>> dataToWrite, List<Tuple<string, List<Person>>> divisionLists)
    {
        bool written = true;

        dataToWrite.Add( new List<object>() { $"Division List" } );
        dataToWrite.Add(new List<object>() { "" });

        // write headings
        var values = new List<object>();
        foreach (var divisionList in divisionLists)
        {
            values.Add(divisionList.Item1);
        }
        dataToWrite.Add(values);

        int row = 0;
        while (written)
        {
            written = false;
            values = new List<object>();
            for (int col = 0; col < divisionLists.Count; col++)
            {
                if (divisionLists[col].Item2.Count > row)
                {
                    values.Add(divisionLists[col].Item2[row].Name);
                    written = true;
                }
                else
                {
                    values.Add(string.Empty);
                }
            }
            row++;
            dataToWrite.Add(values);
        }
    }
}

