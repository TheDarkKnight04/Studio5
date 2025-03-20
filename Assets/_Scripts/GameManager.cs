using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    [SerializeField] private LivesCounterUI livesCounter;
    [SerializeField] private ScoreUI scoreCounter;

    private int currentBrickCount;
    private int totalBrickCount;
    public int score = 0;

    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        // fire audio here
        // implement particle effect here
        // add camera shake here
        currentBrickCount--;

        score++;
        scoreCounter.UpdateScore(score);

        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        if(currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();

    }

    public void KillBall()
    {
        maxLives--;
        livesCounter.UpdateLives(maxLives);

        if (maxLives == 0) {
            SceneHandler.Instance.LoadGameOverScene();
            StartCoroutine(Wait());
        }
        ball.ResetBall();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.5f);
        SceneHandler.Instance.LoadMenuScene();
    }
}
