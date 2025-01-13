using System.Collections.Generic;
using GJG.GridSystem.Match;
using GJG.Items.ItemColor;
using Unity.Mathematics;
using GJG.Items;

namespace GJG.GridSystem
{
    public class GroupChecker
    {
        private MatchStrategy _matchStrategy;
        private GridData _gridData;
        private ItemPainter _painter;
        private GameGrid _gameGrid;

        private int2 _index, _gridSize;
        private int _groupDatasIndex, _groupDatasLength;
        private List<GridData.GroupData> _groupDataList;
        private HashSet<int2> _gridCheckFlag = new();
        private HashSet<int2> _toRemove = new();

        public GroupChecker(GameGrid gameGrid, GridData gridData, ItemPainter painter)
        {
            _gridData = gridData;
            _painter = painter;
            _gameGrid = gameGrid;

            _matchStrategy = new(gameGrid, _gridData.MatchStrategyType);

            _gridSize = _gridData.GridSize;

            _groupDatasLength = _gridData.GroupDatas.Length;

            // grup datasini cekip maxcount u buyukten kucuge dogru olacak sekilde sorting yaptik
            _groupDataList = new(_gridData.GroupDatas);
            _groupDataList.Sort((x, y) => y.MaxCount.CompareTo(x.MaxCount));
        }

        /// <summary> butun gridi kurallarina gore boyama yapar </summary>
        public void CheckAllGrid()
        {
            ItemType currentItemType;
            _gridCheckFlag.Clear();

            for (_index.x = 0; _index.x < _gridSize.x; _index.x++)
            {
                for (_index.y = 0; _index.y < _gridSize.y; _index.y++)
                {
                    // bunlar zaten daha onceden gruplama yapilarak boyama islemleri yapildigi icin atlaniyor
                    if (_gridCheckFlag.Contains(_index)) continue;

                    _toRemove = _matchStrategy.Strategy.GetMatchesItem(_index);
                    currentItemType = GetItemType(_toRemove.Count);

                    // gruplar zaten kontrol edilerek bulundugu icin bunlari bir daha kontrol etmemek icin atliyoruz
                    foreach (int2 removeIndex in _toRemove)
                    {
                        _gridCheckFlag.Add(removeIndex);
                        _painter.Paint(_gameGrid.GetItem(removeIndex), _gameGrid.GetNode(removeIndex).ItemBase.ColorType, (int)currentItemType);
                    }
                }
            }
        }

        public int GetActiveGroupCount()
        {
            int groupCount = 0;
            _gridCheckFlag.Clear();

            for (_index.x = 0; _index.x < _gridSize.x; _index.x++)
            {
                for (_index.y = 0; _index.y < _gridSize.y; _index.y++)
                {
                    // bunlar zaten daha onceden sayildigi icin atla
                    if (_gridCheckFlag.Contains(_index)) continue;

                    _toRemove = _matchStrategy.Strategy.GetMatchesItem(_index);

                    if (_toRemove.Count > 0) groupCount++;

                    // gruplar zaten kontrol edilerek bulundugu icin bunlari bir daha kontrol etmemek icin atliyoruz
                    _gridCheckFlag.UnionWith(_toRemove);
                }
            }

            return groupCount;
        }

        public void CheckJustItem(int2 index)
        {
            if (!_gameGrid.IsValidIndex(index)) return;

            _toRemove = _matchStrategy.Strategy.GetMatchesItem(index);

            foreach (int2 removeIndex in _toRemove)
            {
                _painter.Paint(_gameGrid.GetItem(removeIndex), _gameGrid.GetNode(removeIndex).ItemBase.ColorType, (int)GetItemType(_toRemove.Count));
            }
        }

        /// <summary> group counttan büyük en küçük maxcount u bulur o da bize tipi verir </summary>
        private ItemType GetItemType(int groupCount)
        {
            // maxcountlara gore buyukten kucuge siraladigimiz icin grup sayisinin maxcounttan buyuk oldugu datayi bulmammiz yeterli
            for (_groupDatasIndex = 0; _groupDatasIndex < _groupDatasLength; _groupDatasIndex++)
            {
                if (groupCount < _groupDataList[_groupDatasIndex].MaxCount) continue;
                return _groupDataList[_groupDatasIndex].ItemType;
            }

            // hicbirinden buyuk olmadigi icin default u return ettik
            return ItemType.Level_1;
        }
    }
}