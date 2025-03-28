using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform ObjectTarget;

    [Header("Flip Rotation Stats")]
    [SerializeField] float flipYRotationTime = 0.5f;

    private Coroutine turnCoroutine;

    // Update is called once per frame
    void Update()
    {
        if (ObjectTarget)
        {
            transform.position = ObjectTarget.position;
        }
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount;
        float yRotation = 0f;
        if (ObjectTarget.rotation.y == 0)
            endRotationAmount = 180f;
        else
            endRotationAmount = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < flipYRotationTime)
        {
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }
}



