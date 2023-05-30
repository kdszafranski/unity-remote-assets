using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Defective.JSON;
using TMPro;

public class Giphy : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI ratingText;
    [SerializeField] GameObject gifCardUIPrefab;

    [SerializeField] string giphyAPIKey;
    [SerializeField] string giphyAPIBaseUrl;

    List<GameObject> gifs;


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

        // update UI
        titleText.text = title.stringValue;
        ratingText.text = rating.stringValue;



        // create card
        // set parent to panel
        // populate fields
    }

  
}
