using UnityEngine;

public class Artificial : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < srs.Length; i++)
        {
            
            srs[i].sortingOrder = -((int)srs[i].transform.position.y * 10 + (int)srs[i].transform.position.x);
        }
    }
}
