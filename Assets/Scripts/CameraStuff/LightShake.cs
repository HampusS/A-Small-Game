using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShake : MonoBehaviour {
    public ShakeProperties testProperties;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<CameraShake>().StartShake(testProperties);
        }
    }
}
