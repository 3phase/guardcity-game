using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    TMP_Text nameText;

    public void SetAvatar(Sprite avatarSprite, string name)
    {
        image.sprite = avatarSprite;
        nameText.text = name;
    }
}
