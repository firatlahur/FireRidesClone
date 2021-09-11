using UnityEngine;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    [HideInInspector] public bool _isGameOver;

    public LevelManager levelManager;
    public GameObject player, line, wallContainer, scoreContainer;
    public Image gameOverImage;
    public ScoreText scoreText;

    private Rigidbody _playerKinematic;
    private PlayerBehavior _playerBehavior;
    private LineBehavior _lineBehavior;
    private WallMovement _wallMovement;
    private ScoreMovement _scoreMovement;

    private int _playerLayer;
    private float _speed;
    private Vector3 _startPos;

    private void Awake()
    {
        _playerKinematic = player.transform.GetComponent<Rigidbody>();
        _playerBehavior = player.transform.GetComponent<PlayerBehavior>();
        _lineBehavior = line.transform.GetComponent<LineBehavior>();
        _wallMovement = wallContainer.transform.GetComponent<WallMovement>();
        _scoreMovement = scoreContainer.transform.GetComponent<ScoreMovement>();

        levelManager.nextLevelAccess = false;
        levelManager.gameStarted = false;

        _speed = 6f;
        levelManager.collisionCount = 0;
        _playerLayer = 10;

        _startPos = this.transform.position;
    }

    private void Update()
    {
        if(levelManager.collisionCount == levelManager.objCollectedForNextLevel && !_isGameOver)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, Time.deltaTime * _speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            levelManager.nextLevelAccess = true;
            scoreText.NextLevelScored();
            this.transform.position = _startPos;
        }
    }

    public void GameOver()
    {
        _isGameOver = true;

        _playerKinematic.isKinematic = true;

        _playerBehavior.enabled = false;
        _lineBehavior.enabled = false;
        _wallMovement.enabled = false;
        _scoreMovement.enabled = false;

        gameOverImage.gameObject.SetActive(true);
    }
}
