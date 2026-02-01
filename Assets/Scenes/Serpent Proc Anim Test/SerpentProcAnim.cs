using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SerpentProcAnim : MonoBehaviour
{
    public int legnth;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPos;
    public Vector3[] segmentV;
    public Transform targetDir;
    public float targetDist;
    public float moveSpeed = 5.0f;
    public float smoothSpeed;
    public Transform[] bodySegments;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = legnth;
        segmentPos = new Vector3[legnth];
        segmentV = new Vector3[legnth];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector2.MoveTowards(transform.position, cursorPos, moveSpeed * Time.deltaTime);

        segmentPos[0] = targetDir.position;
        bodySegments[0].transform.position = segmentPos[0];

        for (int i = 1; i < segmentPos.Length; i++)
        {
            segmentPos[i] = Vector3.SmoothDamp(segmentPos[i], segmentPos[i - 1] + targetDir.right * targetDist, ref segmentV[i], smoothSpeed);
            bodySegments[i].transform.position = segmentPos[i];
        }
        lineRenderer.SetPositions(segmentPos);
    }
}
