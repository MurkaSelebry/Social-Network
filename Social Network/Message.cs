// User.cs
using System;
using System.Collections.Generic;

public class User
{
    public string Username { get; }
    public string Password { get; }
    public string Name { get; set; }
    public List<User> Friends { get; } // Список друзей
    public List<Message> Messages { get; } // Список сообщений

    public User(string username, string password, string name)
    {
        Username = username;
        Password = password;
        Name = name;
        Friends = new List<User>();
        Messages = new List<Message>();
    }

    public void AddFriend(User friend)
    {
        if (!Friends.Contains(friend))
        {
            Friends.Add(friend);
        }
    }

    public void SendMessage(User friend, string text)
    {
        if (Friends.Contains(friend))
        {
            Message message = new Message(text, DateTime.Now, this);
            friend.Messages.Add(message);
            Messages.Add(message);
        }
    }
}

// Message.cs

public class Message
{
    public string Text { get; }
    public DateTime Date { get; }
    public User Sender { get; }

    private System.Windows.HorizontalAlignment alignment;

    public Message(string text, DateTime date, User sender)
    {
        Text = text;
        Date = date;
        Sender = sender;
// alignment = sender == currentUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    }

    public System.Windows.HorizontalAlignment Alignment { get; set; }

}
