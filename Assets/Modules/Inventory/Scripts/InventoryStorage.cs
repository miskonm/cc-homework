using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Inventories
{
    public class InventoryStorage : IEnumerable<Item>
    {
        private const int EMPTY_CELL_ID = -1;

        private readonly int[] _cells;
        private readonly Dictionary<int, Vector2Int> _positionsById;
        private readonly Dictionary<int, Item> _itemsById;

        public int Height { get; }
        public int ItemsCount => _itemsById.Count;
        public int Width { get; }

        public InventoryStorage(int height, int width)
        {
            Height = height;
            Width = width;

            int cellsCount = width * height;
            _cells = new int[cellsCount];
            _positionsById = new Dictionary<int, Vector2Int>(cellsCount / 2);
            _itemsById = new Dictionary<int, Item>(cellsCount / 2);

            Array.Fill(_cells, EMPTY_CELL_ID);
        }

        public Vector2Int AddItem(Item item, int startX, int startY)
        {
            MarkCellsWithId(startX, startY, item.Size.x, item.Size.y, item.Id);

            var position = new Vector2Int(startX, startY);
            _positionsById.Add(item.Id, position);

            _itemsById.Add(item.Id, item);

            return position;
        }

        public void Clear()
        {
            Array.Fill(_cells, EMPTY_CELL_ID);
            _positionsById.Clear();
            _itemsById.Clear();
        }

        public bool ContainsKey(int itemId)
        {
            return _itemsById.ContainsKey(itemId);
        }

        public void CopyTo(Item[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            foreach (KeyValuePair<int, Vector2Int> kvp in _positionsById)
            {
                int id = kvp.Key;
                Vector2Int pos = kvp.Value;
                Item item = _itemsById[id];

                for (var y = 0; y < item.Size.y; y++)
                {
                    for (var x = 0; x < item.Size.x; x++)
                    {
                        int mx = pos.x + x;
                        int my = pos.y + y;

                        if (mx >= 0 && mx < cols && my >= 0 && my < rows)
                        {
                            matrix[mx, my] = item;
                        }
                    }
                }
            }
        }

        public bool FindFreePosition(int sizeX, int sizeY, out Vector2Int position)
        {
            if (sizeX <= 0 || sizeY <= 0)
            {
                throw new ArgumentException("Size must be positive");
            }

            for (var y = 0; y <= Height - sizeY; y++)
            {
                for (var x = 0; x <= Width - sizeX; x++)
                {
                    if (HaveEnoughSpace(sizeX, sizeY, x, y))
                    {
                        position = new Vector2Int(x, y);
                        return true;
                    }
                }
            }

            position = default;
            return false;
        }

        public int GetCellItemId(int x, int y)
        {
            return _cells[GetCellIndex(x, y)];
        }

        public IEnumerator<Item> GetEnumerator()
        {
            foreach (Item item in _itemsById.Values)
            {
                yield return item;
            }
        }

        public Item GetItem(int x, int y)
        {
            int id = GetCellItemId(x, y);
            if (id != EMPTY_CELL_ID)
            {
                return _itemsById[id];
            }

            return null;
        }

        public Vector2Int GetStartPosition(int itemId)
        {
            return _positionsById[itemId];
        }

        public bool HaveEnoughSpace(Vector2Int size, Vector2Int startPosition)
        {
            return HaveEnoughSpace(size.x, size.y, startPosition.x, startPosition.y);
        }

        public bool HaveEnoughSpace(int sizeX, int sizeY, int startX, int startY)
        {
            if (startX < 0 || startX >= Width || startX + sizeX > Width || startY < 0 || startY >= Height ||
                startY + sizeY > Height)
            {
                return false;
            }

            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    if (!IsFree(startX + x, startY + y))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsFree(int x, int y)
        {
            return GetCellItemId(x, y) == EMPTY_CELL_ID;
        }

        public void MarkCellsEmpty(Vector2Int startPosition, Vector2Int size)
        {
            MarkCellsWithId(startPosition.x, startPosition.y, size.x, size.y, EMPTY_CELL_ID);
        }

        public void MarkCellsWithId(Vector2Int startPosition, Vector2Int size, int id)
        {
            MarkCellsWithId(startPosition.x, startPosition.y, size.x, size.y, id);
        }

        public void MarkCellsWithId(int startX, int startY, int sizeX, int sizeY, int id)
        {
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {
                    _cells[GetCellIndex(startX + x, startY + y)] = id;
                }
            }
        }

        public void OptimizeSpace()
        {
            if (_itemsById.Count < 1)
            {
                return;
            }

            var oldPositions = new Dictionary<int, Vector2Int>(_positionsById);
            var oldItems = new Dictionary<int, Item>(_itemsById);
            var oldCells = (int[])_cells.Clone();

            var items = new Item[_itemsById.Count];
            var index = 0;
            foreach (KeyValuePair<int, Item> kvp in _itemsById)
            {
                items[index++] = kvp.Value;
            }

            Array.Sort(items, CompareItems);

            Array.Fill(_cells, EMPTY_CELL_ID);
            _positionsById.Clear();
            _itemsById.Clear();

            foreach (Item item in items)
            {
                if (!FindFreePosition(item.Size.x, item.Size.y, out Vector2Int pos))
                {
                    RevertToInitialState();

                    return;
                }

                AddItem(item, pos.x, pos.y);
            }

            int CompareItems(Item a, Item b)
            {
                int areaA = a.Size.x * a.Size.y;
                int areaB = b.Size.x * b.Size.y;
                if (areaA != areaB)
                {
                    return areaB - areaA;
                }

                if (a.Size.x != b.Size.x)
                {
                    return b.Size.x - a.Size.x;
                }

                return b.Size.y - a.Size.y;
            }

            void RevertToInitialState()
            {
                Array.Copy(oldCells, _cells, _cells.Length);
                _positionsById.Clear();
                foreach (KeyValuePair<int, Vector2Int> kv in oldPositions)
                {
                    _positionsById.Add(kv.Key, kv.Value);
                }

                _itemsById.Clear();
                foreach (KeyValuePair<int, Item> kv in oldItems)
                {
                    _itemsById.Add(kv.Key, kv.Value);
                }
            }
        }

        public Vector2Int Remove(Item item)
        {
            Vector2Int position = GetStartPosition(item.Id);

            _positionsById.Remove(item.Id);
            _itemsById.Remove(item.Id);

            MarkCellsWithId(position, item.Size, EMPTY_CELL_ID);

            return position;
        }

        public void SetPosition(int itemId, Vector2Int position)
        {
            _positionsById[itemId] = position;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            var space = " ";
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    int index = x + y * Width;
                    int itemId = _cells[index];

                    sb.Append(itemId != -1 ? itemId : EMPTY_CELL_ID);

                    if (x < Width - 1)
                    {
                        sb.Append(space);
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int GetCellIndex(int x, int y)
        {
            if (x < 0 || x >= Width)
            {
                throw new IndexOutOfRangeException(nameof(x));
            }

            return x + y * Width;
        }
    }
}