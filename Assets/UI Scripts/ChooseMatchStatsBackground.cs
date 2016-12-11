using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseMatchStatsBackground : MonoBehaviour
{
    public Sprite VictoryImage;
    public Sprite DefeatImage;

    public Text TimeText;
    public Text UnitsLostText;
    public Text EnemiesSlainText;

    void Start()
    {
        //Find the image component.
        Image image = this.GetComponent<Image>();

        if (image != null)
        {
            if (PersistentData.WasVictor)
            {
                image.sprite = VictoryImage;
                PersistentData.GenerateReward();
            }
            else
                image.sprite = DefeatImage;
        }

        //Draw in stats
        string time = ToRomanWithZero((int)PersistentData.Time) + " sec.";
        string unitsLost = ToRomanWithZero(PersistentData.UnitsLost);
        string enemiesSlain = ToRomanWithZero(PersistentData.EnemiesKilled);

        TimeText.text = time;
        UnitsLostText.text = unitsLost;
        EnemiesSlainText.text = enemiesSlain;
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

    public void ClickContinue()
    {
        SceneManager.LoadScene("Store");
    }

    public void AudioClick()
    {
        AudioManager.manger.PlayClick();
    }
}
