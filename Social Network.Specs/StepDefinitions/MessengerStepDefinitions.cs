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

        [Given(@"������������ ��������� �� ������ �����")]
        public void Given����������������������������������()
        {
            Assert.AreEqual(t.visibilityLogin,Visibility.Visible);
        }

        [When(@"�� ������ ���������� ��� ������������ � ������")]
        public void When����������������������������������������()
        {
            Assert.AreEqual(t.user2.Username, "user2");
            Assert.AreEqual(t.user2.Password, "password2");
        }

        [Then(@"�� ������ ���� ����������� � ������ ������ ���������")]
        public void Then���������������������������������������������()
        {
            Assert.AreEqual(t.visibilityContacts, Visibility.Visible);
        }

        [Given(@"������������ ����������� � �������")]
        public void Given�������������������������������()
        {
            Assert.AreEqual(t.isAuth, true);
            Assert.AreEqual(t.visibilityContacts, Visibility.Visible);

        }

        [When(@"�� ��������� � ���� ������ ������")]
        public void When����������������������������()
        {
            Assert.AreEqual(t.visibilityContacts, Visibility.Visible);
            t.user2.Friends.ToList();
        }

        [When(@"�� ��������� ������ �����, �������� ��� ��� ������������")]
        public void When������������������������������������������������()
        {
            t.user2.AddFriend(t.user3);
        }

        [Then(@"����� ���� ������ ���� �������� � ������ ������ ������������")]
        public void Then����������������������������������������������������()
        {
            Assert.AreEqual(t.visibilityAddFriend, Visibility.Visible);
            Assert.AreEqual(t.user2.Friends.Contains(t.user3), true);
        }

        [Given(@"������������ ����������� � ������������� ��� � ������")]
        public void Given�����������������������������������������������()
        {
            Assert.AreEqual(t.isAuth, true);
            Assert.AreEqual(t.visibilityContacts, Visibility.Visible);
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
        }

        [When(@"�� ���������� � ���������� ���������")]
        public void When��������������������������������()
        {
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
            count = t.user2.Messages.Count;
            t.user2.SendMessage(t.user3, "hello");
        }

        [Then(@"��������� ������ ���� ���������� �����")]
        public void Then����������������������������������()
        {
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
            Assert.AreEqual(t.user2.Messages.Count>count, true);
        }

        [When(@"���� ���������� ��� ���������")]
        public void When��������������������������()
        {
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
            count = t.user3.Messages.Count;
            t.user2.SendMessage(t.user3, "hello");
        }

        [Then(@"������������ ������ �������� � ������ �������� ��������� � ����")]
        public void Then�������������������������������������������������������()
        {
            Assert.AreEqual(t.visibilityChat, Visibility.Visible);
            Assert.AreEqual(t.user2.Messages.Count > count, true);
        }

    }
}
