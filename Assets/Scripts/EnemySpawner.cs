using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float maxEnemies = 3f;
    public float spawnRate = 1f; //en ennemis par seconde
    public GameObject enemyPrefab;
    public float spawnDirX = 1;

    private float timer;

    //Le static est un raccourci ici, évidemment il faudrait changer ça pour pouvoir supporter plusieurs spawners ...
    //(Compte par écran/ par spawner/ à définir en fonction du jeu que l'on souhaite réaliser)
    public static int enemyCount; 

    void Update()
    {
        timer += Time.deltaTime;
        if(enemyCount < maxEnemies && timer >= 1f/spawnRate) //(1f/spawnRate) car spawnRate est en ennemis/sec
        {
            enemyCount++;
            timer = 0f;
            GameObject enemyGO = Instantiate(enemyPrefab, transform.position, transform.rotation);
            enemyGO.GetComponent<EnemyController>().startDirX = spawnDirX;
        }
    }
}
