using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModel : MonoBehaviour
{
    [SerializeField]
    Transform bulletSpawnLocation;
    public Transform BulletSpawnLocation { get { return bulletSpawnLocation; } }
}
