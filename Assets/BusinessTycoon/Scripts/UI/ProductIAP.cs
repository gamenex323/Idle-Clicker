using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductIAP : MonoBehaviour
{
    public string packName;
    public Button iapButton;
    private void OnEnable()
    {
        OnPurchased();
    }

    public void OnPurchased()
    {
        if (PlayerPrefs.GetInt(packName) == 1)
        {
            iapButton.interactable = false;
            iapButton.transform.GetChild(0).GetComponent<Text>().text = "Purchased";
        }
        else
        {
            iapButton.interactable = true;
            iapButton.transform.GetChild(0).GetComponent<Text>().text = "Purchase";
        }

    }
}
