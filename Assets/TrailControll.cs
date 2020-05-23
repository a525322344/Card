using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailControll : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 helpPosition;

    public float allTime=0.5f;
    public float prensentTime;
    public AnimationCurve movespeedCurve;
    public float kuanda;

    private bool ison;
    public void StartMove(Transform targetTrans)
    {
        startPosition = transform.position;
        targetPosition = targetTrans.position;
        float x = Random.Range(startPosition.x- kuanda, targetPosition.x+ kuanda);
        float y = Random.Range(targetPosition.y- kuanda, startPosition.y+ kuanda);
        helpPosition = new Vector3(x, y, 0.5f * startPosition.z + 0.5f * targetPosition.z);
        ison = true;
    }

    void Update()
    {
        if (ison)
        {
            prensentTime += Time.deltaTime / allTime;
            if (prensentTime > 1)
            {
                prensentTime = 1;
            }
            float index = movespeedCurve.Evaluate(prensentTime);
            Vector3 forntPoint = Vector3.Lerp(startPosition, helpPosition, index);
            Vector3 backPoint = Vector3.Lerp(helpPosition, targetPosition, index);
            Vector3 resultPoint = Vector3.Lerp(forntPoint, backPoint, index);
            transform.position = resultPoint;
        }
    }
}
