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

    private List<ParticleSystem> _majorCostSystemPool;
    [SerializeField] private ParticleSystem _majorCostParticles = null;
    [SerializeField] private Transform _majorCostParent = null;
    [SerializeField] private int _majorCostPoolSize = 5;

    private List<ParticleSystem> _minorSystemPool;
    [SerializeField] private ParticleSystem _minorParticles = null;
    [SerializeField] private Transform _minorParent = null;
    [SerializeField] private int _minorPoolSize = 5;

    private List<ParticleSystem> _majorNoCostSystemPool;
    [SerializeField] private ParticleSystem _majorNoCostParticles = null;
    [SerializeField] private Transform _majorNoCostParent = null;
    [SerializeField] private int _majorNoCostPoolSize = 5;

    private List<ParticleSystem> _clockSystemPool;
    [SerializeField] private ParticleSystem _clockParticles = null;
    [SerializeField] private Transform _clockParent = null;
    [SerializeField] private int _clockPoolSize = 5;

    private void Awake() {
        ResetParticleSystem(_majorCostParticles);
        ResetParticleSystem(_minorParticles);
        ResetParticleSystem(_majorNoCostParticles);
        ResetParticleSystem(_clockParticles);
    }

    private void InitSystems() {
        _majorCostSystemPool = new List<ParticleSystem>(_majorCostPoolSize);
        BuildPool(_majorCostSystemPool, _majorCostParticles, _majorCostParent, _majorCostPoolSize);
        _minorSystemPool = new List<ParticleSystem>(_minorPoolSize);
        BuildPool(_minorSystemPool, _minorParticles, _minorParent, _minorPoolSize);
        _majorNoCostSystemPool = new List<ParticleSystem>(_majorNoCostPoolSize);
        BuildPool(_majorNoCostSystemPool, _majorNoCostParticles, _majorNoCostParent, _majorNoCostPoolSize);
        _clockSystemPool = new List<ParticleSystem>(_clockPoolSize);
        BuildPool(_clockSystemPool, _clockParticles, _clockParent, _clockPoolSize);
    }

    public ParticleSystem RegisterParticle(ParticleSystemType type) {
        switch (type) {
            case ParticleSystemType.MajorCost:
                return GetSystem(_majorCostSystemPool);
            case ParticleSystemType.Minor:
                return GetSystem(_minorSystemPool);
            case ParticleSystemType.MajorNoCost:
                return GetSystem(_majorNoCostSystemPool);
            case ParticleSystemType.clock:
                return GetSystem(_clockSystemPool);
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
            case ParticleSystemType.MajorCost:
                ResetParticleSystem(system);
                _majorCostSystemPool.Add(system);
                break;
            case ParticleSystemType.Minor:
                ResetParticleSystem(system);
                _minorSystemPool.Add(system);
                break;
            case ParticleSystemType.MajorNoCost:
                ResetParticleSystem(system);
                _majorNoCostSystemPool.Add(system);
                break;
            case ParticleSystemType.clock:
                ResetParticleSystem(system);
                _clockSystemPool.Add(system);
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
    MajorCost,
    Minor,
    MajorNoCost,
    clock
}