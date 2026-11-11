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
    public GameObject errorPopupPanel; 
    public TMP_Text errormessage; 

    public void CloseErrorPopup()
    {
        if (errorPopupPanel != null)
            errorPopupPanel.SetActive(false);
    }

    //For the Previous Button
    public void undoScene()
    {
        Debug.Log("Previous Button Clicked!");
        SceneManager.LoadScene("SignUp");
    }

    //For the player to submit their player game profile
    public void SubmitProfile()
    {
        if (string.IsNullOrEmpty(playerNameInput.text) || string.IsNullOrEmpty(birthdayInput.text))
        {
            errormessage.text = "Please fill in all fields.";
            if (errorPopupPanel != null) errorPopupPanel.SetActive(true);
            return;
        }

        //Parse the birthday text (Expected format: YYYY-MM-DD)
        string dateFormat = "dd-MM-yyyy";
        
        if (!DateTime.TryParseExact(birthdayInput.text.Trim(), dateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime parsedBirthday))
        {
            errormessage.text = $"Invalid date format! Please use {dateFormat}";
            if (errorPopupPanel != null) errorPopupPanel.SetActive(true);
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

        // Construct the full Player JSON object 
        PlayerData newPlayer = new PlayerData(
            UserDataHolder.userId,
            UserDataHolder.email,
            UserDataHolder.password,
            playerNameInput.text,
            birthdayTimestamp,
            AvatarSelector.selectedAvatar,
            "",
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
                    SceneManager.LoadScene("SampleScene"); 
                }
            });
    }
}