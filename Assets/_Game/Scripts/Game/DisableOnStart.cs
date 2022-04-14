
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToDisable;

    private void Start() {
        foreach (var obj in _objectsToDisable) {
            if (obj != null) obj.SetActive(false);
        }
    }
}
