using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.DataProtection
{
    // DataProtection class implements IDataProtection and provides methods for securing data
    public class DataProtecion : IDataProtection
    {
        private readonly IDataProtector _protector;
        // Constructor to create a data protector using the IDataProtectionProvider
        public DataProtecion(IDataProtectionProvider provider)
        {
            // Create a protector with a specific purpose (string identifier)
            _protector = provider.CreateProtector("ShoppingPlatform-security-v1");
        }
        // Method to protect sensitive text by encrypting it
        public string Protect(string text)
        {
            return _protector.Protect(text);
        }
        // Method to unprotect previously protected text by decrypting it
        public string UnProtect(string protectedText)
        {
           return _protector.Unprotect(protectedText);
        }
    }
}
