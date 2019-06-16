using System;
using Eflatun.UnityCommon.Utils.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneUiManager : MonoBehaviour
{
    [SerializeField] private EventButton _newGameButton;
    [SerializeField] private string _gameSceneName;
    [SerializeField] private string _titleSceneName;

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

        SceneManager.LoadScene(_gameSceneName);
    }
}
