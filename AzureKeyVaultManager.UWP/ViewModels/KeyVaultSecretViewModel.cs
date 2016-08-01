﻿using AzureKeyVault.Connectivity.Contracts;
using AzureKeyVault.Connectivity.Decorators;
using System.ComponentModel;
using System.Windows.Input;

namespace AzureKeyVaultManager.UWP.ViewModels
{
    public class KeyVaultSecretViewModel : KeyVaultSecretDecorator, IKeyVaultItemViewModel, INotifyPropertyChanged
    {
        private string _secret;

        public KeyVaultSecretViewModel(IKeyVaultSecret secret)
            : base(secret)
        {
        }

        public ICommand ShowSecret { get; set; }

        public ICommand ShowDeleteConfirmation { get; set; }

        public string Secret
        {
            get
            {
                return _secret;
            }
            set
            {
                _secret = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Secret)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
