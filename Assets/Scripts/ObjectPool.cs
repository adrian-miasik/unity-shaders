using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : Object
    {
        [Header("Object Pooling:")]
        [SerializeField] protected T pooledObject;
        [SerializeField] protected int initialSize; 
            
        private readonly Queue<T> pool = new Queue<T>();

        private static ObjectPool<T> instance { get; set; }
    
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("Removing singleton instance since one already exists.");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (pooledObject == null)
            {
                Debug.LogWarning("Unable to generate pool.", gameObject);
                return;
            }

            GeneratePool(initialSize);
        }
        
        private void GeneratePool(int sizeOfPool)
        {
            for (int i = 0; i < sizeOfPool; i++)
            {
                GenerateObject();
            }
        }
        
        /// <summary>
        /// Get/generate object
        /// </summary>
        /// <returns></returns>
        public T Fetch()
        {
            if (IsPoolDry())
                return GenerateObject();

            return pool.Dequeue();
        }

        /// <summary>
        /// Return object to pool
        /// </summary>
        /// <param name="_pooledObject"></param>
        public void Return(T _pooledObject)
        {
            pool.Enqueue(_pooledObject);
        }

        private bool IsPoolDry()
        {
            return pool.Count <= 0;
        }
        
        private T GenerateObject()
        {
            T _genObject = Instantiate(pooledObject, transform);
            pool.Enqueue(_genObject);
            return _genObject;
        }
    }
}