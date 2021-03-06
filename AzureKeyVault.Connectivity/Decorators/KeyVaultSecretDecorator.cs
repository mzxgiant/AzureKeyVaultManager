﻿using AzureKeyVault.Connectivity.Contracts;
using System;

namespace AzureKeyVault.Connectivity.Decorators
{
    public class KeyVaultSecretDecorator : IKeyVaultSecret
    {
        private readonly IKeyVaultSecret _secret;

        public KeyVaultSecretDecorator(IKeyVaultSecret secret)
        {
            _secret = secret;
        }

        public virtual DateTimeOffset Created
        {
            get
            {
                return _secret.Created;
            }
        }

        public virtual DateTimeOffset? Expires
        {
            get
            {
                return _secret.Expires;
            }
        }

        public virtual string Name
        {
            get
            {
                return _secret.Name;
            }
        }

        public virtual DateTimeOffset? Updated
        {
            get
            {
                return _secret.Updated;
            }
        }

        public virtual DateTimeOffset? ValidAfter
        {
            get
            {
                return _secret.ValidAfter;
            }
        }
    }
}
