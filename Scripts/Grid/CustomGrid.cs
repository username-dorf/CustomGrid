using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Urmobi.Path;

namespace TomMatch.Scripts.Grid
{
    public class CustomGrid : MonoBehaviour
    {
        [SerializeField] private GridCellPool pool;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;

        
        public int[,] FieldMatrix
        {
            get
            {
                int[,] matrix = new int[FieldSize.x,FieldSize.y];
                foreach (var gridCell in _gridCells)
                {
                    var cellCoordinates = gridCell.Coordinates;
                    matrix[cellCoordinates.x, cellCoordinates.y] = gridCell.Value;
                }

                return matrix;
            }
        }
        
        
        private Coroutine _gridLayoutDisablerRoutine;
        private int TotalCellAmount => Variables.CustomGrid.CellAmountWithBorder;
        private Vector2Int FieldSize => Variables.CustomGrid.FieldSize+Variables.CustomGrid.BorderCellAmount;

        private List<GridCell> _gridCells;

        private void Awake()
        {
            ConfigureGridLayout();
            _gridCells= pool.GetFieldCells(TotalCellAmount,FieldSize.x,FieldSize.y);
            CreateField(_gridCells,EnableGridLayoutGroup,DisableLayoutGroup);
        }

        private void CreateField(List<GridCell> cells,UnityAction onStart=null,UnityAction onComplete=null)
        {
            onStart?.Invoke();
            foreach (var gridCell in cells)
            {
                gridCell.transform.SetParent(transform);
            }
            onComplete?.Invoke();
        }

        #region Public

        public GridCell FindCell(Vector2Int coordinates)
        {
            foreach (var gridCell in _gridCells)
            {
                if (gridCell.Coordinates.Equals(coordinates)) return gridCell;
            }
            return null;
        }

        public bool IsNeighbour(Vector2Int cellA,Vector2Int cellB,out List<Vector2Int> path)
        {
            path = new List<Vector2Int>();
            var directions = Variables.CustomGrid.NeighbourDirections;
            foreach (var direction in directions)
            {
                if ((cellA + direction).Equals(cellB))
                {
                    path.AddRange(new List<Vector2Int>{cellA,cellB});
                    Debug.Log("Cells are Neighbour");
                    return true;
                }
            }
            return false;
        }

        public bool GetPath(Vector2Int from, Vector2Int to,  out List<Vector2Int> path,bool isLog=true)
        {
            path = new List<Vector2Int>();
            if (IsNeighbour(from, to, out path))
            {
                if(isLog)LogPath(path);
                return true;
            }

            if (UPath.IsOneLine(from, to, out path))
            {
                if (IsPathExist(path))
                {
                    if(isLog)Debug.Log("Exit by find path on one line");
                    return true;
                }
            }

            if (UPath.IsCornersOfSquare(from,to, out var pathA,out var pathB))
            {
                if(isLog)Debug.Log("Enable square path find");
                if (IsPathExist(pathA))
                {
                    if(isLog)Debug.Log("Exit by square L sign path A");
                    path = pathA;
                    return true;
                }
                if (IsPathExist(pathB))
                {
                    if(isLog)Debug.Log("Exit by square L sign path B");
                    path = pathB;
                    return true;
                }
            }
            if(isLog)Debug.Log($"Enable shortest path");
            var length=UPath.BFS(FieldMatrix,from,to,out  path);
            if(isLog)LogPath(path);
            if (path.Count > 0) return true;
            return false;
        }

        #endregion
        
        private bool IsPathExist(List<Vector2Int> path,bool isLog=true)
        {
            if (path.Count <= 0)
            {
                Debug.Log("path is empty");
                return false;
            }
            var lockedCells = path.FindAll(x => FindCell(x).IsEmpty == false);
            if (lockedCells.Count <= 0)
            {
                if (isLog)
                {
                    LogPath(path);
                }

                Debug.Log("Path exist ang logged");
                return true;
            }
            return false;
        }

        private void LogPath(List<Vector2Int> path)
        {
            foreach (var point in path)
            {
                FindCell(point).HighlightError();
            }
        }
        #region Testing

        [Button()]
        private void __TestFind()
        {
            var cell = FindCell(new Vector2Int(3, 6));
            cell.HighlightInfo();
        }

        [Button()]
        private void __TestPath()
        {
            var fromCell = new Vector2Int(3,6);
            var toCell = new Vector2Int(4,8);

            FindCell(fromCell).HighlightInfo();
            FindCell(toCell).HighlightInfo();

            if (GetPath(fromCell,toCell,out var path))
            {
                Debug.Log("path finded");
            }

            UPath.GetTurnAmount(path);

        }

        #endregion
        
        #region GridLayoutGroup

        private void ConfigureGridLayout()
        {
            var fieldSize=Variables.CustomGrid.FieldSize+Variables.CustomGrid.BorderCellAmount;
            var cellSize=Variables.CustomGrid.CellSize;
            var spacing=Variables.CustomGrid.CellSpacing;
            
            gridLayoutGroup.cellSize = cellSize;
            gridLayoutGroup.constraintCount = fieldSize.y;
            gridLayoutGroup.spacing = spacing;

            
            var rect = gridLayoutGroup.GetComponent<RectTransform>();
            var xSize = ((cellSize.x + spacing.x) * fieldSize.x) + spacing.x;
            var ySize = ((cellSize.y + spacing.y) * fieldSize.y) + spacing.y;
            rect.sizeDelta = new Vector2(xSize, ySize);
        }

        private void EnableGridLayoutGroup()
        {
            if (!gridLayoutGroup.enabled) gridLayoutGroup.enabled = true;
        }
        
        private void DisableLayoutGroup()
        {
            if(_gridLayoutDisablerRoutine!=null) StopCoroutine(_gridLayoutDisablerRoutine);
            _gridLayoutDisablerRoutine = StartCoroutine(GridLayoutGroupDisabler());
        }
        private IEnumerator GridLayoutGroupDisabler()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return new WaitForEndOfFrame();  
            }
            gridLayoutGroup.enabled = false;
        }
        #endregion
    }

}
