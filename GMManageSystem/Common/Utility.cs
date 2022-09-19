using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Web;

namespace GMManageSystem.Common
{
    public class Utility
    {
        public static string GetFirstMacAddress()
        {
            try
            {
                string macAddresses = string.Empty;

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        if (string.IsNullOrEmpty(macAddresses))
                        {
                            macAddresses = nic.GetPhysicalAddress().ToString();
                        }
                        else
                        {
                            macAddresses += "-" + nic.GetPhysicalAddress().ToString();
                        }
                        break;
                    }
                }
                return macAddresses.Replace(":", "-");
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取MAC地址,只能获取本机
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址 
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac.Replace(':', '-');
            }
            catch
            {
                return "";
            }
        }
    }
}