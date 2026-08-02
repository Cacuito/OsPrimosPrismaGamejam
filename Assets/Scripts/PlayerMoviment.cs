using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 movimento;

    [Header("Config")]
    public float speed = 5f;

    public bool podeMover = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (podeMover)
        {
            float moveX = Input.GetAxisRaw("Horizontal");  
            float moveY = Input.GetAxisRaw("Vertical");

            if(Mathf.Abs(moveX) > 0)
            {
                movimento = new Vector2(moveX,0).normalized;
            }
            else if(Mathf.Abs(moveY) > 0)
            {
                movimento = new Vector2(0, moveY).normalized;
            }
            else
            {
                movimento = Vector2.zero;
            }
        }
        else
        {
            movimento = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimento * speed * Time.fixedDeltaTime);
    }
}
