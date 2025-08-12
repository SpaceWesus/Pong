using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerLevel10 : PlayerController
{

    [Header("Level 10 References")]
    [SerializeField] protected GameObject ActiveBalls;
    [SerializeField] protected GameObject ballPrefab;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Shoot();
    }

    public void Shoot()
    {
        // Add in Shoot logic here

        // If left click && ball counter < ball max
            // instantiate a new ballprefab as a child of AvtiveBalls at current paddle transform.x + 2
            // apply force to the new ball in direction of aiming reticle 
        // else 
            // play negative sound effect to let player know they cant shoot
            // shake ball counter UI 
            
    }
    

}
