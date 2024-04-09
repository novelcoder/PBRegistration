namespace TestRoundRobin;

using DreaminandSchemin.Managers;
using RoundRobin;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        var teams = new List<string> { "team one", "team two", "team three", "team four", "team five" };
        var rounds = PoolDistribution.MatchesByRound(teams);
        Assert.AreEqual(rounds.Count, 3);
    }

    [TestMethod]
    public void TestLoadDivisionModel()
    {
        const string sheetId = "1ZZtzH4KPuaZMgAeyR_H4W92SaRJ4srdYAnZx-I-ps3A";
        //var mgr = new RoundRobinLoadManager();

        var divisionModel = mgr.LoadBracketModel(sheetId);
    }
}
