using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterPhase : MonoBehaviour
{
    public string nome;
    private bool playerEstaNoPortal = false;

    [SerializeField] private GameObject space;

    void Update()
    {
        if (playerEstaNoPortal && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nome);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEstaNoPortal = true;
            if (space != null) space.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEstaNoPortal = false;
            if (space != null) space.SetActive(false);
        }
    }
}
