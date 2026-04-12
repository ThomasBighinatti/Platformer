using UnityEngine;

namespace Controllers
{
    public class ButterflyController : MonoBehaviour
    {

        [SerializeField] private GameObject bfVisual;
        [SerializeField] private float speed = 1;
        [SerializeField] private float distance = 1;
        private float _time;
        private float _xCoords;
        private float _yCoords;
        private Vector2 _offset;

        private float _previousXPosition;
        private bool IsMovingRight => bfVisual.transform.localPosition.x > _previousXPosition;

        private void Update()
        {
            _previousXPosition = bfVisual.transform.localPosition.x;
            _time += Time.deltaTime * speed;
            _xCoords = Mathf.PerlinNoise(Mathf.Cos(_time) * 1.5f + 10f, Mathf.Sin(_time) * 1.5f + 10f);
            _yCoords = Mathf.PerlinNoise(Mathf.Sin(_time) * 1.5f + 20f, Mathf.Cos(_time) * 1.5f + 20f);

            _offset = new Vector2(_xCoords, _yCoords) * distance;

            bfVisual.transform.localPosition = _offset;
            
            bfVisual.transform.localScale = new Vector3(IsMovingRight ? 1f : -1f, 1f, 1f);
        }
    }
}
