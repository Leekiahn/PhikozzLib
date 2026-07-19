using BansheeGz.BGDatabase;

public class TestData : BaseData
{
    public int Wow { get; private set; }

    public TestData(DB_TestData data) : base(data.Id, data.Name)
    {
        Wow = data.Wow;
    }
}
