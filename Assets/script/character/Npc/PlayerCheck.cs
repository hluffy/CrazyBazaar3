using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NpcController npc = GetComponentInParent<NpcController>();
            if (npc != null)
            {
                npc.StopDestinationSetter();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NpcController npc = GetComponentInParent<NpcController>();
            if (npc != null)
            {
                npc.ReleaseDestinationSetter(); 
            }
        }
    }
}
