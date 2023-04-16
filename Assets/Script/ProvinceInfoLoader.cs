using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ProvinceInfoLoader : MonoBehaviour {
    [SerializeField] private TextMeshPro displayName;
    [SerializeField] private TextMeshPro description;

    private string _provinceName;
    private void Start() {
        _provinceName = transform.parent.name;
        displayName.text = _provinceName;
        description.text = "";
        
        RetrieveProvinceData(_provinceName);
    }
    
    private void RetrieveProvinceData(string provinceName) {
        FirebaseDatabase.DefaultInstance
            .GetReference("provinces").Child(provinceName)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    Debug.LogError(_provinceName + ": " + task.Exception);
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    
                    foreach (DataSnapshot provincesSnapshot in snapshot.Children) {
                        if (description != null) {
                            description.text += "- " + provincesSnapshot.Key + ": " + provincesSnapshot.Value + "\n";
                        } else {
                            Debug.Log("ProvinceInfoText is null");
                            return;
                        }
                    }
                }
            });
    }
}