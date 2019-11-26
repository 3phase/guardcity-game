using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField]
    private TMP_Text message;

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    private int clickedOptionIndex = -1;

    private bool blockingCode = false;
    private bool canFinishBlocking = false;


    private float defaultTimeScale;
    private float defaultFixedTime;


    private void Awake()
    {
        gameObject.SetActive(false);

        int index = 0;
        buttons.ForEach((Button button) =>
        {
            button.onClick.AddListener(() =>
            {
                clickedOptionIndex = index++;
                Hide();
            });
        });
    }


    public void SetMessage(string text)
    {
        message.text = text;
    }


    public void SetButtonBehaviour(int buttonIndex, string buttonText, UnityAction buttonBehaviour)
    {
        if (buttonBehaviour == null)
        {
            throw new System.ArgumentNullException(nameof(buttonBehaviour));
        }

        Button button = buttons[buttonIndex];
        if(button == null)
        {
            Debug.Log("Attempting to access button which doesn't exist!");
            return;
        }

        button.GetComponentInChildren<TMP_Text>().text = buttonText;
        button.onClick.AddListener(buttonBehaviour);
    }


    public void Show()
    {
        gameObject.SetActive(true);

        defaultTimeScale = Time.timeScale;
        defaultFixedTime = Time.fixedDeltaTime;

        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
    }

    public IEnumerator BlockUntilClicked()
    {
        blockingCode = true;
        while(canFinishBlocking == false)
        {
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    public int GetClickedOptionIndex()
    {
        return clickedOptionIndex;
    }


    public void Hide()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedTime;

        // Take responsibility of cleaning up message box if not blocking. Otherwise left to the BlockUntilClicked coroutine.
        if(blockingCode)
        {
            canFinishBlocking = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
