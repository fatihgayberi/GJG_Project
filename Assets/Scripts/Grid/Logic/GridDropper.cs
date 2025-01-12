using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using GJG.Items;
using System;

namespace GJG.GridSystem
{
    [Serializable]
    class GridDropper
    {
        private GameGrid _gameGrid;
        private GroupChecker _groupChecker;
        private GridGenerator _gridGenerator;

        [Serializable]
        private class MoveColumns
        {
            public List<ItemBase> items = new();
            public List<int2> targetIndexies = new();
        }

        private int _rowLength, _columnLength;

        private MoveColumns[] moveColumns;

        public GridDropper(GameGrid gameGrid, GroupChecker groupChecker, GridGenerator gridGenerator)
        {
            _gameGrid = gameGrid;
            _groupChecker = groupChecker;
            _gridGenerator = gridGenerator;

            _rowLength = _gameGrid.RowLength;
            _columnLength = _gameGrid.ColumnLength;

            moveColumns = new MoveColumns[_rowLength];

            for (int i = 0; i < moveColumns.Length; i++)
            {
                moveColumns[i] = new();
            }

            Move().Forget();
        }

        // x -> column
        // y -> row

        public void Drop(HashSet<int> droppedColumns, Dictionary<int, int> blastCount)
        {
            // degisiklik yapilan sutunlari tek tek geziyoruz
            foreach (var columnIndex in droppedColumns)
            {
                // sutunda yer alan indexler
                List<int2> itemIndexies = _gameGrid.GetColumn(columnIndex);
                moveColumns[columnIndex].targetIndexies.Clear();

                foreach (var itemIndex in itemIndexies)
                {
                    if (!_gameGrid.GetNode(itemIndex).IsEmpty) continue;

                    int columnMovingElements = 0;
                    // bosun ustundeki butun indexler
                    for (int j = itemIndex.y; j < _columnLength; j++)
                    {
                        // bosun ustundeki dolu indexleri alıyoruz 
                        if (_gameGrid.GetNode(new int2(itemIndex.x, j)).IsEmpty) continue;

                        // bunlarin hareket etmesi lazim o yuzden bunlari listeye aliyoruz
                        moveColumns[columnIndex].items.Insert(columnMovingElements, _gameGrid.GetItem(new int2(itemIndex.x, j)));

                        ++columnMovingElements;

                        // etkilesime girilmesin diye gridden cikariyoruz
                        if (_gameGrid.GetItem(new int2(itemIndex.x, j)) is ItemBlast itemBlast)
                        {
                            itemBlast.canMatch = false;
                            itemBlast.CanSelect = false;
                        }

                        _gameGrid.RemoveItem(new int2(itemIndex.x, j));
                        _groupChecker.CheckJustItem(new int2(itemIndex.x - 1, j));
                        _groupChecker.CheckJustItem(new int2(itemIndex.x + 1, j));
                    }

                    break;
                }

                // butun sutunu geziyoruz
                foreach (var itemIndex in itemIndexies)
                {
                    // burada item yok
                    if (_gameGrid.GetNode(itemIndex).IsEmpty)
                    {
                        moveColumns[columnIndex].targetIndexies.Add(itemIndex);
                    }
                }

                for (int i = 0; i < blastCount[columnIndex]; i++)
                {
                    // bos alan sayisi doludan daha az o yuzden yukaridan item dusurecegiz
                    ItemBlast itemBlast = _gridGenerator.GetNewItem();

                    itemBlast.canMatch = false;
                    itemBlast.CanSelect = false;

                    itemBlast.gameObject.SetActive(true);

                    Vector3 startPos = _gameGrid.GetNode(moveColumns[columnIndex].targetIndexies[^1]).nodePos;

                    startPos.y += (i + 1) + 5;

                    itemBlast.transform.position = startPos;

                    moveColumns[columnIndex].items.Add(itemBlast);
                }
            }
        }

        private async UniTaskVoid Move()
        {
            while (true)
            {
                await UniTask.Yield();

                // i sutun indexi demek 
                for (int i = 0; i < moveColumns.Length; i++)
                {
                    for (int j = 0; j < moveColumns[i].items.Count; j++)
                    {
                        int2 target = moveColumns[i].targetIndexies[j];

                        moveColumns[i].items[j].transform.position = Vector3.MoveTowards(
                            moveColumns[i].items[j].transform.position,
                            _gameGrid.GetNode(target).nodePos, 5 * Time.deltaTime);

                        if (moveColumns[i].items[j].transform.position == _gameGrid.GetNode(target).nodePos)
                        {
                            if (moveColumns[i].items[j] is ItemBlast itemBlast)
                            {
                                itemBlast.canMatch = true;
                                itemBlast.CanSelect = true;

                                _gameGrid.AddItem(target, itemBlast, _gameGrid.GetNode(target).nodePos);
                                _groupChecker.CheckJustItem(target);

                                moveColumns[i].items.RemoveAt(j);
                                moveColumns[i].targetIndexies.RemoveAt(j);
                                j--;
                            }
                        }
                    }
                }
            }
        }
    }
}
