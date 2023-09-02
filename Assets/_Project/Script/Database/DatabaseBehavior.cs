using System;
using System.Collections;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace FakeMG.Database {
    public class DatabaseBehavior : MonoBehaviour {
        public static DatabaseBehavior Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        public void LoadData(string path, Action<DataSnapshot> callback) {
            FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync()
                .ContinueWithOnMainThread(task => {
                    if (task.IsFaulted) {
                        Debug.LogError("Error retrieving data");
                    } else if (task.IsCompleted) {
                        callback(task.Result);
                    }
                });
        }
        
        public void DownloadImage(string path, RawImage rawImage) {
            StorageReference reference = FirebaseStorage.DefaultInstance.GetReference(path);

            reference.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
                if (!task.IsFaulted && !task.IsCanceled) {
                    StartCoroutine(GetTexture(task.Result.ToString(), rawImage));
                }
            });
        }

        private IEnumerator GetTexture(string url, RawImage rawImage) {
            string encodedUrl = url.Replace(" ", "%20");

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(encodedUrl);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error retrieving texture from: " + encodedUrl + " " + request.error);
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            rawImage.texture = texture;
        }

        // private void RemoveAccentMark(string input) {
        //     input = "Việt Nam là quê hương tôi";
        //     char[] charArray = input.Normalize(NormalizationForm.FormD)
        //         .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        //         .ToArray();
        //     string output = new string(charArray);
        //     Debug.Log(output);
        // }
        //
        // private void WriteDataFromExcel() {
        //     FileStream stream = File.Open("C://Users//Lam//Desktop//file.xlsx", FileMode.Open, FileAccess.Read);
        //     IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //
        //     if (excelReader == null) {
        //         Debug.Log("Invalid file");
        //         return;
        //     }
        //
        //     Debug.Log("Valid file");
        //
        //     excelReader.Read();
        //     List<string> columns = new List<string>();
        //     for (int i = 0; i < excelReader.FieldCount; i++) {
        //         string columnName = excelReader.GetString(i);
        //         if (!string.IsNullOrEmpty(columnName)) {
        //             columns.Add(columnName);
        //         }
        //     }
        //
        //     while (excelReader.Read()) {
        //         Dictionary<string, object> rowData = new Dictionary<string, object>();
        //         string provinceName = "";
        //
        //         for (int i = 0; i < columns.Count; i++) {
        //             if (i == 1) {
        //                 provinceName = excelReader.GetString(i);
        //                 continue;
        //             }
        //
        //             object cellValue = excelReader.GetValue(i);
        //             if (cellValue != null) {
        //                 rowData.Add(columns[i], cellValue);
        //             }
        //         }
        //
        //         _databaseReference.Child("provinces").Child(provinceName).SetValueAsync(rowData);
        //     }
        //
        //     excelReader.Close();
        //     stream.Close();
        // }
    }
}