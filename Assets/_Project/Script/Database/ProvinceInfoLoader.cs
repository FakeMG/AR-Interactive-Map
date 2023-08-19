using System.Collections;
using System.Text;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace FakeMG.Database {
    public class ProvinceInfoLoader : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private RawImage image;

        private string _provinceName;

        private void Awake() {
            _provinceName = transform.parent.name;
            displayName.text = _provinceName;
        }

        private void Start() {
            description.text = "";
        }
    
        public void RetrieveProvinceData(string provinceName) {
            displayName.text = provinceName;
            DownloadImage(provinceName, image);
        
            FirebaseDatabase.DefaultInstance
                .GetReference("provinces").Child(provinceName)
                .GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsFaulted) {
                        Debug.LogError(provinceName + ": " + task.Exception);
                    } else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;
                        description.text = "";
                    
                        foreach (DataSnapshot provincesSnapshot in snapshot.Children) {
                            if (description) {
                                StringBuilder sb = new StringBuilder(description.text);
                                if (provincesSnapshot.Key != "Description")
                                    sb.AppendLine("- " + provincesSnapshot.Key + ": " + provincesSnapshot.Value);
                                description.text = sb.ToString();
                            } else {
                                Debug.Log("ProvinceInfoText is null");
                                return;
                            }
                        }
                    }
                });
        }
    
        private void DownloadImage(string imageName, RawImage rawImage) {
            StorageReference reference = FirebaseStorage.DefaultInstance.GetReference("province icon/" + imageName + ".jpg");

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