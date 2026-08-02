using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] float velocidadeMin;
    [SerializeField] float velocidadeMax;
    float velocidade;
    bool direitaOuEsquerda;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
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

    public void AndarDireita ()
    {
        transform.Translate(Vector3.right * velocidade * Time.deltaTime);

        if (transform.position.x > 11f)
        {
            Destroy(gameObject);
        }
    }

    public void AndarEsquerda ()
    {
        transform.Translate(Vector3.left * velocidade * Time.deltaTime);

        if (transform.position.x < -11f)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
