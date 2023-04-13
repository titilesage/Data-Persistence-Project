using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Xml;

[System.Serializable]
public struct PlayerScore
{
    public string playerName;
    public int score;
}

public class Menu : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public Text bestScoreText;
    public Text playerNameText;
    public Text topScoresText;

    private string playerName;
    private int bestScore;
    private List<PlayerScore> playerScores = new List<PlayerScore>();

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        playerNameInput.text = playerName;
        bestScoreText.text = "Best Score: " + bestScore;
        playerNameText.text = "Player Name: " + playerName;

        LoadScores();
        UpdateTopScores();
    }

    public void SaveData()
    {
        playerName = playerNameInput.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        playerNameText.text = "Player Name: " + playerName;
        AddScore(playerName, bestScore);
    }

    public void LoadData()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        playerNameInput.text = playerName;
        bestScoreText.text = "Best Score: " + bestScore;
        playerNameText.text = "Player Name: " + playerName;
        LoadScores();
        UpdateTopScores();
    }

    public void LanceJeu()
    {
        SceneManager.LoadScene(1);
    }

    public void SortJeu()
    {
        SaveManager.Instance.SaveDatas();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void AddScore(string playerName, int score)
    {
        bool scoreAdded = false;
        for (int i = 0; i < playerScores.Count; i++)
        {
            if (score > playerScores[i].score)
            {
                playerScores.Insert(i, new PlayerScore { playerName = playerName, score = score });
                scoreAdded = true;
                break;
            }
        }

        if (!scoreAdded)
        {
            playerScores.Add(new PlayerScore { playerName = playerName, score = score });
        }

        while (playerScores.Count > 5)
        {
            playerScores.RemoveAt(playerScores.Count - 1);
        }

        SaveScores();
        UpdateTopScores();
    }

    private void SaveScores()
    {
        PlayerPrefs.SetInt("BestScore", playerScores[0].score);
        PlayerPrefs.SetString("BestScorePlayerName", playerScores[0].playerName);
        for (int i = 1; i < playerScores.Count; i++)
        {
            PlayerPrefs.SetInt("TopScore" + i, playerScores[i].score);
            PlayerPrefs.SetString("TopScorePlayerName" + i, playerScores[i].playerName);
        }
    }

    private void LoadScores()
    {
        playerScores.Clear();
        playerScores.Add(new PlayerScore
        {
            playerName = PlayerPrefs.GetString("BestScorePlayerName", "Player"),
            score = PlayerPrefs.GetInt("BestScore", 0)
        });
        for (int i = 1; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("TopScore" + i))
            {
                playerScores.Add(new PlayerScore
                {
                    playerName = PlayerPrefs.GetString("TopScorePlayerName" + i, "Player"),
                    score = PlayerPrefs.GetInt("TopScore" + i, 0)
                });
            }
        }
        UpdateTopScores();
    }
    private void UpdateTopScores()
    {
        topScoresText.text = "";
        for (int i = 0; i < playerScores.Count; i++)
        {
            topScoresText.text += (i + 1) + ". " + playerScores[i].playerName + ": " + playerScores[i].score + "\n";
        }
    }
}
