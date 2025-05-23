﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : SingletonMonoBehavior<SceneHandler>
{
    [Header("Scene Data")]
    [SerializeField] private List<string> levels;
    [SerializeField] private string menuScene;

    [Header("Transition Animation Data")]
    [SerializeField] private Ease menuAnimationType;
    [SerializeField] private Ease gameOverAnimationType;
    [SerializeField] private float animationDuration;
    [SerializeField] private float gameOverAnimationDuration;
    [SerializeField] private RectTransform transitionCanvas;
    [SerializeField] private RectTransform gameOverCanvas;

    private int nextLevelIndex;
    private float initXPosition;
    private float initYPosition;

    protected override void Awake()
    {
        base.Awake();
        initXPosition = transitionCanvas.transform.localPosition.x;
        initYPosition = gameOverCanvas.transform.localPosition.y;
        SceneManager.LoadScene(menuScene);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode _)
    {
        gameOverCanvas.DOLocalMoveY(initYPosition, gameOverAnimationDuration).SetEase(gameOverAnimationType);
        transitionCanvas.DOLocalMoveX(initXPosition, animationDuration).SetEase(menuAnimationType);
    }

    public void LoadNextScene()
    {
        if(nextLevelIndex >= levels.Count)
        {
            LoadMenuScene();
        }
        else
        {
            transitionCanvas.DOLocalMoveX(initXPosition + transitionCanvas.rect.width, animationDuration).SetEase(menuAnimationType);
            StartCoroutine(LoadSceneAfterTransition(levels[nextLevelIndex]));
            nextLevelIndex++;
        }
    }

    public void LoadMenuScene()
    {
        transitionCanvas.DOLocalMoveX(initXPosition + transitionCanvas.rect.width, animationDuration).SetEase(menuAnimationType);
        StartCoroutine(LoadSceneAfterTransition(menuScene));
        nextLevelIndex = 0;
    }

    public void LoadGameOverScene()
    {
        gameOverCanvas.DOLocalMoveY(initYPosition - gameOverCanvas.rect.height, gameOverAnimationDuration).SetEase(gameOverAnimationType);
    }
    private IEnumerator LoadSceneAfterTransition(string scene)
    {
        yield return new WaitForSeconds(animationDuration);
        SceneManager.LoadScene(scene);
    }
}
