using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float startingTime = 30f;
    private float timeRemaining;
    private HashSet<GoblinScript> currentGoblins = new HashSet<GoblinScript>();
    private bool playing = false;

    // Guarda apenas os acertos desta partida específica para controlar a dificuldade
    private int scoreNaRodada = 0;

    void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }
    }

    public void StartGame()
    {
        playing = false;

        // Reset da pontuação da rodada atual
        scoreNaRodada = 0;

        for (int i = 0; i < goblins.Count; i++)
        {
            goblins[i].StopGame();
            goblins[i].SetIndex(i);
        }

        currentGoblins.Clear();

        playButton.SetActive(false);
        outOfTimeText.SetActive(false);
        bombText.SetActive(false);
        gameUI.SetActive(true);

        timeRemaining = startingTime;

        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }

        playing = true;
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

            // A quantidade de goblins em tela agora sobe gradualmente conforme você joga A RODADA
            int nivelDificuldade = scoreNaRodada / 10;

            if (currentGoblins.Count <= nivelDificuldade)
            {
                int index = Random.Range(0, goblins.Count);
                if (!currentGoblins.Contains(goblins[index]))
                {
                    currentGoblins.Add(goblins[index]);
                    goblins[index].Activate(nivelDificuldade, this);
                }
            }
        }
    }

    public void AddScore(int goblinIndex, int pontosPerGoblin)
    {
        // 1. Aumenta a pontuação salva no PlayerPrefs
        ScoreManager.AdicionarPontos(pontosPerGoblin);

        // 2. Aumenta a pontuação da rodada interna
        scoreNaRodada++;

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

    public void SairFase()
    {
        SceneManager.LoadScene("SampleScene");
    }
}