using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Inventories
{
    public class Inventory : IEnumerable<Item>
    {
        private readonly InventoryStorage _storage;

        public event Action<Item, Vector2Int> OnAdded;
        public event Action OnCleared;
        public event Action<Item, Vector2Int> OnMoved;
        public event Action<Item, Vector2Int> OnRemoved;

        public int Count => _storage.ItemsCount;
        public int Height => _storage.Height;
        public int Width => _storage.Width;

        public Inventory(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Width and height must be positive");
            }

            _storage = new InventoryStorage(height, width);
        }

        public Inventory(
            int width,
            int height,
            params KeyValuePair<Item, Vector2Int>[] items
        ) : this(width, height)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (KeyValuePair<Item, Vector2Int> kvp in items)
            {
                AddItem(kvp.Key, kvp.Value);
            }
        }

        public Inventory(int width,
            int height,
            params Item[] items
        ) : this(width, height)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (Item item in items)
            {
                AddItem(item);
            }
        }

        public Inventory(
            int width,
            int height,
            IEnumerable<KeyValuePair<Item, Vector2Int>> items
        ) : this(width, height)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (KeyValuePair<Item, Vector2Int> kvp in items)
            {
                AddItem(kvp.Key, kvp.Value);
            }
        }

        public Inventory(
            int width,
            int height,
            IEnumerable<Item> items
        ) : this(width, height)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (Item item in items)
            {
                AddItem(item);
            }
        }

        /// <summary>
        /// Creates new inventory 
        /// </summary>
        public Inventory(Inventory inventory) : this(inventory.Width, inventory.Height) { }

        /// <summary>
        /// Adds an item on a specified position
        /// </summary>
        public bool AddItem(Item item, Vector2Int position)
        {
            return AddItem(item, position.x, position.y);
        }

        public bool AddItem(Item item, int startX, int startY)
        {
            if (!IsItemValidForAdd(item) || !CanAddItem(item, startX, startY))
            {
                return false;
            }

            AddItemInternal(item, startX, startY);
            return true;
        }

        /// <summary>
        /// Adds an item on a free position
        /// </summary>
        public bool AddItem(Item item)
        {
            if (!IsItemValidForAdd(item))
            {
                return false;
            }

            if (!FindFreePosition(item, out Vector2Int position))
            {
                return false;
            }

            AddItemInternal(item, position.x, position.y);
            return true;
        }

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(Item item, Vector2Int position)
        {
            return CanAddItem(item, position.x, position.y);
        }

        public bool CanAddItem(Item item, int startX, int startY)
        {
            if (!IsItemValidForAdd(item))
            {
                return false;
            }

            return _storage.HaveEnoughSpace(item.Size.x, item.Size.y, startX, startY);
        }

        /// <summary>
        /// Checks for adding an item on a free position
        /// </summary>
        public bool CanAddItem(Item item)
        {
            if (!IsItemValidForAdd(item))
            {
                return false;
            }

            return FindFreePosition(item, out Vector2Int _);
        }

        /// <summary>
        /// Clears all items 
        /// </summary>
        public void Clear()
        {
            if (Count == 0)
            {
                return;
            }

            _storage.Clear();
            OnCleared?.Invoke();
        }

        /// <summary>
        /// Checks if the specified element exists
        /// </summary>
        public bool Contains(Item item)
        {
            if (!IsItemValid(item))
            {
                return false;
            }

            return _storage.ContainsKey(item.Id);
        }

        /// <summary>
        /// Copies items to a specified matrix
        /// </summary>
        public void CopyTo(Item[,] matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            _storage.CopyTo(matrix);
        }

        /// <summary>
        /// Returns a free position for a specified item
        /// </summary>
        public bool FindFreePosition(Item item, out Vector2Int position)
        {
            position = default;
            if (!IsItemValidForAdd(item))
            {
                return false;
            }

            return FindFreePosition(item.Size, out position);
        }

        public bool FindFreePosition(Vector2Int size, out Vector2Int position)
        {
            return _storage.FindFreePosition(size.x, size.y, out position);
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(Vector2Int position)
        {
            return GetItem(position.x, position.y);
        }

        public Item GetItem(int x, int y)
        {
            return _storage.GetItem(x, y);
        }

        /// <summary>
        /// Returns count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            var count = 0;

            foreach (Item item in _storage)
            {
                if (item.Name == name)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Returns positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(Item item)
        {
            var positions = new Vector2Int[item.Size.x * item.Size.y];
            Vector2Int startPoint = _storage.GetStartPosition(item.Id);
            positions[0] = startPoint;

            var i = 0;

            for (var x = 0; x < item.Size.x; x++)
            {
                for (var y = 0; y < item.Size.y; y++)
                {
                    positions[i] = startPoint + new Vector2Int(x, y);
                    i++;
                }
            }

            return positions;
        }

        /// <summary>
        /// Checks if the specified position is free
        /// </summary>
        public bool IsFree(Vector2Int position)
        {
            return IsFree(position.x, position.y);
        }

        public bool IsFree(int x, int y)
        {
            return _storage.IsFree(x, y);
        }

        /// <summary>
        /// Checks if the specified position is occupied
        /// </summary>
        public bool IsOccupied(Vector2Int position)
        {
            return IsOccupied(position.x, position.y);
        }

        public bool IsOccupied(int x, int y)
        {
            return !IsFree(x, y);
        }

        public bool MoveItem(Item item, Vector2Int position)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!Contains(item))
            {
                return false;
            }

            Vector2Int currentPosition = _storage.GetStartPosition(item.Id);
            _storage.MarkCellsEmpty(currentPosition, item.Size);

            if (_storage.HaveEnoughSpace(item.Size, position))
            {
                _storage.MarkCellsWithId(position, item.Size, item.Id);
                _storage.SetPosition(item.Id, position);

                OnMoved?.Invoke(item, position);
                return true;
            }

            _storage.MarkCellsWithId(currentPosition, item.Size, item.Id);
            return false;
        }

        /// <summary>
        /// Rearranges an inventory space with max free slots 
        /// </summary>
        public void OptimizeSpace()
        {
            _storage.OptimizeSpace();
        }

        /// <summary>
        /// Removes specified item
        /// </summary>
        public bool RemoveItem(Item item)
        {
            return RemoveItem(item, out _);
        }

        public bool RemoveItem(Item item, out Vector2Int position)
        {
            position = default;
            if (!IsItemValidForRemove(item))
            {
                return false;
            }

            position = _storage.Remove(item);
            OnRemoved?.Invoke(item, position);
            return true;
        }

        /// <summary>
        /// Returns an inventory matrix in string format
        /// </summary>
        public override string ToString()
        {
            return _storage.ToString();
        }

        public bool TryGetItem(Vector2Int position, out Item item)
        {
            return TryGetItem(position.x, position.y, out item);
        }

        public bool TryGetItem(int x, int y, out Item item)
        {
            item = null;

            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return false;
            }

            item = _storage.GetItem(x, y);
            return item != null;
        }

        public bool TryGetPositions(Item item, out Vector2Int[] positions)
        {
            positions = null;
            if (!IsItemValidForRemove(item))
            {
                return false;
            }

            positions = GetPositions(item);
            return true;
        }

        /// <summary>
        /// Iterates by all items 
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void AddItemInternal(Item item, int startX, int startY)
        {
            Vector2Int position = _storage.AddItem(item, startX, startY);
            OnAdded?.Invoke(item, position);
        }

        private bool IsItemValid(Item item)
        {
            if (item == null)
            {
                return false;
            }

            if (item.Size.x <= 0 || item.Size.y <= 0)
            {
                throw new ArgumentException(nameof(item));
            }

            return true;
        }

        private bool IsItemValidForAdd(Item item)
        {
            return IsItemValid(item) && !_storage.ContainsKey(item.Id);
        }

        private bool IsItemValidForRemove(Item item)
        {
            return IsItemValid(item) && _storage.ContainsKey(item.Id);
        }
    }
}