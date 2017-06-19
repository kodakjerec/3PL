using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;
/*
 * 結合3DES+MD5加密
 * securityKey經過MD5加密後
 * toEncrypt+securityKey再經過3DES加密
 */
namespace MD5
{
    public class MD5Crypt
    {

        /// <summary>

        /// Encrypts a string using a specified security key with

        /// the option to hash.

        /// </summary>

        /// <param name="toEncrypt">String to encrypt</param>

        /// <param name="securityKey">The key to apply to the encryption</param>

        /// <param name="useHashing">Weather hashing is used</param>

        /// <returns>The encrpyted string</returns>

        public static string Encrypt(string toEncrypt, string securityKey, bool useHashing)
        {

            string retVal = string.Empty;



            try
            {

                byte[] keyArray;

                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);



                // Validate inputs

                ValidateInput(toEncrypt);

                ValidateInput(securityKey);



                // If hashing use get hashcode regards to your key

                if (useHashing)
                {

                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(securityKey));



                    // Always release the resources and flush data

                    // of the Cryptographic service provide. Best Practice

                    hashmd5.Clear();

                }

                else
                {

                    keyArray = UTF8Encoding.UTF8.GetBytes(securityKey);

                }



                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();



                // Set the secret key for the tripleDES algorithm

                tdes.Key = keyArray;



                // Mode of operation. there are other 4 modes.

                // We choose ECB (Electronic code Book)

                tdes.Mode = CipherMode.ECB;



                // Padding mode (if any extra byte added)

                tdes.Padding = PaddingMode.PKCS7;



                ICryptoTransform cTransform = tdes.CreateEncryptor();



                // Transform the specified region of bytes array to resultArray

                byte[] resultArray =

                  cTransform.TransformFinalBlock(toEncryptArray, 0,

                  toEncryptArray.Length);



                // Release resources held by TripleDes Encryptor

                tdes.Clear();



                // Return the encrypted data into unreadable string format

                retVal = Convert.ToBase64String(resultArray, 0, resultArray.Length);

            }

            catch (Exception ex)
            {

                throw new EncryptionException(EncryptionException.Code.EncryptionFailure, ex, MethodBase.GetCurrentMethod());

            }



            return retVal;

        }



        /// <summary>

        /// Decrypts a specified key against the original security

        /// key, with the option to hash.

        /// </summary>

        /// <param name="cipherString">String to decrypt</param>

        /// <param name="securityKey">The original security key</param>

        /// <param name="useHashing">Weather hashing is enabled</param>

        /// <returns>The decrypted key</returns>

        public static string Decrypt(string cipherString, string securityKey, bool useHashing)
        {

            string retVal = string.Empty;



            try
            {

                byte[] keyArray;

                byte[] toEncryptArray = Convert.FromBase64String(cipherString);



                // Validate inputs

                ValidateInput(cipherString);

                ValidateInput(securityKey);



                if (useHashing)
                {

                    // If hashing was used get the hash code with regards to your key

                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(securityKey));



                    // Release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();

                }

                else
                {

                    // If hashing was not implemented get the byte code of the key

                    keyArray = UTF8Encoding.UTF8.GetBytes(securityKey);

                }



                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();



                // Set the secret key for the tripleDES algorithm

                tdes.Key = keyArray;



                // Mode of operation. there are other 4 modes. 

                // We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;



                // Padding mode(if any extra byte added)

                tdes.Padding = PaddingMode.PKCS7;



                ICryptoTransform cTransform = tdes.CreateDecryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(

                                     toEncryptArray, 0, toEncryptArray.Length);



                // Release resources held by TripleDes Encryptor

                tdes.Clear();



                // Return the Clear decrypted TEXT

                retVal = UTF8Encoding.UTF8.GetString(resultArray);

            }

            catch (Exception ex)
            {

                throw new EncryptionException(EncryptionException.Code.DecryptionFailure, ex, MethodBase.GetCurrentMethod());

            }



            return retVal;

        }



        /// <summary>

        /// Validates an input value

        /// </summary>

        /// <param name="inputValue">Specified input value</param>

        /// <returns>True | Falue - Value is valid</returns>

        private static bool ValidateInput(string inputValue)
        {

            bool valid = string.IsNullOrEmpty(inputValue);



            if (valid)
            {

                throw new EncryptionException(EncryptionException.Code.InvalidInputValues, MethodBase.GetCurrentMethod());

            }



            return valid;

        }

    }

    public class EncryptionException : ApplicationException
    {

        /// <summary>

        /// Code. This defines the types of exceptions that can be thrown.

        /// </summary>

        public enum Code { UnidentifiedException, EncryptionFailure, DecryptionFailure, InvalidInputValues }



        /// <summary>

        /// Stores the method information for where the exceptino was thrown.

        /// </summary>

        private MethodBase currentMethod = null;



        /// <summary>

        /// Exception with provided error code.

        /// </summary>

        /// <param name="code"></param>

        public EncryptionException(Code code)

            : base(pi_GetErrorMessage(code))
        {

        }



        /// <summary>

        /// Exception with provided code and inner exception.

        /// </summary>

        /// <param name="code">Error code</param>

        /// <param name="InnerException">The containing exception</param>

        public EncryptionException(Code code, Exception InnerException)

            : base(pi_GetErrorMessage(code), InnerException)
        {

        }



        /// <summary>

        /// Exception with provided method

        /// </summary>

        /// <param name="code">Error code</param>

        /// <param name="CurrentMethod">The method where the exception was thrown.</param>

        public EncryptionException(Code code, MethodBase CurrentMethod)

            : base(pi_GetErrorMessage(code))
        {

            this.currentMethod = CurrentMethod;

        }



        /// <summary>

        /// Exception with provided code, method and inner exception.

        /// </summary>

        /// <param name="code">Error code</param>

        /// <param name="InnerException">The containing exception</param>

        /// <param name="CurrentMethod">The method where the exception was thrown.</param>

        public EncryptionException(Code code, Exception InnerException, MethodBase CurrentMethod)

            : base(pi_GetErrorMessage(code), InnerException)
        {

            this.currentMethod = CurrentMethod;

        }



        /// <summary>

        /// Gets the error message from the specified error code.

        /// </summary>

        /// <param name="code">Error code</param>

        /// <returns>Mapped error message.</returns>

        public static string pi_GetErrorMessage(Code code)
        {

            switch (code)
            {

                case Code.EncryptionFailure:

                    return "Unable to encrypt string against specified security key.";

                case Code.DecryptionFailure:

                    return "Unable to decrypt string against specified security key.";

                case Code.InvalidInputValues:

                    return "Input string or security key for MD5 cryptography is invalid.";

                default:

                case Code.UnidentifiedException:

                    return "An unidentified error has occurred.";

            }

        }

    }
}
