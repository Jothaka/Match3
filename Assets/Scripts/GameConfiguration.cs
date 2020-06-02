using UnityEngine;

[CreateAssetMenu()]
public class GameConfiguration : ScriptableObject
{
    public int MovesAmount = 10;
    public int BlockRemovePoints = 10;
    [Tooltip("If the player collects more than the required 3 he gets that many extra points for each additional Block.")]
    public int BonusPointsForExtraBlocks = 2;
    [Tooltip("If the player has collected that many blocks at least 2 times in a row he will get double the points for each following move.")]
    public int ComboTrigger = 5;
}