using UnityEngine;
using UnityEngine.UI;

public class MusicDropdown : MonoBehaviour
{
    private Dropdown dropdown;
    // public Text m_Text;
    public GameObject speaker;

    void Start()
    {
        //Fetch the Dropdown GameObject
        dropdown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });

        //Initialise the Text to say the first value of the Dropdown
        // m_Text.text = "First Value : " + m_Dropdown.value;
    }

    //Ouput the new value of the Dropdown into Text
    private void DropdownValueChanged(Dropdown change)
    {
        speaker.GetComponent<CameraController>().changeMusic(change.value);
        // m_Text.text =  "New Value : " + change.value;
    }
}