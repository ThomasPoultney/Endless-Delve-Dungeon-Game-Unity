using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple script that updates the text of the attached text element to a given value.
/// </summary>
public class UpdateText : MonoBehaviour
{

    ///The Text UI element to be updated
    public Text textToUpdate;

    /// <summary>
    /// Updates the text element attached to the script to the passed value
    /// </summary>
    /// <param name="text"></param>
    /// The new text to be displayed in the UI element.
    public void updateText(string text)
    {
        textToUpdate.text = text;
    }
}
