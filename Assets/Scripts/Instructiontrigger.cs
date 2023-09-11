using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum InstructionType
{
    Run,
    Jump,
    Climb,
    Fire,
    CollectAll
}

public class InstructionTrigger : MonoBehaviour
{
    public InstructionType instructionType;
    public GameObject message;

    public TextMeshProUGUI instructionText;
    public string howToRun = "Press 'D' to move forward and 'A' to move back.";
    public string howToJump = "Press 'Space' to jump.";
    public string howToClimb = "Press 'S' to climb down and 'W' to climb up.";
    public string howToFire = "Press 'F' to fire toward an enemy.";
    public string collectThemAll = "You need to collect 50 pieces of data so you could proceed to the finish.";

    private string GetMessage()
    {
        switch (instructionType)
        {
            case InstructionType.Run:
                return howToRun;
            case InstructionType.Jump:
                return howToJump;
            case InstructionType.Climb:
                return howToClimb;
            case InstructionType.Fire:
                return howToFire;
            case InstructionType.CollectAll:
                return collectThemAll;
            default:
                return "";
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player.
        if (other.CompareTag("Player"))
        {
            message.SetActive(true);

            ShowInstruction(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object exiting the trigger is the player.
        if (other.CompareTag("Player"))
        {
            message.SetActive(false);

            ShowInstruction(false);
        }
    }

    private void ShowInstruction(bool show)
    {
        if (instructionText)
        {
            instructionText.text = show ? GetMessage() : string.Empty;
        }
    }
}
