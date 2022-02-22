#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Utility.Audio.Clips.Base;
using Utility.Audio.Controllers;

namespace _Game.Editor.Sound_System
{
    [CustomEditor(typeof(SfxBase), true)]
    public class SfxBaseEditor : UnityEditor.Editor
    {
        private AudioSourceController _audioSourcePreview;

        public void OnEnable() {
            _audioSourcePreview = EditorUtility.CreateGameObjectWithHideFlags("Audio Clip Preview", HideFlags.HideAndDontSave, typeof(AudioSourceController)).GetComponent<AudioSourceController>();
        }

        public void OnDisable() {
            DestroyImmediate(_audioSourcePreview.gameObject);
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            EditorGUILayout.Separator();

            if (GUILayout.Button("Preview Sfx", GUILayout.Height(40))) {
                ((SfxBase)target).Play(_audioSourcePreview);
            }
        }
    }
}
#endif