using UnityEngine;

public class LineBehavior : MonoBehaviour
{
    public GameObject player;
    public LevelManager levelManager;

    private bool _isCollided;
    private int _wallLayer;

    private LineRenderer _lineRenderer;
    private GameObject[] _collidedWall;

    private void Awake()
    {
        _lineRenderer = this.transform.GetComponent<LineRenderer>();
        _collidedWall = new GameObject[1];

        _wallLayer = 8;
    }
    private void Start()
    {
        this.transform.GetComponent<MeshRenderer>().material.EnableKeyword("_EmissionColor");
    }

    void Update()
    {
        if(levelManager.gameStarted)
        {
            if (Input.GetMouseButton(0))
            {
                LinePosition();
            }

            if (Input.GetMouseButtonUp(0))
            {
                ResetLinePosition();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _wallLayer && !_isCollided && _collidedWall[0] == null)
        {
            _collidedWall[0] = other.gameObject;
            _isCollided = true;
        }
    }

    private void LinePosition()
    {
        if(!_isCollided)
        {
            this.transform.Translate(0, 0, .3f);
            _lineRenderer.SetPosition(0, player.transform.position);
            _lineRenderer.SetPosition(1, this.transform.position);
        }

        if (_collidedWall[0] != null)
        {
            this.transform.parent = _collidedWall[0].transform.parent;
            _lineRenderer.SetPosition(1, this.transform.position);

            if(_lineRenderer.GetPosition(1) == this.transform.position)
            {
                _lineRenderer.SetPosition(0, player.transform.position);
            }
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
    }
}
