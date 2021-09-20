using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FireRidesClone.ScriptableObject;

namespace FireRidesClone.Score
{
    public class ScoreCollision : MonoBehaviour
    {
        public LevelManager levelManager;
        public GameObject scoreObjContainer;

        private List<Transform> _scoreCollisionList;

        private float _scoreGap;
        private const int GreenScoreLayer = 13;

        private void Awake()
        {
            _scoreCollisionList = new List<Transform>();
            _scoreGap = 10f;
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            for (int i = 1; i < scoreObjContainer.transform.childCount; i++)
            {
                _scoreCollisionList.Add(scoreObjContainer.transform.GetChild(i));
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != GreenScoreLayer) return;
            
            _scoreCollisionList.RemoveAt(0);

            collision.transform.parent.position = new Vector3(0,
                Random.Range(3.5f, 4f),
                _scoreCollisionList.Last().transform.position.z + _scoreGap);

            collision.transform.parent.rotation = Quaternion.Euler(Random.Range(-17f, 17f), 0, 0);

            _scoreCollisionList.Add(collision.transform);
            levelManager.collisionCount++;
        }
    }
}

