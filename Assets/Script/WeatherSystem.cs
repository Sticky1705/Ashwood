using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

public class WeatherSystem : MonoBehaviour
{
    [Range(0f, 1f)]
    public float chanceToRainSpring = 0.3f;
    [Range(0f, 1f)]
    public float chanceToRainSummer = 0.1f;
    [Range(0f, 1f)]
    public float chanceToRainFall = 0.4f;
    [Range(0f, 1f)]
    public float chanceToRainWinter = 0.7f;
    [Range(0f, 1f)]
    public float chanceToSnowWinter = 1.0f;

    public GameObject rainEffect;
    public Material rainSkybox;
    public AudioSource rainChannel;
    public AudioClip rainSound;

    public GameObject snowEffect;
    public Material snowSkybox;
    public AudioSource snowChannel;
    public AudioClip snowSound;

    public GameObject lightningEffect;
    public AudioSource thunderChannel;
    public AudioClip thunderSound;
    public float lightningFrequency = 10f;

    public Light sceneLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;

    public bool SpecialWeather;

    public enum WeatherCondition
    {
        Sunny,
        Rainy,
        Snow
    }

    private WeatherCondition currentWeather = WeatherCondition.Sunny;

    private void Start()
    {
        TimeManager.Instance.OnDayPass.AddListener(GenerateRandomWeather);
    }

    private void GenerateRandomWeather()
    {
        TimeManager.Season currentSeason = TimeManager.Instance.currentSeaon;

        float chanceToRain = 0f;

        switch (currentSeason)
        {
            case TimeManager.Season.Spring:
                chanceToRain = chanceToRainSpring;
                break;
            case TimeManager.Season.Summer:
                chanceToRain = chanceToRainSummer;
                break;
            case TimeManager.Season.Fall:
                chanceToRain = chanceToRainFall;
                break;
            case TimeManager.Season.Winter:
                chanceToRain = chanceToRainWinter;
                break;
        }

        if (currentSeason == TimeManager.Season.Winter && Random.value <= chanceToSnowWinter)
        {
            currentWeather = WeatherCondition.Snow;
            SpecialWeather = true;
            Invoke("StartSnow", 1f);
        }
        else if (Random.value <= chanceToRain)
        {
            currentWeather = WeatherCondition.Rainy;
            SpecialWeather = true;
            Invoke("StartRain", 1f);
        }
        else
        {
            currentWeather = WeatherCondition.Sunny;
            SpecialWeather = false;
            StopRain();
            StopSnow();
        }
    }

    private void StartRain()
    {
        if (!rainChannel.isPlaying)
        {
            rainChannel.clip = rainSound;
            rainChannel.loop = true;
            rainChannel.Play();
        }

        RenderSettings.skybox = rainSkybox;
        rainEffect.SetActive(true);

        StartCoroutine(LightningEffect());
    }

    private IEnumerator LightningEffect()
    {
        while (currentWeather == WeatherCondition.Rainy)
        {
            yield return new WaitForSeconds(Random.Range(5f, lightningFrequency));

            Vector3 randomPosition = new Vector3(Random.Range(-50f, 50f), 10f, Random.Range(-50f, 50f));
            GameObject newLightning = Instantiate(lightningEffect, randomPosition, Quaternion.identity);

            thunderChannel.PlayOneShot(thunderSound);

            StartCoroutine(FlickerLight());

            yield return new WaitForSeconds(0.5f);

            Destroy(newLightning);
        }
    }

    private IEnumerator FlickerLight()
    {
        for (int i = 0; i < 3; i++) 
        {
            sceneLight.intensity = maxIntensity;
            yield return new WaitForSeconds(0.1f);
            sceneLight.intensity = minIntensity;
            yield return new WaitForSeconds(0.1f);
        }

        sceneLight.intensity = 1.0f;
    }

    private void StopRain()
    {
        if (rainChannel.isPlaying)
        {
            rainChannel.Stop();
        }

        rainEffect.SetActive(false);
        StopCoroutine(LightningEffect());
        lightningEffect.SetActive(false);
    }

    private void StartSnow()
    {
        if (!snowChannel.isPlaying)
        {
            snowChannel.clip = snowSound;
            snowChannel.loop = true;
            snowChannel.Play();
        }

        RenderSettings.skybox = snowSkybox;
        snowEffect.SetActive(true);
    }

    private void StopSnow()
    {
        if (snowChannel.isPlaying)
        {
            snowChannel.Stop();
        }

        snowEffect.SetActive(false);
    }
}