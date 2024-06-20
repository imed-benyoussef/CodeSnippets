using System;
using System.Security.Cryptography;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Services;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class HashPasswordService : IHashPasswordService
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000; // Number of iterations for the hashing algorithm

        /// <summary>
        /// Hashes the password with a security stamp.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="securityStamp">The security stamp to include in the hash.</param>
        /// <returns>The hashed password as a base64 encoded string.</returns>
        public string HashPassword(string password, string securityStamp)
        {
            // Combine the password and the security stamp
            using (var algorithm = new Rfc2898DeriveBytes(
                password + securityStamp,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                var salt = algorithm.Salt; // Generate a random salt
                var key = algorithm.GetBytes(KeySize); // Generate the key

                // Combine the salt and key into a single byte array
                var hash = new byte[SaltSize + KeySize];
                Array.Copy(salt, 0, hash, 0, SaltSize);
                Array.Copy(key, 0, hash, SaltSize, KeySize);

                // Return the combined salt and key as a base64 encoded string
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Verifies the password against the stored hash and security stamp.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="passwordHash">The stored password hash.</param>
        /// <param name="securityStamp">The security stamp used during hashing.</param>
        /// <returns>True if the password is valid, otherwise false.</returns>
        public bool VerifyPassword(string password, string passwordHash, string securityStamp)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(passwordHash));
            if (string.IsNullOrWhiteSpace(securityStamp)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(securityStamp));

            // Convert the stored password hash from base64 to a byte array
            var hashBytes = Convert.FromBase64String(passwordHash);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize); // Extract the salt from the hashBytes

            // Combine the password and security stamp and hash it using the extracted salt
            using (var algorithm = new Rfc2898DeriveBytes(
                password + securityStamp,
                salt,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                var key = algorithm.GetBytes(KeySize); // Generate the key
                for (int i = 0; i < KeySize; i++)
                {
                    // Compare the generated key with the stored key
                    if (hashBytes[i + SaltSize] != key[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
