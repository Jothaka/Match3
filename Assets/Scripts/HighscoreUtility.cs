using System.IO;
using UnityEngine;

public static class HighscoreUtility
{
    public static SaveGame LoadHighscore()
    {
        string pathString = string.Format("highscore.json");
        SaveGame save = null;
        if (File.Exists(pathString))
        {
            using (StreamReader wr = new StreamReader(pathString))
            {
                string jsonString = wr.ReadLine();
                save = JsonUtility.FromJson<SaveGame>(jsonString);
            }
        }
        return save;
    }

    public static void SaveHighscore(SaveGame scoreToSave)
    {
        string jsonString = JsonUtility.ToJson(scoreToSave);
        string pathString = string.Format("highscore.json");
        using (StreamWriter wr = new StreamWriter(pathString))
        {
            wr.WriteLine(jsonString);
        }
    }
}
