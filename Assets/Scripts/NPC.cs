using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoInteracaoNPC
{
    Neutro,
    MinigameBom,
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
    public LinhaDialogo[] dialogoNeutro;
    public LinhaDialogo[] dialogoPosMinigameBom;
    public LinhaDialogo[] dialogoPosMinigameRuim;
    public LinhaDialogo[] dialogoPosComprarComida;

    public static EstadoInteracaoNPC estadoGlobal = EstadoInteracaoNPC.Neutro;

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

            switch (estadoGlobal)
            {
                case EstadoInteracaoNPC.MinigameBom:
                    scriptDialogo.IniciarDialogo(dialogoPosMinigameBom);
                    break;
                case EstadoInteracaoNPC.MinigameRuim:
                    scriptDialogo.IniciarDialogo(dialogoPosMinigameRuim);
                    break;
                case EstadoInteracaoNPC.ComprouComida:
                    scriptDialogo.IniciarDialogo(dialogoPosComprarComida);
                    break;
                case EstadoInteracaoNPC.Neutro:
                default:
                    scriptDialogo.IniciarDialogo(dialogoNeutro);
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