using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _gameCompletePanel;

    private void OnEnable()
    {
        GameManager.instance.OnGameStarted += GameStarted;
        GameManager.instance.OnGameCompleted += GameCompleted;
    }

    private void OnDisable()
    {
        GameManager.instance.OnGameStarted -= GameStarted;
        GameManager.instance.OnGameCompleted -= GameCompleted;
    }

    private void GameStarted()
    {
        _mainMenuPanel.SetActive(false);
    }

    private void GameCompleted()
    {
        _gameCompletePanel.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        GameManager.instance.OnGameStarted?.Invoke();
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
