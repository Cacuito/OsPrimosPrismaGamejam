using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecompensasManager : MonoBehaviour
{
    public TextMeshProUGUI pontosText;


    [System.Serializable]
    public struct Item
    {
        public string name;
        public int custo;

        public Item(string name, int custo)
        {
            this.name = name;
            this.custo = custo;
        }
    }

    public Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pontosText != null)
        {
            pontosText.text = $"Pontos: {ScoreManager.Pontos}";
        }
    }

    public void SairFase()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ComprarItem(Item item)
    {
        if (ScoreManager.Pontos >= item.custo)
        {
            ScoreManager.AdicionarPontos(-item.custo);
            Debug.Log($"Item {item.name} comprado por {item.custo} pontos.");
        }
        else
        {
            Debug.Log($"Pontos insuficientes para comprar {item.name}. Custo: {item.custo}, Pontos: {ScoreManager.Pontos}");
        }
    }

    public void AdicionarPontos()
    {
        ScoreManager.AdicionarPontos(1000);
    }
}
