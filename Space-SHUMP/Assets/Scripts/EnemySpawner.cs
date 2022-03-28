/**** 
 * Created by: Andrew Nguyen
 * Date Created: March 28, 2022
 * 
 * Last Edited by: Andrew Nguyen
 * Last Edited: March 28, 2022
 * 
 * Description: Spawns enemies
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*** VARIABLES ***/
    [Header("ENEMY SETTINGS")]
    public GameObject[] prefabEnemies; // prefabs for the enemies
    public float enemySpawnPerSecond; //Don't make them all spawn at once, delay the spawn, and determine how many spawn per second
    public float enemyDefaultPadding; //Padding for position 

    private BoundsCheck bndCheck;

    // Start is called before the first frame update
    void Start()
    {
        bndCheck = GetComponent<BoundsCheck>(); //Reference to bounds check component
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    } //end Start()

    public void SpawnEnemy()
    {
        //Pick random enemy to instantiate
        int idx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[idx]);

        //Position enemy above screen with random X position
        //Determine padding around enemy by recording it
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Set initial position
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;

        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;

        go.transform.position = pos;
        //Invoke again

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    } //end SpawnEnemy()

    // Update is called once per frame
    void Update()
    {
        
    }
} //end EnemySpawner class
