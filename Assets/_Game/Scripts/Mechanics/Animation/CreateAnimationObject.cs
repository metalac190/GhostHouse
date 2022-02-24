using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System;

namespace Animations
{
    public class CreateAnimationObject : EditorWindow
    {
        string _name = "AC_";
        AnimatorController _template;
        AnimationClip _idleAnimation;
        AnimationClip _interactionAnimation;
        AnimationClip _postInteractionAnimation;

        string _rootName = string.Empty;

        UnityEngine.Object _artMesh;

        AnimatorController _controller;

        [MenuItem("Tools/CreateAnimationObject")]
        static void Initialize()
        {
            CreateAnimationObject window = ScriptableObject.CreateInstance<CreateAnimationObject>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 750, 500);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Create Object");
            EditorGUILayout.Space();

            _rootName = EditorGUILayout.TextField("Object Name", _rootName);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Place art/ object being animated here");
            _artMesh = EditorGUILayout.ObjectField("Art Mesh", _artMesh, typeof(UnityEngine.Object), true);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            _name = EditorGUILayout.TextField("Animation Controller name", _name);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            _template = (AnimatorController)EditorGUILayout.ObjectField("AC Template", _template, typeof(AnimatorController), true);

            //if (_template == null) EditorGUILayout.HelpBox("Missing", MessageType.Warning);

            //else if (_template.GetType() != typeof(AnimatorController)) EditorGUILayout.HelpBox("Missing", MessageType.Warning);

            /*
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Will add automatically to the controller created above");
            EditorGUILayout.LabelField("but only if created from the specific template");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Use with caution");
            EditorGUILayout.Space();
            _idleAnimation = (AnimationClip)EditorGUILayout.ObjectField("Idle Animation", _idleAnimation, typeof(AnimationClip), true);
            _interactionAnimation = (AnimationClip)EditorGUILayout.ObjectField("Interaction Animation", _interactionAnimation, typeof(AnimationClip), true);
            _postInteractionAnimation = (AnimationClip)EditorGUILayout.ObjectField("Post-Interaction Animation", _postInteractionAnimation, typeof(AnimationClip), true);


            */ 
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Object and Controller"))
            {
                CreateObject();
                this.Close();
            }
            else if (GUILayout.Button("Only Controller"))
            {
                CreateController();
                this.Close();
            }
            GUILayout.EndHorizontal();
        }

        void CreateObject()
        {
            //creates root object
            GameObject rootObject = new GameObject();
            rootObject.name = _rootName;
            rootObject.transform.position = Vector3.zero;

            //adds art
            _artMesh = Instantiate(_artMesh, rootObject.transform, false);
            //puts on interactables layer
            rootObject.layer = 9;

            CreateController();
            //adds animators script
            rootObject.AddComponent<AnimationBehavior>();

            rootObject.GetComponent<Animator>().runtimeAnimatorController = _controller;

        }

        void CreateController()
        {
            string newPath = "Assets/_Game/Entities/Interactables/AnimationControllers/" + _name + ".controller";
            // Creates the controller
            //var controller = AnimatorController.CreateAnimatorControllerAtPath(newPath);

            // copies template over to new controller
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(_template), newPath);

            // loads new controller
            AnimatorController controller = (AnimatorController)AssetDatabase.LoadAssetAtPath(newPath, typeof(AnimatorController));

            /*
            // Get current states 
            AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;
            ChildAnimatorState[] states = rootStateMachine.states;

            AnimatorState idleState = rootStateMachine.defaultState;
            AnimatorState transitionState = states[1].state;
            AnimatorState postTransitionState = states[2].state;

            idleState.motion = _idleAnimation;
            transitionState.motion = _interactionAnimation;
            postTransitionState.motion = _postInteractionAnimation;

            // Add parameters
            //controller.AddParameter("Interact", AnimatorControllerParameterType.Trigger);
            */
            _controller = controller;
        }
    }
}
