using UnityEngine;

namespace TomMatch.Scripts.Grid
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Vector2Int coordinates;
        [SerializeField] private Vector2 position;

        public Vector2Int Coordinates
        {
            get => coordinates;
            set => coordinates = value;
        }

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        private RectTransform _rectTransform;

        private RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
        
        public Vector2 CoreSize => RectTransform.sizeDelta;



        public void AutoCoordinates(int index, int widthAmount)
        {
            var row = (int) (index / widthAmount);
            var column = index % widthAmount;
            Coordinates = new Vector2Int(row, column);
        }
    }
}
