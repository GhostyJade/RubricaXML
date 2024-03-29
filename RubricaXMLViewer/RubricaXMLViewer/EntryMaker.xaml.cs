﻿using RubricaXMLViewer.AddressBook.Data;
using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.Utils;
using System.Windows;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per EntryMaker.xaml
    /// </summary>
    public partial class EntryMaker : Window
    {
        private string bookName;

        public EntryMaker(string bookName)
        {
            this.bookName = bookName;
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.TryGetText();
            string surname = TxtSurname.TryGetText();
            string phoneNumber = TxtPhoneText.TryGetText();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phoneNumber))
            {
                //TODO perform something useful
                return;
            }
            else if (name.Contains(",") || surname.Contains(",") || phoneNumber.Contains(","))
            {
                //TODO perform something useful pt2
                return;
            }
            else
            {
                AddressBookEntry entry = new AddressBookEntry()
                {
                    Name = name,
                    Surname = surname,
                    PhoneNumber = phoneNumber
                };
                NetworkManager.Instance.SendNewAddressBookEntry(entry, bookName);
                NetworkManager.Instance.Receive();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
