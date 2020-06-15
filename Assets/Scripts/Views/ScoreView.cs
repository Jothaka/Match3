using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    public Text ScoreText;
    public Text PreviousMoveText;
    public Text HighscoreText;
    public GameObject ComboText;

    public void SetViewVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void SetCurrentHighscore(int highscore)
    {
        HighscoreText.text = highscore.ToString();
    }

    public void SetScore(int totalScore, int score)
    {
        ScoreText.text = string.Format("Total Score: {0}", totalScore);
        PreviousMoveText.text = string.Format("Previous Move: {0}", score);
    }

    public void SetComboTextVisibilty(bool visible)
    {
        ComboText.SetActive(visible);
    }
}