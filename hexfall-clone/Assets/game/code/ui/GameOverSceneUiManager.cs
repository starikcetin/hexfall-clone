using Eflatun.UnityCommon.Utils.UI;
using starikcetin.hexfallClone.game.databases;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace starikcetin.hexfallClone.game.ui
{
    public class GameOverSceneUiManager : MonoBehaviour
    {
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
            Utils.LogConditional(nameof(NewGameButtonOnClick));

            SceneManager.LoadScene(SceneDatabase.Instance.GameScene.ScenePath);
        }
    }
}
