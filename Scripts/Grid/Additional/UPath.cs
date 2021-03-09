using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urmobi.Path
{

    public class UPath : MonoBehaviour
    {
        #region Shortest Path
        
       
            static int ROW = 0; 
            static int COL = 0;
            
        public class queueNode 
        { 
            
            public Vector2Int pt;
            public int dist;

            public List<Vector2Int> path;
            public queueNode(Vector2Int pt, int dist) 
            { 
                this.pt = pt; 
                this.dist = dist; 
            } 
            
            public queueNode(Vector2Int pt, int dist,List<Vector2Int> path) 
            { 
                this.pt = pt; 
                this.dist = dist;
                this.path = path;
            } 
        }
  
        // check whether given cell (row, col)  
        // is a valid cell or not. 
        static bool isValid(int row, int col) 
        { 
            // return true if row number and  
            // column number is in range 
            return (row >= 0) && (row < ROW) && 
                   (col >= 0) && (col < COL); 
        } 
          
        // These arrays are used to get row and column 
        // numbers of 4 neighbours of a given cell 
        static int []rowNum = {-1, 0, 0, 1}; 
        static int []colNum = {0, -1, 1, 0}; 
         
        
        // function to find the shortest path between 
        // a given source cell to a destination cell. 
        public static int BFS(int [,]mat, Vector2Int src, 
            Vector2Int dest,out List<Vector2Int> path) 
        { 
            ROW = mat.GetLength(0);
            COL = mat.GetLength(1);
            // check source and destination cell 
            // of the matrix have value 1 
            if (mat[src.x, src.y] != 1 ||
                mat[dest.x, dest.y] != 1)
            {
                path = null;
                return -1;
            }

            bool [,]visited = new bool[ROW, COL]; 
              
            // Mark the source cell as visited 
            visited[src.x, src.y] = true; 
          
            // Create a queue for BFS 
            Queue<queueNode> q = new Queue<queueNode>(); 
              
            // Distance of source cell is 0 
            queueNode s = new queueNode(src, 0,new List<Vector2Int>{src}); 
            q.Enqueue(s); // Enqueue source cell 
          
            // Do a BFS starting from source cell 
            while (q.Count != 0) 
            {
                queueNode curr = q.Peek(); 
                Vector2Int pt = curr.pt; 
          
                // If we have reached the destination cell, 
                // we are done 
                if (pt.x == dest.x && pt.y == dest.y)
                {
                    var line = string.Empty;
                    for (int i = 0; i < curr.path.Count; i++)
                    {
                        line += " " + curr.path[i];
                    }

                    Debug.Log($"Length is {curr.path.Count} and path is {line}");
                    path = curr.path;
                    return curr.dist;
                }

                // Otherwise dequeue the front cell  
                // in the queue and enqueue 
                // its adjacent cells 
               
                q.Dequeue(); 
          
                for (int i = 0; i < 4; i++) 
                { 
                    int row = pt.x + rowNum[i]; 
                    int col = pt.y + colNum[i]; 
                      
                    // if adjacent cell is valid, has path  
                    // and not visited yet, enqueue it. 
                    if (isValid(row, col) &&  
                            mat[row, col] == 1 &&  
                       !visited[row, col]) 
                    { 
                        // mark cell as visited and enqueue it 
                        visited[row, col] = true;
                        var list = new List<Vector2Int>(curr.path);
                        list.Add(new Vector2Int(row,col));
                        queueNode Adjcell = new queueNode(new Vector2Int(row, col), 
                                                              curr.dist + 1,list);
                        
                        q.Enqueue(Adjcell); 
                    } 
                } 
            } 
          
            // Return -1 if destination cannot be reached 
            path = null;
            return -1; 
        }

        #endregion


        #region ShortPass

        #region One line
        public static bool IsOneLine(Vector2Int from, Vector2Int to,out List<Vector2Int> path)
        {
            //check is on one line and line free
            path = new List<Vector2Int>();
            var isHorizontalSame = from.x.Equals(to.x);
            var isVerticalSame = from.y.Equals(to.y);
            if ( isHorizontalSame || isVerticalSame)
            {
                if (isVerticalSame)
                {
                    AddPathX(from,to,path);
                    return true;
                }

                if (isHorizontalSame)
                {
                    AddPathY(from,to,path);
                    return true;
                }
            }

            return false;
        }

        private static void AddPathX(Vector2Int from, Vector2Int to, List<Vector2Int> path)
        {
            if (from.x<to.x)
            {
                Debug.Log($"{from.x} < {to.x}");
                for (int i = from.x; i <= to.x; i++)
                {
                    path.Add(new Vector2Int(i,from.y));
                }
            }
            else
            {
                Debug.Log($"{from.x} > {to.x}");
                for (int i = from.x; i >= to.x; i--)
                {
                    path.Add(new Vector2Int(i,from.y));
                }
            }
        }
        private static void AddPathY(Vector2Int from, Vector2Int to, List<Vector2Int> path)
        {
            if (from.y < to.y)
            {
                Debug.Log($"{from.y} < {to.y}");
                for (int i = from.y; i <= to.y; i++)
                {
                    path.Add(new Vector2Int(from.x, i));
                }
            }
            else
            {
                Debug.Log($"{from.y} > {to.y}");
                for (int i = from.y; i >= to.y; i--)
                {
                    path.Add(new Vector2Int(from.x, i));
                }
            }
        }
        #endregion


        #region Square


        private static bool IsHigher(Vector2Int cellA, Vector2Int cellB)
        {
            if (cellA.y < cellB.y)
            {
                Debug.Log($"{cellA} is higher {cellB}");
                return true;
            }
            Debug.Log($"{cellA} is NOT higher {cellB}");
            return false;
        }

        private static bool IsLefter(Vector2Int cellA, Vector2Int cellB)
        {
            if (cellA.x < cellB.x)
            {
                Debug.Log($"{cellA} is lefter {cellB}");
                return true;
            }
            Debug.Log($"{cellA} is NOT lefter {cellB}");
            return false;
        }
        
        public static bool IsCornersOfSquare(Vector2Int A, Vector2Int B,out List<Vector2Int> pathA, out List<Vector2Int> pathB)
        {
            pathA = new List<Vector2Int>();
            pathB = new List<Vector2Int>();

            Debug.Log("Variant 1");
            //A * *
            //* * B
            if (IsHigher(A, B) && IsLefter(A, B))
            {
                Debug.Log("Variant 1 working");
                #region PathA
                
                for (int i = A.x; i <= B.x; i++)
                {
                    pathA.Add(new Vector2Int(i,A.y));
                }

                for (int i = A.y; i <= B.y; i++)
                {
                    pathA.Add(new Vector2Int(B.x,i));
                }
                
                #endregion
                
                #region PathB
                
                for (int i = A.y; i <= B.y; i++)
                {
                    pathB.Add(new Vector2Int(A.x,i));
                }
                for (int i = A.x; i <= B.x; i++)
                {
                    pathB.Add(new Vector2Int(i,B.y));
                }

                #endregion
                return true;
            }

            Debug.Log("Variant 2");
            //B * *
            //* * A
            if (!IsHigher(A, B) && !IsLefter(A, B))
            {
                Debug.Log("Variant 2 working");
                #region PathA
                
                for (int i = B.x; i <= A.x; i++)
                {
                    pathA.Add(new Vector2Int(i,B.y));
                }

                for (int i = B.y; i <= A.y; i++)
                {
                    pathA.Add(new Vector2Int(A.x,i));
                }
                
                #endregion
                
                #region PathB
                
                for (int i = B.y; i <= A.y; i++)
                {
                    pathB.Add(new Vector2Int(B.x,i));
                }
                for (int i = B.x; i <= A.x; i++)
                {
                    pathB.Add(new Vector2Int(i,A.y));
                }

                return true;

                #endregion
            }
            
            Debug.Log("Variant 3");
            //* * B
            //A * *
            if (!IsHigher(A, B) && IsLefter(A, B))
            {
                Debug.Log("Variant 3 working");
                #region PathA
                
                for (int i = A.x; i <= B.x; i++)
                {
                    pathA.Add(new Vector2Int(i,B.y));
                }

                for (int i = B.y; i <= A.y; i++)
                {
                    pathA.Add(new Vector2Int(A.x,i));
                }
                
                #endregion
                
                #region PathB
                
                for (int i = B.y; i <= A.y; i++)
                {
                    pathB.Add(new Vector2Int(B.x,i));
                }
                for (int i = A.x; i <= B.x; i++)
                {
                    pathB.Add(new Vector2Int(i,A.y));
                }

                return true;

                #endregion
            }
            
            Debug.Log("Variant 4");
            //* * A
            //B * *
            if (IsHigher(A, B) && !IsLefter(A, B))
            {
                Debug.Log("Variant 4 working");
                #region PathA
                
                for (int i = B.x; i <= A.x; i++)
                {
                    pathA.Add(new Vector2Int(i,A.y));
                }

                for (int i = A.y; i <= B.y; i++)
                {
                    pathA.Add(new Vector2Int(B.x,i));
                }
                
                #endregion
                
                #region PathB
                
                for (int i = A.y; i <= B.y; i++)
                {
                    pathB.Add(new Vector2Int(A.x,i));
                }
                for (int i = A.x; i <= B.x; i++)
                {
                    pathB.Add(new Vector2Int(i,B.y));
                }

                return true;

                #endregion
            }

            
            
            return false;
        }
        #endregion

        #endregion

        #region Direction

        public static int GetTurnAmount(List<Vector2Int> path)
        {
            var binary = new List<int>();
            for (int i = 1; i < path.Count; i++)
            {
                if (IsStepHorizontal(path[i - 1], path[i]))
                {
                    binary.Add(1);
                }

                if (IsStepVertical(path[i - 1], path[i]))
                {
                    binary.Add(0);
                }
            }
            var cache = binary[0];
            var turnAmount = 0;
            for (int i = 1; i < binary.Count; i++)
            {
                if (binary[i] != cache)
                {
                    cache = binary[i];
                    turnAmount++;
                }
            }

            Debug.Log($"Turn amount {turnAmount}");
            return 0;
        }
        
        private static bool IsStepHorizontal(Vector2Int current, Vector2Int next)
        {
            if (current + Vector2Int.right == next || current + Vector2Int.left == next) return true;
            return false;
        }

        private static bool IsStepVertical(Vector2Int current, Vector2Int next)
        {
            if (current + Vector2Int.down == next || current + Vector2Int.up == next) return true;
            return false;
        }

        #endregion
    }

   
}
