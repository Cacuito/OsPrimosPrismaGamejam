using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [Header("Configuração da Cena")]
    [SerializeField] private GameObject dialogoLobby;

    private const string CHAVE_DIALOGO_LOBBY = "DialogoLobbyJaVisto";

    private void Start()
    {
        if (PlayerPrefs.GetInt(CHAVE_DIALOGO_LOBBY, 0) == 0)
        {
            IniciarDialogoPrimeiraVez();
        }
        else
        {
            if (dialogoLobby != null)
            {
                dialogoLobby.SetActive(false);
            }
        }
    }

    private void IniciarDialogoPrimeiraVez()
    {
        if (dialogoLobby != null)
        {
            dialogoLobby.SetActive(true);
        }

        PlayerPrefs.SetInt(CHAVE_DIALOGO_LOBBY, 1);
        PlayerPrefs.Save();
    }

    public void ResetarHistoricoDialogo()
    {
        PlayerPrefs.DeleteKey(CHAVE_DIALOGO_LOBBY);
        PlayerPrefs.Save();
        Debug.Log("Histórico do diálogo do Lobby foi resetado!");
    }
}
