using UnityEngine;

public class JournalMemory : MonoBehaviour
{
    [SerializeField] private JournalMemoryType _ending = JournalMemoryType.True;
    [SerializeField] private GameObject _obj;

    private void OnEnable() {
        bool unlocked = true;
        switch (_ending) {
            case JournalMemoryType.True:
                //unlocked = DataManager.Instance.trueEndingUnlocked;
                break;
            case JournalMemoryType.Bad:
                //unlocked = DataManager.Instance.badEndingUnlocked;
                break;
            case JournalMemoryType.Sister:
                //unlocked = DataManager.Instance.sisterEndingUnlocked;
                break;
            case JournalMemoryType.Cousin:
                //unlocked = DataManager.Instance.cousinEndingUnlocked;
                break;
        }
        _obj.SetActive(unlocked);
    }
}

public enum JournalMemoryType
{
    True,
    Bad,
    Sister,
    Cousin
}