using UnityEngine;

public class CharacterDeath : MonoBehaviour
{
    public Vector2 deathVelocity = Vector2.zero;
    public float deathTorque = 60f;
    public float destructionDelay = 5f;

    public void Die()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.None;
        rb.linearVelocity = deathVelocity;
        rb.AddTorque(deathTorque);
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, destructionDelay);

        //Player Check : 
        CharacterController2D cc = GetComponent<CharacterController2D>();
        if(cc != null) //C'est le joueur : logique custom
        {
            Destroy(cc);
            //Desactive le smooth follow de la camera : 
            FindObjectOfType<SmoothFollow>().enabled = false;
            FindObjectOfType<GameManager>().GameOver();
        }
        else //C'est un ennemi, on descend donc le compte dans le spawner
        {
            EnemySpawner.enemyCount--;
        }
    }
}
