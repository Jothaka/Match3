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

        SetPlayerInput();
        SaveGame saveGame = HighscoreUtility.LoadHighscore();
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

    //extend for other platform input e.g. android
    private void SetPlayerInput()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                Player.SetInputComponent(new MouseInputComponent());
                break;
        }
    }

    private void OnPlayerMove(int blocksRemoved)
    {
        Score.SetScoreForRemovedBlocks(blocksRemoved);
        allowedMoves--;
        if (allowedMoves == 0)
        {
            Player.enabled = false;
            Score.HideScoring();

            SaveGame saveGame = HighscoreUtility.LoadHighscore();
            if (saveGame != null)
            {
                if (saveGame.Score < Score.Score)
                    HighscoreUtility.SaveHighscore(new SaveGame() { Score = Score.Score });
            }
            else
                HighscoreUtility.SaveHighscore(new SaveGame() { Score = Score.Score });


            View.SetFinalScore(Score.Score);
            View.SetGameViewVisibility(true);
        }
    }
}