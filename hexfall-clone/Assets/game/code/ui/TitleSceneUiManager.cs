using Eflatun.UnityCommon.Utils.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace starikcetin.hexfallClone.game.ui
{
    public class TitleSceneUiManager : MonoBehaviour
    {
        [SerializeField] private EventButton _newGameButton;
        [SerializeField] private SceneReference _gameScene;

        private void Start()
        {
            _newGameButton.OnClick += NewGameButtonOnClick;
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

            SceneManager.LoadScene(_gameScene.ScenePath);
        }
    }
}
