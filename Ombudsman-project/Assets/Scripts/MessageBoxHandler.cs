using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct MessageBoxChoiceBehaviour
{
    public string text;
    public UnityAction behaviour;

    public MessageBoxChoiceBehaviour(string text, UnityAction behaviour)
    {
        this.text = text;
        this.behaviour = behaviour;
    }

    public MessageBoxChoiceBehaviour(string text)
    {
        this.text = text;
        // TODO: Fix my autism.
        this.behaviour = () => { };
    }
}


public class MessageBoxHandler : MonoBehaviour
{
    [SerializeField]
    private MessageBox messageBoxPrefabOneChoice;

    [SerializeField]
    private MessageBox messageBoxPrefabTwoChoices;

    [SerializeField]
    private MessageBox messageBoxPrefabThreeChoices;


    public static MessageBoxHandler boxHandler { get; private set; }


    private void Awake()
    {
        if(boxHandler == null)
        {
            boxHandler = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public MessageBox DisplayMessage(string message, MessageBoxChoiceBehaviour choiceOneBehaviour)
    {
        MessageBox messageBox = Instantiate(messageBoxPrefabOneChoice, gameObject.transform);
        BuildMessageBox(messageBox, message, new List<MessageBoxChoiceBehaviour>() { choiceOneBehaviour});
        messageBox.Show();

        return messageBox;
    }


    public MessageBox DisplayMessage(string message, MessageBoxChoiceBehaviour choiceOneBehaviour, MessageBoxChoiceBehaviour choiceTwoBehaviour)
    {
        MessageBox messageBox = Instantiate(messageBoxPrefabTwoChoices, gameObject.transform);
        BuildMessageBox(messageBox, message, new List<MessageBoxChoiceBehaviour>() { choiceOneBehaviour, choiceTwoBehaviour});
        messageBox.Show();

        return messageBox;
    }


    public MessageBox DisplayMessage(string message, MessageBoxChoiceBehaviour choiceOneBehaviour, MessageBoxChoiceBehaviour choiceTwoBehaviour, MessageBoxChoiceBehaviour choiceThreeBehaviour)
    {
        MessageBox messageBox = Instantiate(messageBoxPrefabThreeChoices, gameObject.transform);
        BuildMessageBox(messageBox, message, new List<MessageBoxChoiceBehaviour>() { choiceOneBehaviour, choiceTwoBehaviour, choiceThreeBehaviour });
        messageBox.Show();

        return messageBox;
    }


    private void BuildMessageBox(MessageBox messageBox, string message, List<MessageBoxChoiceBehaviour> choiceBehaviours)
    {
        messageBox.SetMessage(message);
        for(int i = 0; i < choiceBehaviours.Count; i++)
        {
            messageBox.SetButtonBehaviour(i, choiceBehaviours[i].text, choiceBehaviours[i].behaviour);
        }
    }
}
