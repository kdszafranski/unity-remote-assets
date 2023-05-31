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
    [SerializeField] string giphyAPIBaseUrl;

    List<GameObject> gifs;
    // string imageUrl;


    public void loadGifs() 
    {
        Debug.Log("getting gifs");
        
        StartCoroutine(giphyGetRandom());
    }   

    IEnumerator giphyGetRandom()
    {
        // https://api.giphy.com/v1/gifs/random?api_key=Si5ViEEb7tiGsg74eWmCUJYRsvipGFOo&tag=&rating=g

        string endpoint = giphyAPIBaseUrl + "random?";
        endpoint += "api_key=" + giphyAPIKey;

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

        // convert, pull out field values
        JSONObject jsonObject = new JSONObject(jsonText);
        var data = jsonObject["data"];
        var title = data["title"];
        var rating = data["rating"];
        var images = data["images"];
        var image = images["fixed_width_still"];
        var imageUrl = image["url"].stringValue;

        // update UI
        titleText.text = title.stringValue;
        ratingText.text = imageUrl;

        // get actual image file
        StartCoroutine(GetImageFromUrl(imageUrl));

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
