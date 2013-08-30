using UnityEngine;
using System.Collections;

public class Center : MonoBehaviour {

    Texture background;
    int imageWidth = Screen.width;
    int imageHeight = Screen.height;


	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (guiTexture.texture) {
            Rect tmp = guiTexture.pixelInset;
            tmp.width = imageWidth;
            tmp.height = imageHeight;
            tmp.x = -imageWidth / 2;
            tmp.y = -imageHeight / 2;

            guiTexture.pixelInset = tmp;
        }       
	}


    //void OnGUI(){
    //    if (state == STATE.On) {
    //        Debug.Log("Draw On");
    //        GUI.DrawTexture(new Rect(Screen.width / 2 - 128, Screen.height / 2 - 128, background.width, background.width), background);
    //        
    //    }
    //}
}
