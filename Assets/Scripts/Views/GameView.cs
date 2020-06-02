using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Text FinalScoreText;

    public void SetGameViewVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void SetFinalScore(int score)
    {
        FinalScoreText.text = string.Format("Final Score: {0}", score);
    }
}
