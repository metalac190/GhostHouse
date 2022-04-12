using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableParticlePool : MonoBehaviour
{
    private static InteractableParticlePool _instance;
    public static InteractableParticlePool Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<InteractableParticlePool>();
                if (_instance != null) _instance.InitSystems();
            }
            return _instance;
        }
    }

    private List<ParticleSystem> _majorSystemPool;
    [SerializeField] private ParticleSystem _majorParticles = null;
    [SerializeField] private Transform _majorParent = null;
    [SerializeField] private int _majorPoolSize = 5;

    private List<ParticleSystem> _minorSystemPool;
    [SerializeField] private ParticleSystem _minorParticles = null;
    [SerializeField] private Transform _minorParent = null;
    [SerializeField] private int _minorPoolSize = 5;

    private List<ParticleSystem> _misleadingSystemPool;
    [SerializeField] private ParticleSystem _misleadingParticles = null;
    [SerializeField] private Transform _misleadingParent = null;
    [SerializeField] private int _misleadingPoolSize = 5;

    private void Awake() {
        ResetParticleSystem(_majorParticles);
        ResetParticleSystem(_minorParticles);
        ResetParticleSystem(_misleadingParticles);
    }

    private void InitSystems() {
        _majorSystemPool = new List<ParticleSystem>(_majorPoolSize);
        BuildPool(_majorSystemPool, _majorParticles, _majorParent, _majorPoolSize);
        _minorSystemPool = new List<ParticleSystem>(_minorPoolSize);
        BuildPool(_minorSystemPool, _minorParticles, _minorParent, _minorPoolSize);
        _misleadingSystemPool = new List<ParticleSystem>(_misleadingPoolSize);
        BuildPool(_misleadingSystemPool, _misleadingParticles, _misleadingParent, _misleadingPoolSize);
    }

    public ParticleSystem RegisterParticle(ParticleSystemType type) {
        switch (type) {
            case ParticleSystemType.Major:
                return GetSystem(_majorSystemPool);
            case ParticleSystemType.Minor:
                return GetSystem(_minorSystemPool);
            case ParticleSystemType.Misleading:
                return GetSystem(_misleadingSystemPool);
            default:
                return null;
        }
    }

    public void UnregisterParticle(ParticleSystem system, ParticleSystemType type) {
        StartCoroutine(WaitToReturn(system, type));
    }

    private IEnumerator WaitToReturn(ParticleSystem system, ParticleSystemType type) {
        system.Stop();
        yield return new WaitForSeconds(4);
        switch (type) {
            case ParticleSystemType.Major:
                ResetParticleSystem(system);
                _majorSystemPool.Add(system);
                break;
            case ParticleSystemType.Minor:
                ResetParticleSystem(system);
                _minorSystemPool.Add(system);
                break;
            case ParticleSystemType.Misleading:
                ResetParticleSystem(system);
                _misleadingSystemPool.Add(system);
                break;
        }
    }

    private static ParticleSystem GetSystem(List<ParticleSystem> pool) {
        var system = pool[0];
        if (pool.Count == 1) return Instantiate(system, system.transform.parent);
        pool.Remove(system);
        return system;
    }

    private static void ResetParticleSystem(ParticleSystem system)
    {
        var main = system.main;
        main.playOnAwake = false;
        main.loop = true;
        system.Stop();
        system.gameObject.SetActive(false);
    }

    private static void BuildPool(List<ParticleSystem> pool, ParticleSystem system, Transform parent, int size)
    {
        ResetParticleSystem(system);
        for (int i = 0; i < size; i++)
        {
            pool.Add(Instantiate(system, parent));
        }
    }
}

public enum ParticleSystemType
{
    Major,
    Minor,
    Misleading
}