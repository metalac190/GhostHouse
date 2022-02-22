using System.Collections.Generic;
using UnityEngine;

namespace Utility.ObjectPooling
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToPool = null;
        [SerializeField] private int _initialPoolSize = 5;

        protected GameObject ObjectToPool => _objectToPool;

        protected List<GameObject> Pool;

        private void Awake() {
            BuildInitialPool();
        }

        private void BuildInitialPool() {
            Pool = new List<GameObject>(_initialPoolSize);

            for (int i = Pool.Count; i < _initialPoolSize; ++i) {
                PoolNewObject();
            }
        }

        internal virtual void PoolNewObject() {
            GameObject obj = Instantiate(_objectToPool, transform);
            obj.SetActive(false);
            Pool.Add(obj);
        }

        public GameObject GetObject() {
            if (Pool.Count == 0) PoolNewObject();
            var obj = Pool[0];
            Pool.Remove(obj);
            obj.SetActive(true);
            return obj;
        }

        public void ReturnObject(GameObject obj) {
            if (Pool.Contains(obj)) return;
            Pool.Add(obj);
            obj.SetActive(false);
        }
    }
}