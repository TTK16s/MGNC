using UnityEngine;
using System.Collections;

public class Key : Item {

    PlayerController player;

    public enum CodeColor { GENERIC, BLUE, YELLOW, RED }
    public CodeColor colorCode;

    void Start()
    {
        typeString = "Key";
        itemName = gameObject.name;
        player = Singletons.playerInstance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            if (colorCode != CodeColor.GENERIC)
            {
                GivePlayerColorCodedKey();
            }
            player.ProcessItem(this);
            gameObject.SetActive(false);
        }
    }

    void GivePlayerColorCodedKey()
    {
        if (colorCode == CodeColor.BLUE)
            player.blueKey = true;
        else if (colorCode == CodeColor.YELLOW)
            player.yellowKey = true;
        else if (colorCode == CodeColor.RED)
            player.redKey = true;
    }

    public override void Reset()
    {
        gameObject.SetActive(true);
        itemUsed = false;
    }
}
