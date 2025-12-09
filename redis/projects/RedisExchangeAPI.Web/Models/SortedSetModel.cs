namespace RedisExchangeAPI.Web.Models;

public class SortedSetModel
{
    public SortedSetModel(string name, double score)
    {
        Name = name;
        Score = score;
    }

    public string Name { get; set; }
    public double Score { get; set; }
}