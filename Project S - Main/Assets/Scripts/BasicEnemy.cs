using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Combatant
{
    /**TODO: enemy AI
        Should define 
            movement behavior (direction, speeds, when)
            Attack behavior
            Responsive/event based behavior
            Idle/non-combat activity?
            How much randomness should be present?
    */

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    //updates not based on current framerate
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
