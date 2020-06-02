using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private const int COMBOMULTIPLIER = 2;
    private const int COMBOREQUIREMENT = 2;

    public int Score { get; private set; }

    public ScoreView View;

    //configvalues
    private int blockRemovePoints = 10;
    private int bonusPointsForExtraBlocks = 2;
    private int comboTrigger = 5;

    private int comboCounter = 0;

    public void SetGameConfig(GameConfiguration config)
    {
        blockRemovePoints = config.BlockRemovePoints;
        bonusPointsForExtraBlocks = config.BonusPointsForExtraBlocks;
        comboTrigger = config.ComboTrigger;
    }

    public void SetScoreForRemovedBlocks(int blocksRemovedAmount)
    {
        if (blocksRemovedAmount >= comboTrigger)
        {
            comboCounter++;
        }
        else
        {
            View.SetComboTextVisibilty(false);
            comboCounter = 0;
        }

        int scoreThisMove = blocksRemovedAmount * blockRemovePoints;
        scoreThisMove += bonusPointsForExtraBlocks * (Mathf.Max(blocksRemovedAmount - GameController.MINBLOCKSREMOVED, 0));

        if (comboCounter > COMBOREQUIREMENT)
        {
            View.SetComboTextVisibilty(true);
            scoreThisMove *= COMBOMULTIPLIER;
        }

        Score += scoreThisMove;
        View.SetScore(Score, scoreThisMove);
    }

    public void HideScoring()
    {
        View.SetViewVisibility(false);
    }
}
