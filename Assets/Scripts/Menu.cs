using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] private Button botaoContinuar;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject config;

    // Start is called before the first frame update
    void Start()
    {
        VerificarSalvamento();
    }

    // Update is called once per frame
    void VerificarSalvamento()
    {
        bool temJogoSalvo = PlayerPrefs.HasKey("PontosSalvos");

        if (botaoContinuar != null)
        {
            botaoContinuar.interactable = temJogoSalvo;
        }
    }

    public void Continuar()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Reiniciar()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        SceneManager.LoadScene("SampleScene");
    }

    public void SairJogo()
    {
        Application.Quit();
    }

    public void AbrirConfig()
    {
        menu.SetActive(false);
        config.SetActive(true);
    }

    public void FecharConfig()
    {
        menu.SetActive(true);
        config.SetActive(false);
    }
}
