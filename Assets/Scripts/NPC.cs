using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string nome;
    private bool playerEstaNoNPC = false;

    [SerializeField] private GameObject space;

    void Update()
    {
        if (playerEstaNoNPC && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nome);
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
