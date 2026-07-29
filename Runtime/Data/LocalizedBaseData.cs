using PhikozzLib;
using UnityEngine;

public abstract class LocalizedBaseData : BaseData
{
    public string LocaleTableRef { get; private set; }
    public string LocaleEntryRef { get; private set; }
    
    protected LocalizedBaseData(int id, string name, string localeTableRef, string localeEntryRef) : base(id, name)
    {
        LocaleTableRef = localeTableRef;
        LocaleEntryRef = localeEntryRef;
    }
}
