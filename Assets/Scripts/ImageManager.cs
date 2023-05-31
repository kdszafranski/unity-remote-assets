using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using Defective.JSON;
using TMPro;

public class ImageManager : MonoBehaviour
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
        endpoint += "?api_key=" + apiKey ;

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
        foreach (JSONObject element in images.list)
        {
            StartCoroutine(GetImageFromUrl(imagePath.stringValue, element));
        }

    }

    IEnumerator GetImageFromUrl(string imagePath, JSONObject jsonObject)
    {
        string imageUrl = imagePath + jsonObject.GetField("path").stringValue;
        Debug.Log("getting image:");
        Debug.Log(imageUrl);
        Debug.Log("------");

        // get image and create card to display it
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log("ERROR: " + www.downloadHandler.error);
        } else {
            // Show results as text
            // update UI
            // instantiate card
            Texture2D imageTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            GameObject card = GameObject.Instantiate<GameObject>(imageCardPrefab, imageLayoutGroup.transform);
            
            // Image is the first child in prefab GO
            Image image = card.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI title = card.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (image != null) {
                // put texture into UI sprite
                image.sprite = Sprite.Create(imageTex, new Rect(0, 0, imageTex.width, imageTex.height), new Vector2(0.5f, 0.5f));
                image.preserveAspect = true;
                // update card title
                title.text = jsonObject.GetField("description").stringValue;
            }
        }
    }

  
}
