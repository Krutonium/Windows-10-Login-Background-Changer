Imports System.IO
Imports System.Security.AccessControl
Imports System.Security.Principal


Public Class Form1
    Dim Temp As String = Path.GetTempPath & "BackgroundChanger"
    Dim PRIFileSource As String = System.Environment.GetEnvironmentVariable("windir") & "\SystemResources\Windows.UI.Logon\Windows.UI.Logon.pri"
    Dim SysResources As String = System.Environment.GetEnvironmentVariable("windir") & "\SystemResources\Windows.UI.Logon\"
    Dim PRIFile As String = Temp & "/Windows.UI.Logon.pri"
    Dim NewPriFile As String = Temp & "/Windows.UI.Logon_new.pri"
    Dim First As Boolean = True
    Dim Second As Boolean = True
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Sets up everything, gets permissions, makes a backup.
        If My.Settings.FirstRun = True Then
            Dim P = MsgBox("I am not liable if anything happens to your computer; even if it explodes or causes the apocolypse. By using this software, you agree that anything that happens is your own fault, and that I can not be held accountable. Do you agree?", MsgBoxStyle.YesNo, "Agreement")
            If P = MsgBoxResult.No Then
                Me.Close()
            Else
                My.Settings.FirstRun = False 'We no longer need to show the disclaimer.
            End If
        End If
        My.Settings.Save()
        Me.Icon = My.Resources.Icon         'Much easier to change icons using this.
        If Directory.Exists(Temp) = True Then
            Directory.Delete(Temp, True)        'If our directory in %temp% exists, delete it.
        End If
        Directory.CreateDirectory(Temp)         'Now make it again.
        PictureBox1.BackColor = Color.Transparent   'This is the picturebox that shows the usericon/password box. We need it to be transparent of course.
        PictureBox1.Parent = PBPreview              'Fixes an issue with WinForms that makes it so you can't see a picturebox through a picturebox.

        'Dim P As

        'TakeOwnDir(SysResources)
        'TakeOwnFile(PRIFileSource)
        'TakeOwnFile(PRIFileSource & ".bak")

        If File.Exists(PRIFileSource & ".bak") = False Then
            File.Copy(PRIFileSource, PRIFileSource & ".bak")        'Make a backup if one doesn't already exist.
            MsgBox("Backup created.")
        End If
        File.Copy(PRIFileSource & ".bak", PRIFile)                  'Copy the backup to %temp% so we can work on it, as well as have it as vanilla.
        Me.MaximizeBox = False
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle  'No resizing, since the form doesn't dynamically layout (Feel free to implement!
    End Sub

    Private Sub cmdDoMod_Click(sender As Object, e As EventArgs) Handles cmdDoMod.Click
        If File.Exists(tbFilePath.Text) = True Then
            If CheckBox1.Checked = True Then
                ReEncode()                      'If they checked ReEncode, Grab the image and save it in temp as a jpg
            End If
            File.Delete(PRIFileSource)          'Delete the file we are going to mod if it exists
            File.Copy(PRIFileSource & ".bak", PRIFileSource)    'Copy a new copy of it.
            File.WriteAllBytes(Temp & "/PS.ps1", My.Resources.CLW_Script)   'Extract the Powershell Application that does the actual modding.
            Dim PSi As New ProcessStartInfo
            PSi.Arguments = "-ExecutionPolicy Bypass -file " & Temp & "/PS.ps1 " & PRIFile & " " & NewPriFile & " " & tbFilePath.Text
            'Line Above: sets up the command line needed to run the powershell file. Spaces will mess it up :/
            PSi.FileName = "powershell" 'Powershell of course.
            PSi.WindowStyle = ProcessWindowStyle.Hidden
            'PSi.FileName = Temp & "/PS.ps1"
            Dim P As Process = Process.Start(PSi)
            P.WaitForExit()                         'Wait for the powershell code to finish.
            If P.ExitCode = 0 = False Then
                MsgBox("A Severe error occured, please check for an update.")       'See if an error occured.
                Application.Exit()
            End If
            'Process.Start("powershell", "-ExecutionPolicy Bypass -file " & Temp & "/PS.ps1 " & PRIFile & " " & NewPriFile & " " & tbFilePath.Text & " " & ComboBox1.SelectedText)
            Do Until File.Exists(NewPriFile)
                Threading.Thread.Sleep(1000)        'Wait for Windows to finish writing the new PRI File.
            Loop
            File.Copy(NewPriFile, PRIFileSource, True)          'Copies our new PRI File to SystemResources
            MsgBox("Done, Lock your computer to see your new background!")
            'Application.Exit()     'Used to close here.
        Else
            MsgBox("Please choose a color or an image before you hit this button.")
        End If
    End Sub

    Private Sub cmdBrowse_Click(sender As Object, e As EventArgs) Handles cmdBrowse.Click
        Dim P = OFD.ShowDialog()
        If P = Windows.Forms.DialogResult.OK Then
            Call SelectImage(OFD.FileName)      'User selects image and we feed it to SelectImage.
        End If
    End Sub

    Private Sub SelectImage(ByVal Image As String)
        tbFilePath.Enabled = True
        Try
            If Image.ToUpper.EndsWith(".JPG") And CheckBox1.Checked = False Then
                PBPreview.Load(Image)
                tbFilePath.Text = Image                 'JPG and PNG seem to work most consistantly, others are hit and miss.
            ElseIf Image.ToUpper.EndsWith(".PNG") Then
                PBPreview.Load(Image)
                tbFilePath.Text = Image
            Else
                Call ReEncode()     'ReEncodes the image as a jpg.
            End If
        Catch ex As Exception
            MsgBox("Not a valid file.")
        End Try
    End Sub

    Private Sub ReEncode()
        Try
            Dim ImageFile As System.Drawing.Image = System.Drawing.Image.FromFile(OFD.FileName)
            Dim Rand As New Random
            Randomize()
            Dim RandNum As String = Rand.Next(0, 100000)
            RandNum += ".jpg"
            RandNum = "\" & RandNum
            ImageFile.Save(Temp & RandNum, Imaging.ImageFormat.Jpeg)
            PBPreview.Load(Temp & RandNum)
            tbFilePath.Text = (Temp & RandNum)          'Takes the image, loads it, saves it as jpg, using a random number for the file name.
        Catch ex As Exception
            MsgBox("Not a valid image file.")
        End Try
    End Sub
    Private Sub cmdSolidColor_Click(sender As Object, e As EventArgs) Handles cmdSolidColor.Click
        ColorD.ShowDialog()
        Dim X As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim Y As Integer = Screen.PrimaryScreen.Bounds.Height
        Dim rand As New Random
        Randomize()
        Dim RandNum As String = rand.Next(0, 100000)
        RandNum += ".jpg"
        RandNum = "\" & RandNum
        Using Bmp As New Bitmap(X, Y, Imaging.PixelFormat.Format32bppPArgb)         'Generates a solid color image at 300ppi, at the resolution of the primary screen.
            Bmp.SetResolution(300, 300)
            Using G = Graphics.FromImage(Bmp)
                If ColorD.Color = Nothing Then
                    ColorD.Color = Color.Black
                End If
                G.Clear(ColorD.Color)
                G.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                G.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            End Using
            Bmp.Save(Temp & RandNum, Imaging.ImageFormat.Jpeg)
        End Using
        PBPreview.Load(Temp & RandNum)
        tbFilePath.Enabled = False
        tbFilePath.Text = (Temp & RandNum)
    End Sub

    Private Sub cmdCredits_Click(sender As Object, e As EventArgs) Handles cmdCredits.Click
        About.ShowDialog()
    End Sub

    Private Sub chkUserIcon_CheckedChanged(sender As Object, e As EventArgs) Handles chkUserIcon.CheckedChanged
        If chkUserIcon.Checked = False Then
            PictureBox1.Image = My.Resources.login_noUser
            For Each Folder In Directory.GetDirectories("C:\Users\Public\AccountPictures")  'Hides the account picture
                Directory.Move(Folder, Folder & "_old")
            Next
        Else
            PictureBox1.Image = My.Resources.login
            For Each Folder In Directory.GetDirectories("C:\Users\Public\AccountPictures")      'UnHides it.
                If Folder.EndsWith("_old") Then
                    Dim T As String = Folder
                    T = T.Replace("_old", "")
                    Directory.Move(Folder, T)
                End If
            Next
        End If
    End Sub

    Private Sub chkNoBackground_CheckedChanged(sender As Object, e As EventArgs) Handles chkNoBackground.CheckedChanged
        If chkNoBackground.Checked = True Then
            cmdBrowse.Enabled = False
            cmdSolidColor.Enabled = False
            Try
                tbFilePath.Text = Temp & "\Accent.png"
                File.WriteAllBytes(Temp & "\Accent.png", My.Resources.transparent)      'Sets a transparent PNG as the background, makes windows use Accent Colors instead.
                PBPreview.Load(Temp & "\Accent.png")
            Catch ex As Exception
            End Try
        Else
            cmdBrowse.Enabled = True
            cmdSolidColor.Enabled = True
        End If
    End Sub

    Private Sub cmdRevert_Click(sender As Object, e As EventArgs) Handles cmdRevert.Click
        File.Delete(PRIFileSource)
        File.Copy(PRIFileSource & ".bak", PRIFileSource)        'Reverts Changes.
        MsgBox("Done.")
    End Sub
End Class
