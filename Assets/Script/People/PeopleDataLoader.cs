using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class PeopleDataLoader : MonoBehaviour {
    [SerializeField] private GameObject contentHolder;
    [SerializeField] private GameObject template;

    private void Start() {
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
                        go.SetActive(true);
                        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = dataSnapshot.Key;
                        go.transform.parent = contentHolder.transform;
                    }
                }
            });
    }
}