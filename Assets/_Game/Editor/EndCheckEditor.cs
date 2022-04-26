#if UNITY_EDITOR
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EndCheck))]
public class EndCheckEditor : Editor
{
    private List<Interactable> inScene;
    private int count;
    private int cost;
    private int trueEnd;
    private List<Interactable> trueInt;
    private int sisters;
    private List<Interactable> sisterInt;
    private int cousins;
    private List<Interactable> cousinInt;

    private void OnEnable() {
        var stories = (StoryInteractable[])Resources.FindObjectsOfTypeAll(typeof(StoryInteractable));
        inScene = new List<Interactable>();
        foreach (var story in stories) {
            if (!EditorUtility.IsPersistent(story.transform.root.gameObject)) {
                if (story.Interaction != null) inScene.Add(story.Interaction);
                if (story.AltInteraction != null) inScene.Add(story.AltInteraction);
            }
        }
        var interactables = (Interactable[])Resources.FindObjectsOfTypeAll(typeof(Interactable));
        count = 0;
        cost = 0;
        trueEnd = 0;
        trueInt = new List<Interactable>();
        sisters = 0;
        sisterInt = new List<Interactable>();
        cousins = 0;
        cousinInt = new List<Interactable>();

        foreach (var interactable in interactables) {
            count++;
            if (interactable.Cost > 0) {
                cost++;
            }
            if (interactable.TrueEndingPoints > 0) {
                trueEnd += interactable.TrueEndingPoints;
                trueInt.Add(interactable);
            }
            if (interactable.SisterEndPoints > 0) {
                sisters += interactable.SisterEndPoints;
                sisterInt.Add(interactable);
            }
            if (interactable.CousinEndingPoints > 0) {
                cousins += interactable.CousinEndingPoints;
                cousinInt.Add(interactable);
            }
        }
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GUILayout.Label("Interactions in Scene: " + inScene.Count);
        int cost = 0;
        int truePoints = 0;
        int sisterPoints = 0;
        int cousinPoints = 0;
        foreach (Interactable i in inScene) {
            if (i.Cost > 0) {
                cost++;
            }
            if (i.TrueEndingPoints > 0) {
                truePoints++;
            }
            if (i.SisterEndPoints > 0) {
                sisterPoints++;
            }
            if (i.CousinEndingPoints > 0) {
                cousinPoints++;
            }
        }
        GUILayout.Label("Total cost: " + cost);
        GUILayout.Label("True Points: " + truePoints);
        GUILayout.Label("Sister Points: " + sisterPoints);
        GUILayout.Label("Cousin Points: " + cousinPoints);
        foreach (Interactable i in inScene) {
            EditorGUILayout.ObjectField(i, typeof(Interactable), true);
        }
        
        GUILayout.Space(25);
        GUIStyle style = new GUIStyle {
            wordWrap = true
        };
        GUILayout.Label("Warning: To force update this object, in the project search bar, type \'t:Interactable\' and select all of them." +
                        "Then come back to this object and it should work. No clue why this is needed.", style);
        GUILayout.Space(10);
        GUILayout.Label("Total Interactions: " + count);
        GUILayout.Label("Interactions with Cost: " + cost);
        GUILayout.Space(10);
        GUILayout.Label("True: " + trueEnd);
        foreach (var i in trueInt) {
            EditorGUILayout.ObjectField(i, typeof(Interactable), true);
        }
        GUILayout.Space(10);
        GUILayout.Label("Sister: " + sisters);
        foreach (var i in sisterInt) {
            EditorGUILayout.ObjectField(i, typeof(Interactable), true);
        }
        GUILayout.Space(10);
        GUILayout.Label("Cousins: " + cousins);
        foreach (var i in cousinInt) {
            EditorGUILayout.ObjectField(i, typeof(Interactable), true);
        }
        GUILayout.Space(10);
    }
}
#endif