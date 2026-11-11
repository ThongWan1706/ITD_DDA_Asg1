using System;
using System.Collections.Generic;

[System.Serializable]
public class CartItem
{
    public string sceneName;
    public string imagePath; 
    public long timestamp; 

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
    public long birthday;
    public string avatarId;
    public string promoCode; // Added promo code key for later use
    public List<CartItem> cart;

    public PlayerData(string id, string email, string password, string playername, long birthday, string avatarId, string promoCode, List<CartItem> cart)
    {
        this.id = id;
        this.email = email;
        this.password = password;
        this.playername = playername;
        this.birthday = birthday;
        this.avatarId = avatarId;
        this.promoCode = promoCode;
        this.cart = cart;
    }
}