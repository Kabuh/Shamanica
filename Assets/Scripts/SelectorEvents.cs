using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorEvents : MonoBehaviour
{
    private void OnMouseExit()
    {
        this.gameObject.SetActive(false);
    }
}
