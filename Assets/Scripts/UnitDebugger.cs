using UnityEngine;
using UnityEngine.UI;


public class UnitDebugger : MonoBehaviour
{
    public Puppet unit;
    public GameObject panel;

    public Text unitLabel;
    public Text teamLabel;
    public Text typeLabel;
    public Text energyLabel;
    public Text actionLabel;
    public Text moveCntLabel;

    bool popup = false;

	public void Awake()
	{
        gameObject.SetActive(ConstFile.DEBUG);
	}

    public void Update()
    {
        if (ConstFile.DEBUG)
        {
            unitLabel.text = "Unit: " + unit.id;
            teamLabel.text = "Team: " + unit.team;
            typeLabel.text = "Type: " + unit.currType;
            energyLabel.text = string.Format("Energy: {0}/{1}", unit.energy, unit.maxEnergy);
            actionLabel.text = "Action: " + unit.currAction;
            moveCntLabel.text = "Move Cnt: " + unit.moveCnt;
        }
    }

    public void ButtonToggle()
    {
        popup = !popup;
        panel.SetActive(popup);
    }
}
