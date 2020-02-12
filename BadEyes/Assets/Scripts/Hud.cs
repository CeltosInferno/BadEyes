using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public GameObject cubePrefab;
    public void GenerateCube()
    {
        Instantiate(cubePrefab);
    }
}
