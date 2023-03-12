using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }
    }
}
