using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace Wonnasmith.Pooling
{
    [Serializable]
    public class Pool<T> : IInitializable
    {
        [Serializable]
        public class PoolData
        {
            public T t;
            public bool isActive;

            public PoolData(T t, bool isActive)
            {
                this.t = t;
                this.isActive = isActive;
            }
        }

        [Serializable]
        public class PoolableData
        {
            public GameObject poolPrefab;
            public int poolCount;
        }

        [SerializeField] private PoolableData poolableData;
        [SerializeField] private HashSet<PoolData> _poolDatas;

        public HashSet<PoolData> PoolDatas { get => _poolDatas; set => _poolDatas = value; }

        /// <summary> Başlangıç için istenilen sayida objeyi poola kazandirir </summary>
        public void Initialize(int poolCount)
        {
            if (poolableData == null) return;

            poolableData.poolCount = poolCount;

            Initialize();
        }

        /// <summary> Başlangıç için default sayida objeyi poola kazandirir </summary>
        public void Initialize()
        {
            if (poolableData == null) return;

            for (int i = 0; i < poolableData.poolCount; i++)
            {
                PoolObjectGenerator();
            }
        }

        /// <summary> Kullanılabilir pool objesi return eder </summary>
        public T GetPoolObject()
        {
            PoolDatas ??= new HashSet<PoolData>();

            foreach (PoolData poolData in PoolDatas)
            {
                if (poolData == null) continue;
                if (!poolData.isActive) continue;

                poolData.isActive = false;

                PoolDatas.Remove(poolData);
                PoolDatas.Add(poolData);

                return poolData.t;
            }

            PoolData poolData1 = PoolObjectGenerator();

            if (poolData1 == null) return default;

            poolData1.isActive = false;

            return poolData1.t;
        }

        /// <summary> Pool Objesini poola geri kazandırır </summary>
        public void RePoolObject(T t)
        {
            if (t == null) return;
            if (PoolDatas == null) return;

            foreach (PoolData poolData in PoolDatas)
            {
                if (poolData == null) continue;
                if (poolData.t == null) continue;
                if (!poolData.t.Equals(t)) continue;

                poolData.isActive = true;

                return;
            }
        }

        /// <summary> Her seyi poola geri kazandırır </summary>
        public void AllRePoolObject()
        {
            if (PoolDatas == null) return;

            foreach (PoolData poolData in PoolDatas)
            {
                if (poolData == null) continue;

                poolData.isActive = true;
            }
        }

        /// <summary> Bir pool Objesi oluşturur </summary>
        private PoolData PoolObjectGenerator()
        {
            if (poolableData == null) return null;
            if (poolableData.poolPrefab == null) return null;

            GameObject newPoolObject = GameObject.Instantiate(poolableData.poolPrefab);

            if (newPoolObject == null) return null;

            T t = newPoolObject.GetComponent<T>();

            newPoolObject.SetActive(false);

            if (t == null) return null;

            PoolData poolData = new(t, true);

            PoolDatas ??= new HashSet<PoolData>();

            PoolDatas.Add(poolData);

            return poolData;
        }
    }
}