using UnityEngine;
using System.Collections.Generic;

public static class SaveManager
{
    private const string SAVE_KEY = "TowerOfLondon_SaveData";

    [System.Serializable]
    private class LevelProgress
    {
        public LevelCreator.LevelId levelId;
        public int bestMoves;
    }

    [System.Serializable]
    private class GameSaveData
    {
        public List<LevelProgress> results = new List<LevelProgress>();
    }

    public static void SaveLevelResult(LevelCreator.LevelId level, int moves)
    {
        GameSaveData data = LoadData();
        LevelProgress existing = data.results.Find(r => r.levelId == level);

        if (existing != null)
        {
            if (moves < existing.bestMoves)
                existing.bestMoves = moves;
        }
        else
        {
            data.results.Add(new LevelProgress
            {
                levelId = level,
                bestMoves = moves
            });
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public static int GetBestResult(LevelCreator.LevelId level)
    {
        GameSaveData data = LoadData();
        LevelProgress result = data.results.Find(r => r.levelId == level);
        return result != null ? result.bestMoves : int.MaxValue;
    }

    private static GameSaveData LoadData()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            return JsonUtility.FromJson<GameSaveData>(json);
        }
        return new GameSaveData();
    }

    public static void DeleteSaveData()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            PlayerPrefs.DeleteKey(SAVE_KEY);
            PlayerPrefs.Save();
        }
    }
}