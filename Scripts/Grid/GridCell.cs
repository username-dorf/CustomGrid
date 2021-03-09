using UnityEngine;

namespace TomMatch.Scripts.Grid
{
    public class GridCell : Cell
    {

        [SerializeField] private int _value;

        public int Value => _value;

        //if 1 can move
        public bool IsEmpty
        {
            get
            {
                if (_value <= 0) return false;
                return true;
            }
        }
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        public bool IsEnabled()
        {
            return gameObject.activeSelf;
        }

        

        
        
        #region Gizmos
        
        #region HighLighting

        public void HighlightError()
        {
            ChangeColor(Color.red);
        }
        public void HighlightInfo()
        {
            ChangeColor(Color.yellow);
        }

        public void ResetHighLight()
        {
            ChangeColor(Color.green);
        }

        #endregion

        private Color _mainColor=Color.green;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = _mainColor;
            DrawRect();
        }

        private void ChangeColor(Color color)
        {
            _mainColor = color;
        }
        
        private void DrawRect()
        {
            var rect = new Rect(gameObject.transform.position,CoreSize);
            Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0.01f), new Vector3(rect.size.x/200, rect.size.y/200, 0.01f));
        }

        #endregion
    }
}
