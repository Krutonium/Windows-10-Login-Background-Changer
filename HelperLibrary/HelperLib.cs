using Microsoft.Win32.Security;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace HelperLibrary
{
    public class HelperLib
    {
        public static void TakeOwnership(string FD)
        {
            try
            {
                var myProcToken = new AccessTokenProcess(Process.GetCurrentProcess().Id, TokenAccessType.TOKEN_ALL_ACCESS | TokenAccessType.TOKEN_ADJUST_PRIVILEGES);
                myProcToken.EnablePrivilege(new Microsoft.Win32.Security.TokenPrivilege(Microsoft.Win32.Security.TokenPrivilege.SE_TAKE_OWNERSHIP_NAME, true));
                SecurityIdentifier identifier = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
                NTAccount identity = (NTAccount)identifier.Translate(typeof(NTAccount));
                if (File.Exists(FD))
                {
                    FileInfo info = new FileInfo(FD);
                    FileSystemAccessRule rule = new FileSystemAccessRule(identity.Value, FileSystemRights.FullControl, AccessControlType.Allow);
                    FileSecurity accessControl = info.GetAccessControl(AccessControlSections.Owner);
                    accessControl.SetOwner(new NTAccount(identity.Value));
                    info.SetAccessControl(accessControl);
                    accessControl.AddAccessRule(rule);
                    info.SetAccessControl(accessControl);
                }
                if (Directory.Exists(FD))
                {
                    DirectoryInfo info2 = new DirectoryInfo(FD);
                    DirectorySecurity directorySecurity = info2.GetAccessControl(AccessControlSections.All);
                    directorySecurity.SetOwner(identity);
                    info2.SetAccessControl(directorySecurity);
                    directorySecurity.AddAccessRule(new FileSystemAccessRule(identity, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                    info2.SetAccessControl(directorySecurity);
                }
                Clear(FD);
            }
            catch (Exception)
            {
            }
        }

        public static void Clear(string Folder)
        {
            try
            {
                if (Directory.Exists(Folder))
                {
                    foreach (string str in Directory.GetDirectories(Folder, "*.*", SearchOption.AllDirectories))
                    {
                        File.SetAttributes(str, FileAttributes.Normal);
                    }
                    foreach (string str2 in Directory.GetFiles(Folder, "*.*", SearchOption.AllDirectories))
                    {
                        File.SetAttributes(str2, FileAttributes.Normal);
                    }
                }
                if (File.Exists(Folder))
                {
                    File.SetAttributes(Folder, FileAttributes.Normal);
                }
            }
            catch
            {
            }
        }
    }
}