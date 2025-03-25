using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform cam;
    private Vector3 camStartPos;
    private float distance;

    private GameObject[] backgrounds;
    private Material[] materials;
    private float[] backSpeed;

    private float fartherstBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;
        
        int backCount = transform.childCount;
        materials = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            materials[i] = backgrounds[i].GetComponent<Renderer>().material;
        }
        BackSpeedCalculate(backCount);
    }

    private void BackSpeedCalculate(int backCount)
    {
        for(int i = 0;i < backCount; i++)
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > fartherstBack)
            {
                fartherstBack = backgrounds[i].transform .position.z - cam.position .z;
            }
        }

        for (int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / fartherstBack;
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector2(cam.position.x, cam.position.y);
        distance = cam.position.x - camStartPos.x;
        float speed;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            speed = backSpeed[i] * parallaxSpeed;
            materials[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
