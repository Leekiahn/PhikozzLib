using System.Collections.Generic;

namespace PhikozzLib
{
    public class DataContainer<T> where T : BaseData
    {
        private readonly Dictionary<int, T> _dataById = new Dictionary<int, T>();
        private readonly Dictionary<string, T> _dataByName = new Dictionary<string, T>();
        private readonly List<T> _dataList = new List<T>();
    
        public DataContainer(List<T> dataList)
        {
            foreach (var data in dataList)
            {
                _dataById[data.Id] = data;
                _dataByName[data.Name] = data;
                _dataList.Add(data);
            }
        }

        public T Get(int id)
        {
            if (_dataById.TryGetValue(id, out var data))
            {
                return data;
            }
            return null;
        }
    
        public T Get(string name)
        {
            if (_dataByName.TryGetValue(name, out var data))
            {
                return data;
            }
            return null;
        }
    
        public IEnumerable<T> GetAll()
        {
            return _dataList;
        }

    }
}
