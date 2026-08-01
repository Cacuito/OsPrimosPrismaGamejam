using UnityEngine;

public static class ScoreManager
{
    private const string CHAVE_PONTOS = "PontosSalvos";

    public static int Pontos
    {
        get
        {
            return PlayerPrefs.GetInt(CHAVE_PONTOS, 0);
        }
    }

    public static void AdicionarPontos(int qtd)
    {
        int novosPontos = Pontos + qtd;

        if (novosPontos < 0) novosPontos = 0;

        PlayerPrefs.SetInt(CHAVE_PONTOS, novosPontos);
        PlayerPrefs.Save();

        Debug.Log($"Pontos atualizados: {novosPontos}");
    }

    public static void ResetarPontos()
    {
        PlayerPrefs.SetInt(CHAVE_PONTOS, 0);
        PlayerPrefs.Save();

        Debug.Log("Pontos resetados.");
    }
}