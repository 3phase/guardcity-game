using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ImageController : MonoBehaviour
{
    public delegate void SpriteDelegate(Sprite texture);
    // TODO: Make it coroutine and handle getting resources from server.

    public IEnumerator GetSprite(string filename, SpriteDelegate OnGetSpriteDelegate)
    {
        string path = Path.Combine(Application.streamingAssetsPath, filename + ".png");

        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(path);

        yield return textureRequest.SendWebRequest();

        if (textureRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(textureRequest.error);
            throw new UnityException("Unable to get " + filename + " with web request to " + path);
        }
        else
        {
            Texture2D webTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture as Texture2D;
            Sprite webSprite = Sprite.Create(webTexture, new Rect(0.0f, 0.0f, webTexture.width, webTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            OnGetSpriteDelegate(webSprite);
        }

        yield break;
    }
}
