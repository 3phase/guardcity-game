using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleBuilder : Editor
{

    [MenuItem("Assets/BuildBundle")]
    static async void BuildBundle()
    {
        try
        {
            string bundleName = "";
            List<string> assetPaths = new List<string>();

            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] == "-bundleName")
                {
                    if (i + 1 < args.Length)
                    {
                        bundleName = args[i + 1];
                    }
                    else
                    {
                        Debug.LogError("Cannot set bundle name since it is out of range!");
                    }
                }
                else if (args[i] == "-assetPaths")
                {
                    for (int j = i + 1; j < args.Length && args[j][0] != '-'; j++)
                    {
                        assetPaths.Add(args[j]);
                    }
                }
            }

            if(bundleName == "")
            {
                throw new System.FormatException("Bundle name not given!");
            }

            if(assetPaths.Count == 0)
            {
                throw new System.FormatException("Asset paths not given!");
            }


            List<UnityWebRequestAsyncOperation> webRequests = new List<UnityWebRequestAsyncOperation>();
            foreach (string assetPath in assetPaths)
            {
                if (File.Exists(assetPath) == false)
                {
                    throw new FileNotFoundException("Path given does not exist: " + assetPath);
                }
                UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture("file://" + assetPath);
                UnityWebRequestAsyncOperation asyncOp = webRequest.SendWebRequest();
                webRequests.Add(asyncOp);
            }

            List<Object> assets = new List<Object>();

            List<string> assetNames = new List<string>();
            for (int i = 0; i < webRequests.Count; i++)
            {
                UnityWebRequestAsyncOperation asyncOp = webRequests[i];

                string filename = Path.GetFileNameWithoutExtension(asyncOp.webRequest.uri.AbsolutePath);

                while (asyncOp.isDone == false)
                {
                    await Task.Delay(100);
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(asyncOp.webRequest);

                byte[] textureData = texture.EncodeToPNG();

                string path = Application.dataPath + "/Resources/" + filename + ".png";
                File.WriteAllBytes(path, textureData);

                assetNames.Add("Assets/Resources/" + filename + ".png");
            }


            AssetBundleBuild bundleBuild = new AssetBundleBuild();
            bundleBuild.assetBundleName = bundleName;
            bundleBuild.assetNames = assetNames.ToArray();

            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
            buildMap[0] = bundleBuild;

            BuildPipeline.BuildAssetBundles("Assets/AssetBundles/", buildMap, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.WebGL);
        }
        catch(System.Exception e)
        {
            System.Console.WriteLine(e.ToString() + ": " + e.Message);
            EditorApplication.Exit(-1);
        }

        EditorApplication.Exit(0);
    }
}
