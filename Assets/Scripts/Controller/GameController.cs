using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public const int MINBLOCKSREMOVED = 3;

    [Header("References")]
    public PlayerController Player;
    public ScoreController Score;
    public GameView View;

    [Header("Configuration")]
    public GameConfiguration Config;

    private int allowedMoves;

    void Start()
    {
        allowedMoves = Config.MovesAmount;
        Player.OnBlocksRemoved += OnPlayerMove;
        Score.SetGameConfig(Config);
    }

    //Triggered by UnityUI
    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    //Triggered by UnityUI
    public void OnReplayButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    private void OnPlayerMove(int blocksRemoved)
    {
        Score.SetScoreForRemovedBlocks(blocksRemoved);
        allowedMoves--;
        if (allowedMoves == 0)
        {
            Player.enabled = false;
            Score.HideScoring();
            View.SetFinalScore(Score.Score);
            View.SetGameViewVisibility(true);
        }
    }
}