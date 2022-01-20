using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        //LeviController controller = other.GetComponent<LeviController>();
        Combatant combatant = other.GetComponent<Combatant>();
        if (combatant != null)
        {
            combatant.ChangeHealth(-1);
        }
    }
}
