using UnityEngine;

namespace FireRidesClone.Score
{
    public class ScoreInstantiate : MonoBehaviour
    {
        private float _scoreGap;

        public GameObject whiteScore, scoreObjContainer;

        private void Awake()
        {
            _scoreGap = 8f;
        }

        private void Start()
        {
            InstantiateScore();
        }

        private void InstantiateScore()
        {
            float scoreGapCalculator = 0;

            for (int i = 0; i < 40; i++)
            {
                if (i % 10 == 0)
                {
                    scoreGapCalculator += _scoreGap;
                    GameObject whiteScoreInstantiate = Instantiate(whiteScore, whiteScore.transform.position + new Vector3(0, Random.Range(3.5f, 4f), scoreGapCalculator), Quaternion.identity);
                    whiteScoreInstantiate.transform.SetParent(scoreObjContainer.transform, false);
                }
            }
        }
    }
}
