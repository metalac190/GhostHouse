using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEditor;
using UnityEditor.Animations;

namespace NPC {

    [RequireComponent(typeof(Animator))]
    public class NPCAnimationAutomation : MonoBehaviour
    {

        [SerializeField] private AnimatorOverrideController _animOverrideController = null;
        [SerializeField] GameObject _cube = null;

        NPCBase _npcBase;
        Animator _animator;

        private void Awake()
        {
            _npcBase = GetComponent<NPCBase>();
            _animator = GetComponent<Animator>();
        }


        private void Start()
        {
            OverrideCurrentAnimations();
        }

        private void OverrideCurrentAnimations()
        {
            _animator.runtimeAnimatorController = _animOverrideController;

            foreach (AnimationClip clip in _animOverrideController.animationClips)
            {
                Debug.Log(clip.name);
            }

            _animOverrideController["Idle"] = _npcBase.IdleAnimation;
        }

        public void DropCube()
        {
            GameObject cube = Instantiate(_cube);
        }
    }
}
