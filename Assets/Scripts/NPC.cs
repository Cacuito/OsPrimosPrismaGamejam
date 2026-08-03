using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoInteracaoNPC
{
    Inicial,
    Idle,
    MinigameBom,
    MinigameNeutro,
    MinigameRuim,
    ComprouComida
}

public class NPC : MonoBehaviour
{
    public string nome;
    private bool playerEstaNoNPC = false;

    [SerializeField] private GameObject space;
    [SerializeField] private Dialogo scriptDialogo;
    [SerializeField] private PlayerMoviment player;

    [Header("Expressões Visuais do NPC")]
    public Sprite spriteNeutro;
    public Sprite spriteFeliz;
    public Sprite spriteChateado;

    [Header("Diálogos do NPC")]
    public LinhaDialogo[] dialogoInicial;
    public LinhaDialogo[] dialogoIdle;
    public LinhaDialogo[] dialogoPosMinigameBom;
    public LinhaDialogo[] dialogoPosMinigameNeutro;
    public LinhaDialogo[] dialogoPosMinigameRuim;
    public LinhaDialogo[] dialogoPosComprarComida;

    public static Dictionary<string, EstadoInteracaoNPC> estadosGlobais = new Dictionary<string, EstadoInteracaoNPC>();

    public EstadoInteracaoNPC estadoAtual
    {
        get
        {
            if (estadosGlobais.ContainsKey(nome))
            {
                return estadosGlobais[nome];
            }
            return EstadoInteracaoNPC.Inicial; 
        }
        set
        {
            estadosGlobais[nome] = value;
        }
    }

    void Update()
    {
        if (playerEstaNoNPC && space != null && !space.activeSelf && !scriptDialogo.gameObject.activeInHierarchy)
        {
            space.SetActive(true);
        }

        if (playerEstaNoNPC && Input.GetKeyDown(KeyCode.Space))
        {
            if (scriptDialogo.gameObject.activeInHierarchy) return;

            space.SetActive(false);
            player.podeMover = false;

            // Define qual sprite será usado baseado no estado
            Sprite spriteAtual = spriteNeutro; 

            switch (estadoAtual)
            {
                case EstadoInteracaoNPC.MinigameBom:
                case EstadoInteracaoNPC.ComprouComida:
                    spriteAtual = spriteFeliz;
                    break;
                case EstadoInteracaoNPC.MinigameRuim:
                    spriteAtual = spriteChateado;
                    break;
                case EstadoInteracaoNPC.MinigameNeutro:
                case EstadoInteracaoNPC.Idle:
                case EstadoInteracaoNPC.Inicial:
                    spriteAtual = spriteNeutro;
                    break;
            }

            // Inicia o diálogo passando o texto e a expressão visual correta
            switch (estadoAtual)
            {
                case EstadoInteracaoNPC.MinigameBom:
                    scriptDialogo.IniciarDialogo(dialogoPosMinigameBom, spriteAtual);
                    break;
                case EstadoInteracaoNPC.MinigameNeutro:
                    scriptDialogo.IniciarDialogo(dialogoPosMinigameNeutro, spriteAtual);
                    break;
                case EstadoInteracaoNPC.MinigameRuim:
                    scriptDialogo.IniciarDialogo(dialogoPosMinigameRuim, spriteAtual);
                    break;
                case EstadoInteracaoNPC.ComprouComida:
                    scriptDialogo.IniciarDialogo(dialogoPosComprarComida, spriteAtual);
                    break;
                case EstadoInteracaoNPC.Idle:
                    scriptDialogo.IniciarDialogo(dialogoIdle, spriteAtual);
                    break;
                case EstadoInteracaoNPC.Inicial:
                default:
                    scriptDialogo.IniciarDialogo(dialogoInicial, spriteAtual);
                    estadoAtual = EstadoInteracaoNPC.Idle;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEstaNoNPC = true;
            if (space != null && !scriptDialogo.gameObject.activeInHierarchy)
            {
                space.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEstaNoNPC = false;
            if (space != null)
            {
                space.SetActive(false);
            }
        }
    }
}