using UnityEngine;

public class DialogData : BaseData
{
    public string Text { get; private set; }
    public int NextId { get; private set; }
    
    public DialogData(DB_Test2Data data)
    {
        Id = data.id;
        Name = data.name;
        Text = data.text;
        NextId = data.next;
    }
}
