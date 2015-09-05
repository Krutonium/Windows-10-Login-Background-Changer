using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace HelperLibrary
{
    public class HelperLib
    {
        public static void TakeOwnership(string fd)
        {
            try
            {
                var myProcToken = new AccessTokenProcess(Process.GetCurrentProcess().Id,
                    TokenAccessType.TOKEN_ALL_ACCESS | TokenAccessType.TOKEN_ADJUST_PRIVILEGES);
                myProcToken.EnablePrivilege(new TokenPrivilege(TokenPrivilege.SE_TAKE_OWNERSHIP_NAME, true));
                var identifier = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
                var identity = (NTAccount)identifier.Translate(typeof(NTAccount));
                if (File.Exists(fd))
                {
                    var info = new FileInfo(fd);
                    var rule = new FileSystemAccessRule(identity.Value, FileSystemRights.FullControl,
                        AccessControlType.Allow);
                    var accessControl = info.GetAccessControl(AccessControlSections.Owner);
                    accessControl.SetOwner(new NTAccount(identity.Value));
                    info.SetAccessControl(accessControl);
                    accessControl.AddAccessRule(rule);
                    info.SetAccessControl(accessControl);
                }
                if (Directory.Exists(fd))
                {
                    var info2 = new DirectoryInfo(fd);
                    var directorySecurity = info2.GetAccessControl(AccessControlSections.All);
                    directorySecurity.SetOwner(identity);
                    info2.SetAccessControl(directorySecurity);
                    directorySecurity.AddAccessRule(new FileSystemAccessRule(identity, FileSystemRights.FullControl,
                        InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None,
                        AccessControlType.Allow));
                    info2.SetAccessControl(directorySecurity);
                }
                Clear(fd);
            }
            catch (Exception)
            {
            }
        }

        public static void Clear(string folder)
        {
            try
            {
                if (Directory.Exists(folder))
                {
                    foreach (var str in Directory.GetDirectories(folder, "*.*", SearchOption.AllDirectories))
                    {
                        File.SetAttributes(str, FileAttributes.Normal);
                    }
                    foreach (var str2 in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
                    {
                        File.SetAttributes(str2, FileAttributes.Normal);
                    }
                }
                if (File.Exists(folder))
                {
                    File.SetAttributes(folder, FileAttributes.Normal);
                }
            }
            catch
            {
            }
        }
    }
}