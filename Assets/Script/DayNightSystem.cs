using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DayNightSystem : MonoBehaviour
{
    public Light directionalLight;
    public float dayDuration = 24.0f;
    public int currentHour;
    float currentTimeOfDay = 0.35f;

    public List<SkyboxTimeMapping> TimeMappings;

    float blendedValue = 0.0f;
    bool lockNextDayTrigger = false;

    public WeatherSystem weatherSystem;

    // Update is called once per frame
    void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDuration;
        currentTimeOfDay %= 1;

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 350) - 90, 170, 0));

        if (weatherSystem.SpecialWeather == false)
        {
            UpdateSkyBox();
        }

        if (currentHour == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }

        if (currentHour != 0)
        {
            lockNextDayTrigger = false;
        }
    }

    private void UpdateSkyBox()
    {
        Material currentSkyBox = null;
        foreach (SkyboxTimeMapping mapping in TimeMappings)
        {
            if (currentHour == mapping.Hour)
            {
                currentSkyBox = mapping.skyboxMaterial;

                if (currentSkyBox.shader.name == "Custom/SkyboxTransition")
                {
                    blendedValue += Time.deltaTime;
                    blendedValue = Mathf.Clamp01(blendedValue);

                    currentSkyBox.SetFloat("_TransitionFactor", blendedValue);
                }
                else
                {
                    blendedValue = 0;

                }

                break;
            }
        }

        if (currentSkyBox != null)
        {
            RenderSettings.skybox = currentSkyBox;
        }
    }
}

[System.Serializable]

public class SkyboxTimeMapping
{
    public string phaseName;
    public int Hour;
    public Material skyboxMaterial;
}
