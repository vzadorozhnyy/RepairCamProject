using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DataModel.DB {
    internal class NativeMethods {
        // ReSharper disable InconsistentNaming
        public enum ALG_ID {
            CALG_MD5 = 0x00008003,
            CALG_RC4 = ((3 << 13) | (4 << 9) | 1)
        }

        // ReSharper restore InconsistentNaming

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptAcquireContext(out IntPtr phProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptCreateHash(IntPtr hProv, ALG_ID algid, IntPtr hKey, uint dwFlags, out IntPtr phHash);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptHashData(IntPtr hHash, byte[] pbData, int dwDataLen, uint dwFlags);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptDeriveKey(IntPtr hProv, ALG_ID algid, IntPtr hBaseData, uint dwFlags, ref IntPtr phKey);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDestroyHash(IntPtr hHash);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptDecrypt(IntPtr hKey, IntPtr hHash, [MarshalAs(UnmanagedType.Bool)] bool final, uint dwFlags, byte[] pbData, ref int pdwDataLen);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, [MarshalAs(UnmanagedType.Bool)] bool final, uint dwFlags, byte[] pbData, ref int pdwDataLen, int dataLen);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptDestroyKey(IntPtr hKey);
    }

    public class Cryptograph {
        private const string Container = "StudyLog";
        private const string Password = "dfjhhrjgrhngjgtmkdkl";

        private const uint ProvRsaFull = 1;
        private const uint CryptNewkeyset = 0x00000008;
        private const string MsDefProv = "Microsoft Base Cryptographic Provider v1.0";
        private const uint CryptDeletekeyset = 0x00000010;

        public static string Encrypt(string value) {
            IntPtr lProvider;
            IntPtr lHash = IntPtr.Zero;
            IntPtr lKey = IntPtr.Zero;

            if (!NativeMethods.CryptAcquireContext(out lProvider, Container, MsDefProv, ProvRsaFull, CryptNewkeyset))
                if (!NativeMethods.CryptAcquireContext(out lProvider, Container, MsDefProv, ProvRsaFull, 0))
                    throw new Exception("Error during initializing of the cryptographic service provider. Code: " + Marshal.GetLastWin32Error());

            try {
                if (!NativeMethods.CryptCreateHash(lProvider, NativeMethods.ALG_ID.CALG_MD5, IntPtr.Zero, 0, out lHash))
                    throw new Exception("Error during initializing of the cryptographic service provider hash object. Code: " + Marshal.GetLastWin32Error());

                byte[] lPassword = Encoding.ASCII.GetBytes(Password);

                if (!NativeMethods.CryptHashData(lHash, lPassword, lPassword.Length, 0))
                    throw new Exception("Error during initializing of the cryptographic service provider hash object. Code: " + Marshal.GetLastWin32Error());

                if (!NativeMethods.CryptDeriveKey(lProvider, NativeMethods.ALG_ID.CALG_RC4, lHash, 0, ref lKey))
                    throw new Exception("Error during initializing of the cryptographic service provider session key. Code: " + Marshal.GetLastWin32Error());

                int lResultLength = value.Length * sizeof (char);
                NativeMethods.CryptEncrypt(lKey, IntPtr.Zero, true, 0, null, ref lResultLength, 0);
                byte[] lData = Encoding.Unicode.GetBytes(value);

                if (!NativeMethods.CryptEncrypt(lKey, IntPtr.Zero, true, 0, lData, ref lResultLength, lResultLength))
                    throw new Exception("Error in cryptographic service provider during encryption. Code: " + Marshal.GetLastWin32Error());

                byte[] lRes = PrepareValueAfterEcrypt(lData);
                string lV = Encoding.ASCII.GetString(lRes);
                return lV;
            } finally {
                NativeMethods.CryptDestroyKey(lKey);
                NativeMethods.CryptDestroyHash(lHash);
                NativeMethods.CryptAcquireContext(out lProvider, Container, MsDefProv, ProvRsaFull, CryptDeletekeyset);
            }
        }

        public static string Decrypt(string value) {
            IntPtr lProvider;
            IntPtr lHash = IntPtr.Zero;
            IntPtr lKey = IntPtr.Zero;

            if (!NativeMethods.CryptAcquireContext(out lProvider, Container, MsDefProv, ProvRsaFull, CryptNewkeyset))
                if (!NativeMethods.CryptAcquireContext(out lProvider, Container, MsDefProv, ProvRsaFull, 0))
                    throw new Exception("Error during initializing of the cryptographic service provider. Code: " + Marshal.GetLastWin32Error());

            try {
                if (!NativeMethods.CryptCreateHash(lProvider, NativeMethods.ALG_ID.CALG_MD5, IntPtr.Zero, 0, out lHash))
                    throw new Exception("Error during initializing of the cryptographic service provider hash object. Code: " + Marshal.GetLastWin32Error());
                byte[] lPassword = Encoding.ASCII.GetBytes(Password);

                if (!NativeMethods.CryptHashData(lHash, lPassword, lPassword.Length, 0))
                    throw new Exception("Error during initializing of the cryptographic service provider hash object. Code: " + Marshal.GetLastWin32Error());

                if (!NativeMethods.CryptDeriveKey(lProvider, NativeMethods.ALG_ID.CALG_RC4, lHash, 0, ref lKey))
                    throw new Exception("Error during initializing of the cryptographic service provider session key. Code: " + Marshal.GetLastWin32Error());

                byte[] lValue = PrepareValueToDecrypt(value);
                int lResultLength = value.Length;

                NativeMethods.CryptEncrypt(lKey, IntPtr.Zero, true, 0, null, ref lResultLength, 0);

                lResultLength = lValue.Length;
                if (!NativeMethods.CryptDecrypt(lKey, IntPtr.Zero, true, 0, lValue, ref lResultLength))
                    throw new Exception("Error in cryptographic service provider during decryption. Code: " + Marshal.GetLastWin32Error());

                string lRes = Encoding.Unicode.GetString(lValue);
                return lRes;
            } finally {
                NativeMethods.CryptDestroyKey(lKey);
                NativeMethods.CryptDestroyHash(lHash);
                NativeMethods.CryptAcquireContext(out lProvider, Container, MsDefProv, ProvRsaFull, CryptDeletekeyset);
            }
        }

        private static byte[] PrepareValueToDecrypt(string value) {
            int lSize = value.Length / 2;
            byte[] lNew = new byte[lSize];

            for (int l = 0; l < lSize; l++) {
                byte lHi = (byte) (value[l << 1] - ((value[l << 1] > 57) ? 55 : 48));
                byte lLo = (byte) (value[(l << 1) + 1] - ((value[(l << 1) + 1] > 57) ? 55 : 48));
                lNew[l] = (byte) ((lHi << 4) + lLo);
            }
            return lNew;
        }

        private static byte[] PrepareValueAfterEcrypt(byte[] value) {
            int lSize = value.Length * 2;
            byte[] lNew = new byte[lSize];

            for (int l = 0; l < value.Length; l++) {
                byte lHi = (byte) ((value[l] & 0xF0) >> 4);
                byte lLo = (byte) (value[l] & 0x0F);
                lNew[l << 1] = (byte) (lHi + ((lHi > 9) ? 55 : 48));
                lNew[(l << 1) + 1] = (byte) (lLo + ((lLo > 9) ? 55 : 48));
            }
            return lNew;
        }

        // ReSharper disable InconsistentNaming

        // ReSharper restore InconsistentNaming
    }
}