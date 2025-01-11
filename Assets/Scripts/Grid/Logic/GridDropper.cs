using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

namespace GJG.GridSystem
{
    class GridDropper
    {
        private GameGrid _gameGrid;

        public GridDropper(GameGrid gameGrid)
        {
            _gameGrid = gameGrid;
        }

        public void Drop(int columnIndex)
        {
            List<int2> itemIndexies = _gameGrid.GetColumn(columnIndex).ToList();

            for (int i = 0; i < itemIndexies.Count; i++)
            {
                if (_gameGrid.GetNode(itemIndexies[i]).IsEmpty)
                {
                    // itemIndexies[i] ilk bos indeximiz 

                    for (int j = i + 1; j < itemIndexies.Count; j++)
                    {
                        if (!_gameGrid.GetNode(itemIndexies[j]).IsEmpty)
                        {
                            // itemIndexies[j] ustteki ilk dolu index

                            ItemMove(_gameGrid.GetItem(itemIndexies[j]).transform, _gameGrid.GetNode(itemIndexies[i]).nodePos).Forget();
                            _gameGrid.Swap(itemIndexies[i], itemIndexies[j]);
                            break;
                        }
                    }
                }
            }
        }

        private async UniTaskVoid ItemMove(Transform itemTR, Vector3 targetPos)
        {
            while (true)
            {
                await UniTask.Yield();

                itemTR.position = Vector3.MoveTowards(itemTR.position, targetPos, 5 * Time.deltaTime);

                if (itemTR.position == targetPos) break;
            }
        }
    }
}

