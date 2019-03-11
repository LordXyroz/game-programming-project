using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DayNightCycle : NetworkBehaviour {
    [Header("Colors")]
    public Color fogDayColor;
    public Color fogNightColor;
    [SyncVar]
    public Color targetColor;
    [SyncVar]
    public Color currentColor;

    [Header("Light strength")]
    public float maxLight = 0.75f;
    public float minLight = 0f;
    [SyncVar]
    public float currentLight = 0.75f;

    public float maxAmbient = 0.5f;
    public float minAmbient = 0.25f;
    [SyncVar]
    public float currentAmbient = 0.5f;

    [Header("Skybox density")]
    public float skyboxMax = 1.25f;
    public float skyboxMin = 0f;
    [SyncVar]
    public float skyboxCurrent = 1.25f;

    [Header("Variables")]
    public float secPerCycle = 1f;
    [SyncVar]
    public double dt = 0.0;
    [SyncVar]
    public float sine;
    [SyncVar]
    public float cosine;
    [SyncVar]
    public float progress;
    [SyncVar]
    public int sunState;

    [Header("Objects")]
    public Light sun;
    public Light ambient;
    public Material skybox;

	// Use this for initialization
	void Start () {
        RenderSettings.fog = true;
        targetColor = fogDayColor;
        progress = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        // Calculates sine and cosine values
        dt += Time.deltaTime * Mathf.PI / (secPerCycle / 2f);
        sine = Mathf.Sin((float)dt);
        cosine = Mathf.Cos((float)dt);

        // Uses sine and cosine to find part of day/night cycle
        if ((sine > -1 && sine < 0) && (cosine > 0 && cosine < 1))
            sunState = 0;  // 0.00 - 6.00
        if ((sine > 0 && sine < 1) && (cosine > 0 && cosine < 1))
            sunState = 1;  // 6.00 - 12.00
        if ((sine > 0 && sine < 1) && (cosine > -1 && cosine < 0))
            sunState = 2;  // 12.00 - 18.00
        if ((sine > -1 && sine < 0) && (cosine > -1 && cosine < 0))
            sunState = 3;  // 18.00 - 0.00

        /// Day/Night cycle determination
        /// Lerps light brightness, skybox density and fog color based on the cycle
        /// Had to do it this way since there was trouble with light shining through terrain at night
        if (sunState == 1) // 6.00 - 12.00, sunrise/day
        {
            // Progress increases, slowly lerping towards max values
            progress += (Time.deltaTime * Mathf.PI * 5f) / (secPerCycle / 2f);
            progress = Mathf.Clamp(progress, 0, 1);

            currentLight = Mathf.Lerp(minLight, maxLight, progress);
            skyboxCurrent = Mathf.Lerp(skyboxMin, skyboxMax, progress);
            currentAmbient = Mathf.Lerp(minAmbient, maxAmbient, progress);
            targetColor = fogDayColor;
        }
        // Using the sun's angle so we don't start decreasing until the sun is low on the horizon
        else if (sunState == 2 && transform.eulerAngles.x < 20f) // 12.00 - 18.00, day/evening
        {
            // Progress decreases, slowly lerping towards min values
            progress -= (Time.deltaTime * Mathf.PI) / (secPerCycle / 2f);
            progress = Mathf.Clamp(progress, 0, 1);

            currentLight = Mathf.Lerp(minLight, maxLight, progress);
            currentAmbient = Mathf.Lerp(minAmbient, maxAmbient, progress);
            skyboxCurrent = Mathf.Lerp(skyboxMin, skyboxMax, progress);
        }
        else if (sunState == 3) // 18.00 - 0.00, evening/night
        {
            // Progress decreases, slowly lerping towards min values
            progress -= (Time.deltaTime * Mathf.PI * 2f) / (secPerCycle / 2f);
            progress = Mathf.Clamp(progress, 0, 1);

            currentLight = Mathf.Lerp(minLight, maxLight, progress);
            skyboxCurrent = Mathf.Lerp(skyboxMin, skyboxMax, progress);
            currentAmbient = Mathf.Lerp(minAmbient, maxAmbient, progress);
            targetColor = fogNightColor;
        }

        // Updating based on which part of the day/night cycle
        transform.position = new Vector3(Mathf.Cos((float)dt), Mathf.Sin((float)dt), 0);
        transform.LookAt(Vector3.zero);

        sun.intensity = currentLight;
        ambient.intensity = currentAmbient;
        skybox.SetFloat("_AtmosphereThickness", skyboxCurrent);

        // Fog
        currentColor = RenderSettings.fogColor;
        RenderSettings.fogColor = Color.Lerp(currentColor, targetColor, 0.25f * Time.deltaTime);
	}
}
