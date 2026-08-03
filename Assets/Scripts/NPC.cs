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

            switch (estadoAtual)
            {
                case EstadoInteracaoNPC.MinigameBom:
                    scriptDialogo.IniciarDialogo(dialogoPosMinigameBom);
                    break;
                case EstadoInteracaoNPC.MinigameNeutro:
                    scriptDialogo.IniciarDialogo(dialogoPosMinigameNeutro);
                    break;
                case EstadoInteracaoNPC.MinigameRuim:
                    scriptDialogo.IniciarDialogo(dialogoPosMinigameRuim);
                    break;
                case EstadoInteracaoNPC.ComprouComida:
                    scriptDialogo.IniciarDialogo(dialogoPosComprarComida);
                    break;
                case EstadoInteracaoNPC.Idle:
                    scriptDialogo.IniciarDialogo(dialogoIdle);
                    break;
                case EstadoInteracaoNPC.Inicial:
                default:
                    scriptDialogo.IniciarDialogo(dialogoInicial);
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