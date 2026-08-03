using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public string nome;
    private bool playerEstaNoNPC = false;

    [SerializeField] private GameObject space;
    [SerializeField] private GameObject dialogo;
    [SerializeField] private PlayerMoviment player;

    void Update()
    {
        if (playerEstaNoNPC && Input.GetKeyDown(KeyCode.Space))
        {
            dialogo.SetActive(true);
            space.SetActive(false);
            player.podeMover = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEstaNoNPC = true;
            if (space != null) space.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEstaNoNPC = false;
            if (space != null) space.SetActive(false);
        }
    }
}
