using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoblinGameManager : MonoBehaviour
{
    [SerializeField] private List<GoblinScript> goblins;

    [Header("UI Objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject botaoSair;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject bombText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Diálogo")]
    public Dialogo scriptDialogo;

    public float startingTime = 30f;
    private float timeRemaining;
    private HashSet<GoblinScript> currentGoblins = new HashSet<GoblinScript>();
    private bool playing = false;

    private int scoreNaRodada = 0;
    private int pontosIniciais;

    void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }

        if (playButton != null) playButton.SetActive(true);
        if (botaoSair != null) botaoSair.SetActive(true);
        if (gameUI != null) gameUI.SetActive(false);
    }

    public void StartGame()
    {
        MoralSystem.AdicionarMoral(10, "S");
        MoralSystem.AdicionarMoral(-15, "G");

        playing = false;
        scoreNaRodada = 0;
        pontosIniciais = ScoreManager.Pontos;

        for (int i = 0; i < goblins.Count; i++)
        {
            goblins[i].StopGame();
            goblins[i].SetIndex(i);
        }

        currentGoblins.Clear();

        if (playButton != null) playButton.SetActive(false);
        if (botaoSair != null) botaoSair.SetActive(false);
        if (outOfTimeText != null) outOfTimeText.SetActive(false);
        if (bombText != null) bombText.SetActive(false);
        if (gameUI != null) gameUI.SetActive(true);

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
            if (outOfTimeText != null) outOfTimeText.SetActive(true);
        }
        else
        {
            if (bombText != null) bombText.SetActive(true);
        }

        foreach (GoblinScript goblin in goblins)
        {
            goblin.StopGame();
        }

        playing = false;
        if (gameUI != null) gameUI.SetActive(false);

        int pontosGanhos = ScoreManager.Pontos - pontosIniciais;
        string falaFinal = "";
        EstadoInteracaoNPC resultadoMinigame;

        if (pontosGanhos >= 100)
        {
            falaFinal = "Isso ai! Esses Goblins aprenderam a licao!";
            resultadoMinigame = EstadoInteracaoNPC.MinigameBom;
        }
        else if (pontosGanhos > 50)
        {
            falaFinal = "Foi quase bom. Treine mais para ir melhor no proximo festival.";
            resultadoMinigame = EstadoInteracaoNPC.MinigameNeutro;
        }
        else
        {
            falaFinal = "Parece que voce tem pena dos Goblin.  Nao precisa poupa-los, eles nem se machucam…";
            resultadoMinigame = EstadoInteracaoNPC.MinigameRuim;
        }

        NPC.estadosGlobais["Dragótica"] = resultadoMinigame;
        NPC.estadosGlobais["Sereia"] = EstadoInteracaoNPC.Idle;
        NPC.estadosGlobais["Golem"] = EstadoInteracaoNPC.Idle;

        if (scriptDialogo != null)
        {
            scriptDialogo.IniciarDialogoPosJogo(falaFinal, playButton, botaoSair);
        }
        else
        {
            if (playButton != null) playButton.SetActive(true);
            if (botaoSair != null) botaoSair.SetActive(true);
        }
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
        ScoreManager.AdicionarPontos(pontosPerGoblin);

        scoreNaRodada++;

        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }

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