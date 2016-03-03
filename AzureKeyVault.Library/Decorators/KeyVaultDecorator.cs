﻿using AzureKeyVaultManager.Contracts;
using System;

namespace AzureKeyVaultManager.Decorators
{
    public class KeyVaultDecorator : IKeyVault
    {
        private readonly IKeyVault _vault;

        public KeyVaultDecorator(IKeyVault vault)
        {
            _vault = vault;
        }

        public virtual string Name
        {
            get { return _vault.Name; }
        }

        public virtual string ResourceGroup
        {
            get { return _vault.ResourceGroup; }
        }

        public virtual Uri Uri
        {
            get { return _vault.Uri; }
        }

        public virtual string Id
        {
            get { return _vault.Id; }
        }

        public virtual Guid TenantId
        {
            get { return _vault.TenantId; }
        }
    }
}
