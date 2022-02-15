using System.Collections.Generic;
using UnityEngine;

namespace Utility.ObjectPooling
{
    public class PoolManager<T> where T : Component
    {
        private List<T> _pool = new List<T>();
        private string _defaultName = "PoolObject";
        private Transform _poolParent;
        private T _prefabReference;

        public void BuildInitialPool(Transform poolParent, string name, int size) {
            _poolParent = poolParent;
            _defaultName = name;

            for (int i = _pool.Count; i < size; ++i) {
                CreateNewObject(name, poolParent);
            }
        }

        public void BuildInitialPool(Transform poolParent, T prefab, int size) {
            _poolParent = poolParent;
            if (prefab != null) _defaultName = prefab.gameObject.name;
            _prefabReference = prefab;

            for (int i = _pool.Count; i < size; ++i) {
                CreateNewObject(prefab, poolParent);
            }
        }

        public T GetObject() {
            if (_pool.Count > 0) {
                return RemoveFromPool(_pool[0]);
            }
            return RemoveFromPool(_prefabReference != null ? CreateNewObject(_prefabReference, _poolParent) : CreateNewObject(_defaultName, _poolParent));
        }

        public void ReturnObject(T input) {
            if (_pool.Contains(input)) return;
            AddToPool(input);
        }

        private T CreateNewObject(string name, Transform parent) {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent);
            return AddToPool(obj.AddComponent<T>());
        }

        private T CreateNewObject(T prefab, Transform parent) {
            if (prefab == null) return CreateNewObject(_defaultName, parent);
            T obj = Object.Instantiate(prefab.gameObject, parent).GetComponent<T>();
            return AddToPool(obj);
        }

        private T AddToPool(T input) {
            _pool.Add(input);
            input.gameObject.SetActive(false);
            return input;
        }

        private T RemoveFromPool(T output) {
            _pool.Remove(output);
            output.gameObject.SetActive(true);
            return output;
        }
    }
}