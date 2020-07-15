using System;
using System.Net;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

using System.Xml;

public class TestImageDownload : MonoBehaviour
{
    string url = "https://pin.it/l3n3xhwckmzukg";

    // Start is called before the first frame update
    IEnumerator Start()
    {
        using (var uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();

            var data = uwr.downloadHandler.text;
            if (data != null)
            {
                var replaced = data.Replace("\"", "");
                var lookup = "og:image content=";
                var startingIndex = replaced.IndexOf(lookup);
                startingIndex += lookup.Length;
                var endingIndex = replaced.IndexOf(" ", startingIndex);
                var imageUrl = replaced.Substring(startingIndex, endingIndex - startingIndex);
                Debug.Log("we got some data " + imageUrl);
            }
        }
    }
}
