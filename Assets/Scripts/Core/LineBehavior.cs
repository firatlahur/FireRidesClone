using FireRidesClone.ScriptableObject;
using UnityEngine;

namespace FireRidesClone.Core
{
    public class LineBehavior : MonoBehaviour
    {
        public GameObject player;
        public LevelManager levelManager;

        private bool _isCollided;
        private const int WallLayer = 8;

        private LineRenderer _lineRenderer;
        private GameObject[] _collidedWall;
        private Rigidbody _playerRigidbody;
        private bool _isCollidedWallNotNull;

        private void Awake()
        {
            if (_collidedWall != null)
            {
                _isCollidedWallNotNull = _collidedWall[0] != null;
            }
            _lineRenderer = this.transform.GetComponent<LineRenderer>();
            _collidedWall = new GameObject[1];
            _playerRigidbody = player.transform.GetComponent<Rigidbody>();
        }
        
        private void Start()
        {
            this.transform.GetComponent<MeshRenderer>().material.EnableKeyword("_EmissionColor");
        }

        void Update()
        {
            if (!levelManager.gameStarted) return;
            
            if(_playerRigidbody.drag > 0 && !Input.GetMouseButton(0))
            {
                _playerRigidbody.drag -= 1f;
            }

            if (Input.GetMouseButton(0))
            {
                LinePosition();
            }

            if (Input.GetMouseButtonUp(0))
            {
                ResetLinePosition();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != WallLayer || _isCollided || _collidedWall[0] != null) return;
            
            _collidedWall[0] = other.gameObject;
            _isCollided = true;
            _isCollidedWallNotNull = true;
        }

        private void LinePosition()
        {
            _playerRigidbody.drag += .5f;
            
            if (!_isCollided)
            {
                this.transform.Translate(0, 0, .3f);
                _lineRenderer.SetPosition(0, player.transform.position);
                _lineRenderer.SetPosition(1, this.transform.position);
            }

            if (!_isCollidedWallNotNull) return;
            
            this.transform.parent = _collidedWall[0].transform.parent;
            _lineRenderer.SetPosition(1, this.transform.position);

            if (_lineRenderer.GetPosition(1) == this.transform.position)
            {
                _lineRenderer.SetPosition(0, player.transform.position);
            }
        }

        private void ResetLinePosition()
        {
            this.transform.position = player.transform.position;
            _lineRenderer.SetPosition(1, Vector3.zero);
            _lineRenderer.SetPosition(0, Vector3.zero);

            this.transform.parent = null;
            this.transform.parent = player.transform;

            _collidedWall[0] = null;
            _isCollided = false;
            _isCollidedWallNotNull = false;
        }
    }
}

