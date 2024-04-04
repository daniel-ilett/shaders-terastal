using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerastalCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float startHeight;
    [SerializeField] private float endHeight;
    [SerializeField] private Vector3 finalOffset;
    [SerializeField] private float duration;

    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private AnimationCurve rotationCurve;

    [SerializeField] private Animator targetAnim;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(RotateAnimation());
        }
    }

    private IEnumerator RotateAnimation()
    {
        Camera.main.fieldOfView = 10.0f;

        for(float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            float x = speedCurve.Evaluate(t / duration);
            float yRotation = rotationCurve.Evaluate(x) * Mathf.Deg2Rad;

            float height = Mathf.Lerp(startHeight, endHeight, t / duration);
            Vector3 targetPos = target.position + new Vector3(0.0f, height, 0.0f);

            Vector3 position = targetPos + new Vector3(Mathf.Cos(yRotation) * 2.0f, 0.0f, Mathf.Sin(yRotation) * 2.0f);
            transform.position = position;
            transform.LookAt(targetPos, Vector3.up);
            transform.Rotate(5.0f, 0.0f, 3.5f, Space.Self);

            yield return null;
        }

        StartCoroutine(FinalFocus());
        //Camera.main.fieldOfView = 60.0f;
    }

    private IEnumerator FinalFocus()
    {
        targetAnim.SetTrigger("Honk");
        Vector3 startPos = transform.position;

        for(float t = 0.0f; t < 1.0f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos, target.position + finalOffset, t);

            Vector3 targetPos = target.position + new Vector3(0.0f, 0.4f, 0.0f);
            Quaternion targetRot = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);

            Camera.main.fieldOfView = Mathf.Lerp(10.0f, 25.0f, t);

            yield return null;
        }
    }
}
