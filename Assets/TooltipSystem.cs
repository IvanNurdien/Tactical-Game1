using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public TooltipManager tooltip;

    private void Awake()
    {
        current = this;
    }
    

    public static void Show(string headerContent, string messageContent)
    {
        current.tooltip.SetText(headerContent, messageContent);
        current.tooltip.gameObject.GetComponent<Animator>().SetBool("isShowing", true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.GetComponent<Animator>().SetBool("isShowing", false);

    }
}
