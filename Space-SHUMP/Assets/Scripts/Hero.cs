/**** 
 * Created by: Your Name
 * Date Created: March 16, 2022
 * 
 * Last Edited by: Andrew Nguyen
 * Last Edited: March 30, 2022
 * 
 * Description: Hero ship controller
****/

/** Using Namespaces **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase] //forces selection of parent object
public class Hero : MonoBehaviour
{
    /*** VARIABLES ***/

    //Singleton for the Playership. 
    #region PlayerShip Singleton
    static public Hero SHIP; //refence GameManager
   
    //Check to make sure only one gm of the GameManager is in the scene
    void CheckSHIPIsInScene()
    {

        //Check if instnace is null
        if (SHIP == null)
        {
            SHIP = this; //set SHIP to this game object
        }
        else //else if SHIP is not null send an error
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.SHIP");
        }
    }//end CheckGameManagerIsInScene()
    #endregion

    GameManager gm; //reference to game manager

    [Header("Ship Movement")]
    public float speed = 10;
    public float rollMult = -45; //The tilting from side to side
    public float pitchMult = 30; //The tilt from up to down

    [Space(10)]

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed;

    [Space(10)]

    private GameObject lastTriggerGo; //reference to the last triggering game object
    
    [Header("Shield Settings")]
    [SerializeField] //show in inspector
    private float _shieldLevel = 1; //level for shields
    public int maxShield = 4; //maximum shield level
    
    //method that acts as a field (property), if the property falls below zero the game object is desotryed
    public float shieldLevel
    {
        get { return (_shieldLevel); }
        set
        {
            _shieldLevel = Mathf.Min(value, maxShield); //Min returns the smallest of the values, therby making max sheilds 4

            //if the sheild is going to be set to less than zero
            if (value < 0)
            {
                Destroy(this.gameObject);
                Debug.Log(gm.name);
                gm.LostLife();
                
            }

        }
    }

    /*** MEHTODS ***/

    //Awake is called when the game loads (before Start).  Awake only once during the lifetime of the script instance.
    void Awake()
    {
        CheckSHIPIsInScene(); //check for Hero SHIP
    }//end Awake()

    //Start is called once before the update
    private void Start()
    {
        gm = GameManager.GM; //find the game manager
    }//end Start()

    // Update is called once per frame (page 551)
    void Update()
    {
        //player input
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //Change transform based on axis
        Vector3 pos = transform.position;

        //Movement based on x or y axis
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;

        transform.position = pos;

        //Also rotate the ship
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0); //Nothing with Z-axis so leave it as 0

        //Check for spacebar (firing)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TempFire();
        }
    }//end Update()


    //Taking Damage
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered from: " + other.gameObject.name);
        Transform rootT = other.gameObject.transform.root;
        //Transform root returns topmost transform in the hierarchy, the parent

        GameObject go = rootT.gameObject; //Game object of parent

        if(go == lastTriggerGo)
        {
            return;
        } //end if

        lastTriggerGo = go; //Set trigger to last trigger

        if (go.tag == "Enemy")
        {
            Debug.Log("Hit an enemy");
            shieldLevel--;
            Destroy(go);
        }
        else
        {
            Debug.Log("Triggered by a non-Enemy " + go.name);
        }

    }//end OnTriggerEnter()

    void TempFire()
    {
        GameObject projGo = Instantiate<GameObject>(projectilePrefab);
        projGo.transform.position = transform.position; //Spawn projectile at the hero's location
        //Make physics move the projectile based off velocity 
        Rigidbody rb = projGo.GetComponent<Rigidbody>();
        rb.velocity = Vector3.up * projectileSpeed;
    }

    public void AddToScore(int value)
    {
        gm.UpdateScore(value);
    }
}
