using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Cam : MonoBehaviour, IPointerClickHandler
{
    public RawImage imageTarget;
    public RawImage image1;
    public RawImage image2;
    public RawImage image3;

    private WebCamTexture camera;
    private WebCamTexture camera1;
    private WebCamTexture camera2;
    private WebCamTexture camera3;

    public Text textUI;
    public bool camStarted;

    // Start is called before the first frame update
    void Start() {
        camStarted = false;
        textUI.text= "";
        /*camStarted = false;
        StartCam();*/
    }

    void Update() {

    }

    public void OnPointerClick(PointerEventData eventData) {
         textUI.text += "" + camStarted;
        if (camStarted == false) {
            StartCam();
        }
        if (camStarted == true){
            StopCam();
        }   
//        StartCam();
    }

    private void StartCam() {

        camStarted = true;
        textUI.text = "Debugging" + Environment.NewLine;

        WebCamDevice[] devices = WebCamTexture.devices;
        
        if (devices.Length == 0)
        {
            textUI.text += "No camera detected\n";
        }

        foreach(var device in devices){
            textUI.text += device.name;
        }
        
        if(devices.Length >=1)
        {
            camera = new WebCamTexture(devices[0].name);
            camera.Play();   
            imageTarget.texture = camera;
        }
        if(devices.Length >=2)
        {
            camera1 = new WebCamTexture(devices[1].name);
            camera1.Play();   
            image1.texture = camera1;
        }
        if(devices.Length >=3)
        {
            camera2 = new WebCamTexture(devices[2].name);
            camera2.Play();   
            image2.texture = camera2;
        }
        if(devices.Length >=4)
        {
            camera3 = new WebCamTexture(devices[3].name);
            camera3.Play();   
            image3.texture = camera3;
        }
        
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

    private void StopCam(){
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length >=1)
        {
            camera.Stop();   
        }
        if(devices.Length >=2)
        {
            camera1.Stop();   
        }
        if(devices.Length >=3)
        {
            camera2.Stop();   
        }
        if(devices.Length >=4)
        {
            camera3.Stop();   
        }
    }
}