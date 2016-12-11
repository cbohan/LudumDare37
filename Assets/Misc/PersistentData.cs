using UnityEngine;

public class PersistentData
{
    //Player starts out on level one.
    public static int CurrentLevel = 1;

    //Player start with 1 of each unit
    public static UnitModel[] PlayersUnits = new UnitModel[6];

    //Match stats
    public static int UnitsLost = 2;
    public static int EnemiesKilled = 6;
    public static float Time = 111.3f;
    public static bool WasVictor = true;

    //Moneys!
    public static int Gold = 0;

    //This will be called when the player starts their first game.
    public static void GivePlayerInitialUnits()
    {
        PlayersUnits[0] = UnitModel.GenerateLongswordsman();
        PlayersUnits[1] = UnitModel.GenerateShortswordsman();
        PlayersUnits[2] = UnitModel.GenerateArcher();
        PlayersUnits[3] = null;
        PlayersUnits[4] = null;
        PlayersUnits[5] = null;
    }

    //Generate enemies for a given level
    public static UnitModel[] GenerateEnemies()
    {
        UnitModel[] units = new UnitModel[6];

        if (CurrentLevel == 1)
        {
            units[0] = UnitModel.GenerateShortswordsman();
            units[0].IsAggressive = false;
            units[1] = UnitModel.GenerateShortswordsman();
            units[1].IsAggressive = false;
            units[2] = null;
            units[3] = null;
            units[4] = null;
            units[5] = null;
        }
        else if (CurrentLevel == 2)
        {
            units[0] = UnitModel.GenerateShortswordsman();
            units[1] = UnitModel.GenerateShortswordsman();
            units[2] = UnitModel.GenerateArcher();
            units[3] = null;
            units[4] = null;
            units[5] = null;
        }
        else if (CurrentLevel == 3)
        {
            units[0] = UnitModel.GenerateShortswordsman();
            units[1] = UnitModel.GenerateShortswordsman();
            units[2] = UnitModel.GenerateArcher();
            units[3] = UnitModel.GenerateLongswordsman();
            units[4] = null;
            units[5] = null;
            foreach (UnitModel unit in units)
                if (unit != null)
                    unit.AddExperience(4);
        }
        else if (CurrentLevel == 4)
        {
            units[0] = UnitModel.GenerateShortswordsman();
            units[1] = UnitModel.GenerateShortswordsman();
            units[2] = UnitModel.GenerateArcher();
            units[3] = UnitModel.GenerateLongswordsman();
            units[4] = UnitModel.GenerateArcher();
            units[5] = null;
            foreach (UnitModel unit in units)
                if (unit != null)
                    unit.AddExperience(7);
        }
        else if (CurrentLevel == 5)
        {
            units[0] = UnitModel.GenerateShortswordsman();
            units[1] = UnitModel.GenerateArcher();
            units[2] = UnitModel.GenerateArcher();
            units[3] = UnitModel.GenerateLongswordsman();
            units[4] = UnitModel.GenerateArcher();
            units[5] = UnitModel.GenerateLongswordsman();
            foreach (UnitModel unit in units)
                if (unit != null)
                    unit.AddExperience(10);
        }
        else
        {
            units[0] = UnitModel.GenerateShortswordsman();
            units[1] = UnitModel.GenerateLongswordsman();
            units[2] = UnitModel.GenerateArcher();
            for (int i = 3; i < 6; i++)
            {
                int randomNum = Random.Range(0, 3);

                if (randomNum == 0)
                    units[i] = UnitModel.GenerateLongswordsman();
                else if (randomNum == 1)
                    units[i] = UnitModel.GenerateShortswordsman();
                else
                    units[i] = UnitModel.GenerateArcher();
            }  
            foreach (UnitModel unit in units)
                if (unit != null)
                    unit.AddExperience((CurrentLevel * 7) - 25);
        }


        return units;
    }

    //Generate reward for a given level.
    public static void GenerateReward()
    {
        if (CurrentLevel == 1)
        {
            GiveExperience(1);
            Gold += 4;
        }
        else if (CurrentLevel == 2)
        {
            GiveExperience(1);
            Gold += 5;
        }
        else if (CurrentLevel == 3)
        {
            GiveExperience(2);
            Gold += 6;
        }
        else if (CurrentLevel == 4)
        {
            GiveExperience(2);
            Gold += 6;
        }
        else if (CurrentLevel == 5)
        {
            GiveExperience(3);
            Gold += 6;
        }
        else if (CurrentLevel == 6)
        {
            GiveExperience(3);
            Gold += 6;
        }
        else
        {
            GiveExperience(4);
            Gold += 6;
        }

        //Since we just beat a level increment the level number.
        CurrentLevel++;
    }

    //Give experince to each unit
    private static void GiveExperience(int xp)
    {
        for (int i = 0; i < 6; i++)
        {
            if (PlayersUnits[i] != null)
            {
                PlayersUnits[i].AddExperience(xp);
            }
        }
    } 

    public static void IncrementLevel()
    {
        CurrentLevel++;
    }
}
