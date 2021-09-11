using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public WallMovement wallMovement;
    public GameObject lineObj;
    public LevelManager levelManager;
    public LevelChanger levelChanger;
    public ScoreText scoreText;

    private Rigidbody _rigidBody;
    private LineRenderer lineRend;
    private List<Collider> _colliderTurnedOffList;

    private float _maxGravityLimit;
    private bool _isCollided;
    private int _whiteScoreLayer, _greenScoreLayer, _wallLayer;

    private void Awake()
    {
        _colliderTurnedOffList = new List<Collider>();

        _rigidBody = this.transform.GetComponent<Rigidbody>();
        lineRend = lineObj.GetComponent<LineRenderer>();

        _maxGravityLimit = 1.3f;

        _whiteScoreLayer = 12;
        _greenScoreLayer = 13;
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
            if (_rigidBody.velocity.magnitude > _maxGravityLimit)
            {
                _rigidBody.velocity = _rigidBody.velocity.normalized * _maxGravityLimit;
            }

            if (Input.GetMouseButton(0))
            {

                if (wallMovement._speed < 5 && lineObj.transform.parent != this.transform)
                {
                    wallMovement._speed += 1f;
                }
                WallSpeed();
            }
            else
            {

                if (wallMovement._speed > 5 && lineObj.transform.parent == this.transform)
                {
                    wallMovement._speed -= 1f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _wallLayer)
        {
            levelChanger.GameOver();
        }

        if (other.gameObject.layer == _whiteScoreLayer && other.gameObject.layer != _greenScoreLayer && !_isCollided)
        {
            WhiteHit(other);
        }
        else if (other.gameObject.layer == _greenScoreLayer && other.gameObject.layer != _whiteScoreLayer && !_isCollided)
        {
            GreenHit(other);
        }
    }

    private void WhiteHit(Collider whiteScore)
    {
        _isCollided = true;

        whiteScore.transform.GetComponent<SphereCollider>().enabled = false;
        whiteScore.transform.GetComponent<MeshRenderer>().enabled = false;
        whiteScore.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        scoreText.WhiteScored();

        _colliderTurnedOffList.Add(whiteScore);
        _colliderTurnedOffList.Add(whiteScore.transform.GetChild(0).GetComponent<SphereCollider>());

        StartCoroutine("ColliderTurnOn");
    }

    private void GreenHit(Collider greenScore)
    {
        _isCollided = true;

        greenScore.transform.GetComponent<SphereCollider>().enabled = false;
        greenScore.transform.GetComponent<MeshRenderer>().enabled = false;
        greenScore.transform.parent.GetComponent<MeshRenderer>().enabled = false;

        scoreText.GreenScored();

        _colliderTurnedOffList.Add(greenScore);
        _colliderTurnedOffList.Add(greenScore.transform.parent.GetComponent<SphereCollider>());

        StartCoroutine("ColliderTurnOn");
    }

    private IEnumerator ColliderTurnOn()
    {
        yield return new WaitForSeconds(.4f);

        foreach (Collider col in _colliderTurnedOffList)
        {
            col.transform.GetComponent<SphereCollider>().enabled = true;
            col.transform.GetComponent<MeshRenderer>().enabled = true;
        }
        _colliderTurnedOffList.Clear();
        _isCollided = false;
        StopCoroutine("ColliderTurnOn");
    }

    public void WallSpeed()
    {
        if (LinePos(this.gameObject, lineObj)) //player is behind the line
        {
            wallMovement._speed += .15f;

            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,
                                                                     new Vector3(0, lineRend.GetPosition(1).y, 0),
                                                                     1f * Time.deltaTime);
        }
        else
        {
           wallMovement._speed -= .15f;

            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,
                                                                     new Vector3(0, lineRend.GetPosition(1).y, 0),
                                                                     4.5f * Time.deltaTime);
        }
    }

    public bool LinePos(GameObject player, GameObject line)
    {
        bool playerIsBehindLine;
        if (line.transform.position.z > player.transform.position.z)
        {
            playerIsBehindLine = true; //in front of the line
        }
        else
        {
            playerIsBehindLine = false; //behind the line
        }
        return playerIsBehindLine;
    }
}
