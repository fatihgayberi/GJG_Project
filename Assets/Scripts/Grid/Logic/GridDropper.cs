using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using GJG.Items;
using System;

namespace GJG.GridSystem
{
    [Serializable]
    public class GridDropper
    {
        private GameGrid _gameGrid;
        private GroupChecker _groupChecker;
        private GridGenerator _gridGenerator;

        [Serializable]
        public class MoveColumns
        {
            public List<int2> nodes = new();
            public List<ItemBase> items = new();
            public List<int2> targetIndexies = new();
        }

        private int _rowLength, _columnLength;

        // private MoveColumns[] moveColumns;

        // column - MoveColumns

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
                    Debug.LogError("icinde var");
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
                    Debug.LogError("icinde yok");
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

        public DictionaryTest moveColumnDictionaryForTest = new();

        [SerializeField]
        // public Dictionary<int, List<MoveColumns>> moveColumnDictionary = new();

        public GridDropper(GameGrid gameGrid, GroupChecker groupChecker, GridGenerator gridGenerator)
        {
            _gameGrid = gameGrid;
            _groupChecker = groupChecker;
            _gridGenerator = gridGenerator;

            _rowLength = _gameGrid.RowLength;
            _columnLength = _gameGrid.ColumnLength;

            InitMoveCollumsDictionary();

            Move().Forget();
        }

        private void InitMoveCollumsDictionary()
        {
            Debug.Log("InitMoveCollumsDictionary");
            // var tmp = new Dictionary<int, List<MoveColumns>>(moveColumnDictionary);
            var tmp = moveColumnDictionaryForTest.ToCopy();

            // moveColumnDictionary.Clear();
            moveColumnDictionaryForTest.Clear();

            for (int i = 0; i < _rowLength; i++)
            {
                // moveColumnDictionary.Add(i, new());
                moveColumnDictionaryForTest.Add(i, new());

                MoveColumns moveColumn = new();
                // moveColumnDictionary[i].Add(moveColumn);
                moveColumnDictionaryForTest.Get(i).Value.Add(moveColumn);

                List<int2> itemIndexies = _gameGrid.GetColumn(i);

                foreach (var itemIndex in itemIndexies)
                {
                    int2 currentIndex = itemIndex;
                    ItemBase itemBase = _gameGrid.GetItem(currentIndex);

                    if (itemBase is ItemObstacle)
                    {
                        moveColumn = new();
                        // moveColumnDictionary[i].Add(moveColumn);
                        moveColumnDictionaryForTest.Get(i).Value.Add(moveColumn);
                    }
                    else
                    {
                        moveColumn.nodes.Add(currentIndex);
                    }
                }

                if (tmp.Count > 0)
                {
                    // var oldColums = tmp[i];
                    DictionaryPairTest oldColums = tmp.Get(i);
                    // var newColums = moveColumnDictionary[i];
                    DictionaryPairTest newColums = moveColumnDictionaryForTest.Get(i);

                    foreach (var oldColum in oldColums.Value)
                    {
                        foreach (var newColum in newColums.Value)
                        {
                            if (oldColum.nodes.Count is 0) continue;
                            if (newColum.nodes.Contains(oldColum.nodes[0]))
                            {
                                newColum.items.AddRange(oldColum.items);
                            }
                        }
                    }
                }
            }
        }

        // x -> column
        // y -> row

        public void Drop(HashSet<int> droppedColumns, Dictionary<int, List<int2>> blastCount, bool recreateGrid = false)
        {
            if (recreateGrid)
            {
                InitMoveCollumsDictionary();
            }

            // degisiklik yapilan sutunlari tek tek geziyoruz
            foreach (var columnIndex in droppedColumns)
            {
                // sutunda yer alan indexler

                // var columnInColumn = moveColumnDictionary[columnIndex];
                DictionaryPairTest columnInColumn = moveColumnDictionaryForTest.Get(columnIndex);

                for (int i = 0; i < columnInColumn.Value.Count; i++)
                {
                    var column = columnInColumn.Value[i];
                    column.targetIndexies.Clear();

                    // griddeki itemleri kontrol eder
                    InGridCheck(column);

                    // target indexleri bulur
                    FindTargetIndex(column);

                    if (i == columnInColumn.Value.Count - 1)
                    {
                        int blastedInThisColumn = 0;
                        var blatsObjects = blastCount[columnIndex];

                        foreach (var blatsObject in blatsObjects)
                        {
                            if (column.nodes.Contains(blatsObject))
                            {
                                ++blastedInThisColumn;
                            }
                        }

                        if (blastedInThisColumn > 0)
                        {
                            DropNewItemCheck(column.targetIndexies.Count - column.items.Count, column);
                        }
                    }
                }
            }
        }

        private void InGridCheck(MoveColumns moveColumns)
        {
            foreach (var itemIndex in moveColumns.nodes)
            {
                if (!_gameGrid.GetNode(itemIndex).IsEmpty) continue;

                int columnMovingElements = 0;
                // bosun ustundeki butun indexler
                for (int j = itemIndex.y; j < _columnLength; j++)
                {
                    // bosun ustundeki dolu indexleri alıyoruz 
                    if (_gameGrid.GetNode(new int2(itemIndex.x, j)).IsEmpty) continue;
                    if (_gameGrid.GetItem(new int2(itemIndex.x, j)) is ItemObstacle) break;

                    // bunlarin hareket etmesi lazim o yuzden bunlari listeye aliyoruz
                    moveColumns.items.Insert(columnMovingElements, _gameGrid.GetItem(new int2(itemIndex.x, j)));

                    ++columnMovingElements;

                    // etkilesime girilmesin diye gridden cikariyoruz
                    if (_gameGrid.GetItem(new int2(itemIndex.x, j)) is ItemBlast itemBlast)
                    {
                        itemBlast.CanMatch = false;
                        itemBlast.CanSelect = false;
                    }

                    _gameGrid.RemoveItem(new int2(itemIndex.x, j));
                    _groupChecker.CheckJustItem(new int2(itemIndex.x - 1, j));
                    _groupChecker.CheckJustItem(new int2(itemIndex.x + 1, j));
                }

                break;
            }
        }

        private void FindTargetIndex(MoveColumns moveColumns)
        {
            // butun sutunu geziyoruz
            foreach (var itemIndex in moveColumns.nodes)
            {
                // burada item yok
                if (_gameGrid.GetNode(itemIndex).IsEmpty)
                {
                    moveColumns.targetIndexies.Add(itemIndex);
                }
            }
        }

        private void DropNewItemCheck(int belowCount, MoveColumns moveColumns)
        {
            for (int i = 0; i < belowCount; i++)
            {
                // bos alan sayisi doludan daha az o yuzden yukaridan item dusurecegiz
                ItemBase itemBase = _gridGenerator.GetNewItem();

                if (itemBase is ItemBlast itemBlast)
                {
                    itemBlast.CanMatch = false;
                    itemBlast.CanSelect = false;

                    itemBlast.gameObject.SetActive(true);

                    Vector3 startPos = _gameGrid.GetNode(moveColumns.targetIndexies[^1]).nodePos;

                    startPos.y += (i + 1) + 5;

                    itemBlast.transform.position = startPos;

                    moveColumns.items.Add(itemBlast);
                }
            }
        }

        private async UniTaskVoid Move()
        {
            Vector3 targetPos;
            int2 targetIndex;
            ItemBase currentItemBase;
            DictionaryPairTest moveColumnPair;
            MoveColumns moveColumn;

            while (true)
            {
                await UniTask.Yield();

                // sutunlar listesi
                for (int i = 0; i < moveColumnDictionaryForTest.pairList.Count; i++)
                {
                    moveColumnPair = moveColumnDictionaryForTest.pairList[i];

                    // sutun icindeki sutunlar listesi
                    for (int j = 0; j < moveColumnPair.Value.Count; j++)
                    {
                        moveColumn = moveColumnPair.Value[j];

                        for (int k = 0; k < moveColumn.items.Count; k++)
                        {
                            if (moveColumn.targetIndexies.Count <= k)
                            {
                                Debug.Log("ERROR" + new int3(i, j, k));
                            }
                            if (0 > k) continue;

                            targetIndex = moveColumn.targetIndexies[k];
                            targetPos = _gameGrid.GetNode(targetIndex).nodePos;
                            currentItemBase = moveColumn.items[k];

                            currentItemBase.transform.position = Vector3.MoveTowards(currentItemBase.transform.position, targetPos, 5 * Time.deltaTime);

                            if (currentItemBase.transform.position == targetPos)
                            {
                                if (currentItemBase is ItemBlast itemBlast)
                                {
                                    itemBlast.CanMatch = true;
                                    itemBlast.CanSelect = true;

                                    _gameGrid.AddItem(targetIndex, itemBlast, targetPos);
                                    _groupChecker.CheckJustItem(targetIndex);

                                    moveColumn.items.RemoveAt(k);
                                    moveColumn.targetIndexies.RemoveAt(k);
                                    k--;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
