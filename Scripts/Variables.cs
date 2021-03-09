using System.Collections.Generic;
using UnityEngine;

namespace TomMatch.Scripts
{
    public class Variables 
    {

        public class Core
        {
            public const int MaxTurnAmount = 2;
        }
        public class CustomGrid
        {
            public static readonly Vector2 CellSize = new Vector2(110, 110);
            public static readonly Vector2Int FieldSize = new Vector2Int(7,12);
            public static readonly Vector2 CellSpacing = new Vector2(0, 0);
            public static int CellAmount => FieldSize.x * FieldSize.y;
            public static int CellAmountWithBorder => (FieldSize.x+BorderCellAmount.x) * (FieldSize.y+BorderCellAmount.y);

            public static readonly Vector2Int BorderCellAmount = new Vector2Int(1, 1);


            public static readonly List<Vector2> NeighbourDirections = new List<Vector2>
            {
                new Vector2(-1,0),//left
                new Vector2(1,0),//right
                new Vector2(0,-1),//up
                new Vector2(0,1),//down
                
            };
        }
        
    }
}
