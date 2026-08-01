using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinScript : MonoBehaviour
{
    [Header("Goblins Variedade")]
    [SerializeField] private Sprite goblin;
    [SerializeField] private Sprite goblin2;
    [SerializeField] private Sprite goblin2Quebrado;
    [SerializeField] private Sprite goblinHitado;
    [SerializeField] private Sprite goblin2Hitado;
    [SerializeField] private Sprite bombaSprite;

    private Vector2 startPosition = new Vector2(0, -1.56f);
    private Vector2 endPosition = Vector2.zero;

    private float showDuration = 0.5f;
    private float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Vector2 boxOffset;
    private Vector2 boxSize;
    private Vector2 boxOffsetHidden;
    private Vector2 boxSizeHidden;

    private bool hittable = true;

    public enum GoblinType
    {
        Goblin1,
        Goblin2,
        Bomb
    }
    private GoblinType goblinType;
    private float hardRate = 0.25f;
    private float bombRate = 0f;
    private int lives;

    private GoblinGameManager gameManager;
    private int goblinIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        boxOffset = boxCollider2D.offset;
        boxSize = boxCollider2D.size;
        boxOffsetHidden = new Vector2(boxOffset.x, -startPosition.y / 2f);
        boxSizeHidden = new Vector2(boxSize.x, 0f);
    }

    public void SetIndex(int index)
    {
        goblinIndex = index;
    }

    public void Activate(int level, GoblinGameManager manager)
    {
        gameManager = manager;
        SetLevel(level);
        CreateNext();
        StopAllCoroutines();
        StartCoroutine(ShowHide(startPosition, endPosition));
    }

    private void OnMouseDown()
    {
        if (hittable)
        {
            switch (goblinType)
            {
                case GoblinType.Goblin1:
                    spriteRenderer.sprite = goblinHitado;
                    StopAllCoroutines();
                    hittable = false;
                    gameManager.AddScore(goblinIndex);
                    StartCoroutine(QuickHide());
                    break;

                case GoblinType.Goblin2:
                    if (lives == 2)
                    {
                        spriteRenderer.sprite = goblin2Quebrado;
                        lives--;
                    }
                    else
                    {
                        spriteRenderer.sprite = goblin2Hitado;
                        StopAllCoroutines();
                        hittable = false;
                        gameManager.AddScore(goblinIndex);
                        StartCoroutine(QuickHide());
                    }
                    break;

                case GoblinType.Bomb:
                    hittable = false;
                    StopAllCoroutines();
                    gameManager.GameOver(1);
                    break;
            }
        }
    }

    private IEnumerator ShowHide(Vector2 start, Vector2 end)
    {
        transform.localPosition = start;
        float elapsed = 0f;

        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffsetHidden, boxOffset, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSizeHidden, boxSize, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
        boxCollider2D.offset = boxOffset;
        boxCollider2D.size = boxSize;

        yield return new WaitForSeconds(duration);

        elapsed = 0f;

        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffset, boxOffsetHidden, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSize, boxSizeHidden, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = start;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;

        if (hittable)
        {
            hittable = false;
            gameManager.Missed(goblinIndex, goblinType != GoblinType.Bomb);
        }
    }

    private IEnumerator QuickHide()
    {
        yield return new WaitForSeconds(0.25f);
        Hide();
    }

    public void Hide()
    {
        transform.localPosition = startPosition;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;
    }

    public void StopGame()
    {
        hittable = false;
        StopAllCoroutines();
        Hide();
    }

    private void CreateNext()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < bombRate)
        {
            goblinType = GoblinType.Bomb;
            if (bombaSprite != null) spriteRenderer.sprite = bombaSprite;
            lives = 1;
        }
        else if (randomValue < hardRate + bombRate)
        {
            goblinType = GoblinType.Goblin2;
            spriteRenderer.sprite = goblin2;
            lives = 2;
        }
        else
        {
            goblinType = GoblinType.Goblin1;
            spriteRenderer.sprite = goblin;
            lives = 1;
        }

        hittable = true;
    }

    private void SetLevel(int level)
    {
        bombRate = Mathf.Min(level * 0.025f, 0.25f);
        hardRate = Mathf.Min(level * 0.025f, 1f);

        float durationMin = Mathf.Clamp(1 - level * 0.1f, 0.01f, 1f);
        float durationMax = Mathf.Clamp(2 - level * 0.1f, 0.01f, 2f);
        duration = Random.Range(durationMin, durationMax);
    }
}