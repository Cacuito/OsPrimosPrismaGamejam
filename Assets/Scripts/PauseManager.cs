using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("Interface")]
    public GameObject painelDePause;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject config;

    private bool jogoPausado = false;

    private void Start()
    {
        painelDePause.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
            {
                Despausar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        painelDePause.SetActive(true); 
        Time.timeScale = 0f;           
        jogoPausado = true;
    }

    public void Despausar()
    {
        painelDePause.SetActive(false);
        Time.timeScale = 1f;           
        jogoPausado = false;
    }

    public void voltarMenu()
    {
        Time.timeScale = 1f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menuinicial");
    }

    public void VoltarMundo()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void AbrirConfig()
    {
        pause.SetActive(false);
        config.SetActive(true);
    }

    public void SairConfig()
    {
        pause.SetActive(true);
        config.SetActive(false);
    }
}