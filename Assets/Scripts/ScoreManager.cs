using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int Pontos { get; private set; } = 0;

    public static void AdicionarPontos(int qtd)
    {
        Pontos += qtd;
        Debug.Log($"Pontos: {Pontos}");
    }

    public static void ResetarPontos()
    {
        Pontos = 0;
        Debug.Log($"Pontos resetados: {Pontos}");
    }
}
