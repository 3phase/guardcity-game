﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    public delegate void TextureDelegate(Texture2D texture);

    public void GetTexture(string path, TextureDelegate OnGetTextureDelegate)
    {
        path = path.Split('.')[0]; // workaround due to extension included in api.

        Texture2D resourcesTexture = Resources.Load<Texture2D>(path);
        if(resourcesTexture == null)
        {
            throw new UnityException("Failed to get image " + path + " locally. Getting texture from server not implemented yet!");
        }
        OnGetTextureDelegate(resourcesTexture);
    }
}
