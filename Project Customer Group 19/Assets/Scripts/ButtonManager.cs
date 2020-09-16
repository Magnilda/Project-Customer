using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttons = new List<Button>();
    private Button selectedButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DisableButtons()
    {
        foreach (Button button in buttons)
        {
            if (selectedButton == null)
                return;

            if (button != selectedButton)
                button.gameObject.SetActive(false);
        }
    }

    private void EnableButtons()
    {
        foreach (Button button in buttons)
        {
            if (selectedButton == null)
                return;

            if (button != selectedButton)
                button.gameObject.SetActive(false);
        }
    }

    public Button SelectedButton { get => selectedButton; set => selectedButton = value; }
}
