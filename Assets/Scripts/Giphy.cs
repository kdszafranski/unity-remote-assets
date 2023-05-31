using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using Defective.JSON;
using TMPro;

public class Giphy : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Image uiImage;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI ratingText;
    [SerializeField] GameObject gifCardUIPrefab;

    [SerializeField] string giphyAPIKey;
    [SerializeField] string assetBaseUrl;

    List<GameObject> gifs;
    // string imageUrl;

    // called on UI button click
    public void loadAssets() 
    {
        Debug.Log("getting assets");
        
        StartCoroutine(getAssetsFromUrl());
    }   

    IEnumerator getAssetsFromUrl()
    {
       
        string endpoint = assetBaseUrl;
        //endpoint += "api_key=" + giphyAPIKey;

        UnityWebRequest www = UnityWebRequest.Get(endpoint);
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            displayImages(www.downloadHandler.text);
        }
    }

    void displayImages(string jsonText) 
    {
        Debug.Log(jsonText);

        //convert, pull out field values
        JSONObject jsonObject = new JSONObject(jsonText);
        var imagePath = jsonObject["imagePath"];
        var images = jsonObject["images"];

        // update UI
        titleText.text = images[1].GetField("description").stringValue;

        //// get actual image file
        //StartCoroutine(GetImageFromUrl(imageUrl));

    }

    IEnumerator GetImageFromUrl(string url)
    {
        // url = url.Replace("media3", "i");
        url = url.Replace("\\", "");
        url = url.Replace("media1", "i");
        url = url.Replace("media2", "i");
        url = url.Replace("media3", "i");
        url = url.Replace("media4", "i");
        Debug.Log("getting image:");
        Debug.Log(url);
        Debug.Log("------");
        //url = "http://www.kdszafranski.com/img/game-images/add-logo.png";

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log("ERROR: " + www.downloadHandler.error);
        }
        else {
            // Show results as text
            Debug.Log("here");
            Texture2D imageTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            uiImage.sprite = Sprite.Create(imageTex, new Rect(0, 0, imageTex.width, imageTex.height), new Vector2(0.5f, 0.5f));
            uiImage.preserveAspect = true;
            //uiImage.rectTransform.
        }
    }

  
}
