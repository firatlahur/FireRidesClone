using FireRidesClone.UI;
using FireRidesClone.Wall;
using System.Collections;
using System.Collections.Generic;
using FireRidesClone.ScriptableObject;
using UnityEngine;

namespace FireRidesClone.Core
{
    public class PlayerBehavior : MonoBehaviour
    {
        public WallMovement wallMovement;
        public GameObject lineObj;
        public LevelManager levelManager;
        public LevelChanger levelChanger;
        public ScoreText scoreText;

        private Rigidbody _rigidBody;
        private LineRenderer _lineRend;
        private List<Collider> _colliderTurnedOffList;

        private bool _isCollided;
        private const int WhiteScoreLayer = 12, GreenScoreLayer = 13, WallLayer = 8;

        private void Awake()
        {
            _colliderTurnedOffList = new List<Collider>();

            _rigidBody = this.transform.GetComponent<Rigidbody>();
            _lineRend = lineObj.GetComponent<LineRenderer>();
        }

        private void Start()
        {
            this.transform.GetComponent<MeshRenderer>().material.EnableKeyword("_EmissionColor");
            wallMovement.speed = 5f;
        }
        
        void FixedUpdate()
        {
            if (!levelManager.gameStarted) return;
            
            if (Input.GetMouseButton(0))
            {
                WallSpeed();

                float drag = _rigidBody.drag;
                
                if (LinePos(this.gameObject, lineObj))
                {
                    drag += .5f * Time.fixedDeltaTime;
                    _rigidBody.drag = drag;
                    wallMovement.speed += drag / 5f * Time.fixedDeltaTime;
                }
                else
                {
                    _rigidBody.drag += .1f * Time.fixedDeltaTime;
                    wallMovement.speed += drag / 5f * Time.fixedDeltaTime;
                }
            }
            else
            {
                if (_rigidBody.drag <= 5f) // check drag when no m1
                {
                    Debug.Log("aaa");
                    _rigidBody.drag = 5f;
                    //  wallMovement._speed = _rigidBody.drag;
                }

                if (wallMovement.speed >= 5)
                {
                    wallMovement.speed = 5f;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.layer)
            {
                case WallLayer:
                    levelChanger.GameOver();
                    break;
                case WhiteScoreLayer when other.gameObject.layer != GreenScoreLayer && !_isCollided:
                    WhiteHit(other);
                    break;
                case GreenScoreLayer when other.gameObject.layer != WhiteScoreLayer && !_isCollided:
                    GreenHit(other);
                    break;
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

            StartCoroutine(nameof(ColliderTurnOn));
        }

        private void GreenHit(Collider greenScore)
        {
            _isCollided = true;

            greenScore.transform.GetComponent<SphereCollider>().enabled = false;
            Transform green;
            (green = greenScore.transform).GetComponent<MeshRenderer>().enabled = false;
            
            
            Transform whiteScore = green.parent;
            
            whiteScore.GetComponent<MeshRenderer>().enabled = false;

            scoreText.GreenScored();

            _colliderTurnedOffList.Add(greenScore);
            _colliderTurnedOffList.Add(whiteScore.GetComponent<SphereCollider>());

            StartCoroutine(nameof(ColliderTurnOn));
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
            StopCoroutine(nameof(ColliderTurnOn));
        }

        private void WallSpeed()
        {
            if (LinePos(gameObject, lineObj)) //player is behind the line
            {
                

                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,
                                                                         new Vector3(0, _lineRend.GetPosition(1).y, 0),
                                                                         1.5f * Time.deltaTime);
            }
            else
            {

                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,
                                                                         new Vector3(0, _lineRend.GetPosition(1).y, 0),
                                                                         4.5f * Time.deltaTime);
            }
        }

        private static bool LinePos(GameObject player, GameObject line)
        {
            bool playerIsBehindLine = line.transform.position.z > player.transform.position.z;
            return playerIsBehindLine;
        }
    }
}
