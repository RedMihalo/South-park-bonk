using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cam : MonoBehaviour
{
    private Renderer CamTarget;
    public RawImage CamImage;
    // public Texture CamTexture;
    public Text message;

    // Start is called before the first frame update
    void Start()
    {
        CamTarget = GetComponent<Renderer>();

        WebCamTexture camera = null;
        WebCamDevice[] cameras = WebCamTexture.devices;
        foreach (var c in cameras)
            if (c.isFrontFacing)
            {
                camera = new WebCamTexture(c.name);
                break;
            }

        if (camera != null)
            camera.Play();
        CamImage.texture = camera;
        CamTarget.material.color = Color.white;

        string messagetext = "Number of cameras: " + cameras.Length.ToString();
        int fromt = 0;
        foreach (var c in cameras)
            fromt += c.isFrontFacing ? 1 : 0;
        messagetext += "\nNumber of front facing cameras: " + fromt;
        message.text = messagetext;

    }
}