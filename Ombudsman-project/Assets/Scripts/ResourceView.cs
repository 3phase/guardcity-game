﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceView : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text deltaText;

    [SerializeField]
    private TMPro.TMP_Text amountText;

    private int amount;

    private Animator animator;

    private static string animationAddTriggerName = "AddTrigger";
    private static string animationSubstractTriggerName = "SubstractTrigger";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void Alter(int delta, bool activateAnimation = true)
    {
        if (delta == 0) { return; }

        amount += delta;

        deltaText.text = delta.ToString();
        amountText.text = amount.ToString();

        if (activateAnimation)
        {
            animator.SetTrigger(delta > 0 ? animationAddTriggerName : animationSubstractTriggerName);
        }

    }

    public void SetAmount(int amount)
    {
        // Dont use because for some reason it doesn't work.
        int delta = amount - this.amount;

        Alter(amount);
    }

    public int GetAmount()
    {
        return amount;
    }
}
