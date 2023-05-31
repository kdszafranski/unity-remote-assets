using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using Defective.JSON;
using TMPro;

public class Giphy : MonoBehaviour
{
    [SerializeField] GameObject imageLayoutGroup;
    [SerializeField] GameObject imageCardPrefab;

    [SerializeField] string apiKey;
    [SerializeField] string assetBaseUrl;


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


        // get actual image files
        string imageUrl = "";
        foreach (JSONObject element in images.list)
        {

            //titleText.text = images[1].GetField("description").stringValue;
            imageUrl = imagePath.stringValue + element.GetField("path").stringValue;
            StartCoroutine(GetImageFromUrl(imageUrl));
        }

    }

    IEnumerator GetImageFromUrl(string url)
    {
        Debug.Log("getting image:");
        Debug.Log(url);
        Debug.Log("------");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log("ERROR: " + www.downloadHandler.error);
        }
        else {
            // Show results as text
            // update UI
            // instantiate card
            Texture2D imageTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            GameObject card = GameObject.Instantiate<GameObject>(imageCardPrefab, imageLayoutGroup.transform);
            
            // Image is the first child in prefab GO
            Image image = card.transform.GetChild(0).GetComponent<Image>();
            if(image != null)
            {
                image.sprite = Sprite.Create(imageTex, new Rect(0, 0, imageTex.width, imageTex.height), new Vector2(0.5f, 0.5f));
                image.preserveAspect = true;
            }
        }
    }

  
}
