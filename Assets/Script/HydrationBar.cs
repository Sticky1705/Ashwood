using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;

public class HydrationBar : MonoBehaviour
{
    private Slider slider;
    public Text hydrationText;

    public GameObject playerState;

    private float currentHydration, maxHydration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHydration = playerState.GetComponent<PlayerState>().currentHydration;
        maxHydration = playerState.GetComponent<PlayerState>().maxHydration;

        float fillValue = currentHydration / maxHydration;
        slider.value = fillValue;

        hydrationText.text = currentHydration + "/" + maxHydration;
    }
}
