using Eflatun.UnityCommon.Utils.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace starikcetin.hexfallClone.game
{
    public class GameOverSceneUiManager : MonoBehaviour
    {
        [SerializeField] private SceneReference _gameScene;
        [SerializeField] private EventButton _newGameButton;
        [SerializeField] private Text _score;

        private void Start()
        {
            _newGameButton.OnClick += NewGameButtonOnClick;

            _score.text = ScoreDatabase.Instance.Score.ToString();
        }

        private void OnDestroy()
        {
            if (_newGameButton)
            {
                _newGameButton.OnClick -= NewGameButtonOnClick;
            }
        }

        private void NewGameButtonOnClick()
        {
            Debug.Log(nameof(NewGameButtonOnClick));

            SceneManager.LoadScene(_gameScene.ScenePath);
        }
    }
}
