using System;
using System.Collections.Generic;

[System.Serializable]
public class CartItem
{
    public string sceneName;
    public string imagePath; 
    public long timestamp; // for the birthday of the player

    public CartItem(string sceneName, string imagePath, long timestamp)
    {
        this.sceneName = sceneName;
        this.imagePath = imagePath;
        this.timestamp = timestamp;
    }
}

[System.Serializable]
public class PlayerData
{
    public string id;
    public string email;
    public string password;
    public string playername;
    public long birthday; // Unix timestamp in milliseconds
    public List<CartItem> cart;

    public PlayerData (string id, string email, string password, string playername, long birthday, List<CartItem> cart)
    {
        this.id = id;
        this.email = email;
        this.password = password;
        this.playername = playername;
        this.birthday = birthday;
        this.cart = cart;
    }
}