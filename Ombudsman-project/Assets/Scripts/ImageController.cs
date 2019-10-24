using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    public delegate void SpriteDelegate(Sprite texture);
    // TODO: Make it coroutine and handle getting resources from server.

    public void GetSprite(string path, SpriteDelegate OnGetSpriteDelegate)
    {
        path = path.Split('.')[0]; // workaround due to extension included in api.

        Sprite resourcesTexture = Resources.Load<Sprite>(path);
        if (resourcesTexture == null)
        {
            throw new UnityException("Failed to get image " + path + " locally. Getting texture from server not implemented yet!");
        }
        OnGetSpriteDelegate(resourcesTexture);
    }
}
