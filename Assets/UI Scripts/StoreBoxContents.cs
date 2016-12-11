using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreBoxContents : MonoBehaviour
{
    //Which unit is this box for?
    public int BoxNumber;

    //Stats text.
    public Text UnitType;
    public Text Health;
    public Text Attack;
    public Text Block;
    public Text XP;

    //Symbols
    public Image HealthImage;
    public Image SwordImage;
    public Image ShieldImage;
    public Text XPText;

	void Start ()
    {
        //Unrecoverable loss.
        if (PersistentData.Gold == 0)
        {
            SceneManager.LoadScene("MainMenu");
        }

        FillBox();
	}

    public void FillBox()
    {
        if (PersistentData.PlayersUnits[BoxNumber] != null)
        {
            //Hide the unit's stats.
            this.HealthImage.enabled = true;
            this.SwordImage.enabled = true;
            this.ShieldImage.enabled = true;
            this.XPText.enabled = true;
            this.UnitType.enabled = true;
            this.Health.enabled = true;
            this.Attack.enabled = true;
            this.Block.enabled = true;
            this.XP.enabled = true;

            //Fill in all the unit's stats.
            this.UnitType.text = PersistentData.PlayersUnits[BoxNumber].Type.ToString();
            this.Health.text = PersistentData.PlayersUnits[BoxNumber].MaxHealth.ToString("0.0");
            this.Attack.text = PersistentData.PlayersUnits[BoxNumber].Attack.ToString("0.0");
            this.Block.text = PersistentData.PlayersUnits[BoxNumber].Block.ToString("0.0");
            this.XP.text = PersistentData.PlayersUnits[BoxNumber].Experience.ToString();
        }
        else
        {
            //Hide the unit's stats.
            this.HealthImage.enabled = false;
            this.SwordImage.enabled = false;
            this.ShieldImage.enabled = false;
            this.XPText.enabled = false;
            this.UnitType.enabled = false;
            this.Health.enabled = false;
            this.Attack.enabled = false;
            this.Block.enabled = false;
            this.XP.enabled = false;
        }
    }
	
	void Update ()
    {
		
	}
}
