using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [Header("Índice do Item no RecompensasManager")]
    public int itemIndex;

    public RecompensasManager manager;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        if (manager == null)
        {
            manager = FindObjectOfType<RecompensasManager>();
        }

        if (manager != null && itemIndex < manager.items.Length)
        {
            string chaveSalvamento = "Comprado_" + manager.items[itemIndex].name;

            if (PlayerPrefs.GetInt(chaveSalvamento, 0) == 1)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        if (button != null)
        {
            button.onClick.AddListener(OnClickButton);
        }
    }

    void OnClickButton()
    {
        if (manager != null && itemIndex < manager.items.Length)
        {
            RecompensasManager.Item itemSelecionado = manager.items[itemIndex];

            bool comprouComSucesso = manager.ComprarItem(itemSelecionado);

            if (comprouComSucesso)
            {
                string chaveSalvamento = "Comprado_" + itemSelecionado.name;
                PlayerPrefs.SetInt(chaveSalvamento, 1);
                PlayerPrefs.Save();

                gameObject.SetActive(false);
            }
        }
    }
}