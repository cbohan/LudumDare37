using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnUnits : MonoBehaviour
{
    private float gameStartTime;

    public GameObject playerLongswordsman;
    public GameObject playerShortswordsman;
    public GameObject playerArcher;
    public GameObject[] playerSpawnLocations;

    public GameObject enemyLongswordsman;
    public GameObject enemyShortswordsman;
    public GameObject enemyArcher;
    public GameObject[] enemySpawnLocations;

    void Start()
    {
        //Reset match stats.
        PersistentData.UnitsLost = 0;
        PersistentData.EnemiesKilled = 0;
        gameStartTime = Time.fixedTime;

        UnitModel[] playerUnits = PersistentData.PlayersUnits;
        UnitModel[] enemyUnits = PersistentData.GenerateEnemies();

        //Clean up unit lists.
        Base_Unit.unitList.Clear();
        Base_PlayerUnit.PlayerUnitList.Clear();
        Base_EnemyUnit.EnemyUnitList.Clear();

        //Spawn player's units.
        for (int i = 0; i < 6; i++)
        {
            if (playerUnits[i] != null)
            {
                GameObject unit;
                if (playerUnits[i].Type == UnitModel.UnitType.Longswordsman)
                    unit = Instantiate(playerLongswordsman);
                else if (playerUnits[i].Type == UnitModel.UnitType.Shortswordsman)
                    unit = Instantiate(playerShortswordsman);
                else
                    unit = Instantiate(playerArcher);

                Debug.Log("SPAWNING: " + playerUnits[i].Type);

                unit.transform.position = playerSpawnLocations[i].transform.position;

                //Pass the units stats to the game object.
                Base_PlayerUnit playerUnit = unit.GetComponent<Base_PlayerUnit>();
                if (playerUnit != null)
                {
                    playerUnit.Health = playerUnits[i].Health;
                    playerUnit.MaxHealth = playerUnits[i].MaxHealth;
                    playerUnit.Attack = playerUnits[i].Attack;
                    playerUnit.Block = playerUnits[i].Block;
                    playerUnit.Range = playerUnits[i].Range;
                    playerUnit.AttackCooldown = playerUnits[i].AttackCooldown;
                    playerUnit.Experience = playerUnits[i].Experience;
                    //player units can't be aggressive
                }
            }
        }

        Debug.Log("------");

        //Spawn enemy units.
        for (int i = 0; i < 6; i++)
        {
            if (enemyUnits[i] != null)
            {
                GameObject unit;
                if (enemyUnits[i].Type == UnitModel.UnitType.Longswordsman)
                    unit = Instantiate(enemyLongswordsman);
                else if (enemyUnits[i].Type == UnitModel.UnitType.Shortswordsman)
                    unit = Instantiate(enemyShortswordsman);
                else
                    unit = Instantiate(enemyArcher);

                unit.transform.position = enemySpawnLocations[i].transform.position;
                unit.transform.rotation = enemySpawnLocations[i].transform.rotation;

                //Pass the units stats to the game object.
                Base_EnemyUnit enemyUnit = unit.GetComponent<Base_EnemyUnit>();
                if (enemyUnit != null)
                {
                    enemyUnit.Health = enemyUnits[i].Health;
                    enemyUnit.MaxHealth = enemyUnits[i].MaxHealth;
                    enemyUnit.Attack = enemyUnits[i].Attack;
                    enemyUnit.Block = enemyUnits[i].Block;
                    enemyUnit.Range = enemyUnits[i].Range;
                    enemyUnit.AttackCooldown = enemyUnits[i].AttackCooldown;
                    enemyUnit.Experience = enemyUnits[i].Experience;
                    enemyUnit.isAggressive = enemyUnits[i].IsAggressive;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        //Let the match run a bit. For not-buggyness.
        if (Time.fixedTime - this.gameStartTime > 5)
        {
            //Check if the player has either won or lost and end there.
            if (Base_PlayerUnit.PlayerUnitList.Count <= 0)
                EndMatch(false);
            else if (Base_EnemyUnit.EnemyUnitList.Count <= 0)
                EndMatch(true);
        }
    }

    private void EndMatch(bool wasVictor)
    {
        //Record the match length.
        PersistentData.Time = Time.fixedTime - this.gameStartTime;
        PersistentData.WasVictor = wasVictor;

        //Update the player's unit data.
        int i = 0;
        foreach (Base_PlayerUnit unit in Base_PlayerUnit.PlayerUnitList)
        {
            PersistentData.PlayersUnits[i] = new UnitModel();

            if (unit is Player_Longswordsman)
                PersistentData.PlayersUnits[i].Type = UnitModel.UnitType.Longswordsman;
            else if (unit is Player_Shortswordsman)
                PersistentData.PlayersUnits[i].Type = UnitModel.UnitType.Shortswordsman;
            else
                PersistentData.PlayersUnits[i].Type = UnitModel.UnitType.Archer;

            Debug.Log("SAVING: " + PersistentData.PlayersUnits[i].Type);

            PersistentData.PlayersUnits[i].Health = unit.MaxHealth;
            PersistentData.PlayersUnits[i].MaxHealth = unit.MaxHealth;
            PersistentData.PlayersUnits[i].Attack = unit.Attack;
            PersistentData.PlayersUnits[i].Block = unit.Block;
            PersistentData.PlayersUnits[i].Range = unit.Range;
            PersistentData.PlayersUnits[i].AttackCooldown = unit.AttackCooldown;
            PersistentData.PlayersUnits[i].Experience = unit.Experience;
            PersistentData.PlayersUnits[i].IsAggressive = true;

            i++;
        }

        for (int n = i; n < 6; n++)
        {
            Debug.Log("SAVING: NULL");
            PersistentData.PlayersUnits[n] = null;
        }

        Debug.Log("--------");

        //Go to the match stats screen.
        SceneManager.LoadScene("MatchStats");
    }
}