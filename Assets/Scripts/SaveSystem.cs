using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public double coins;
    public double totalTaps;
    public List<int> upgradeOwned;

    public GameData(GameManager gameManager)
    {
        coins = gameManager.coins;
        totalTaps = gameManager.totalTaps;
        upgradeOwned = new List<int>();

        foreach (var upgrade in gameManager.upgrades)
        {
            upgradeOwned.Add(upgrade.owned);
        }
    }
}

public static class SaveSystem
{
    private const string SaveKey = "GameSave";

    public static void SaveGame(GameManager gameManager)
    {
        try
        {
            GameData data = new GameData(gameManager);
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
            Debug.Log("Game saved successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public static GameData LoadGame()
    {
        try
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
                GameData data = JsonUtility.FromJson<GameData>(json);
                Debug.Log("Game loaded successfully");
                return data;
            }
            else
            {
                Debug.Log("No save file found");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return null;
        }
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
        Debug.Log("Save file deleted");
    }
}
