using FireRidesClone.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FireRidesClone.UI
{
    public class ScoreText : MonoBehaviour
    {
        public Text scoreText, scoreAmount, expressions, highScoreText;
        public LevelManager levelManager;
        public LevelChanger levelChanger;
        public GameObject player, line;

        private Material _playerMaterial, _lineMaterial;

        private float _score, _streakTimer, _waitTime;
        private int _greenStreak;

        private void Awake()
        {
            highScoreText.text = PlayerPrefs.GetFloat("HighScore").ToString("0.0");
            _playerMaterial = player.transform.GetComponent<MeshRenderer>().material;
            _lineMaterial = line.transform.GetComponent<LineRenderer>().material;
            _waitTime = .4f;
        }

        private void FixedUpdate()
        {
            if (levelManager.gameStarted && !levelChanger._isGameOver)
            {
                _score += .3f * Time.fixedDeltaTime;
                _streakTimer += .3f * Time.fixedDeltaTime;
            }
            scoreText.text = _score.ToString("0.0");

            if (_score > levelManager.highScore)
            {
                levelManager.highScore = _score;
            }

            if (_streakTimer >= 3f)
            {
                _greenStreak = 0;
                _streakTimer = 0;
            }

            if (_greenStreak == 0)
            {
                _playerMaterial.SetColor("_EmissionColor", Color.yellow * 1f);
                _lineMaterial.SetColor("_EmissionColor", Color.yellow * 1f);
            }

            if (levelChanger._isGameOver)
            {
                SetEndGameScore();
            }

        }


        public void WhiteScored()
        {
            StartCoroutine("WhiteScoreAmountManager");
        }

        public void GreenScored()
        {
            StartCoroutine("GreenScoreAmountManager");
        }

        public void NextLevelScored()
        {
            StartCoroutine("NextLevelScoreManager");
        }

        private void SetEndGameScore()
        {
            if (_score > PlayerPrefs.GetFloat("HighScore"))
            {
                PlayerPrefs.SetFloat("HighScore", _score);
            }
        }

        private IEnumerator GreenScoreAmountManager()
        {
            if (_greenStreak < 4)
            {
                _greenStreak++;
            }

            switch (_greenStreak)
            {
                case 0:
                    _greenStreak = 0;
                    break;
                case 1:
                    _score += 2f;
                    _streakTimer = 0;
                    _lineMaterial.SetColor("_EmissionColor", Color.yellow * 1.5f);
                    _playerMaterial.SetColor("_EmissionColor", Color.yellow * 1.5f);
                    scoreAmount.text = "+2";
                    expressions.text = "WOW";
                    break;
                case 2:
                    _score += 3f;
                    _streakTimer = 0;
                    _lineMaterial.SetColor("_EmissionColor", Color.yellow * 2f);
                    _playerMaterial.SetColor("_EmissionColor", Color.yellow * 2f);
                    scoreAmount.text = "+3";
                    expressions.text = "COOL";
                    break;
                case 3:
                    _score += 4f;
                    _streakTimer = 0;
                    _lineMaterial.SetColor("_EmissionColor", Color.yellow * 2.5f);
                    _playerMaterial.SetColor("_EmissionColor", Color.yellow * 2.5f);
                    scoreAmount.text = "+4";
                    expressions.text = "AWESOME";
                    break;
                case 4:
                    _score += 5f;
                    _streakTimer = 0f;
                    _lineMaterial.SetColor("_EmissionColor", Color.yellow * 3f);
                    _playerMaterial.SetColor("_EmissionColor", Color.yellow * 3f);
                    scoreAmount.text = "+5";
                    expressions.text = "MIND BLOWING";
                    break;

            }
            scoreAmount.color = Color.green;
            scoreAmount.gameObject.SetActive(true);
            expressions.gameObject.SetActive(true);
            yield return new WaitForSeconds(_waitTime);
            scoreAmount.gameObject.SetActive(false);
            expressions.gameObject.SetActive(false);
        }

        private IEnumerator WhiteScoreAmountManager()
        {
            _greenStreak = 0;
            _score += 1f;
            scoreAmount.text = "+1";
            scoreAmount.color = Color.white;
            scoreAmount.gameObject.SetActive(true);
            yield return new WaitForSeconds(_waitTime);
            scoreAmount.gameObject.SetActive(false);
        }

        private IEnumerator NextLevelScoreManager()
        {
            _score += 10f;
            scoreAmount.text = "+10";
            scoreAmount.color = Color.white;
            scoreAmount.gameObject.SetActive(true);
            yield return new WaitForSeconds(_waitTime);
            scoreAmount.gameObject.SetActive(false);
        }
    }
}

