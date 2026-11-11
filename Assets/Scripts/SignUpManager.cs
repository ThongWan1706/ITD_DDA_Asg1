using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class SignUpManager : MonoBehaviour
{
    public TMP_InputField email; 
    public TMP_InputField password; 
    public TMP_Text errormessage; 
    public GameObject errorPopupPanel; 

    public void CloseErrorPopup()
    {
        if (errorPopupPanel != null)
            errorPopupPanel.SetActive(false);
    }

    //For the Previous Button
    public void undoScene()
    {
        SceneManager.LoadScene("LoginPage");
    }

    private bool CheckInputs()
    {
        errormessage.text = "";
        if (email.text.Length == 0)
        {
            errormessage.text = "Please enter a proper email address.";
            if (errorPopupPanel != null) errorPopupPanel.SetActive(true);
            return false;
        }
        else if (password.text.Length == 0)
        {
            errormessage.text = "Please enter a proper password.";
            if (errorPopupPanel != null) errorPopupPanel.SetActive(true);
            return false;
        }
        else if (password.text.Length < 8)
        {
            errormessage.text = "Please enter at least 8 characters password";
            if (errorPopupPanel != null) errorPopupPanel.SetActive(true);
            return false;
        }
        return true;
    }

    //For user that haven't create their account beforehand
    public void Signup()
    {
        if (CheckInputs() == false) return;

        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
                var firebaseException = task.Exception.GetBaseException() as FirebaseException;
                var errorCode = (AuthError)firebaseException.ErrorCode;

                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        errormessage.text = "Please enter an email address";
                        break;
                    case AuthError.MissingPassword:
                        errormessage.text = "Please enter a password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        errormessage.text = "Email is already registered";
                        break;
                    case AuthError.InvalidEmail:
                        errormessage.text = "Please enter a proper email";
                        break;
                    default:
                        errormessage.text = "Issue in Authentication";
                        break;
                }
                if (errorPopupPanel != null) errorPopupPanel.SetActive(true);
            }
            else
            {
                var AuthResult = task.Result;
                Debug.Log($"User created, new user ID is: {AuthResult.User.UserId}");

                //Temporarily save data to pass to the next scene
                UserDataHolder.userId = AuthResult.User.UserId;
                UserDataHolder.email = email.text;
                UserDataHolder.password = password.text;

                //Move to the next scene where they enter profile details
                SceneManager.LoadScene("CreateUserProfileScene");
            }
        });
    }
}