using System;
using System.Collections.Generic;
using static GJG.GridSystem.GridDropper;

namespace GJG.Test
{
    /// <summary>
    /// Dictionary uzerinde class onizlenmesi yapilamadigi icin yaptığım bir yapidir
    /// </summary>
    class VirtualDictionary
    {
        [Serializable]
        public class DictionaryPairTest
        {
            public int Key;
            public List<MoveColumns> Value;
        }

        [Serializable]
        public class DictionaryTest
        {
            public List<DictionaryPairTest> pairList = new();

            public int Count => pairList.Count;

            public void Add(int key, List<MoveColumns> Value)
            {
                if (Get(key) != null)
                {
                    //Debug.LogError("icinde var");
                    return;
                }

                DictionaryPairTest dictionaryPairTest = new();
                dictionaryPairTest.Key = key;
                dictionaryPairTest.Value = Value;
                pairList.Add(dictionaryPairTest);
            }

            public void Remove(int key)
            {
                if (Get(key) == null)
                {
                    //Debug.LogError("icinde yok");
                    return;
                }

                pairList.RemoveAt(key);
            }

            public void Clear()
            {
                pairList.Clear();
            }

            public DictionaryTest ToCopy()
            {
                List<DictionaryPairTest> copyPair = new(pairList);
                DictionaryTest copyDictionary = new DictionaryTest();

                copyDictionary.pairList = copyPair;

                return copyDictionary;
            }

            public DictionaryPairTest Get(int i)
            {
                foreach (var item in pairList)
                {
                    if (item.Key == i)
                    {
                        return item;
                    }
                }

                return null;
            }
        }
    }
}