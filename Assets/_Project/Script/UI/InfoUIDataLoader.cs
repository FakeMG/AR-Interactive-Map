using FakeMG.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FakeMG.UI {
    public class InfoUIDataLoader : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI content;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private RawImage image;

        public void LoadDataToUI(TextMeshProUGUI peopleName) {
            LoadDataToUI("people", peopleName.text);
        }

        public void LoadDataToUI(string path, string dataName) {
            title.text = dataName;
            DatabaseBehavior.Instance.DownloadImage(path + " icon", image);
            
            DatabaseBehavior.Instance.LoadData(path + "/" + dataName + "/Description", snapshot => {
                content.text = "";
                string data = snapshot.Value.ToString();
                data = data.Replace("\\n", "\n");
                content.text = data;
            });
        }
    }
}