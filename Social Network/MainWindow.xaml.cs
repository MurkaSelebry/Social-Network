using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Social_Network
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();
        private User currentUser;
        private User selectedFriend;

        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();

            // Создаем тестовых пользователей
            User user1 = new User("user1", "", "User 1");
            User user2 = new User("user2", "password2", "User 2");
            User user3 = new User("user3", "password3", "User 3");
            User user4 = new User("user4", "password3", "User 4");

            // Добавляем друзей
            user1.AddFriend(user2);
            user1.AddFriend(user3);
            user2.AddFriend(user1);
            user2.AddFriend(user3);
            user3.AddFriend(user1);
            user3.AddFriend(user2);

            // Создаем некоторые тестовые сообщения
            user1.SendMessage(user2, "Привет, друг 2!");
            user2.SendMessage(user1, "Привет, друг 1!");
            user3.SendMessage(user1, "Как дела, друг 1?");
            user1.SendMessage(user3, "Все хорошо, спасибо!");

            // Добавляем пользователей в словарь для доступа к ним
            users.Add(user1.Username, user1);
            users.Add(user2.Username, user2);
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

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string messageText = MessageBox.Text;
            if (!string.IsNullOrWhiteSpace(messageText) && selectedFriend != null)
            {
                currentUser.SendMessage(selectedFriend, messageText);

                // Очистить поле ввода сообщения
                MessageBox.Text = string.Empty;

                UpdateMessagesList();
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
