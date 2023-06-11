using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Project.Task_1
{
    public class GridManager : MonoBehaviour
    {
        // accessors
        public int GridSize
        {
            set => gridSize = value;
        }
      
        
        //private fields
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private int gridSize;
        
        
        //private accessors
        private CameraManager _cameraManager;
        private UIManager _uiManager;
        private List<GameObject> _objectPool;
        private Cell[,] _cells;
   
        
        [Inject]
        private void OnInstaller(CameraManager cameraManager,UIManager uiManager)
        {
            _cameraManager = cameraManager;
            _uiManager = uiManager;
        }

        private void Start()
        {
            GenerateGrid();
        }

        public void GenerateGrid()
        {
            _cameraManager.SetOrthographicSize(gridSize); // set camera size according to grid
            
            DisablePoolObjects();
            
            // initialize cell array
            _cells = new Cell[gridSize, gridSize];
            
            //Set controller pivot 
            var offset = (float)gridSize / 2 - 0.5f;
            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    // check if there's existing object in the pool
                    var cellObject = _objectPool.FirstOrDefault(x => !x.activeSelf);
                    if (cellObject != null)
                    {
                        cellObject.transform.position = new Vector3(i - offset, j - offset);
                        cellObject.SetActive(true);
                    }
                    else
                    {
                        // if cell object does not created yet, do it and ad it to pool
                        Vector3 spawnPosition = new Vector3(i - offset, j - offset);
                        cellObject = Instantiate(cellPrefab,spawnPosition , Quaternion.identity, transform);
                        _objectPool.Add(cellObject);
                    }

                    
                    // place cell object into grid
                    var gridElement = cellObject.GetComponent<Cell>();
                    gridElement.GridManager = this;
                    gridElement.UIManager = _uiManager;
                    _cells[i, j] = gridElement;
                    gridElement.Row = i;
                    gridElement.Col = j;
                }
            }
        }

        
        // remove pool object when game init
        private void DisablePoolObjects()
        {
            _objectPool ??= new List<GameObject>();
            for (var index = 0; index < _objectPool.Count; index++)
            {
                var obj = _objectPool[index];
                if (obj == null)
                {
                    _objectPool.RemoveAt(index);
                    index--;
                    continue;
                }

                obj.SetActive(false);
            }
        }

        private static readonly Vector2Int[] NeighborIndexes =
        {
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1)
        };

        public List<Cell> CheckNeighborhood(int x, int y, List<Cell> oldNeighbors = null)
        {
            oldNeighbors ??= new List<Cell>();
            foreach (var index in NeighborIndexes)
            {
                //Check neighbor coordinates are out of the bound of the array
                if (x + index.x >= gridSize || x + index.x < 0 || y + index.y >= gridSize || y + index.y < 0) continue;
                var neighbor = _cells[x + index.x, y + index.y];
                if (neighbor == null || !neighbor.IsActive || oldNeighbors.Contains(neighbor)) continue;
                oldNeighbors.Add(neighbor);
                CheckNeighborhood(neighbor.Row, neighbor.Col, oldNeighbors);
            }

            return oldNeighbors;
        }
    }
}