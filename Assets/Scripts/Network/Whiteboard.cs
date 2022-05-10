using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public enum DrawingColor
{
    Blue,
    Red,
    Black
}

public class Whiteboard : MonoBehaviour
{

    private const int textureSize = 1024;
    private const int penSize = 2;
    private Texture2D texture;
    private Color[] currentColor;
    private Color[] blue = Enumerable.Repeat<Color>(Color.blue, penSize * penSize).ToArray<Color>();
    private Color[] red = Enumerable.Repeat<Color>(Color.red, penSize * penSize).ToArray<Color>();
    private Color[] black = Enumerable.Repeat<Color>(Color.black, penSize * penSize).ToArray<Color>();

    private PhotonView photonView;

    private DrawingColor drawingColor;
    private bool touching, touchingLast;
    private float posX, posY;
    private float lastX, lastY;

    private const int bufferSize = 10;
    private int bufferIndex = 0;
    private int[] drawingXBuffer = new int[bufferSize];
    private int[] drawingYBuffer = new int[bufferSize];
    private Color[] networkColor;
    private int lastNetworkX;
    private int lastNetworkY;
    private bool firstNetworkTouch = true;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        texture = new Texture2D(textureSize, textureSize);
        renderer.material.mainTexture = texture;

        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (touching)
        {
            int x = (int)(posX * textureSize - (penSize / 2));
            int y = (int)(posY * textureSize - (penSize / 2));

            if (touchingLast)
            {
                texture.SetPixels(x, y, penSize, penSize, currentColor);
                for (float t = 0.01f; t < 1f; t += 0.01f)
                {
                    int lerpX = (int)Mathf.Lerp(lastX, (float)x, t);
                    int lerpY = (int)Mathf.Lerp(lastY, (float)y, t);
                    texture.SetPixels(lerpX, lerpY, penSize, penSize, currentColor);
                }
                
                texture.Apply();
            }

            lastX = (float)x;
            lastY = (float)y;


            drawingXBuffer[bufferIndex] = x;
            drawingYBuffer[bufferIndex] = y;


            bufferIndex++;
            if(bufferIndex >= bufferSize)
            {
                photonView.RPC("RecieveDrawingRPC", RpcTarget.Others, drawingXBuffer, drawingYBuffer, bufferSize, drawingColor, true);
                bufferIndex = 0;
            }
        }
        else if(bufferIndex != 0)
        {
            photonView.RPC("RecieveDrawingRPC", RpcTarget.Others, drawingXBuffer, drawingYBuffer, bufferIndex + 1, drawingColor, false);
            bufferIndex = 0;
        }
        else if(touchingLast == true && touching == false && bufferIndex == 0)
        {
            photonView.RPC("RecieveDrawingRPC", RpcTarget.Others, new int[0], new int[0], 0, drawingColor, false);
        }

        touchingLast = touching;
    }

    public void ToogleTouch(bool touching)
    {
        this.touching = touching;
    }

    public void SetTouchPosition(float x, float y)
    {
        posX = x;
        posY = y;
    }

    public void SetColor(DrawingColor color)
    {
        drawingColor = color;
        switch (color)
        {
            case DrawingColor.Blue:
                this.currentColor = blue;
                break;
            case DrawingColor.Black:
                this.currentColor = black;
                break;
            case DrawingColor.Red:
                this.currentColor = red;
                break;
        }
    }

    private void SetNetworkColor(DrawingColor color)
    {
        switch (color)
        {
            case DrawingColor.Blue:
                this.networkColor = blue;
                break;
            case DrawingColor.Black:
                this.networkColor = black;
                break;
            case DrawingColor.Red:
                this.networkColor = red;
                break;
        }
    }

    [PunRPC]
    public void RecieveDrawingRPC(int[] x, int[] y, int size, DrawingColor color, bool ongoing)
    {
        if (size == 0)
        {
            firstNetworkTouch = true;
            Debug.Log("Touch ended");
            return;
        }

        SetNetworkColor(color);

        if (ongoing)
        {
            if (firstNetworkTouch)
            {
                texture.SetPixels(x[0], y[0], penSize, penSize, this.networkColor);
                firstNetworkTouch = false;
            }
            else
            {
                for (float t = 0.01f; t < 1f; t += 0.01f)
                {
                    int lerpX = (int)Mathf.Lerp(lastNetworkX, (float)x[0], t);
                    int lerpY = (int)Mathf.Lerp(lastNetworkY, (float)y[0], t);
                    texture.SetPixels(lerpX, lerpY, penSize, penSize, this.networkColor);
                }
            }
        }
        else
        {
            firstNetworkTouch = true;
            for (float t = 0.01f; t < 1f; t += 0.01f)
            {
                int lerpX = (int)Mathf.Lerp(lastNetworkX, (float)x[0], t);
                int lerpY = (int)Mathf.Lerp(lastNetworkY, (float)y[0], t);
                texture.SetPixels(lerpX, lerpY, penSize, penSize, this.networkColor);
            }
        }

        for (int i = 1; i < size; i++)
        {
            for (float t = 0.01f; t < 1f; t += 0.01f)
            {
                int lerpX = (int)Mathf.Lerp((float)x[i-1], (float)x[i], t);
                int lerpY = (int)Mathf.Lerp((float)y[i-1], (float)y[i], t);
                texture.SetPixels(lerpX, lerpY, penSize, penSize, this.networkColor);
            }
        }

        lastNetworkX = x[size-1];
        lastNetworkY = y[size-1];

        texture.Apply();
    }
}
