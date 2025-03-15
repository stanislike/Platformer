using UnityEngine;

public class KillPlayerOnContact : MonoBehaviour
{
    public float verticalNormalThreshold = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float normalY = collision.GetContact(0).normal.y;
        //On teste si la normale de la collision ne pointe pas vers le bas, cad. si le joueur ne vient pas d'au dessus :
        if (collision.gameObject.CompareTag("Player") && normalY > -verticalNormalThreshold)
        {
            collision.collider.GetComponent<CharacterDeath>().Die();
        }
    }
}
