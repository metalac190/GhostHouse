using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Yarn.Unity;

public class YarnObject : MonoBehaviour
{
    [Tooltip("The active child instance. If no default is provided before runtime, no instance will be active.")]
    public string ActiveInstance = "";

    Dictionary<string, GameObject> _instances;

    void Start()
    {
        LoadInstances();
        if (ActiveInstance != "")
        {
            SetInstance(ActiveInstance);
        }
    }

    /// <summary>
    /// Caches references to child locations
    /// </summary>
    void LoadInstances()
    {
        _instances = new Dictionary<string, GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            _instances.Add(child.name, child.gameObject);

            child.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Activates requested instance and deactivates previous instance.
    /// </summary>
    /// <param name="newInstance"> The name of a child GameObject </param>
    [YarnCommand("activate")]
    public void SetInstance(string newInstance)
    {
        // reset previously active instance
        if (_instances.ContainsKey(ActiveInstance))
        {
            _instances[ActiveInstance].SetActive(false);
        }

        // check requested instance is in cache
        if (!_instances.ContainsKey(newInstance))
        {
            // reload cache and try again
            LoadInstances();

            if (!_instances.ContainsKey(newInstance))
            {
                throw new System.Exception($"{newInstance} under {name} was not instance found.");
            }
        }

        // activate next instance
        _instances[newInstance].SetActive(true);
        ActiveInstance = newInstance;
    }

    /// <summary>
    /// Play the requested timeline on the currently active instance
    /// </summary>
    /// <param name="timelineName"> The name of the requested timeline </param>
    [YarnCommand("animate")]
    public void Animate(string timelineName)
    {
        if (!_instances.ContainsKey(ActiveInstance))
        {
            throw new System.Exception($"{ActiveInstance} under {name} was not instance found.");
        }

        MultiDirector multiDirector = _instances[ActiveInstance].GetComponent<MultiDirector>();
        if (multiDirector == null)
        {
            throw new System.Exception($"No multidirector component was found on {ActiveInstance}.");
        }

        multiDirector.Play(timelineName);
    }

    /// <summary>
    /// Wrapper of Animate(string), but waits till animation is done playing.
    /// </summary>
    /// <param name="timelineName"></param>
    /// <param name="waitFlag"></param>
    [YarnCommand("locked_animate")]
    public IEnumerator LockedAnimate(string timelineName)
    {
        Animate(timelineName);

        PlayableDirector director = _instances[ActiveInstance].GetComponent<PlayableDirector>();
        while (director.state == PlayState.Playing)
        {
            yield return null;
        }
    }
}