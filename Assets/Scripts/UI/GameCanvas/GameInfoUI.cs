using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoUI : MonoBehaviour
{
    [SerializeField] Text moveCountText;
    [SerializeField] Text clearConditionCountText;

    public void UpdateClearConditionCount(int count)
    {
        clearConditionCountText.text = count.ToString();
    }

    public void UpdateMoveCount(int count)
    {
        moveCountText.text = count.ToString();
    }
}