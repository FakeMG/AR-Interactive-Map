using System.Text;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class ProvinceInfoLoader : MonoBehaviour {
    [SerializeField] private TextMeshPro displayName;
    [SerializeField] private TextMeshPro description;

    private string _provinceName;
    private void Start() {
        _provinceName = transform.parent.name;
        displayName.text = _provinceName;
        description.text = "";
    }
    
    public void RetrieveProvinceData(string provinceName) {
        displayName.text = provinceName;
        
        FirebaseDatabase.DefaultInstance
            .GetReference("provinces").Child(provinceName)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    Debug.LogError(provinceName + ": " + task.Exception);
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    description.text = "";
                    
                    foreach (DataSnapshot provincesSnapshot in snapshot.Children) {
                        if (!ReferenceEquals(description, null)) {
                            StringBuilder sb = new StringBuilder(description.text);
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
}