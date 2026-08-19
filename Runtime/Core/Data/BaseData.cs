namespace PhikozzLib
{
    public abstract class BaseData
    {
        public string Name { get; protected set; }

        protected BaseData(string name)
        {
            Name = name;
        }
    }
}

