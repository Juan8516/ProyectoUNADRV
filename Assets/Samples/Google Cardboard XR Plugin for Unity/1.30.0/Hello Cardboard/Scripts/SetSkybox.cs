using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkybox : MonoBehaviour
{
    public Material skyboxMaterial;

    void Start()
    {
        RenderSettings.skybox = skyboxMaterial;
    }
}
