using System;
using Eflatun.UnityCommon.Utils.UI;
using UnityEngine;

public class TitleSceneUiManager : MonoBehaviour
{
    [SerializeField] private EventButton _newGameButton;

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

        LoadGameScene();
        InitializeNewGame();
        UnloadTitleScene();
    }

    private void LoadGameScene()
    {
        Debug.Log(nameof(LoadGameScene));

        throw new NotImplementedException();
    }

    private void InitializeNewGame()
    {
        Debug.Log(nameof(InitializeNewGame));

        throw new NotImplementedException();
    }

    private void UnloadTitleScene()
    {
        Debug.Log(nameof(UnloadTitleScene));

        throw new NotImplementedException();
    }
}
