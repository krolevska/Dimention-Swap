using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum InstructionType
{
    Run,
    Jump,
    Climb,
    Fire,
    CollectAll,
    Dimension
}

public class InstructionTrigger : MonoBehaviour
{
    public InstructionType instructionType;
    public GameObject message;

    public TextMeshProUGUI instructionText;
    private string howToRun = "Press 'D' to move forward and 'A' to move back.";
    private string howToJump = "Press 'Space' to jump.";
    private string howToClimb = "Press 'S' to climb down and 'W' to climb up.";
    private string howToFire = "Press 'F' to fire toward an enemy.";
    private string collectThemAll = "You need to collect 50 pieces of data so you could proceed to the finish.";
    private string switchDimension = "You can hide in the Shadow Dimension to avoid Police. Though, you wouldn't be able to gather the data from Shadow. Press 'R' to switch dimensions";


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
                case InstructionType.Dimension:
                return switchDimension;
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
