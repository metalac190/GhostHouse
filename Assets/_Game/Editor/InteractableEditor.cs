#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mechanics.Level_Mechanics;

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    private List<StoryInteractable> _stories;
    private List<InteractableResponseBase> _responses;
    private List<Entry> _entries;

    public void OnEnable() {
        var interactable = (Interactable)target;
        if (interactable == null) return;
        var stories = (StoryInteractable[])Resources.FindObjectsOfTypeAll(typeof(StoryInteractable));
        _stories = new List<StoryInteractable>();
        foreach (var story in stories) {
            if (!EditorUtility.IsPersistent(story.transform.root.gameObject))
                if (story.Interaction == interactable || story.AltInteraction == interactable)
                    _stories.Add(story);
        }
        var responses = (InteractableResponseBase[])Resources.FindObjectsOfTypeAll(typeof(InteractableResponseBase));
        _responses = new List<InteractableResponseBase>();
        foreach (var response in responses) {
            if (!EditorUtility.IsPersistent(response.transform.root.gameObject))
                if (response.Interactable == interactable)
                    _responses.Add(response);
        }
        var entries = (Entry[])Resources.FindObjectsOfTypeAll(typeof(Entry));
        _entries = new List<Entry>();
        foreach (var entry in entries) {
            if (!EditorUtility.IsPersistent(entry.transform.root.gameObject))
                if (entry.Interactable == interactable)
                    _entries.Add(entry);
        }
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GUIStyle title1 = new GUIStyle() {
            fontSize = 20
        };
        GUIStyle title2 = new GUIStyle() {
            fontStyle = FontStyle.Bold
        };
        bool storiesScene = _stories != null && _stories.Count > 0;
        bool responsesScene = _responses != null && _responses.Count > 0;
        bool entriesScene = _entries != null && _entries.Count > 0;
        GUILayout.Space(24);
        var header = (storiesScene || responsesScene ? "" : "No ") + "References in Scene";
        GUILayout.Label(header, title1);
        if (storiesScene) {
            GUILayout.Space(16);
            GUILayout.Label("Story Interactables", title2);
            foreach (var i in _stories) {
                EditorGUILayout.ObjectField(i, typeof(StoryInteractable), true);
            }
        }
        if (responsesScene) {
            GUILayout.Space(16);
            GUILayout.Label("Interaction Responses", title2);
            foreach (var response in _responses) {
                EditorGUILayout.ObjectField(response, typeof(InteractableResponseBase), true);
            }
        }
        if (entriesScene) {
            GUILayout.Space(16);
            GUILayout.Label("Journal Entries", title2);
            foreach (var entry in _entries) {
                EditorGUILayout.ObjectField(entry, typeof(Entry), true);
            }
        }
    }
}
#endif