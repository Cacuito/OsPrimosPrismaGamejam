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
    public AnzolPesca anzolScript; 

    [Header("Interface da UI")]
    public GameObject playButton;
    public GameObject botaoSair; 
    public GameObject gameUI;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;

    [Header("Diálogo e Câmera")]
    public Dialogo scriptDialogo;
    public LinhaDialogo[] dialogoInicial;
    public float zoomCameraSize = 3.5f; 
    public float velocidadeZoom = 2f;

    [Header("Configurações da Partida")]
    public float tempoDeJogo = 30f;

    private float tempoRestante;
    private bool jogando = false;
    private Coroutine corrotinaSpawns;

    private int pontosIniciais; 
    private float tamanhoCameraOriginal;

    void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{ScoreManager.Pontos}";
        }

        if (playButton != null) playButton.SetActive(true);
        if (botaoSair != null) botaoSair.SetActive(true);
        if (gameUI != null) gameUI.SetActive(false);

        if (Camera.main != null)
        {
            tamanhoCameraOriginal = Camera.main.orthographicSize;
        }
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

    public void AoClicarEmJogar()
    {
        if (playButton != null) playButton.SetActive(false);
        if (botaoSair != null) botaoSair.SetActive(false);
        
        StartCoroutine(RotinaInicialEZoom());
    }

    private IEnumerator RotinaInicialEZoom()
    {
        Debug.Log("1. Rotina de Zoom Iniciada. Esperando diálogo...");

        if (scriptDialogo != null && dialogoInicial != null && dialogoInicial.Length > 0)
        {
            scriptDialogo.IniciarDialogo(dialogoInicial);
            yield return new WaitUntil(() => !scriptDialogo.gameObject.activeInHierarchy);
        }

        Debug.Log("2. Diálogo concluído. Verificando Câmera...");

        if (Camera.main != null)
        {
            float tamanhoAtual = Camera.main.orthographicSize;
            float t = 0f;

            Debug.Log($"3. Câmera encontrada! Tamanho atual: {tamanhoAtual} | Indo para: {zoomCameraSize}");

            while (t < 1f)
            {
                // Usando unscaledDeltaTime para ignorar se o jogo estiver pausado (TimeScale = 0)
                t += Time.unscaledDeltaTime * velocidadeZoom;
                Camera.main.orthographicSize = Mathf.Lerp(tamanhoAtual, zoomCameraSize, t);
                yield return null;
            }
            Camera.main.orthographicSize = zoomCameraSize;
            Debug.Log("4. Zoom In concluído com sucesso!");
        }
        else
        {
            Debug.LogError("ERRO: Camera.main não foi encontrada. Verifique se a câmera tem a tag 'MainCamera'.");
        }

        StartGame();
    }

    private IEnumerator RotinaZoomOut()
    {
        if (Camera.main != null)
        {
            float tamanhoAtual = Camera.main.orthographicSize;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime * velocidadeZoom;
                Camera.main.orthographicSize = Mathf.Lerp(tamanhoAtual, tamanhoCameraOriginal, t);
                yield return null;
            }
            Camera.main.orthographicSize = tamanhoCameraOriginal;
        }
    }

    private void StartGame()
    {
        MoralSystem.AdicionarMoral(10, "D");
        MoralSystem.AdicionarMoral(-15, "S");

        pontosIniciais = ScoreManager.Pontos; 
        tempoRestante = tempoDeJogo;
        jogando = true;

        if (playButton != null) playButton.SetActive(false);
        if (botaoSair != null) botaoSair.SetActive(false);
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

        if (corrotinaSpawns != null) StopCoroutine(corrotinaSpawns);
        if (anzolScript != null) anzolScript.BloquearAnzol();

        LimparPeixesDaTela();
        if (gameUI != null) gameUI.SetActive(false);

        StartCoroutine(RotinaZoomOut());

        int pontosGanhos = ScoreManager.Pontos - pontosIniciais;
        string falaFinal = "";

        if (pontosGanhos >= 100)
        {
            falaFinal = $" Nossa! Macacos me mordam, voce é um eximio pescador!";
        }
        else if (pontosGanhos > 50)
        {
            falaFinal = $"Boa pescaria marujo, voce foi bem nessa.";
        }
        else
        {
            falaFinal = "Poxa… Tenta de novo, na proxima voce consegue!";
        }

        NPC.estadosGlobais["Sereia"] = EstadoInteracaoNPC.MinigameRuim;
        NPC.estadosGlobais["Golem"] = EstadoInteracaoNPC.MinigameNeutro;
        NPC.estadosGlobais["Dragótica"] = EstadoInteracaoNPC.MinigameBom;

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