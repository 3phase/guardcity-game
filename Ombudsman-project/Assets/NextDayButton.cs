using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextDayButton : MonoBehaviour
{
    private Button sleepButton;

    private GainsController gainsController;

    private void Awake()
    {
        sleepButton = GetComponent<Button>();
        sleepButton.onClick.AddListener(Sleep);
    }

    private void Start()
    {
        gainsController = GainsController.GetInstance();
    }

    private void Sleep()
    {
        // TODO: Animation
        Debug.Log("Sleep");
        gainsController.RestoreEnergy();
    }
}
