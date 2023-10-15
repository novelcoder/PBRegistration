namespace TestRoundRobin;

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
}
