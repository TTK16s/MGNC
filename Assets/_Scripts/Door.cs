using UnityEngine;
using System.Collections;

public class Door : Item {

    PlayerController player;

    public bool hasColorCode = false;
    public enum CodeColor { GENERIC, BLUE, YELLOW, RED }
    public CodeColor colorCode;

    void Start()
    {
        player = Singletons.playerInstance;
    }

    public void Open()
    {
        if (CheckPlayerForColorCodedKey())
        {
            gameObject.SetActive(false);
            player.ProcessItem(this);
        }
    }

    bool CheckPlayerForColorCodedKey()
    {
        bool canihasthekey = false;

        if (colorCode == CodeColor.BLUE && player.blueKey == true)
            canihasthekey = true;
        else if (colorCode == CodeColor.YELLOW && player.yellowKey == true)
            canihasthekey = true;
        else if (colorCode == CodeColor.RED && player.redKey == true)
            canihasthekey = true;
        else if (colorCode == CodeColor.GENERIC)
            canihasthekey = true;

            return canihasthekey;
    } 

    public override void Reset()
    {
        gameObject.SetActive(true);
    }
}
