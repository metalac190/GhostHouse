using UnityEngine;

public class JournalMemory : MonoBehaviour
{
    [SerializeField] private JournalMemoryType _ending = JournalMemoryType.True;
    [SerializeField] private GameObject _obj = null;

    private void OnEnable() {
        bool unlocked = true;
        switch (_ending) {
            case JournalMemoryType.True:
                unlocked = DataManager.Instance.endingUnlocks[0];
                break;
            case JournalMemoryType.Bad:
                unlocked = DataManager.Instance.endingUnlocks[1];
                break;
            case JournalMemoryType.Sister:
                unlocked = DataManager.Instance.endingUnlocks[2];
                break;
            case JournalMemoryType.Cousin:
                unlocked = DataManager.Instance.endingUnlocks[3];
                break;
        }
        if (_obj != null) _obj.SetActive(unlocked);
    }
}

public enum JournalMemoryType
{
    True,
    Bad,
    Sister,
    Cousin
}