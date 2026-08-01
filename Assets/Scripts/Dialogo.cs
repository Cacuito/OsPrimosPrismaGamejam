using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] falas;
    public float velocidadeTexto;

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
            gameObject.SetActive(false);
        }
    }
}
