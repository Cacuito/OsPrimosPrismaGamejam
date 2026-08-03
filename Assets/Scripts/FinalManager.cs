using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalManager : MonoBehaviour
{
    public string id;

    [Header("Objetos de Final")]
    [SerializeField] private GameObject finalBom;
    [SerializeField] private GameObject finalRuim;

    [Header("Personagens")]
    [SerializeField] private GameObject personagemBom;
    [SerializeField] private GameObject personagemRuim;

    [Header("Sistemas de Diálogo Separados")]
    [SerializeField] private Dialogo dialogoFinalBom;
    [SerializeField] private Dialogo dialogoFinalRuim;

    [Header("Configuração de Cena")]
    [SerializeField] private string nomeCenaMenu = "MenuInicial";

    void Start()
    {
        IniciarCenaFinal();
    }

    void IniciarCenaFinal()
    {
        bool eFinalBom = false;

        if (id == "D")
        {
            eFinalBom = (MoralSystem.moralD >= 100);
        }
        else if (id == "S")
        {
            eFinalBom = (MoralSystem.moralS >= 100);
        }
        else if (id == "G")
        {
            eFinalBom = (MoralSystem.moralG >= 100);
        }

        Dialogo dialogoAtivo = null;

        if (eFinalBom)
        {
            if (finalBom != null) finalBom.SetActive(true);
            if (personagemBom != null) personagemBom.SetActive(true);
            dialogoAtivo = dialogoFinalBom;
        }
        else
        {
            if (finalRuim != null) finalRuim.SetActive(true);
            if (personagemRuim != null) personagemRuim.SetActive(true);
            dialogoAtivo = dialogoFinalRuim;
        }

        if (dialogoAtivo != null)
        {
            StartCoroutine(AguardarFimDoDialogo(dialogoAtivo));
        }
        else
        {
            Debug.LogWarning("Nenhum script de Diálogo foi atribuído no Inspector do FinalManager!");
        }
    }

    private IEnumerator AguardarFimDoDialogo(Dialogo dialogo)
    {
        yield return new WaitUntil(() => dialogo.gameObject.activeSelf);

        yield return new WaitWhile(() => dialogo.gameObject.activeSelf);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(nomeCenaMenu);
    }
}