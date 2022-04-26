
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToDisable = new List<GameObject>();

    private void Start() {
        foreach (var obj in _objectsToDisable) {
            if (obj != null) obj.SetActive(false);
        }
    }
}
