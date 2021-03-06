﻿using AzureKeyVault.Connectivity.Contracts;
using AzureKeyVault.Connectivity.Rest.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AzureKeyVault.Connectivity.KeyVaultWrapper;
using Newtonsoft.Json;

namespace AzureKeyVault.Connectivity.Rest.Http
{
    class KeyVaultRestClient : RestClientBase, IKeyVaultService
    {
        private readonly Uri _root;

        public KeyVaultRestClient(IKeyVault keyVault, HttpClient client)
            : base(client, "2015-06-01")
        {
            _root = keyVault.Uri;
        }

        public async Task<IKeyVaultKey> CreateKey(string keyName, bool isHsmStored, bool enabled, string[] keyOps)
        {
            var command = new
            {
                kty = isHsmStored ? "RSA-HSM" : "RSA",
                key_ops = keyOps,
                key_size = 2048, // static value
                attributes = new
                {
                    enabled = enabled,
                },                
            };
            
            var uri = new Uri(_root, $"keys/{keyName}/create?api-version={Version}");
            var result = await Post<AzureKeyVaultKey>(uri, JsonConvert.SerializeObject(command), "application/json");
            return result;
        }

        public async Task<IKeyVaultSecret> CreateSecret(string secretName, string value)
        {
            var command = new
            {
                value = value
            };

            var uri = new Uri(_root, $"secrets/{secretName}?api-version={Version}");
            var result = await Put<AzureKeyVaultSecret>(uri, JsonConvert.SerializeObject(command), "application/json");
            return result;
        }

        public async Task<ICollection<IKeyVaultSecret>> GetSecrets()
        {
            var uri = new Uri(_root, $"secrets?api-version={Version}");
            var data = await Get<JsonValues<AzureKeyVaultSecret>>(uri);
            if (data.Value == null)
                return new List<IKeyVaultSecret>();
            return data.Value.Select(x => x as IKeyVaultSecret).ToList();
        }

        public async Task<String> GetSecretValue(IKeyVaultSecret secret)
        {
            var uri = new Uri(_root, $"secrets/{secret.Name}?api-version={Version}");
            var data = await Get<AzureKeyVaultSecretValue>(uri);
            return data.Value;
        }

        public async Task<String> SetSecretValue(IKeyVaultSecret secret, string value)
        {
            var command = new
            {
                value = value
            };
                        
            //var putData = $"{{ \"value\": \"{value}\" }}";
            var uri = new Uri(_root, $"secrets/{secret.Name}?api-version={Version}");
            var data = await Put<AzureKeyVaultSecretValue>(uri, JsonConvert.SerializeObject(command), "application/json");
            return data.Value;
        }

        public async Task<ICollection<IKeyVaultKey>> GetKeys()
        {
            var uri = new Uri(_root, $"keys?api-version={Version}");
            var data = await Get<JsonValues<AzureKeyVaultKey>>(uri);
            if (data.Value == null)
                return new List<IKeyVaultKey>();
            return data.Value.Select(x => x as IKeyVaultKey).ToList();
        }

        public async Task<String> GetKeyValue(IKeyVaultKey key)
        {
            var uri = new Uri(_root, $"keys/{key.Name}?api-version={Version}");
            var data = await Get<dynamic>(uri);
            //var data = await Get<AzureKeyVaultKeyValue>(uri);
            if (data == null)
                return null;
            return JsonConvert.SerializeObject(new { key = ((dynamic)data).key });
        }

        public async Task Delete(IKeyVaultKey key)
        {
            var uri = new Uri(_root, $"keys/{key.Name}?api-version={Version}");
            await Delete(uri);
        }

        public async Task Delete(IKeyVaultSecret secret)
        {
            var uri = new Uri(_root, $"secrets/{secret.Name}?api-version={Version}");
            await Delete(uri);
        }

        public async Task<string> Encrypt(IKeyVaultKey key, KeyVaultAlgorithm algorithm, string valueData)
        {
            var uri = new Uri(_root, $"keys/{key.Name}/encrypt?api-version={Version}");
            var command = new
            {
                alg = algorithm.GetConfigurationString(),
                value = valueData
            };
            var data = await Post(uri, JsonConvert.SerializeObject(command), "application/json");
            return data.value;
        }

        public async Task<string> Decrypt(IKeyVaultKey key, KeyVaultAlgorithm algorithm, string valueData)
        {
            var uri = new Uri(_root, $"keys/{key.Name}/decrypt?api-version={Version}");
            var command = new
            {
                alg = algorithm.GetConfigurationString(),
                value = valueData
            };
            var data = await Post(uri, JsonConvert.SerializeObject(command), "application/json");
            var base64EncodedData = (string)((dynamic)data).value;
            return base64EncodedData.PadRight(base64EncodedData.Length + (4 - base64EncodedData.Length % 4) % 4, '=');
        }

        public async Task<string> Sign(IKeyVaultKey key, KeyVaultAlgorithm algorithm, string digest)
        {
            var uri = new Uri(_root, $"keys/{key.Name}/sign?api-version={Version}");
            var command = new
            {
                alg = algorithm.GetConfigurationString(),
                value = digest
            };
            var data = await Post(uri, JsonConvert.SerializeObject(command), "application/json");
            return data.value;
        }

        public async Task<bool> Verify(IKeyVaultKey key, KeyVaultAlgorithm algorithm, string digest, string valueToVerify)
        {
            var uri = new Uri(_root, $"keys/{key.Name}/verify?api-version={Version}");
            var command = new
            {
                alg = algorithm.GetConfigurationString(),
                digest = digest,
                value = valueToVerify
            };
            var data = await Post(uri, JsonConvert.SerializeObject(command), "application/json");
            return data.value;
        }

        public async Task<string> Wrap(IKeyVaultKey key, KeyVaultAlgorithm algorithm, string valueToWrap)
        {
            var uri = new Uri(_root, $"keys/{key.Name}/wrapkey?api-version={Version}");
            var command = new
            {
                alg = algorithm.GetConfigurationString(),
                value = valueToWrap
            };
            var data = await Post(uri, JsonConvert.SerializeObject(command), "application/json");
            return data.value;
        }

        public async Task<string> Unwrap(IKeyVaultKey key, KeyVaultAlgorithm algorithm, string valueToUnwrap)
        {
            var uri = new Uri(_root, $"keys/{key.Name}/unwrapkey?api-version={Version}");
            var command = new
            {
                alg = algorithm.GetConfigurationString(),
                value = valueToUnwrap
            };
            var data = await Post(uri, JsonConvert.SerializeObject(command), "application/json");
            var base64EncodedData = (string)((dynamic)data).value;
            return base64EncodedData.PadRight(base64EncodedData.Length + (4 - base64EncodedData.Length % 4) % 4, '=');
        }
    }
}
