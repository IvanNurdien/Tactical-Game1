using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{

    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        //gameObject.SetActive(false);
    }

    public void SetText(string headerContent, string messageContent)
    {
        headerField.text = headerContent;
        contentField.text = messageContent;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Input.mousePosition;

        /*float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);*/

        transform.position = position;
    }
}
