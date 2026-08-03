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

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        ComecarDialogo();
    }

    // Update is called once per frame
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

    IEnumerator DigitaFala ()
    {
        //Digita uma letra de cada vez
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
            personagem.SetActive(false);
            player.podeMover = true;
        }
    }
}
