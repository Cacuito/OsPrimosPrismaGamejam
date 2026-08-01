using UnityEngine;

public class MovimentoPescaGame : MonoBehaviour
{
    public float velocidade = 5f;

    [SerializeField] private bool ladoDireita = true;
    public bool podeMover = false; // TRAVADO POR PADRÃO!

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        AtualizarOrientacaoSprite();
    }

    void Update()
    {
        // Se a permissão estiver desativada, a vara NÃO se move de jeito nenhum!
        if (!podeMover) return;

        if (ladoDireita)
        {
            transform.Translate(Vector3.right * velocidade * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * velocidade * Time.deltaTime);
        }
    }

    public void SetDirecaoInicial(bool irParaDireita)
    {
        ladoDireita = irParaDireita;
        AtualizarOrientacaoSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && podeMover)
        {
            ladoDireita = !ladoDireita;
            AtualizarOrientacaoSprite();
        }
    }

    private void AtualizarOrientacaoSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !ladoDireita;
        }
    }
}