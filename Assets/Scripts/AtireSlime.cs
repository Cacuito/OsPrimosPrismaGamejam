using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AcerteSlime : MonoBehaviour
{
    [SerializeField] public GameObject slime;
    [SerializeField] public int quantidadeSlimes;
    [SerializeField] float velocidadeMin;
    [SerializeField] float velocidadeMax;

    [Header("UI Objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TextMeshProUGUI scoreText;

    private bool playing = false;
    private int scoreNaRodada = 0;
    private int slimesAtivos = 0;
    private int slimesParaSpawnar;

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

            if (intervaloAtual >= intervaloSlime && slimesParaSpawnar > 0)
            {
                SpawnSlime();
                intervaloSlime = Random.Range(0, 2);
                intervaloAtual = 0;
            }
        }
    }

    void SpawnSlime()
    {
        if (slimesParaSpawnar > 0)
        {
            int randomInt = Random.Range(1, 7);
            if (randomInt == 1)
            {
                Instantiate(slime, new Vector3(-11f, 2.5f, 0f), Quaternion.identity);
            }
            else if (randomInt == 2)
            {
                Instantiate(slime, new Vector3(-11f, 0f, 0f), Quaternion.identity);
            }
            else if (randomInt == 3)
            {
                Instantiate(slime, new Vector3(-11f, -2.5f, 0f), Quaternion.identity);
            }
            else if (randomInt == 4)
            {
                Instantiate(slime, new Vector3(11f, 2.5f, 0f), Quaternion.identity);
            }
            else if (randomInt == 5)
            {
                Instantiate(slime, new Vector3(11f, 0f, 0f), Quaternion.identity);
            }
            else
            {
                Instantiate(slime, new Vector3(11f, -2.5f, 0f), Quaternion.identity);
            }
            
            slimesParaSpawnar--;
            slimesAtivos++;
        }
    }

    public void IniciarJogo()
    {
        slimesParaSpawnar = quantidadeSlimes;
        scoreNaRodada = 0;
        slimesAtivos = 0;
        playButton.SetActive(false);
        gameUI.SetActive(true);

        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }

        playing = true;
    }

    public void AddScore(int pontosPerGoblin)
    {
        ScoreManager.AdicionarPontos(pontosPerGoblin);
        scoreNaRodada++;

        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }
    }

    public void SlimeDestruido()
    {
        slimesAtivos--;
        
        if (slimesParaSpawnar <= 0 && slimesAtivos <= 0)
        {
            FinalizarJogo();
        }
    }

    private void FinalizarJogo()
    {
        playing = false;
        gameUI.SetActive(false);
        playButton.SetActive(true);
    }

    public void SairFase()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}