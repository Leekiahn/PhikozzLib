using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "SaveConfig", menuName = "PhikozzLib/Save/SaveConfig")]
    public class SaveConfig : ScriptableObject
    {
        [SerializeField] private eSaveType _saveType = eSaveType.Json;
        [SerializeField] private string _saveDirectory = "Save";
        
        
        public eSaveType SaveType => _saveType;
        public string SaveDirectory => _saveDirectory;
    }
}
