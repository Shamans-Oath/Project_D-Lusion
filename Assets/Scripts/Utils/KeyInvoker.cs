using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class KeyInvoker : MonoBehaviour
{
    public KeyEvent[] keyEvents;
    public GameObject visualParent;
    // Start is called before the first frame update
    void Start()
    {
        if (keyEvents.Length <= 0) return;

        for (int i = 0; i < keyEvents.Length; i++)
        {
            GameObject tmpObj = Instantiate(new GameObject("KeyEventReference" + i));

            tmpObj.transform.parent = visualParent.transform;
            TextMeshProUGUI textmesh = tmpObj.AddComponent<TextMeshProUGUI>();
            textmesh.UpdateFontAsset();
            textmesh.fontSize = 0;
            textmesh.text = keyEvents[i].KeyCode.ToString() + " to " + keyEvents[i].name;
            textmesh.verticalAlignment = TMPro.VerticalAlignmentOptions.Top;
            textmesh.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
            textmesh.autoSizeTextContainer = true;
            textmesh.fontSizeMin = 10;
            textmesh.fontSizeMax = 72;
            textmesh.enableWordWrapping = true;
            textmesh.enableAutoSizing = true;

            textmesh.raycastTarget = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (keyEvents.Length <= 0) return;

        for (int i = 0; i < keyEvents.Length; i++)
        {
            if (Input.GetKeyDown(keyEvents[i].KeyCode))
            {
                keyEvents[i].callEvent.Invoke();
            }

        }
    }

    public void EnableVisualReference()
    {
        visualParent.SetActive(!visualParent.activeSelf);
    }
}

[System.Serializable]
public class KeyEvent
{
    public string name;
    public KeyCode KeyCode;
    public UnityEvent callEvent;
}
