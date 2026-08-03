using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] float velocidadeMin;
    [SerializeField] float velocidadeMax;
    float velocidade;
    bool direitaOuEsquerda;
    private AcerteSlime gameManager;
    public int pontoPorSlime = 5;

    void Start()
    {
        gameManager = FindObjectOfType<AcerteSlime>();

        velocidade = Random.Range(velocidadeMin, velocidadeMax);
        if (transform.position.x == 11)
        {
            direitaOuEsquerda = true;
        }
        else
        {
            direitaOuEsquerda = false;
        }
    }

    void Update()
    {
        if (direitaOuEsquerda)
        {
            AndarEsquerda();
        }
        else
        {
            AndarDireita();
        }
    }

    public void AndarDireita()
    {
        transform.Translate(Vector3.right * velocidade * Time.deltaTime);

        if (transform.position.x > 11f)
        {
            RemoverSlime();
        }
    }

    public void AndarEsquerda()
    {
        transform.Translate(Vector3.left * velocidade * Time.deltaTime);

        if (transform.position.x < -11f)
        {
            RemoverSlime();
        }
    }

    void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.AddScore(pontoPorSlime);
        }
        RemoverSlime();
    }

    private void RemoverSlime()
    {
        Destroy(gameObject);
    }
}