#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Mechanics.Level_Mechanics;

namespace Mechanics.Animations
{
    public class CreateAnimationObject : EditorWindow
    {
        string _controllerName = "AC_";
        string _rootName = string.Empty;
        bool _isInteractable = true;

        AnimatorController _controllerTemplate;
        AnimatorController _controller;

        //AnimationClip _idleAnimation;
        //AnimationClip _interactionAnimation;
        //AnimationClip _postInteractionAnimation;

        GameObject _animatedObject;

        [MenuItem("Tools/Create Animation Object")]
        private static void Initialize() {
            CreateAnimationObject window = ScriptableObject.CreateInstance<CreateAnimationObject>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 500f, 300f);
            window.minSize = new Vector2(500f, 300f);
            window.maxSize = new Vector2(500f, 300f);
            window.titleContent = new GUIContent("Create Animation Object");
            window.Show();
        }

        private void OnGUI() {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            //EditorGUILayout.LabelField("Create Object");

            _rootName = EditorGUILayout.TextField("Object Name", _rootName);

            EditorGUILayout.Space();
            EditorGUILayout.Space();


            _isInteractable = EditorGUILayout.Toggle("Is this Object Interactable", _isInteractable);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Place art/ object being animated here");
            _animatedObject = (GameObject)EditorGUILayout.ObjectField("Object To Animate", _animatedObject, typeof(GameObject), true);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            _controllerName = EditorGUILayout.TextField("Animation Controller Name", _controllerName);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            _controllerTemplate = (AnimatorController)EditorGUILayout.ObjectField("AC Template", _controllerTemplate, typeof(AnimatorController), true);

            EditorGUILayout.EndVertical();

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
            if (GUILayout.Button("Create Object and Controller")) {
                CreateObject();
            }
            else if (GUILayout.Button("Only Controller")) {
                CreateController();
            }
            else if (GUILayout.Button("Exit")) {
                this.Close();
            }
            GUILayout.EndHorizontal();
        }

        private void CreateObject() {
            //creates root object
            GameObject rootObject = new GameObject();
            rootObject.name = _rootName;
            rootObject.transform.position = Vector3.zero;

            //creates empty art GameObject as child of root
            GameObject emptyArt = new GameObject();
            emptyArt.transform.parent = rootObject.transform;
            emptyArt.name = "Art";

            //places the prefab of the art to be animated
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GetAssetPath(_animatedObject));
            PrefabUtility.InstantiatePrefab(prefab, emptyArt.transform);

            //creates the controller
            CreateController();
            //adds animators script
            rootObject.AddComponent<AnimationBehavior>();
            rootObject.GetComponent<Animator>().runtimeAnimatorController = _controller;

            if (_isInteractable)
            {
                rootObject.AddComponent<StoryInteractable>();
                rootObject.AddComponent<InteractableResponse>();
            }
        }

        private void CreateController() {
            string newPath = "Assets/_Game/Entities/Interactables/AnimationControllers/" + _controllerName + ".controller";
            // Creates the controller
            //var controller = AnimatorController.CreateAnimatorControllerAtPath(newPath);


            if (_controllerTemplate != null) {
                // copies template over to new controller
                AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(_controllerTemplate), newPath);

                // loads new controller from template
                AnimatorController controller = (AnimatorController)AssetDatabase.LoadAssetAtPath(newPath, typeof(AnimatorController));
                _controller = controller;
            }
            else {
                AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(newPath);
                _controller = controller;
            }


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
        }
    }
}
#endif