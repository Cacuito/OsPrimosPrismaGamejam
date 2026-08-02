using UnityEngine;

[ExecuteAlways]
public class FixCameraBug : MonoBehaviour
{
    void Awake()
    {
        // Força a redefinição de todas as câmeras na cena
        Camera[] cameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in cameras)
        {
            cam.ResetWorldToCameraMatrix();
            cam.ResetProjectionMatrix();

            // Garante que a Main Camera volte para uma posição segura no 2D
            if (cam.CompareTag("MainCamera"))
            {
                cam.transform.position = new Vector3(0, 0, -10);
                cam.orthographic = true;
                cam.nearClipPlane = 0.3f;
                cam.farClipPlane = 1000f;
            }
        }
        Debug.Log("✅ Matrizes de Câmera resetadas com sucesso!");
    }
}