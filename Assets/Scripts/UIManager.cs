using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager GetInstance { get { return instance; } }

    public TextMeshProUGUI selectedText;

    private void Awake() {
        instance = this;
    }

    public void SetSelectText(string selectText ) {
        selectedText.text = selectText;
    }
}
