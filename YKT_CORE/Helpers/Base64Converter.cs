using System.Security.Cryptography;
using System.Text;

namespace YKT.CORE.Helpers
{
    public static class Base64Converter
    {
        public static byte[] GetBase64Content(string[] token)
        {
            var bytes = new List<byte>();
            try
            {
                bytes = (Convert.FromBase64String($"{token[1]}")).ToList();
                return bytes.ToArray();
            }
            catch
            {
                try
                {
                    bytes = (Convert.FromBase64String($"{token[1]}=")).ToList();
                    return bytes.ToArray();
                }
                catch
                {
                    try
                    {
                        bytes = (Convert.FromBase64String($"{token[1]}==")).ToList();
                        return bytes.ToArray();
                    }
                    catch { }
                }
            }
            return null;
        }

        public static string GenerarEncriptada(string password)
        {
            if (password != string.Empty)
            {
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                return Convert.ToBase64String(hashBytes);
            }
            return password;
        }

        public static string RecuperarContraseña(string contraseñaEncriptadaBase64)
        {
            byte[] hashBytes = Convert.FromBase64String(contraseñaEncriptadaBase64);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes("", salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            bool sonIguales = CompararBytes(hashBytes, hash);
            if (sonIguales)
            {
                return Encoding.UTF8.GetString(hashBytes, 36, hashBytes.Length - 36);
            }
            else
            {
                return null;
            }
        }

        public static bool CompararBytes(byte[] array1, byte[] array2)
        {
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
