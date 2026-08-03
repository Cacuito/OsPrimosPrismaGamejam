using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct LinhaDialogo
{
    [TextArea(2, 5)]
    public string texto;
    public bool falaDoJogador;
}

public class Dialogo : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public LinhaDialogo[] falas;
    public float velocidadeTexto;
    public GameObject personagem;
    [SerializeField] public PlayerMoviment player;
    [SerializeField] public GameObject canvas;

    public TextMeshProUGUI textoMoral;
    public string idPersonagem;

    [SerializeField] public bool npcBarraca;
    [SerializeField] private CameraZoom scriptDeZoom;
    
    public Color corNPC = Color.white;
    public Color corJogador = Color.cyan;

    private int index;
    private bool posJogo = false;
    private GameObject btnIniciarTemp;
    private GameObject btnSairTemp;

    void Start()
    {
        textComponent.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == falas[index].texto)
            {
                ProximaFala();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = falas[index].texto;
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

    IEnumerator DigitaFala()
    {
        if (falas[index].falaDoJogador)
        {
            textComponent.color = corJogador;
        }
        else
        {
            textComponent.color = corNPC;
        }

        foreach (char c in falas[index].texto.ToCharArray())
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
            EncerrarDialogo();
        }
    }

    private void EncerrarDialogo()
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

    public void IniciarDialogo(LinhaDialogo[] novasFalas)
    {
        falas = novasFalas;
        index = 0;
        posJogo = false;
        textComponent.text = string.Empty;
        
        // Garante que o objeto pai (Canvas) esteja ativo na cena
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
        
        if (personagem != null)
        {
            personagem.SetActive(true);
        }

        StartCoroutine(DigitaFala());
    }

    public void IniciarDialogoPosJogo(string fala, GameObject btnIniciar, GameObject btnSair)
    {
        falas = new LinhaDialogo[1];
        falas[0] = new LinhaDialogo { texto = fala, falaDoJogador = false };
        index = 0;
        posJogo = true;
        btnIniciarTemp = btnIniciar;
        btnSairTemp = btnSair;

        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(true);
        }

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