using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    private Text self;

    private void Start()
    {
        self = GetComponent<Text>();
    }
    private void Update()
    {
        Color temp = self.color;
        if (temp.a > 0)
        {
            temp.a -= 1f;
            self.color = temp;
        }
    }

    public void ChangeTextAndColor(string text)
    {
        self.text = text;
        Color temp = self.color;
        temp.a = 255.0f;
        self.color = temp;
    }
}
