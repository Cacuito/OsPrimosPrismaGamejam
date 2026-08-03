using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecompensasManager : MonoBehaviour
{
    public TextMeshProUGUI pontosText;
    public TextMeshProUGUI texto;

    public TextMeshProUGUI[] itemTexts;

    public int moralPorComida = 30;


    [System.Serializable]
    public struct Item
    {
        public string name;
        public int custo;
        public string id;
        public bool éItemFinal;

        public Item(string name, int custo, string id, bool éItemFinal)
        {
            this.name = name;
            this.custo = custo;
            this.id = id;
            this.éItemFinal = éItemFinal;
        }
    }

    public Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (itemTexts != null && i < itemTexts.Length)
            {
                itemTexts[i].text = $"{items[i].name} - Custo: {items[i].custo}";
            }
        }
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

    public bool ComprarItem(Item item)
    {
        if (ScoreManager.Pontos >= item.custo)
        {
            if (!item.éItemFinal)
            {
                if (item.id == "D") MoralSystem.AdicionarMoral(moralPorComida, item.id);
                else if (item.id == "S") MoralSystem.AdicionarMoral(moralPorComida, item.id);
                else if (item.id == "G") MoralSystem.AdicionarMoral(moralPorComida, item.id);
            }
            if (item.éItemFinal)
            {
                if (item.id == "D") SceneManager.LoadScene("CenaFinalD");
                else if (item.id == "S") SceneManager.LoadScene("CenaFinalS");
                else if (item.id == "G") SceneManager.LoadScene("CenaFinalG");
            }

            ScoreManager.AdicionarPontos(-item.custo);
            texto.text = $"{item.name} comprado por {item.custo} pontos.";
            Debug.Log($"Item {item.name} comprado por {item.custo} pontos.");
            return true;
        }
        else
        {
            Debug.Log($"Pontos insuficientes para comprar {item.name}. Custo: {item.custo}, Pontos: {ScoreManager.Pontos}");
            texto.text = $"Pontos insuficientes para comprar {item.name}. Custo: {item.custo}, Pontos: {ScoreManager.Pontos}";
            return false;
        }
    }

    public void AdicionarPontos()
    {
        ScoreManager.AdicionarPontos(1000);
    }
}
