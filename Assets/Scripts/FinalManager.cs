using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class FinalManager : MonoBehaviour
{
    public string id;
    [SerializeField] private GameObject finalBom;
    [SerializeField] private GameObject finalRuim;

    // Start is called before the first frame update
    void Start()
    {
        IniciarCenaFinal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IniciarCenaFinal()
    {
        if (id == "D")
        {
            if (MoralSystem.moralD >= 100) finalBom.SetActive(true);
            else if (MoralSystem.moralD < 100) finalRuim.SetActive(true);
        }
        else if (id == "S")
        {
            if (MoralSystem.moralS >= 100) finalBom.SetActive(true);
            else if (MoralSystem.moralS < 100) finalRuim.SetActive(true);

        }
        else if (id == "G")
        {
            if (MoralSystem.moralG >= 100) finalBom.SetActive(true);
            else if (MoralSystem.moralG < 100) finalRuim.SetActive(true);
        }
    }
}
