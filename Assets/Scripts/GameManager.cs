using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameOverDuration = 2f;
    public GameObject gameOverOverlay;

    public void GameOver()
    {
        gameOverOverlay.SetActive(true);
        StartCoroutine(ScheduleSceneRestart());
    }

    private IEnumerator ScheduleSceneRestart()
    {
        yield return new WaitForSeconds(gameOverDuration);
        EnemySpawner.enemyCount = 0; //Obligation de reset la variable statique (voir commentaire ds EnemySpawner)
        SceneManager.LoadScene(0); //Relance la scène à l'index de build 0 (voir BuildSettings)
    }
}
