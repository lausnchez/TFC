using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCounterController : MonoBehaviour
{
    [SerializeField] private HandView handView;
    [SerializeField] private TMPro.TextMeshProUGUI handCounterText;

    private void Update()
    {
        if (handView!=null && handCounterText!=null)
        {
            handCounterText.text = $"{handView.currentCards} / {handView.cardMax}";
        }
    }
}
