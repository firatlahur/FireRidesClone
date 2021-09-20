using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FireRidesClone.ScriptableObject;

namespace FireRidesClone.Wall
{
    public class WallCollision : MonoBehaviour
    {
        public GameObject wallContainer;
        public LevelManager levelManager;

        private List<Transform> _bottomWallList, _topWallList;

        private int _bottomWallValue, _topWallValue, _firstWallColor, _secondWallColor, _wallLayer;

        private void Awake()
        {
            _bottomWallList = new List<Transform>();
            _topWallList = new List<Transform>();

            _firstWallColor = 0;
            _secondWallColor = 1;

            _wallLayer = 8;
        }

        private IEnumerator Start() //because actual walls are instantiated on start so we need a little bit delay here for the list
        {
            for (int i = 1; i < wallContainer.transform.childCount; i++)
            {
                if (i % 2 == 0)
                {
                    _bottomWallList.Add(wallContainer.gameObject.transform.GetChild(i));
                }

                foreach (Transform bottomWall in _bottomWallList)
                {
                    if (!_topWallList.Contains(bottomWall))
                    {
                        _topWallList.Add(wallContainer.gameObject.transform.GetChild(i));
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }

        private void Update()
        {
            WallColorChanger();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == _wallLayer)
            {
                _bottomWallValue++;
                _topWallValue++;

                if (collision.transform.position.y < 2f && _bottomWallValue < 20)
                {
                    _bottomWallList.RemoveAt(0);
                    collision.transform.position = new Vector3(0, Random.Range(-.5f, 1f), _bottomWallList.Last().transform.position.z + .5f);
                    _bottomWallList.Add(collision.transform);
                }

                if (collision.transform.position.y > 2f && _topWallValue < 20)
                {
                    _topWallList.RemoveAt(0);
                    collision.transform.position = new Vector3(0, Random.Range(7, 8.5f), _topWallList.Last().transform.position.z + .5f);
                    _topWallList.Add(collision.transform);
                }


                if (collision.transform.position.y < 2f && _bottomWallValue >= 20)
                {
                    _bottomWallList.RemoveAt(0);
                    collision.transform.position = new Vector3(0, Random.Range(.5f, 1.3f), _bottomWallList.Last().transform.position.z + .5f);
                    _bottomWallList.Add(collision.transform);
                }

                if (collision.transform.position.y > 2f && _topWallValue >= 20)
                {
                    _topWallList.RemoveAt(0);
                    collision.transform.position = new Vector3(0, Random.Range(8.5f, 8.8f), _topWallList.Last().transform.position.z + .5f);
                    _topWallList.Add(collision.transform);
                }

                if (_bottomWallValue == 40 || _topWallValue == 40)
                {
                    _bottomWallValue = 0;
                    _topWallValue = 0;
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void WallColorChanger()
        {
            if (!levelManager.nextLevelAccess) return;
            
            for (int i = 0; i < _topWallList.Count; i++)
            {
                if (i % 2 == 0)
                {
                    _topWallList[i].transform.GetComponent<MeshRenderer>().material =
                        levelManager.wallColors[_firstWallColor];
                }

                if (i % 2 != 0)
                {
                    _topWallList[i].transform.GetComponent<MeshRenderer>().material =
                        levelManager.wallColors[_secondWallColor];
                }
            }

            for (int i = 0; i < _bottomWallList.Count; i++)
            {
                if (i % 2 == 0)
                {
                    _bottomWallList[i].transform.GetComponent<MeshRenderer>().material =
                        levelManager.wallColors[_firstWallColor];
                }

                if (i % 2 != 0)
                {
                    _bottomWallList[i].transform.GetComponent<MeshRenderer>().material =
                        levelManager.wallColors[_secondWallColor];
                }
            }

            if (_secondWallColor == levelManager.wallColors.Length - 1) return;
            
            _firstWallColor += 2;
            _secondWallColor += 2;
        }
    }
}

