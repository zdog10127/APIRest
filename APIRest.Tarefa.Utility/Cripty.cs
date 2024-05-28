using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APIRest.Tarefa.Utility
{
    public class Cripty
    {
        private const string secretKey = "$001AF55a8W15D9Z94d@8qR4g5rTY001CZ==_W";

        /// <summary>
        /// Criptografa um texto
        /// </summary>
        /// <param name="valor">texto a ser criptografado</param>
        /// <returns>texto criptografado</returns>
        public static string Criptografar(string valor)
        {
            try
            {
                return EncryptSymmetric("DESCryptoServiceProvider", valor);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Descriptografa um texto que foi criptografado com esse mesmo algoritmo
        /// </summary>
        /// <param name="valor">texto a ser descriptografado</param>
        /// <returns>texto descriptografado</returns>
        public static string Descriptografar(string valor)
        {
            try
            {
                return DecryptSymmetric("DESCryptoServiceProvider", valor);
            }
            catch
            {
                return string.Empty;
            }
        }

        #region Asymmetric Cryptography

        #region Privates Attributes

        private const int SALT_SIZE = 4; // Bytes
        private const int SHA1Managed_Length = 28; // Digits
        private static HashAlgorithm HashProvider;

        #endregion

        #region Private Methods

        /// <summary>
        /// Cria um hash a partir do password e do salt utilizando um algoritmo de criptografia
        /// </summary>
        /// <param name="Data">Password</param>
        /// <param name="Salt">Salt</param>
        /// <returns>Array de bytes</returns>
        private static byte[] ComputeHash(byte[] Data, byte[] Salt)
        {
            var DataAndSalt = new byte[Data.Length + SALT_SIZE];

            Array.Copy(Data, DataAndSalt, Data.Length);
            Array.Copy(Salt, 0, DataAndSalt, Data.Length, SALT_SIZE);
            return HashProvider.ComputeHash(DataAndSalt); // Aplicação do algoritmo de criptografia
        }

        /// <summary>
        /// Retorna o Hash e o Salt através de parâmetros de saida
        /// </summary>
        /// <param name="Data">Password</param>
        /// <param name="Hash">Parametro de saida do Hash</param>
        /// <param name="Salt">Parametro de saída do Salt</param>
        private static void GetHashAndSalt(byte[] Data, out byte[] Hash, out byte[] Salt)
        {
            Salt = new byte[SALT_SIZE];

            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(Salt);
            }

            Hash = ComputeHash(Data, Salt);
        }

        /// <summary>
        /// Verifica se o hash gerado a partir dos parâmetros Data e Salt é idêntico ao parâmetro Hash
        /// </summary>
        /// <param name="Data">Password</param>
        /// <param name="Hash">Hash</param>
        /// <param name="Salt">Salt</param>
        /// <returns>True | False</returns>
        private static bool VerifyHash(byte[] Data, byte[] Hash, byte[] Salt)
        {
            byte[] NewHash = ComputeHash(Data, Salt);

            if (NewHash.Length != Hash.Length)
            {
                return false;
            }

            for (int Lp = 0; Lp < Hash.Length; Lp++)
            {
                if (!Hash[Lp].Equals(NewHash[Lp]))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Cria um hash criptografado a partir de um algoritmo de criptografia e um password
        /// </summary>
        /// <param name="hashInstance">Algoritmo de criptografia. 
        /// Implementados: SHA1Managed </param>
        /// <param name="plaintext">Password</param>
        /// <returns>Hash</returns>
        private static string CreateHash(string hashInstance, string plaintext)
        {
            byte[] HashOut;
            byte[] SaltOut;

            Type generic = Type.GetType("System.Security.Cryptography." + hashInstance);
            HashProvider = (HashAlgorithm)Activator.CreateInstance(generic);

            GetHashAndSalt(Encoding.UTF8.GetBytes(plaintext), out HashOut, out SaltOut);
            string HashAndSalt = Convert.ToBase64String(HashOut) + Convert.ToBase64String(SaltOut);

            return HashAndSalt;
        }

        /// <summary>
        /// Valida se o password informado gera um hash idêntico com o hash armazenado
        /// </summary>
        /// <param name="hashInstance">Algoritmo de criptografia. 
        /// Implementados: SHA1Managed </param>
        /// <param name="plaintext">Password</param>
        /// <param name="hashedText">Hash</param>
        /// <returns>True | False</returns>
        private static bool CompareHash(string hashInstance, string plaintext, string hashedText)
        {
            string Hash = string.Empty;
            string Salt = string.Empty;


            Type generic = Type.GetType("System.Security.Cryptography." + hashInstance);
            HashProvider = (HashAlgorithm)Activator.CreateInstance(generic);

            switch (hashInstance)
            {
                case "SHA1Managed":

                    if (hashedText.Length != (SHA1Managed_Length + 8)) // Hash + Salt
                    {
                        return false;
                    }

                    Hash = hashedText.Substring(0, SHA1Managed_Length);
                    Salt = hashedText.Substring(SHA1Managed_Length);
                    break;
            }

            byte[] DataToVerify = Encoding.UTF8.GetBytes(plaintext);
            byte[] HashToVerify = Convert.FromBase64String(Hash);
            byte[] SaltToVerify = Convert.FromBase64String(Salt);
            return VerifyHash(DataToVerify, HashToVerify, SaltToVerify);
        }

        #endregion

        #endregion

        #region Symmetric Cryptography

        #region Private Attributes

        private static readonly byte[] _Salt = new byte[]
        {
            0x45, 0xF1, 0x61, 0x6e, 0x20, 0x00, 0x65, 0x64, 0x76, 0x65, 0x64,
            0x03, 0x76
        };

        private static SymmetricAlgorithm SymmetricProvider;

        #endregion

        #region Private Methods

        /// <summary>
        /// Criptografa dados a partir de um algoritmo de criptografia informado
        /// </summary>
        /// <param name="symmetricInstance">Algoritmo de criptografia.
        /// Implementados: RijndaelManaged, TripleDESCryptoServiceProvider, RC2CryptoServiceProvider, DESCryptoServiceProvider</param>
        /// <param name="clearData">Dado a ser criptografado</param>
        /// <param name="Key">Chave de segurança</param>
        /// <param name="IV">Vetor de incremento</param>
        /// <returns>Array de bytes</returns>
        private static byte[] EncryptSymmetric(string symmetricInstance, byte[] clearData, byte[] Key, byte[] IV)
        {
            using (var ms = new MemoryStream())
            {
                CryptoStream cs = null;

                try
                {
                    //Type generic = Type.GetType("DESCryptoServiceProvider");
                    //SymmetricProvider = (SymmetricAlgorithm)Activator.CreateInstance(generic);

                    DESCryptoServiceProvider SymmetricProvider = new DESCryptoServiceProvider();
                    SymmetricProvider.Key = Key;
                    SymmetricProvider.IV = IV;
                    cs = new CryptoStream(ms, SymmetricProvider.CreateEncryptor(), CryptoStreamMode.Write);
                    cs.Write(clearData, 0, clearData.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
                catch
                {
                    throw new Exception("Erro ao realizar a criptografia com o algoritmo informado.");
                }
                finally
                {
                    cs.Close();
                }
            }
        }

        /// <summary>
        /// Descriptograda dados a partir de um algoritmo de criptografia informado
        /// </summary>
        /// <param name="symmetricInstance">Algoritmo de criptografia.
        /// Implementados: RijndaelManaged, TripleDESCryptoServiceProvider, RC2CryptoServiceProvider, DESCryptoServiceProvider</param>
        /// <param name="cipherData">Dado criptografado</param>
        /// <param name="Key">Chave de sergurança</param>
        /// <param name="IV">Vetor de incremento</param>
        /// <returns>Array de bytes</returns>
        private static byte[] DecryptSymmetric(string symmetricInstance, byte[] cipherData, byte[] Key, byte[] IV)
        {
            using (var ms = new MemoryStream())
            {
                CryptoStream cs = null;

                try
                {
                    //Type generic = Type.GetType("System.Security.Cryptography." + symmetricInstance);
                    //SymmetricProvider = (SymmetricAlgorithm)Activator.CreateInstance(generic);

                    DESCryptoServiceProvider SymmetricProvider = new DESCryptoServiceProvider();

                    SymmetricProvider.Key = Key;
                    SymmetricProvider.IV = IV;
                    cs = new CryptoStream(ms, SymmetricProvider.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(cipherData, 0, cipherData.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
                catch
                {
                    throw new Exception("Erro ao realizar a descriptografia com o algoritmo informado.");
                }
                finally
                {
                    cs.Close();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Descriptograda uma string a partir de um algoritmo de criptografia informado
        /// </summary>
        /// <param name="symmetricInstance">Algoritmo de criptografia.
        /// Implementados: RijndaelManaged, TripleDESCryptoServiceProvider, RC2CryptoServiceProvider, DESCryptoServiceProvider</param>
        /// <param name="plaintext">String a ser criptografada</param>
        /// <returns>String descriptografa</returns>
        public static string DecryptSymmetric(string symmetricInstance, string plaintext)
        {
            byte[] KEY = null;
            byte[] IV = null;
            byte[] valueBytes = Convert.FromBase64String(plaintext);

            using (var pdb = new Rfc2898DeriveBytes(secretKey, _Salt))
            {
                switch (symmetricInstance)
                {
                    case "RijndaelManaged":
                        KEY = pdb.GetBytes(32);
                        IV = pdb.GetBytes(16);
                        break;
                    case "TripleDESCryptoServiceProvider":
                        KEY = pdb.GetBytes(24);
                        IV = pdb.GetBytes(8);
                        break;
                    case "RC2CryptoServiceProvider":
                        KEY = pdb.GetBytes(16);
                        IV = pdb.GetBytes(8);
                        break;
                    case "DESCryptoServiceProvider":
                        KEY = pdb.GetBytes(8);
                        IV = pdb.GetBytes(8);
                        break;
                }

            }

            byte[] decryptedData = DecryptSymmetric(symmetricInstance, valueBytes, KEY, IV);
            return Encoding.Unicode.GetString(decryptedData);
        }

        /// <summary>
        /// Criptografa uma string a partir de um algoritmo de criptografia informado
        /// </summary>
        /// <param name="symmetricInstance">Algoritmo de criptografia.
        /// Implementados: RijndaelManaged, TripleDESCryptoServiceProvider, RC2CryptoServiceProvider, DESCryptoServiceProvider   </param>
        /// <param name="plaintext">String criptografada</param>
        /// <returns>String criptografada</returns>
        public static string EncryptSymmetric(string symmetricInstance, string plaintext)
        {
            byte[] KEY = null;
            byte[] IV = null;
            byte[] valueBytes = Encoding.Unicode.GetBytes(plaintext);

            using (var pdb = new Rfc2898DeriveBytes(secretKey, _Salt))
            {
                switch (symmetricInstance)
                {
                    case "RijndaelManaged":
                        KEY = pdb.GetBytes(32);
                        IV = pdb.GetBytes(16);
                        break;
                    case "TripleDESCryptoServiceProvider":
                        KEY = pdb.GetBytes(24);
                        IV = pdb.GetBytes(8);
                        break;
                    case "RC2CryptoServiceProvider":
                        KEY = pdb.GetBytes(16);
                        IV = pdb.GetBytes(8);
                        break;
                    case "DESCryptoServiceProvider":
                        KEY = pdb.GetBytes(8);
                        IV = pdb.GetBytes(8);
                        break;
                }
            }

            byte[] encryptedData = EncryptSymmetric(symmetricInstance, valueBytes, KEY, IV);
            return Convert.ToBase64String(encryptedData);
        }

        #endregion

        #endregion  
    }
}
