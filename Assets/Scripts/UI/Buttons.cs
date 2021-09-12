using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FireRidesClone.UI
{
    public class Buttons : MonoBehaviour
    {
        public GameObject player;
        public GameObject line;
        public LevelManager levelManager;
        public Button gameStartButton;
        public Text score, highScore;

        private Rigidbody _playerKinematic;
        private Animator _playerAnimator;

        private void Awake()
        {
            _playerKinematic = player.transform.GetComponent<Rigidbody>();
            _playerAnimator = player.transform.GetComponent<Animator>();
        }

        public void GameStart()
        {
            _playerKinematic.isKinematic = false;
            _playerAnimator.enabled = false;
            levelManager.gameStarted = true;

            line.transform.gameObject.SetActive(true);
            gameStartButton.gameObject.SetActive(false);
            score.gameObject.SetActive(true);
            highScore.gameObject.SetActive(false);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

