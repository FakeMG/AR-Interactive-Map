using System.Collections;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace FakeMG.Database {
    public class PeopleListDataLoader : MonoBehaviour {
        [SerializeField] private GameObject contentHolder;
        [SerializeField] private GameObject template;

        private void Awake() {
            // Nếu tắt list nhanh quá thì sẽ ko load đc ảnh
            RetrievePeopleName();
        }

        private void RetrievePeopleName() {
            FirebaseDatabase.DefaultInstance
                .GetReference("people")
                .GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsFaulted) {
                        Debug.Log("Error retrieving people data");
                    } else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;

                        foreach (DataSnapshot dataSnapshot in snapshot.Children) {
                            GameObject go = Instantiate(template, contentHolder.transform);
                            go.transform.parent = contentHolder.transform;
                            go.SetActive(true);
                        
                            DownloadImage(dataSnapshot.Key, go.transform.GetChild(0).GetComponent<RawImage>());
                            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = dataSnapshot.Key;
                        }
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