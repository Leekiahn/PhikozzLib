using BansheeGz.BGDatabase;

public class TestData : BaseData
{
    public int Wow { get; private set; }

    public TestData(DB_TestData data)
    {
        Id = data.Id;
        Name = data.Name;
        Wow = data.Wow;
    }
}
