using Eflatun.UnityCommon.Utils.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneUiManager : MonoBehaviour
{
    [SerializeField] private EventButton _newGameButton;
    [SerializeField] private SceneAsset _gameScene;

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
        Debug.Log(nameof(NewGameButtonOnClick));

        SceneManager.LoadScene(_gameScene.name);
    }
}
