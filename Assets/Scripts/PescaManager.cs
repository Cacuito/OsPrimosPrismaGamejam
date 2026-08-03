using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        // Garante que o placar exibe a pontuação total salva no início
        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }

        // Deixa a UI inicial pronta
        if (playButton != null) playButton.SetActive(true);
        if (gameUI != null) gameUI.SetActive(false);
    }

    void Update()
    {
        if (jogando)
        {
            // Atualiza o tempo
            tempoRestante -= Time.deltaTime;

            if (timeText != null)
            {
                timeText.text = $"{(int)tempoRestante / 60}:{(int)tempoRestante % 60:D2}";
            }

            // Atualiza o placar global
            if (scoreText != null)
            {
                scoreText.text = $"{ScoreManager.Pontos}";
            }

            // Fim do tempo
            if (tempoRestante <= 0)
            {
                tempoRestante = 0;
                EncerrarPartida();
            }
        }
    }

    // Função para ligar no evento OnClick() do seu Botão de Play
    public void StartGame()
    {
        MoralSystem.AdicionarMoral(10, "D");
        MoralSystem.AdicionarMoral(-15, "S");

        tempoRestante = tempoDeJogo;
        jogando = true;

        // Ajusta a UI
        if (playButton != null) playButton.SetActive(false);
        if (gameUI != null) gameUI.SetActive(true);

        // Destrava a vara e o anzol para o jogador conseguir pescar
        if (anzolScript != null)
        {
            anzolScript.IniciarJogoAnzol();
        }

        // Inicia o spawn contínuo de peixes
        if (corrotinaSpawns != null) StopCoroutine(corrotinaSpawns);
        corrotinaSpawns = StartCoroutine(SpawnPeixesRoutine());
    }

    public void EncerrarPartida()
    {
        jogando = false;

        // Para de gerar novos peixes
        if (corrotinaSpawns != null)
        {
            StopCoroutine(corrotinaSpawns);
        }

        // Trava o anzol e a vara
        if (anzolScript != null)
        {
            anzolScript.BloquearAnzol();
        }

        // Remove os peixes que sobraram nadando na tela
        LimparPeixesDaTela();

        // Reexibe o botão de Play para poder jogar de novo
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

                // Garante que o peixe saiba para qual lado nadar de acordo com o spawn
                MovimentoPescaGame movimento = peixe.GetComponent<MovimentoPescaGame>();
                if (movimento != null)
                {
                    // Spawns do índice 4 em diante (spawns 5 a 9) vão para a esquerda (false)
                    bool irParaDireita = spawnIndex < 4;
                    movimento.SetDirecaoInicial(irParaDireita);
                }
            }

            yield return new WaitForSeconds(2f); // Tempo entre o nascimento de cada peixe
        }
    }

    private void LimparPeixesDaTela()
    {
        // Procura todos os peixes que ficaram soltos na tela e os destrói
        GameObject[] peixesRestantes = GameObject.FindGameObjectsWithTag("Fish");
        foreach (GameObject peixe in peixesRestantes)
        {
            Destroy(peixe);
        }
    }
}