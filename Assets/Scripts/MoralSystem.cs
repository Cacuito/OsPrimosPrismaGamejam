using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoralSystem
{
    private const string CHAVE_MORAL_D = "MoralSalvaD";
    private const string CHAVE_MORAL_S = "MoralSalvaS";
    private const string CHAVE_MORAL_G = "MoralSalvaG";

    public static int moralD
    {
        get
        {
            return PlayerPrefs.GetInt(CHAVE_MORAL_D, 0);
        }
    }

    public static int moralS
    {
        get
        {
            return PlayerPrefs.GetInt(CHAVE_MORAL_S, 0);
        }
    }

    public static int moralG
    {
        get
        {
            return PlayerPrefs.GetInt(CHAVE_MORAL_G, 0);
        }
    }

    public static void AdicionarMoral(int qtd, string pers)
    {
        if (pers == "D")
        {
            int novaMoralD = moralD + qtd;
            if (novaMoralD < 0) novaMoralD = 0;
            PlayerPrefs.SetInt(CHAVE_MORAL_D, novaMoralD);
            Debug.Log($"Moral atualizada D: {novaMoralD}");
        }
        else if (pers == "S")
        {
            int novaMoralS = moralS + qtd;
            if (novaMoralS < 0) novaMoralS = 0;
            PlayerPrefs.SetInt(CHAVE_MORAL_S, novaMoralS);
            Debug.Log($"Moral atualizada S: {novaMoralS}");
        }
        else if (pers == "G")
        {
            int novaMoralG = moralG + qtd;
            if (novaMoralG < 0) novaMoralG = 0;
            PlayerPrefs.SetInt(CHAVE_MORAL_G, novaMoralG);
            Debug.Log($"Moral atualizada G: {novaMoralG}");
        }

        PlayerPrefs.Save();
        Debug.Log("Moral atualizada");
    }

    public static void ResetarMoral()
    {
        PlayerPrefs.SetInt(CHAVE_MORAL_D, 0);
        PlayerPrefs.SetInt(CHAVE_MORAL_S, 0);
        PlayerPrefs.SetInt(CHAVE_MORAL_G, 0);
        PlayerPrefs.Save();

        Debug.Log("Moral resetada.");
    }
}