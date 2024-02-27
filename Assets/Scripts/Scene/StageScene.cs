using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageScene : MonoBehaviour
{
    [SerializeField] Image image;
    public GameObject afterPopup;
    private void Awake()
    {
        if (PlayerPrefs.GetString("SnowWhiteClear") == "true")
        {
            this.gameObject.GetComponent<Image>().sprite = image.sprite;
            this.gameObject.GetComponent<PopupManager2>().popup2 = afterPopup;
        }
    }
}
