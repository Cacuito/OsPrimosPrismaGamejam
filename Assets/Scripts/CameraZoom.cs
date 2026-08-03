using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zoomSizeAlvo = 3f; 
    [SerializeField] private float velocidadeZoom = 2f;
    [SerializeField] private Transform centroDaBarraca; 

    private float tamanhoOriginal;
    private Vector3 posicaoOriginal;

    void Start()
    {
        if (mainCamera == null) 
        {
            mainCamera = Camera.main;
        }

        tamanhoOriginal = mainCamera.orthographicSize;
        posicaoOriginal = mainCamera.transform.position;
    }

    public void IniciarZoomParaMinigame()
    {
        StartCoroutine(AnimacaoZoom());
    }

    private IEnumerator AnimacaoZoom()
    {
        float tempo = 0;
        
        Vector3 posicaoAlvo = centroDaBarraca != null 
            ? new Vector3(centroDaBarraca.position.x, centroDaBarraca.position.y, posicaoOriginal.z) 
            : posicaoOriginal;

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * velocidadeZoom;
            
            mainCamera.orthographicSize = Mathf.Lerp(tamanhoOriginal, zoomSizeAlvo, tempo);
            mainCamera.transform.position = Vector3.Lerp(posicaoOriginal, posicaoAlvo, tempo);
            
            yield return null; 
        }
    }

    public void VoltarZoomOriginal()
    {
        StartCoroutine(AnimacaoZoomOut());
    }

    private IEnumerator AnimacaoZoomOut()
    {
        float tempo = 0;
        float tamanhoAtual = mainCamera.orthographicSize;
        Vector3 posicaoAtual = mainCamera.transform.position;

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * velocidadeZoom;
            
            mainCamera.orthographicSize = Mathf.Lerp(tamanhoAtual, tamanhoOriginal, tempo);
            mainCamera.transform.position = Vector3.Lerp(posicaoAtual, posicaoOriginal, tempo);
            
            yield return null; 
        }
    }
}