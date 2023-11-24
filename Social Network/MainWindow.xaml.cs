using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
// This is a personal academic project. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com

namespace Social_Network
{
   public class Test
   {
       public Visibility visibilityLogin = Visibility.Visible;
       public Visibility visibilityContacts = Visibility.Visible;
       public Visibility visibilityAddFriend = Visibility.Visible;
       public Visibility visibilityChat = Visibility.Visible;
       public User user2 = new User("user2", "password2", "User 2");
       public User user3 = new User("user3", "password3", "User 3");
        public bool isAuth = true;

   }
    public partial class MainWindow : Window
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();
        private User currentUser;
        private User selectedFriend;


        public MainWindow()
        {
            InitializeComponent();

            // Создаем тестовых пользователей
            Bot chatBot = new Bot("chatbot", "", "Chat Bot");
            User user1 = new User("user1", "", "User 1");
            Bot chatBotBing = new Bot("user2", "password2", "Chat Bot Bing");
            User user3 = new User("user3", "password3", "User 3");
            User user4 = new User("user4", "password3", "User 4");

            // Добавляем друзей
            chatBot.AddFriend(user1);
            user1.AddFriend(chatBot);
            user1.AddFriend(chatBotBing);
            user1.AddFriend(user3);
            chatBotBing.AddFriend(user1);
            chatBotBing.AddFriend(user3);
            user3.AddFriend(user1);
            user3.AddFriend(chatBotBing);

            chatBot.SendMessage(user1, "Привет, я чат бот!");
            
            // Создаем некоторые тестовые сообщения
            user1.SendMessage(chatBotBing, "Привет, друг 2!");
            chatBotBing.SendMessage(user1, "Привет, я чат бот бинг!");
            user3.SendMessage(user1, "Как дела, друг 1?");
            user1.SendMessage(user3, "Все хорошо, спасибо!");

            // Добавляем пользователей в словарь для доступа к ним
            users.Add(user1.Username, user1);
            users.Add(chatBotBing.Username, chatBotBing);
            users.Add(user3.Username, user3);
            users.Add(user4.Username, user4);

            currentUser = user1; // Устанавливаем текущего пользователя для тестирования
            UpdateContactsList();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginBox.Text;
            string password = PasswordBox.Password;

            if (users.ContainsKey(username) && users[username].Password == password)
            {
                currentUser = users[username];
                LoginScreen.Visibility = Visibility.Collapsed;
                ContactsScreen.Visibility = Visibility.Visible;
                UpdateContactsList();
            }
            else
            {
                LoginMessageBlock.Text = "Invalid username or password";
                LoginMessageBlock.Visibility = Visibility.Visible;
            }
        }

        private void UpdateContactsList()
        {
            ContactsList.Items.Refresh(); // Очистить элементы управления перед обновлением
            ContactsList.ItemsSource = currentUser.Friends;
        }

        private void ContactsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedFriend = (User)ContactsList.SelectedItem;
            if (selectedFriend != null)
            {
                ChatScreen.Visibility = Visibility.Visible;
                ChatName.Text = selectedFriend.Name;
                UpdateMessagesList();
            }
        }

        private void SearchFriendButton_Click(object sender, RoutedEventArgs e)
        {
            string searchUsername = SearchUsernameBox.Text;
            if (users.ContainsKey(searchUsername))
            {
                User friend = users[searchUsername];
                currentUser.AddFriend(friend);
                UpdateContactsList();
                SearchUsernameBox.Text = string.Empty;
                AddFriendScreen.Visibility = Visibility.Collapsed;
                ContactsScreen.Visibility = Visibility.Visible;
            }
            else
            {
                SearchErrorMessage.Text = "Пользователь с таким Username не найден.";
                SearchErrorMessage.Visibility = Visibility.Visible;
            }
        }


        private void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            ContactsScreen.Visibility = Visibility.Collapsed;
            AddFriendScreen.Visibility = Visibility.Visible;
            SearchUsernameBox.Text = string.Empty;
            SearchErrorMessage.Visibility = Visibility.Collapsed;
        }



        private void UpdateMessagesList()
        {
            MessagesList.Items.Refresh(); // Очистить элементы управления перед обновлением
            foreach (Message message in selectedFriend.Messages)
            {
                message.Alignment = message.Sender == currentUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            }
            MessagesList.ItemsSource = selectedFriend.Messages;
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string messageText = MessageBox.Text;
            if (!string.IsNullOrWhiteSpace(messageText) && selectedFriend != null)
            {
                currentUser.SendMessage(selectedFriend, messageText);

                // Очистить поле ввода сообщения
                MessageBox.Text = string.Empty;

                UpdateMessagesList();
            }
            SendButton.IsEnabled = false;
            if (selectedFriend is Bot)
            {
                var bot = (Bot)selectedFriend;
                var task = bot.AnswerMessage(messageText);
                await task.ContinueWith(t =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        SendButton.IsEnabled = true;
                    });
                    bot.SendMessage(currentUser, t.Result);
                    Dispatcher.Invoke(() =>
                    {
                        UpdateMessagesList();
                    });
                });
            }

        }

        private void BackLogin_Click(object sender, RoutedEventArgs e)
        {
            ContactsScreen.Visibility = Visibility.Collapsed;
            LoginScreen.Visibility = Visibility.Visible;
        }

        private void BackToContactsButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрыть экран добавления в друзья и показать контакты
            AddFriendScreen.Visibility = Visibility.Collapsed;
            ContactsScreen.Visibility = Visibility.Visible;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ChatScreen.Visibility = Visibility.Collapsed;
            selectedFriend = null;
        }
    }
}
