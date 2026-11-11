using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;

public class ProfileSetupManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_InputField birthdayInput; 
    public TMP_Text errormessage;

    public void SubmitProfile()
    {
        if (string.IsNullOrEmpty(playerNameInput.text) || string.IsNullOrEmpty(birthdayInput.text))
        {
            errormessage.text = "Please fill in all fields.";
            return;
        }

        // Convert birthday string to a long timestamp
        long birthdayTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); 
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Initialize an empty or default starting cart for the AR game screenshots
        List<CartItem> initialCart = new List<CartItem>
        {
            new CartItem("AR_Initial_Scene", "Screenshots/default.png", currentTimestamp)
        };

        // Construct the full Player JSON object matching your schema
        PlayerData newPlayer = new PlayerData(
            UserDataHolder.userId,
            UserDataHolder.email,
            UserDataHolder.password,
            playerNameInput.text,
            birthdayTimestamp,
            initialCart
        );

        string json = JsonUtility.ToJson(newPlayer);

        // Save to Firebase Database under the user's ID
        FirebaseDatabase.DefaultInstance.GetReference("players")
            .Child(UserDataHolder.userId)
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to save player profile: " + task.Exception);
                }
                else
                {
                    Debug.Log("Player profile saved successfully!");
                    SceneManager.LoadScene("GameLevelScene"); // Move into the actual game
                }
            });
    }
}
