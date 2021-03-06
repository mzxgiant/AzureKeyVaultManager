﻿using AzureKeyVault.Connectivity.Contracts;
using AzureKeyVault.Connectivity.Decorators;
using System.ComponentModel;
using System.Windows.Input;

namespace AzureKeyVaultManager.UWP.ViewModels
{
    public class KeyVaultViewModel : KeyVaultDecorator, INotifyPropertyChanged
    {
        public KeyVaultViewModel(IKeyVault vault) : base(vault)
        {
        }

        public ICommand ShowAccessPermissions { get; set; }

        public ICommand ShowDeleteConfirmation { get; set; }

        public ICommand ShowAddKey { get; set; }

        public ICommand ShowAddSecret { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
