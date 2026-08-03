using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] falas;
    public float velocidadeTexto;
    public GameObject personagem;
    [SerializeField] public PlayerMoviment player;
    [SerializeField] public GameObject canvas;

    public TextMeshProUGUI textoMoral;
    public string idPersonagem;

    [SerializeField] public bool npcBarraca;
    [SerializeField] private CameraZoom scriptDeZoom;
    
    private int index;
    private bool posJogo = false;
    private GameObject btnIniciarTemp;
    private GameObject btnSairTemp;

    void Start()
    {
        textComponent.text = string.Empty;
        ComecarDialogo();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == falas[index])
            {
                ProximaFala();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = falas[index];
            }
        }

        if(textoMoral != null && idPersonagem != null)
        {
            if (idPersonagem == "D")
            {
                textoMoral.text = $"{MoralSystem.moralD}";
            }
            else if (idPersonagem == "S")
            {
                textoMoral.text = $"{MoralSystem.moralS}";
            }
            else if (idPersonagem == "G")
            {
                textoMoral.text = $"{MoralSystem.moralG}";
            }
        }
    }

    void ComecarDialogo()
    {
        index = 0;
        StartCoroutine(DigitaFala());
    }

    IEnumerator DigitaFala()
    {
        foreach (char c in falas[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(velocidadeTexto);
        }
    }

    void ProximaFala()
    {
        if (index < falas.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(DigitaFala());
        }
        else
        {
            if(canvas)
            {
                canvas.SetActive(true);
            }
            
            gameObject.SetActive(false);
            if (personagem != null)
            {
                personagem.SetActive(false);
            }

            if(player)
            {
                player.podeMover = true;
            }

            if (posJogo)
            {
                if (btnIniciarTemp != null) btnIniciarTemp.SetActive(true);
                if (btnSairTemp != null) btnSairTemp.SetActive(true);
                posJogo = false;

                if (scriptDeZoom != null)
                {
                    scriptDeZoom.IniciarZoomParaMinigame();
                }
            }
            else if (npcBarraca)
            {
               if (scriptDeZoom != null)
                {
                    scriptDeZoom.IniciarZoomParaMinigame();
                }
            }
        }
    }

    public void IniciarDialogoPosJogo(string fala, GameObject btnIniciar, GameObject btnSair)
    {
        falas = new string[] { fala };
        index = 0;
        posJogo = true;
        btnIniciarTemp = btnIniciar;
        btnSairTemp = btnSair;

        gameObject.SetActive(true);
        if (personagem != null)
        {
            personagem.SetActive(true);
        }

        if (scriptDeZoom != null)
        {
            scriptDeZoom.VoltarZoomOriginal();
        }
        
        textComponent.text = string.Empty;
        StartCoroutine(DigitaFala());
    }
}