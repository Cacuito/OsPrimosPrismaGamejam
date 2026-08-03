using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AcerteSlime : MonoBehaviour
{
    [SerializeField] public GameObject slimeAzul;
    [SerializeField] public GameObject slimeVermelho;
    [SerializeField] float velocidadeMin;
    [SerializeField] float velocidadeMax;

    [Header("UI Objects")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject botaoIniciar;
    [SerializeField] private GameObject botaoSair;
    [SerializeField] private GameObject cajado;

    [SerializeField] private Dialogo scriptDialogo;

    public float startingTime = 35f;
    private float timeRemaining;
    private bool playing = false;

    private int scoreNaRodada = 0;

    float intervaloAtual = 0f;
    float intervaloSlime = 0f;

    void Start()
    {
        intervaloSlime = Random.Range(0, 2);
        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }
    }

    void Update()
    {
        if (playing)
        {
            intervaloAtual += 1f * Time.deltaTime;

            if (intervaloAtual >= intervaloSlime)
            {
                SpawnSlime();
                intervaloSlime = Random.Range(0, 2);
                intervaloAtual = 0;
            }

            timeRemaining -= Time.deltaTime;

            if (timeText != null)
            {
                timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            }

            if (timeRemaining <= 0)
            {
                FinalizarJogo();
            }
        }
    }

    void SpawnSlime()
    {
        int randomSlime = Random.Range(0, 2);
        GameObject slimeToInstantiate;

        if (randomSlime == 0)
        {
            slimeToInstantiate = slimeAzul;
        }
        else
        {
            slimeToInstantiate = slimeVermelho;
        }

        int randomInt = Random.Range(1, 7);
        if (randomInt == 1)
        {
            Instantiate(slimeToInstantiate, new Vector3(-11f, 2.15f, 0f), Quaternion.identity);
        }
        else if (randomInt == 2)
        {
            Instantiate(slimeToInstantiate, new Vector3(-11f, 0f, 0f), Quaternion.identity);
        }
        else if (randomInt == 3)
        {
            Instantiate(slimeToInstantiate, new Vector3(-11f, -2.1f, 0f), Quaternion.identity);
        }
        else if (randomInt == 4)
        {
            Instantiate(slimeToInstantiate, new Vector3(11f, 2.15f, 0f), Quaternion.identity);
        }
        else if (randomInt == 5)
        {
            Instantiate(slimeToInstantiate, new Vector3(11f, 0f, 0f), Quaternion.identity);
        }
        else
        {
            Instantiate(slimeToInstantiate, new Vector3(11f, -2.1f, 0f), Quaternion.identity);
        }
    }

    public void IniciarJogo()
    {
        MoralSystem.AdicionarMoral(10, "G");
        MoralSystem.AdicionarMoral(-15, "D");

        scoreNaRodada = 0;
        gameUI.SetActive(true);
        timeRemaining = startingTime;

        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }

        playing = true;

        botaoIniciar.SetActive(false);
        botaoSair.SetActive(false);
        cajado.SetActive(true);
    }

    public void AddScore(int pontosSlime)
    {
        ScoreManager.AdicionarPontos(pontosSlime);
        
        if (pontosSlime > 0)
        {
            scoreNaRodada+= pontosSlime;
        }

        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }
    }

    private void FinalizarJogo()
    {
        playing = false;
        gameUI.SetActive(false);
        cajado.SetActive(false);

        Slime[] slimesNaTela = FindObjectsOfType<Slime>();
        foreach (Slime s in slimesNaTela)
        {
            Destroy(s.gameObject);
        }

        string falaFinal = "";
        
        if (scoreNaRodada >= 100)
        {
            falaFinal = "UAU!! Pelo os meus cálculos desse jeito não vou ter mais pontos/moedas para distribuir para os outros.";
        }
        else if (scoreNaRodada > 50)
        {
            falaFinal = "Boa! Mesmo não sendo o máximo, acho que essa quantidade de pontos/moedas já é suficiente! He He.";
        }
        else
        {
            falaFinal = "Eh... tenho certeza que você poderia ter ido melhor! Só não foi seu dia de sorte.";
        }

        NPC.estadosGlobais["Sereia"] = EstadoInteracaoNPC.MinigameNeutro;
        NPC.estadosGlobais["Golem"] = EstadoInteracaoNPC.MinigameBom;
        NPC.estadosGlobais["Dragótica"] = EstadoInteracaoNPC.MinigameRuim;

        if (scriptDialogo != null)
        {
            scriptDialogo.IniciarDialogoPosJogo(falaFinal, botaoIniciar, botaoSair);
        }
    }

    public void SairFase()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}