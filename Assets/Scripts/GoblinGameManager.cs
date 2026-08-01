using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoblinGameManager : MonoBehaviour
{
    [SerializeField] private List<GoblinScript> goblins;

    [Header("UI Objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject bombText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private float startingTime = 30f;
    private float timeRemaining;
    private HashSet<GoblinScript> currentGoblins = new HashSet<GoblinScript>();
    private bool playing = false;

    void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }
    }

    public void StartGame()
    {
        playButton.SetActive(false);
        outOfTimeText.SetActive(false);
        bombText.SetActive(false);
        gameUI.SetActive(true);

        for (int i = 0; i < goblins.Count; i++)
        {
            goblins[i].Hide();
            goblins[i].SetIndex(i);
        }

        currentGoblins.Clear();
        timeRemaining = startingTime;
        playing = true;

        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }
    }

    public void GameOver(int type)
    {
        if (type == 0)
        {
            outOfTimeText.SetActive(true);
        }
        else
        {
            bombText.SetActive(true);
        }

        foreach (GoblinScript goblin in goblins)
        {
            goblin.StopGame();
        }

        playing = false;
        playButton.SetActive(true);
    }

    void Update()
    {
        if (playing)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver(0);
            }

            if (timeText != null)
            {
                timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            }

            int nivel = ScoreManager.Pontos / 10;

            if (currentGoblins.Count <= nivel)
            {
                int index = Random.Range(0, goblins.Count);
                if (!currentGoblins.Contains(goblins[index]))
                {
                    currentGoblins.Add(goblins[index]);
                    goblins[index].Activate(nivel, this);
                }
            }
        }
    }

    public void AddScore(int goblinIndex)
    {
        ScoreManager.AdicionarPontos(1);

        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }

        timeRemaining += 1f;
        currentGoblins.Remove(goblins[goblinIndex]);
    }

    public void Missed(int goblinIndex, bool isGoblin)
    {
        if (isGoblin)
        {
            timeRemaining -= 2f;
        }

        currentGoblins.Remove(goblins[goblinIndex]);
    }
}