Imports System.IO
Public Class Form1
    Dim Temp As String = Path.GetTempPath & "BackgroundChanger"
    Dim PRIFileSource As String = System.Environment.GetEnvironmentVariable("windir") & "\SystemResources\Windows.UI.Logon\Windows.UI.Logon.pri"
    Dim PRIFile As String = Temp & "/Windows.UI.Logon.pri"
    Dim NewPriFile As String = Temp & "/Windows.UI.Logon_new.pri"
    Dim First As Boolean = True
    Dim Second As Boolean = True
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.FirstRun = True Then
            Dim P = MsgBox("I am not liable if anything happens to your computer; even if it explodes or causes the apocolypse. By using this software, you agree that anything that happens is your own fault, and that I can not be held accountable. Do you agree?", MsgBoxStyle.YesNo, "Agreement")
            If P = MsgBoxResult.No Then
                Me.Close()
            Else
                My.Settings.FirstRun = False
            End If
        End If
        My.Settings.Save()
        Me.Icon = My.Resources.Icon
        If Directory.Exists(Temp) = True Then
            Directory.Delete(Temp, True)
        End If
        Directory.CreateDirectory(Temp)
        File.WriteAllBytes(Temp & "/takemyfiles.bat", My.Resources.TakeOwn)
        PictureBox1.BackColor = Color.Transparent
        PictureBox1.Parent = PBPreview
        Dim TakeOwn As New ProcessStartInfo
        TakeOwn.UseShellExecute = True
        TakeOwn.WorkingDirectory = System.Environment.GetEnvironmentVariable("windir")
        TakeOwn.Verb = "runas"
        TakeOwn.WindowStyle = ProcessWindowStyle.Normal
        TakeOwn.FileName = "cmd.exe"
        TakeOwn.Arguments = "/c " & Temp & "/takemyfiles.bat"
        Dim L = Process.Start(TakeOwn)
        Do Until L.HasExited
            System.Threading.Thread.Sleep(100)
        Loop
        If File.Exists(PRIFileSource & ".bak") = False Then
            File.Copy(PRIFileSource, PRIFileSource & ".bak")
            MsgBox("Backup created.")
        End If
        File.Copy(PRIFileSource & ".bak", PRIFile)
        Me.MaximizeBox = False
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
    End Sub

    Private Sub cmdDoMod_Click(sender As Object, e As EventArgs) Handles cmdDoMod.Click
        If File.Exists(tbFilePath.Text) = True Then
            If CheckBox1.Checked = True Then
                ReEncode()
            End If
            File.Delete(PRIFileSource)
            File.Copy(PRIFileSource & ".bak", PRIFileSource)
            File.WriteAllBytes(Temp & "/PS.ps1", My.Resources.CLW_Script)
            Dim PSi As New ProcessStartInfo
            PSi.Arguments = "-ExecutionPolicy Bypass -file " & Temp & "/PS.ps1 " & PRIFile & " " & NewPriFile & " " & Chr(34) & tbFilePath.Text & Chr(34)
            PSi.FileName = "powershell"
            PSi.WindowStyle = ProcessWindowStyle.Hidden
            'PSi.FileName = Temp & "/PS.ps1"
            Dim P As Process = Process.Start(PSi)
            P.WaitForExit()
            If P.ExitCode = 0 = False Then
                MsgBox("A Severe error occured, please check for an update.")
                Application.Exit()
            End If
            'Process.Start("powershell", "-ExecutionPolicy Bypass -file " & Temp & "/PS.ps1 " & PRIFile & " " & NewPriFile & " " & tbFilePath.Text & " " & ComboBox1.SelectedText)
            Do Until File.Exists(NewPriFile)
                Threading.Thread.Sleep(1000)
            Loop
            File.Copy(NewPriFile, PRIFileSource, True)
            MsgBox("Done, Lock your computer to see your new background!")
            'Application.Exit()
        Else
            MsgBox("Please choose a color or an image before you hit this button.")
        End If
    End Sub

    Private Sub cmdBrowse_Click(sender As Object, e As EventArgs) Handles cmdBrowse.Click
        Dim P = OFD.ShowDialog()
        If P = Windows.Forms.DialogResult.OK Then
            Call SelectImage(OFD.FileName)
        End If
    End Sub

    Private Sub SelectImage(ByVal Image As String)
        tbFilePath.Enabled = True
        Try
            If Image.ToUpper.EndsWith(".JPG") And CheckBox1.Checked = False Then
                PBPreview.Load(Image)
                tbFilePath.Text = Image
            ElseIf Image.ToUpper.EndsWith(".PNG") Then
                PBPreview.Load(Image)
                tbFilePath.Text = Image
            Else
                Call ReEncode()
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
            tbFilePath.Text = (Temp & RandNum)
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
        Dim RandNum As String = Rand.Next(0, 100000)
        RandNum += ".jpg"
        RandNum = "\" & RandNum
        Using Bmp As New Bitmap(X, Y, Imaging.PixelFormat.Format32bppPArgb)
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
            For Each Folder In Directory.GetDirectories("C:\Users\Public\AccountPictures")
                Directory.Move(Folder, Folder & "_old")
            Next
        Else
            PictureBox1.Image = My.Resources.login
            For Each Folder In Directory.GetDirectories("C:\Users\Public\AccountPictures")
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
                File.WriteAllBytes(Temp & "\Accent.png", My.Resources.transparent)
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
        File.Copy(PRIFileSource & ".bak", PRIFileSource)
        MsgBox("Done.")
    End Sub
End Class
