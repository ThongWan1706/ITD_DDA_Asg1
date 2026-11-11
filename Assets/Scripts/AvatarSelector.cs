using UnityEngine;
using UnityEngine.UI;

public class AvatarSelector : MonoBehaviour
{
    // Store the selected avatar name or index globally so the profile manager can read it
    public static string selectedAvatar = "Avatar_Default"; 

   
    public void SelectAvatar(string avatarName)
    {
        selectedAvatar = avatarName;
        Debug.Log("Selected Avatar: " + selectedAvatar);
        
    }
}