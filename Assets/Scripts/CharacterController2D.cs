using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpHeight = 6f;
    public float fallGravityMod = 1f;
    public float smallJumpGravityMod = 4f;
    public float verticalNormalThreshold = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool grounded;
    private bool jumpScheduled;
    private Collider2D currentGroundCollider;
    private bool killJump;

    [Header("Debug")]
    public float minJumpHeight;

    //Méthode appellée automatiquement par Unity lorsqu'on change une propriété dans l'inspecteur :
    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
        minJumpHeight = CalculateMinJumpHeight(); //Affiche la hauteur du plus petit saut possible dans la variable MinJumpHeight
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (grounded && Input.GetButtonDown("Jump"))
        {
            jumpScheduled = true;
            animator.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        float xMove = Input.GetAxis("Horizontal") * moveSpeed;
        Vector2 desiredVelocity = Vector2.right * xMove;

        //On conserve la gravité précédente :
        desiredVelocity.y = rb.linearVelocity.y;

        if (jumpScheduled)
        {
            jumpScheduled = false;
            desiredVelocity.y = CalculateJumpImpulse(); 
        }

        if (!grounded)
        {
            if (desiredVelocity.y < 0f) //Si le perso chute (vélocité négative en .y)
            {
                //Time.deltaTime ici est obligatoire car il s'agit d'une accélération :
                desiredVelocity.y += Physics2D.gravity.y * rb.gravityScale * fallGravityMod * Time.deltaTime; 
            }
            else if (!killJump && !Input.GetButton("Jump")) //Sinon, si on n'appuie plus sur la touche, ou si on ne vient pas de tuer un ennemi :
            {
                desiredVelocity.y += Physics2D.gravity.y * rb.gravityScale * smallJumpGravityMod * Time.deltaTime;
            }
        }

        rb.linearVelocity = desiredVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float normalY = collision.GetContact(0).normal.y;

        if (normalY > verticalNormalThreshold) //Ground check
        {
            if (collision.gameObject.CompareTag("Enemy")) //Sur un ennemi
            {
                jumpScheduled = true; //Sauter à nouveau automatiquement
                killJump = true; //Désactiver le système d'appui de touche (le saut ira à sa hauteur maximale)

                collision.collider.GetComponent<CharacterDeath>().Die();
            }
            else //grounded
            {
                grounded = true;
                killJump = false;
                currentGroundCollider = collision.collider;
            }
        }
        else if (normalY < -verticalNormalThreshold)  //Ceiling check
        {
            animator.SetTrigger("StopJump");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider == currentGroundCollider)
        {
            grounded = false;
            currentGroundCollider = null;
        }
    }

    public float CalculateJumpImpulse()
    {
        return Mathf.Sqrt(-2f * Physics2D.gravity.y * rb.gravityScale * jumpHeight);
    }

    private float CalculateMinJumpHeight()
    {
        float impulse = CalculateJumpImpulse();
        //Note : comme on applique toujours la gravité en plus du smallJumpGravityMod, il faut donc y ajouter 1 :
        return impulse * impulse / (-2 * Physics2D.gravity.y * rb.gravityScale * (smallJumpGravityMod + 1));
    }
}


