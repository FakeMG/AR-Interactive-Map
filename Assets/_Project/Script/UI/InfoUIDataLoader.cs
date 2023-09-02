using FakeMG.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FakeMG.UI {
    public class InfoUIDataLoader : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI content;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private RawImage image;

        public void LoadDataToUI(string type, string dataName) {
            title.text = dataName;
            var imagePath = type + " icon" + "/" + dataName + ".jpg"; 
            DatabaseBehavior.Instance.DownloadImage(imagePath, image);
            
            DatabaseBehavior.Instance.LoadData(type + "/" + dataName + "/Description", snapshot => {
                content.text = "";
                string data = snapshot.Value.ToString();
                data = data.Replace("\\n", "\n");
                content.text = data;
            });
        }
    }
}