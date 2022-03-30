/**** 
 * Created by: Andrew Nguyen
 * Date Created: March 30, 2022
 * 
 * Last Edited by: Andrew Nguyen
 * Last Edited: March 30, 2022
 * 
 * Description: Spawns enemies
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // VARIABLES //
    private BoundsCheck bndCheck; //reference to bounds check

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    } //end Awake()

    // Update is called once per frame
    void Update()
    {
        //Going offscreen should destroy the bullet 
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    } //end Update()
}
