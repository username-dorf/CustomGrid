using System;
using System.Collections.Generic;
using UnityEngine;

namespace TomMatch.Scripts.Grid
{
    [Serializable]
    public class GridCellPool
    {
        [SerializeField] private Transform containerTr;
        [SerializeField] private List<GridCell> cells;
        [SerializeField] private GameObject gridCellPrefab;

        public GridCell GetFromPool()
        {
            foreach (var gridCell in cells)
            {
                if (gridCell.IsEnabled()==false)
                {
                    gridCell.SetActive(true);
                    return gridCell;
                }
            }
            return CreateAdditionalCell();
        }

        public void BackToPool(GridCell cell)
        {
            cell.gameObject.transform.SetParent(containerTr);
            cell.SetActive(false);
        }

        private GridCell CreateAdditionalCell()
        {
            var cell = GameObject.Instantiate(gridCellPrefab, containerTr);
            var gridCell = cell.GetComponent<GridCell>();
            cells.Add(gridCell);
            gridCell.SetActive(true);
            return gridCell;
        }

        
        public List<GridCell> GetFieldCells(int cellAmount,int rowCount,int colCount)
        {
            var fieldCells = new List<GridCell>();
            
            for (int i = 0; i < cellAmount; i++)
            {
                var cell=GetFromPool();
                fieldCells.Add(cell);
                cell.AutoCoordinates(i,colCount);
            }

            return fieldCells;
        }
    }
}
