using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreScript : MonoBehaviour
{
    public Text GoldText;
    public GameObject[] Boxes;

	void Start ()
    {
        UpdateGoldText();
	}

    public void UpdateGoldText()
    {
        GoldText.text = "Gold - " + ToRomanWithZero(PersistentData.Gold);
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }	
	}

    public void ClickContinue()
    {
        SceneManager.LoadScene("Arena");
    }

    //Buy button methods.
    public void BuyLongswordsman()
    {
        if (GetFirstOpenSlot() != -1 && PersistentData.Gold >= 1)
        {
            PersistentData.Gold -= 1;
            UpdateGoldText();

            PersistentData.PlayersUnits[GetFirstOpenSlot()] = UnitModel.GenerateLongswordsman();

            UpdateBoxes();
        }
    }

    public void BuyShortswordsman()
    {
        if (GetFirstOpenSlot() != -1 && PersistentData.Gold >= 1)
        {
            PersistentData.Gold -= 1;
            UpdateGoldText();

            PersistentData.PlayersUnits[GetFirstOpenSlot()] = UnitModel.GenerateShortswordsman();

            UpdateBoxes();
        }
    }

    public void BuyArcher()
    {
        if (GetFirstOpenSlot() != -1 && PersistentData.Gold >= 1)
        {
            PersistentData.Gold -= 1;
            UpdateGoldText();

            PersistentData.PlayersUnits[GetFirstOpenSlot()] = UnitModel.GenerateArcher();

            UpdateBoxes();
        }
    }

    public void TrainUnits()
    {
        if (PersistentData.Gold >= 2)
        {
            PersistentData.Gold -= 2;
            UpdateGoldText();

            for (int i = 0; i < 6; i++)
            {
                if (PersistentData.PlayersUnits[i] != null)
                {
                    PersistentData.PlayersUnits[i].AddExperience(1);
                }
            }
            
            UpdateBoxes();
        }
    }

    private void UpdateBoxes()
    {
        foreach (GameObject box in Boxes)
        {
            StoreBoxContents contents = box.GetComponent<StoreBoxContents>();
            if (contents != null)
            {
                contents.FillBox();
            }
        }
    }

    //Get the first open slot.
    private int GetFirstOpenSlot()
    {
        //Return the number of the slot if there is one/
        for (int i = 0; i < 6; i++)
        {
            if (PersistentData.PlayersUnits[i] == null)
            {
                return i;
            }
        }

        //Otherwise, return -1.
        return -1;
    }

    //Copied this from:
    //http://stackoverflow.com/questions/7040289/converting-integers-to-roman-numerals
    private string ToRomanWithZero(int number)
    {
        if (number == 0) return "O";
        return ToRoman(number);
    }
    private string ToRoman(int number)
    {
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return "MMMMM";
    }

    public void AudioClick()
    {
        AudioManager.manger.PlayClick();
    }
}
