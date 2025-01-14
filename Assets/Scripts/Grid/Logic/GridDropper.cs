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

        // column - MoveColumns

        public Dictionary<int, List<MoveColumns>> moveColumnDictionary = new();

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
            //Debug.Log("InitMoveCollumsDictionary");
            var tmp = new Dictionary<int, List<MoveColumns>>(moveColumnDictionary);
            // var tmp = moveColumnDictionary.ToCopy();

            // moveColumnDictionary.Clear();
            moveColumnDictionary.Clear();

            for (int i = 0; i < _rowLength; i++)
            {
                // moveColumnDictionary.Add(i, new());
                moveColumnDictionary.Add(i, new());

                MoveColumns moveColumn = new();
                // moveColumnDictionary[i].Add(moveColumn);
                moveColumnDictionary[i].Add(moveColumn);

                List<int2> itemIndexies = _gameGrid.GetColumn(i);

                foreach (var itemIndex in itemIndexies)
                {
                    int2 currentIndex = itemIndex;
                    ItemBase itemBase = _gameGrid.GetItem(currentIndex);

                    if (itemBase is ItemObstacle)
                    {
                        moveColumn = new();
                        // moveColumnDictionary[i].Add(moveColumn);
                        moveColumnDictionary[i].Add(moveColumn);
                    }
                    else
                    {
                        moveColumn.nodes.Add(currentIndex);
                    }
                }

                if (tmp.Count > 0)
                {

                    // var oldColums = tmp[i];
                    List<MoveColumns> oldColums = tmp[i];
                    // var newColums = moveColumnDictionary[i];
                    List<MoveColumns> newColums = moveColumnDictionary[i];

                    foreach (var oldColum in oldColums)
                    {
                        foreach (var newColum in newColums)
                        {
                            if (oldColum.nodes.Count is 0) continue;
                            if (newColum.nodes.Contains(oldColum.nodes[0]))
                            {
                                newColum.targetIndexies.AddRange(oldColum.targetIndexies);
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
                List<MoveColumns> columnInColumn = moveColumnDictionary[columnIndex];

                for (int i = 0; i < columnInColumn.Count; i++)
                {
                    var column = columnInColumn[i];
                    column.targetIndexies.Clear();

                    // griddeki itemleri kontrol eder
                    InGridCheck(column);

                    // target indexleri bulur
                    FindTargetIndex(column);

                    if (i == columnInColumn.Count - 1)
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
            while (true)
            {
                await UniTask.Yield();

                ItemMover();
                MoveFinishCheck();
            }
        }

        private void ItemMover()
        {
            Vector3 targetPos;
            int2 targetIndex;
            ItemBase currentItemBase;
            List<MoveColumns> moveColumnPair;
            MoveColumns moveColumn;

            // sutunlar listesi
            for (int i = 0; i < moveColumnDictionary.Count; i++)
            {
                moveColumnPair = moveColumnDictionary[i];

                // sutun icindeki sutunlar listesi
                for (int j = 0; j < moveColumnPair.Count; j++)
                {
                    moveColumn = moveColumnPair[j];

                    for (int k = 0; k < moveColumn.items.Count; k++)
                    {
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

        private void MoveFinishCheck()
        {
            int activeItemsCount = 0;
            List<MoveColumns> moveColumnPair;

            for (int i = 0; i < moveColumnDictionary.Count; i++)
            {
                moveColumnPair = moveColumnDictionary[i];

                // sutun icindeki sutunlar listesi
                for (int j = 0; j < moveColumnPair.Count; j++)
                {
                    activeItemsCount += moveColumnPair[j].items.Count;
                }
            }

            if (activeItemsCount < 0)
            {
                // hareket eden item kalmadi
                GridEvents.MoveFinishAlltItem?.Invoke();
            }
        }
    }
}
