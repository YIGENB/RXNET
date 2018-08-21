using System.Management;

namespace RX.Ext
{
    public static class System
    {
        /// <summary>
        /// 获取服务器本机的MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            string strResult = "";

            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True") strResult = mo["MacAddress"].ToString();
            }

            return strResult;
        }
    }
}
