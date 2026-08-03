using UnityEngine;

public class AnzolPesca : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidadeDescer = 4f;
    public float velocidadeSubir = 6f;
    public float profundidadeMaxima = -4f;

    [Header("Referências")]
    public Transform pontaDaVara;
    public MovimentoPescaGame movimentoVara; // Certifique-se de ter arrastado o script da vara aqui no Inspector!
    public Transform pontoPegaPeixe;

    private LineRenderer lineRenderer;
    private Vector3 pontoDestinoFundo;
    private Vector3 pontoOrigemCalculado;

    public enum EstadoAnzol { Bloqueado, Parado, Descendo, Subindo }
    public EstadoAnzol estadoAtual = EstadoAnzol.Bloqueado;

    private Transform peixePego = null;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (pontoPegaPeixe == null) pontoPegaPeixe = transform;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
        }

        BloquearAnzol(); // Bloqueia tudo na inicialização!
    }

    public void IniciarJogoAnzol()
    {
        estadoAtual = EstadoAnzol.Parado;
        if (movimentoVara != null)
        {
            movimentoVara.podeMover = true; // Só agora a vara se move!
        }
    }

    public void BloquearAnzol()
    {
        estadoAtual = EstadoAnzol.Bloqueado;

        if (movimentoVara != null)
        {
            movimentoVara.podeMover = false; // Trava a vara
        }

        if (peixePego != null) Destroy(peixePego.gameObject);
        peixePego = null;

        if (pontaDaVara != null) transform.position = pontaDaVara.position;
    }

    void Update()
    {
        DesenharLinha();

        switch (estadoAtual)
        {
            case EstadoAnzol.Bloqueado:
                if (pontaDaVara != null) transform.position = pontaDaVara.position;
                if (movimentoVara != null) movimentoVara.podeMover = false;
                break;

            case EstadoAnzol.Parado:
                if (pontaDaVara != null)
                {
                    transform.position = pontaDaVara.position;
                }

                // Disparo do Anzol
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    pontoOrigemCalculado = transform.position;
                    pontoDestinoFundo = new Vector3(pontoOrigemCalculado.x, profundidadeMaxima, pontoOrigemCalculado.z);

                    // --- TRAVA A VARA IMEDIATAMENTE AO DISPARAR ---
                    if (movimentoVara != null)
                    {
                        movimentoVara.podeMover = false;
                    }

                    estadoAtual = EstadoAnzol.Descendo;
                }
                break;

            case EstadoAnzol.Descendo:
                // Garante que a vara continue travada enquanto o anzol desce
                if (movimentoVara != null) movimentoVara.podeMover = false;

                transform.position = Vector3.MoveTowards(transform.position, pontoDestinoFundo, velocidadeDescer * Time.deltaTime);

                if (Vector3.Distance(transform.position, pontoDestinoFundo) < 0.01f)
                {
                    estadoAtual = EstadoAnzol.Subindo;
                }
                break;

            case EstadoAnzol.Subindo:
                // Garante que a vara continue travada enquanto o anzol sobe
                if (movimentoVara != null) movimentoVara.podeMover = false;

                Vector3 pontoRetorno = (pontaDaVara != null) ? pontaDaVara.position : pontoOrigemCalculado;
                transform.position = Vector3.MoveTowards(transform.position, pontoRetorno, velocidadeSubir * Time.deltaTime);

                if (peixePego != null)
                {
                    peixePego.position = pontoPegaPeixe.position;
                }

                if (Vector3.Distance(transform.position, pontoRetorno) < 0.01f)
                {
                    FinalizarPesca();
                }
                break;
        }
    }

    void DesenharLinha()
    {
        if (lineRenderer != null)
        {
            Vector3 inicioLinha = (pontaDaVara != null) ? pontaDaVara.position : transform.position;
            lineRenderer.SetPosition(0, inicioLinha);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (estadoAtual == EstadoAnzol.Descendo && collision.CompareTag("Fish"))
        {
            PegarPeixe(collision.transform);
        }
    }

    private void PegarPeixe(Transform peixe)
    {
        peixePego = peixe;

        MovimentoPescaGame movimentoPeixe = peixe.GetComponent<MovimentoPescaGame>();
        if (movimentoPeixe != null) movimentoPeixe.enabled = false;

        Collider2D col = peixe.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        estadoAtual = EstadoAnzol.Subindo;
    }

    private void FinalizarPesca()
    {
        if (peixePego != null)
        {
            ScoreManager.AdicionarPontos(10);
            Destroy(peixePego.gameObject);
            peixePego = null;
        }

        // LIBERA A VARA DE NOVO APENAS QUANDO O ANZOL VOLTA TOTALMENTE!
        if (movimentoVara != null) movimentoVara.podeMover = true;

        estadoAtual = EstadoAnzol.Parado;
    }
}