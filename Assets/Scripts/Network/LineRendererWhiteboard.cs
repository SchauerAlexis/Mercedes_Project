using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;



public class LineRendererWhiteboard : MonoBehaviour
{
    public GameObject LineRendererPrefab;

    private LineRenderer currentLineRenderer;

    private PhotonView photonView;

    private DrawingColor drawingColor;
    private bool touching, touchingLast;
    private Vector3 touchPosition;
    private Vector3 lastTouchPosition;

    private Color blue = Color.blue;
    private Color black = Color.black;
    private Color red = Color.red;

    private Color currentColor;

    // List of all Lines to delete them later?
    private List<GameObject> Lines = new List<GameObject>();

    private float[] xBuffer;
    private float[] yBuffer;

    // Offset to draw the following lines a little bit forward to avoid LineCollisions
    private float lineOffset = 0f;
    private float lineOffsetAdd = 0.0000001f;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        if (touching)
        {
            // new Touch --> new Line
            if (!touchingLast)
            {
                Lines.Add(Instantiate(LineRendererPrefab, this.transform));
                currentLineRenderer = Lines[Lines.Count - 1].GetComponent<LineRenderer>();
                currentLineRenderer.enabled = true;
                currentLineRenderer.positionCount = 0;
                currentLineRenderer.material.color = currentColor;

                Debug.Log(Lines.Count);
            }

            if (Vector3.Distance(touchPosition, lastTouchPosition) > currentLineRenderer.startWidth)
            {
                // HACK
                touchPosition.z -= lineOffset;
                lineOffset += lineOffsetAdd;


                currentLineRenderer.positionCount++;
                currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, touchPosition);
            }

            lastTouchPosition = touchPosition;
        }

        // End of Touch
        if(!touching && touchingLast)
        {
            Debug.Log("End of Touch");
            Vector3[] positions = new Vector3[currentLineRenderer.positionCount];
            xBuffer = new float[positions.Length];
            yBuffer = new float[positions.Length];
            currentLineRenderer.GetPositions(positions);

            for(int i = 0; i < positions.Length; i++)
            {
                xBuffer[i] = positions[i].x;
                yBuffer[i] = positions[i].y;
            }

            photonView.RPC("RecieveLineRPC", RpcTarget.Others, xBuffer, yBuffer, positions[0].z, drawingColor);
        }

        touchingLast = touching;
    }

    public void RefreshWhiteboard()
    {
        if (Lines.Count > 0)
        {
            photonView.RPC("Refresh", RpcTarget.All);
        }
    }

    public void ToogleTouch(bool touching)
    {
        this.touching = touching;
    }

    public void SetTouchPosition(Vector3 point)
    {
        touchPosition = point;
    }

    public void SetColor(DrawingColor color)
    {
        drawingColor = color;
        switch (color)
        {
            case DrawingColor.Blue:
                currentColor = blue;
                break;
            case DrawingColor.Black:
                currentColor = black;
                break;
            case DrawingColor.Red:
                currentColor = red;
                break;
        }
    }


    private void SetNetworkColor(DrawingColor color)
    {

    }

    [PunRPC]
    public void RecieveLineRPC(float[] x, float[] y, float z, DrawingColor color)
    {
        // HACK
        z -= lineOffset;
        lineOffset += lineOffsetAdd;

        Lines.Add(Instantiate(LineRendererPrefab, this.transform));
        currentLineRenderer = Lines[Lines.Count - 1].GetComponent<LineRenderer>();
        currentLineRenderer.enabled = true;
        SetColor(color);
        currentLineRenderer.material.color = currentColor;

        currentLineRenderer.positionCount = x.Length;

        Vector3[] positions = new Vector3[x.Length];
        for(int i = 0; i < x.Length; i++)
        {
            positions[i].x = x[i];
            positions[i].y = y[i];
            positions[i].z = z;
        }

        currentLineRenderer.SetPositions(positions);
    }

    [PunRPC]
    public void Refresh()
    {
        Debug.Log("Refreshing Whitheboard...");
        Debug.Log(Lines.Count);
        foreach (GameObject line in Lines)
        {
            GameObject.Destroy(line);
        }
    }
}
