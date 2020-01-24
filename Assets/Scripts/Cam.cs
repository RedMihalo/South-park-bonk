using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Cam : MonoBehaviour
{
    //private Renderer CamTarget;
    //public RawImage CamImage;
    // public Texture CamTexture;
    public RawImage image;
    public RawImage image1;
    public RawImage image2;
    public RawImage image3;

    public Text textUI;

    [TextArea]
    public string textytext;
    // Start is called before the first frame update
    void Start()
    {
       /* CamTarget = GetComponent<Renderer>();

        
        */
        //GetComponent<Renderer>().material.mainTexture = camera;
        
        textytext = "Debugging" + Environment.NewLine;
       /* CamTarget = GetComponent<Renderer>();

        
        */
        //GetComponent<Renderer>().material.mainTexture = camera;
        
        WebCamDevice[] devices = WebCamTexture.devices;
        
        if (devices.Length == 0)
        {
            textytext += "No camera detected\n";
        }

        WebCamTexture camera = null;
        WebCamTexture camera1 = null;
        WebCamTexture camera2 = null;
        WebCamTexture camera3 = null;

        foreach(var device in devices){
            textytext  += device.name;
        }
        //textytext  += device.name;
            
           // device
        
        if(devices.Length >=1)
        {
            camera = new WebCamTexture(devices[0].name);
            camera.Play();   
            image.texture = camera;
        }
        if(devices.Length >=2)
        {
            camera1 = new WebCamTexture(devices[1].name);
            camera1.Play();   
            image.texture = camera1;
        }
        if(devices.Length >=3)
        {
            camera2 = new WebCamTexture(devices[2].name);
            camera2.Play();   
            image.texture = camera2;
        }
        if(devices.Length >=4)
        {
            camera3 = new WebCamTexture(devices[3].name);
            camera3.Play();   
            image.texture = camera3;
        }




        textUI.text = textytext;
             
        


        
        /*WebCamDevice[] cameras = WebCamTexture.devices;
        foreach (var c in cameras)
            if (c.isFrontFacing)
            {
                camera = new WebCamTexture(c.name);
                break;
            }*/

       /* if (camera != null)
            camera.Play();
        CamImage.texture = camera;
        CamTarget.material.color = Color.white;

        string messagetext = "Number of cameras: " + cameras.Length.ToString();
        int fromt = 0;
        foreach (var c in cameras)
            fromt += c.isFrontFacing ? 1 : 0;
        messagetext += "\nNumber of front facing cameras: " + fromt;
        message.text = messagetext;*/

    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
         textytext+="CLIIIIIIIIIIIIIICKED";
    }
}