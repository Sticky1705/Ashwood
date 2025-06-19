using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Intance { get; set; }

    public bool OnTarget;
    public GameObject SelectedObject;

    public GameObject interaction_Info_UI;
    Text interaction_text;

    public Image centerDotImage;
    public Image handIcon;

    private void Start()
    {
        OnTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    private void Awake()
    {
        if (Intance != null && Intance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Intance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactAble = selectionTransform.GetComponent<InteractableObject>();

            if (interactAble && interactAble.playerRange)
            {
                OnTarget = true;
                SelectedObject = interactAble.gameObject;
                interaction_text.text = interactAble.GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else
            {
                OnTarget= false;
                interaction_Info_UI.SetActive(false);
            }

        }
        else
        {
            OnTarget = false;
            interaction_Info_UI.SetActive(false);
        }
    }

    /*public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);

        SelectedObject = null;
    }*/

    /*public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }*/
}
