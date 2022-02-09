/*
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Utility.Audio.Clips;
using Utility.Audio.Controllers;

[CustomEditor(typeof(MusicTrack), true)]
public class MusicTrackEditor : Editor
{
    private MusicSourceController _audioSourcePreview;

    public void OnEnable() {
        _audioSourcePreview = EditorUtility.CreateGameObjectWithHideFlags("Audio Clip Preview", HideFlags.HideAndDontSave, typeof(MusicSourceController)).GetComponent<MusicSourceController>();
    }

    public void OnDisable() {
        DestroyImmediate(_audioSourcePreview.gameObject);
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Preview Start", GUILayout.Height(40))) {
            _audioSourcePreview.PlayMusic((MusicTrack)target);
        }
        EditorGUILayout.Separator();
        if (GUILayout.Button("Preview End", GUILayout.Height(40))) {
            _audioSourcePreview.PlayMusic((MusicTrack)target);
        }
    }
}
#endif
*/

