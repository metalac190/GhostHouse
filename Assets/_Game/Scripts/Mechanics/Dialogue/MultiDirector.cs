using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[RequireComponent(typeof(PlayableDirector))]
public class MultiDirector : MonoBehaviour
{
    [SerializeField]
    List<TimelineAsset> _timelines = null;
    PlayableDirector _director;

    void Awake()
    {
        _director = GetComponent<PlayableDirector>();    
    }

    /// <summary>
    /// Searches _timelines for requested timeline and attempts to play it.
    /// </summary>
    /// <param name="timelineName"></param>
    public void Play(string timelineName)
    {
        TimelineAsset timeline = GetTimeline(timelineName);

        if (timeline == null)
        {
            throw new System.Exception($"unable to find timeline: {timelineName} in {name}._timelines");
        }

        if (_director.state == PlayState.Playing)
        {
            throw new System.Exception($"unable to play {timelineName}. {_director.playableAsset.name} is already playing.");
        }

        _director.playableAsset = timeline;
        _director.Play();
    }

    /// <summary>
    /// Searches for the requested timeline
    /// </summary>
    /// <param name="timelineName"></param>
    /// <returns> reference to the timeline or null </returns>
    TimelineAsset GetTimeline(string timelineName)
    {
        foreach (TimelineAsset timeline in _timelines)
        {
            if (timeline.name == timelineName)
            {
                return timeline;
            }
        }

        return null;
    }
}
