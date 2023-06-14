using System.Collections;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Script.UI {
    public class InfoUIDataLoader : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI content;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private RawImage image;
        
        public void LoadPeopleData(TextMeshProUGUI peopleName) {
            title.text = peopleName.text;
            DownloadImage(peopleName.text, image);
            
            FirebaseDatabase.DefaultInstance
                .GetReference("people").Child(peopleName.text).Child("Description")
                .GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsFaulted) {
                        Debug.Log("Error retrieving people data");
                    } else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;
                        
                        content.text = "";
                        string data = snapshot.Value.ToString();
                        data = data.Replace("\\n", "\n");
                        content.text = data;
                    }
                });
        }
        
        public void LoadProvinceData(TextMeshProUGUI peopleName) {
            title.text = peopleName.text;
            DownloadImage(peopleName.text, image);
            
            FirebaseDatabase.DefaultInstance
                .GetReference("provinces").Child(peopleName.text).Child("Description")
                .GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsFaulted) {
                        Debug.Log("Error retrieving people data");
                    } else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;
                        
                        content.text = "";
                        string data = snapshot.Value.ToString();
                        data = data.Replace("\\n", "\n");
                        content.text = data;
                    }
                });
        }
        
        private void DownloadImage(string imageName, RawImage rawImage) {
            StorageReference reference = FirebaseStorage.DefaultInstance.GetReference("people icon/" + imageName + ".jpg");

            reference.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
                if (!task.IsFaulted && !task.IsCanceled) {
                    StartCoroutine(GetTexture(task.Result.ToString(), rawImage));
                }
            });
        }

        IEnumerator GetTexture(string url, RawImage rawImage) {
            string encodedUrl = url.Replace(" ", "%20");

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(encodedUrl);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error retrieving texture from: "+ encodedUrl + " " + request.error);
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            rawImage.texture = texture;
        }
    }
}