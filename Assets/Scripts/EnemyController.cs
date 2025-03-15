using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //On débute par défaut avec une direction inversée (cd. vers la gauche), 
    //mais il suffit de mettre la variable à 1 pour commencer vers la droite :
    public float startDirX = -1f;
    public float speed = 5f;
    public float horizontalNormalThreshold = 0.5f;

    private Rigidbody2D rb;
    private float dir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = startDirX;
    }

    private void FixedUpdate()
    {
        //On fait exactement la même chose que pour le CharacterController2D (avec dir en plus, qui peut inverser le sens)
        Vector2 desiredVelocity = dir * Vector2.right * speed;
        desiredVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = desiredVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float normalX = collision.GetContact(0).normal.x;
        //On teste si la normale est horizontale grâce à sa composante x : 
        if (normalX > horizontalNormalThreshold || normalX < -horizontalNormalThreshold)
        {
            Flip();
        }
    }

    private void Flip()
    {
        //Inverse le sens (-1 * -1 = 1 && 1 * -1 = -1)
        dir *= -1;
    }
}
