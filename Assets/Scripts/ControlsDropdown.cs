using UnityEngine;
using UnityEngine.UI;

public class ControlsDropdown : MonoBehaviour
{
    private Dropdown dropdown;
    // public Text m_Text;
    public GameObject playerObj;
    public GameObject gridObj;

    private PlayerController player;
    private GridContainer grid;


    void Start()
    {
        //Fetch the Dropdown GameObject
        player = playerObj.GetComponent<PlayerController>();
        grid = gridObj.GetComponent<GridContainer>();
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
        player.setMoveType(change.value);
        grid.setMoveType(change.value);
        // m_Text.text =  "New Value : " + change.value;speaker
    }
}