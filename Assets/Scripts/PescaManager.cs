using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PescaManager : MonoBehaviour
{
    [Header("Prefabs e Spawns")]
    public GameObject[] peixes;
    public GameObject[] spawns;

    [Header("Referências da Partida")]
    public AnzolPesca anzolScript; // Arraste o GameObject do Anzol que possui o script AnzolPesca

    [Header("Interface da UI")]
    public GameObject playButton;
    public GameObject gameUI;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;

    [Header("Configurações da Partida")]
    public float tempoDeJogo = 30f;

    private float tempoRestante;
    private bool jogando = false;
    private Coroutine corrotinaSpawns;

    void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }

        if (playButton != null) playButton.SetActive(true);
        if (gameUI != null) gameUI.SetActive(false);
    }

    void Update()
    {
        if (jogando)
        {
            tempoRestante -= Time.deltaTime;

            if (timeText != null)
            {
                timeText.text = $"{(int)tempoRestante / 60}:{(int)tempoRestante % 60:D2}";
            }

            if (scoreText != null)
            {
                scoreText.text = $"{ScoreManager.Pontos}";
            }

            if (tempoRestante <= 0)
            {
                tempoRestante = 0;
                EncerrarPartida();
            }
        }
    }

    public void StartGame()
    {
        MoralSystem.AdicionarMoral(10, "D");
        MoralSystem.AdicionarMoral(-15, "S");

        tempoRestante = tempoDeJogo;
        jogando = true;

        if (playButton != null) playButton.SetActive(false);
        if (gameUI != null) gameUI.SetActive(true);

        if (anzolScript != null)
        {
            anzolScript.IniciarJogoAnzol();
        }

        if (corrotinaSpawns != null) StopCoroutine(corrotinaSpawns);
        corrotinaSpawns = StartCoroutine(SpawnPeixesRoutine());
    }

    public void EncerrarPartida()
    {
        jogando = false;

        if (corrotinaSpawns != null)
        {
            StopCoroutine(corrotinaSpawns);
        }

        if (anzolScript != null)
        {
            anzolScript.BloquearAnzol();
        }

        LimparPeixesDaTela();

        if (playButton != null) playButton.SetActive(true);

        NPC.estadosGlobais["Sereia"] = EstadoInteracaoNPC.MinigameRuim;
        NPC.estadosGlobais["Golem"] = EstadoInteracaoNPC.MinigameNeutro;
        NPC.estadosGlobais["Dragótica"] = EstadoInteracaoNPC.MinigameBom;
    }

    private IEnumerator SpawnPeixesRoutine()
    {
        while (jogando)
        {
            if (peixes.Length > 0 && spawns.Length > 0)
            {
                int peixeIndex = Random.Range(0, peixes.Length);
                int spawnIndex = Random.Range(0, spawns.Length);

                GameObject peixe = Instantiate(peixes[peixeIndex], spawns[spawnIndex].transform.position, Quaternion.identity);
                peixe.transform.parent = this.transform;

                MovimentoPescaGame movimento = peixe.GetComponent<MovimentoPescaGame>();
                if (movimento != null)
                {
                    bool irParaDireita = spawnIndex < 4;
                    movimento.SetDirecaoInicial(irParaDireita);
                }
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private void LimparPeixesDaTela()
    {
        GameObject[] peixesRestantes = GameObject.FindGameObjectsWithTag("Fish");
        foreach (GameObject peixe in peixesRestantes)
        {
            Destroy(peixe);
        }
    }

    public void SairFase()
    {
        SceneManager.LoadScene("SampleScene");
    }
}