using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcerteSlime : MonoBehaviour
{
    [SerializeField] public GameObject slime;
    [SerializeField] public int quantidadeSlimes;
    [SerializeField] float velocidadeMin;
    [SerializeField] float velocidadeMax;
    float intervaloAtual = 0f;
    float intervaloSlime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        intervaloSlime = Random.Range(0,2);
    }

    // Update is called once per frame
    void Update()
    {
        intervaloAtual += 1f * Time.deltaTime;

        if (intervaloAtual >= intervaloSlime)
        {
            SpawnSlime();
            intervaloSlime = Random.Range(0,2);
            intervaloAtual = 0;
        }
    }

    void SpawnSlime()
    {
        if(quantidadeSlimes > 0)
        {
            int randomInt = Random.Range(1, 7); 
            if (randomInt == 1)
            {
                Instantiate(slime, new Vector3(-11f, 2.5f, 0f), Quaternion.identity);
            }
            else if (randomInt == 2)
            {
                Instantiate(slime, new Vector3(-11f, 0f, 0f), Quaternion.identity);
            }
            else if (randomInt == 3)
            {
                Instantiate(slime, new Vector3(-11f, -2.5f, 0f), Quaternion.identity);
            }
            else if (randomInt == 4)
            {
                Instantiate(slime, new Vector3(11f, 2.5f, 0f), Quaternion.identity);
            }
            else if (randomInt == 5)
            {
                Instantiate(slime, new Vector3(11f, 0f, 0f), Quaternion.identity);
            }
            else 
            {
                Instantiate(slime, new Vector3(11f, -2.5f, 0f), Quaternion.identity);
            }
            quantidadeSlimes -= 1;
        }
    }
}
