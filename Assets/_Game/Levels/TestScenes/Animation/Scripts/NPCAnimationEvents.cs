using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEditor;
using UnityEditor.Animations;

namespace NPC {

    [RequireComponent(typeof(Animator))]
    public class NPCAnimationEvents : MonoBehaviour
    {
        [SerializeField] GameObject _objectToDrop = null;

        public void DropCube()
        {
            if(_objectToDrop != null) 
            {
                GameObject droppedObject = Instantiate(_objectToDrop);
            }
        }
    }
}
