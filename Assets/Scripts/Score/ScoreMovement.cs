using System.Collections;
using UnityEngine;

public class ScoreMovement : MonoBehaviour
{
    public GameObject scoreObjTeleportation;
    public LevelManager levelManager;
    public WallMovement wallMovm;

    private float _speed;
    private bool _isPaused;
    

    void Awake()
    {
        _speed = wallMovm._speed;
    }

    private void FixedUpdate()
    {
        if(!_isPaused && levelManager.gameStarted)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, scoreObjTeleportation.transform.position, Time.deltaTime * _speed);
        }

        if (levelManager.collisionCount == levelManager.objCollectedForNextLevel)
        {
            StartCoroutine("ScoreObjPause");
        }
    }

    private IEnumerator ScoreObjPause()
    {
        _speed = 0f;
        _isPaused = true;
        yield return new WaitUntil(() => levelManager.nextLevelAccess == true);
        levelManager.collisionCount = 0;
        levelManager.nextLevelAccess = false;
        _speed = wallMovm._speed;
        _isPaused = false;
    }
}
