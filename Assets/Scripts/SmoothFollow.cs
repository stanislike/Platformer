using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    public BoxCollider2D boundsCollider;

    public bool simpleFollow;

    private float zOffset;
    private Vector3 velocity = Vector3.zero;


    private void Start()
    {
        zOffset = transform.position.z;
    }

    void FixedUpdate()
    {
        if (simpleFollow)
        {
            transform.position = target.position + Vector3.forward * zOffset;
            return;
        }

        Vector3 containedPos = target.position;
        //containedPos.z = 0f; 
        if (!boundsCollider.bounds.Contains(containedPos))
        {
            containedPos = boundsCollider.bounds.ClosestPoint(containedPos);
            DrawDebugCross(containedPos);
        }

        Vector3 targetPos = containedPos + Vector3.forward * zOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    //Méthode pour dessiner une croix à la position donnée
    //Une bonne idée peut être de conserver ce genre de choses dans une classe d'helpers, pour l'importer facilement dans des nouveaux projets.
    private static void DrawDebugCross(Vector3 position)
    {
        Debug.DrawRay(position + new Vector3(-0.5f, 0.5f, 0f), new Vector3(1f, -1f, 0f), Color.yellow);
        Debug.DrawRay(position + new Vector3(-0.5f, -0.5f, 0f), new Vector3(1f, 1f, 0f), Color.yellow);
    }
}
