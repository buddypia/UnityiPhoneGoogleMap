using UnityEngine;
using System.Collections;

public class GoogleMapScript : MonoBehaviour {

    static float lat;
    static float lon;

    LocationInfo currentGPSPosition;

    public int size = 512;
    public int zoom = 14;
    public WWW DownloadImg;

    // Wait until service initializes
    int maxWait = 20;

    private STATE state = STATE.ENABLE;

    public enum STATE {
        ENABLE,
        WAIT,
        END
    }

    void Start() {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser) {
            PlayerPrefs.SetString("Warning!!", "Location Services is not valid.");
        }

        // Start service before querying location
        Input.location.Start();

        StartCoroutine("Initialize");
        
        InvokeRepeating("RetrieveGPSData", 0, 5);
        //RetrieveGPSData();
    }

    void Update() {

    }

    IEnumerator Initialize() {
        Debug.Log("Initiallize");

        state = STATE.ENABLE;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1) {
            PlayerPrefs.SetString("Warning!!", "Timed out.");
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed) {
            PlayerPrefs.SetString("Warning!!", "Unable to determine device location.");
        }

        Debug.Log("Initiallized");
    }

    IEnumerator Download(string url) {
        Debug.Log("Download");
        Debug.Log("Download URL : " + url);

        DownloadImg = new WWW(url);
        yield return DownloadImg; // Wait for download to complete

        GameObject.FindGameObjectWithTag("Map").guiTexture.texture = DownloadImg.texture;

        state = STATE.ENABLE;

        Debug.Log("Downloaded");
    }

    void RetrieveGPSData() {
        if (state == STATE.ENABLE) {
            state = STATE.WAIT;

            currentGPSPosition = Input.location.lastData;

            lat = currentGPSPosition.latitude;
            lon = currentGPSPosition.longitude;

            if (Input.location.status == LocationServiceStatus.Stopped) {
                // Dummy Data
                lat = 35.710071f;
                lon = 139.810707f;
            }


            Debug.Log("Refresh");
            var url = "http://maps.googleapis.com/maps/api/staticmap";
            var qs = "";
            //if (!autoLocateCenter) {
            //    if (centerLocation.address != "")
            //        qs += "center=" + HTTP.URL.Encode(centerLocation.address);
            //    else {
            qs += "center=" + string.Format("{0},{1}", lat, lon);
            //    }

            qs += "&zoom=" + zoom.ToString();
            //}
            qs += "&size=" + string.Format("{0}x{0}", size);
            qs += "&scale=" + "2";
            qs += "&maptype=" + "roadmap";

            var usingSensor = false;

            #if UNITY_IPHONE
                    usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
            #endif

            qs += "&sensor=" + (usingSensor ? "true" : "false");

            Debug.Log("Refreshed");

            StartCoroutine(Download(url + "?" + qs));
        }
        
    }
}