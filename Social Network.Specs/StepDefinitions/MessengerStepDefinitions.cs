using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using Social_Network;
using System.Windows;

namespace Social_Network.Specs.StepDefinitions
{
    [Binding]
    public class MessengerStepDefinitions
    {
        Test t = new Test();
        int count=0;

        [Given(@"ѕользователь находитс€ на экране входа")]
        public void GivenѕользовательЌаходитс€ЌаЁкране¬хода()
        {
            Assert.AreEqual(t.visibilityLogin,Visibility.Visible);
        }

        [When(@"ќн вводит допустимое им€ пользовател€ и пароль")]
        public void Whenќн¬водитƒопустимое»м€ѕользовател€»ѕароль()
        {
            Assert.AreEqual(t.user2.Username, "user2");
            Assert.AreEqual(t.user2.Password, "password2");
        }

        [Then(@"ќн должен быть авторизован и видеть список контактов")]
        public void ThenќнƒолженЅытьјвторизован»¬идеть—писок онтактов()
        {
            Assert.AreEqual(t.visibilityContacts, Visibility.Visible);
        }

        [Given(@"ѕользователь авторизован в системе")]
        public void Givenѕользовательјвторизован¬—истеме()
        {
            Assert.AreEqual(t.isAuth, true);
            Assert.AreEqual(t.visibilityContacts, Visibility.Visible);

        }

        [When(@"ќн переходит в свой список друзей")]
        public void Whenќнѕереходит¬—вой—писокƒрузей()
        {
            Assert.AreEqual(t.visibilityContacts, Visibility.Visible);
            t.user2.Friends.ToList();
        }

        [When(@"ќн добавл€ет нового друга, указыва€ его им€ пользовател€")]
        public void Whenќнƒобавл€етЌовогоƒруга”казыва€≈го»м€ѕользовател€()
        {
            t.user2.AddFriend(t.user3);
        }

        [Then(@"Ќовый друг должен быть добавлен в список друзей пользовател€")]
        public void ThenЌовыйƒругƒолженЅытьƒобавлен¬—писокƒрузейѕользовател€()
        {
            Assert.AreEqual(t.visibilityAddFriend, Visibility.Visible);
            Assert.AreEqual(t.user2.Friends.Contains(t.user3), true);
        }

        [Given(@"ѕользователь авторизован и просматривает чат с другом")]
        public void Givenѕользовательјвторизован»ѕросматривает„ат—ƒругом()
        {
            Assert.AreEqual(t.isAuth, true);
            Assert.AreEqual(t.visibilityContacts, Visibility.Visible);
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
        }

        [When(@"ќн составл€ет и отправл€ет сообщение")]
        public void Whenќн—оставл€ет»ќтправл€ет—ообщение()
        {
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
            count = t.user2.Messages.Count;
            t.user2.SendMessage(t.user3, "hello");
        }

        [Then(@"—ообщение должно быть отправлено другу")]
        public void Then—ообщениеƒолжноЅытьќтправленоƒругу()
        {
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
            Assert.AreEqual(t.user2.Messages.Count>count, true);
        }

        [When(@"ƒруг отправл€ет ему сообщение")]
        public void Whenƒругќтправл€ет≈му—ообщение()
        {
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
            count = t.user3.Messages.Count;
            t.user2.SendMessage(t.user3, "hello");
        }

        [Then(@"ѕользователь должен получить и видеть вход€щее сообщение в чате")]
        public void Thenѕользовательƒолженѕолучить»¬идеть¬ход€щее—ообщение¬„ате()
        {
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
            Assert.AreEqual(t.user2.Messages.Count > count, true);
        }

    }
}
