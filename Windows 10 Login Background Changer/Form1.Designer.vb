<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cmdDoMod = New System.Windows.Forms.Button()
        Me.tbFilePath = New System.Windows.Forms.TextBox()
        Me.OFD = New System.Windows.Forms.OpenFileDialog()
        Me.cmdBrowse = New System.Windows.Forms.Button()
        Me.PBPreview = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ColorD = New System.Windows.Forms.ColorDialog()
        Me.cmdSolidColor = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdCredits = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.chkUserIcon = New System.Windows.Forms.CheckBox()
        Me.chkNoBackground = New System.Windows.Forms.CheckBox()
        Me.cmdRevert = New System.Windows.Forms.Button()
        CType(Me.PBPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdDoMod
        '
        Me.cmdDoMod.Location = New System.Drawing.Point(411, 394)
        Me.cmdDoMod.Name = "cmdDoMod"
        Me.cmdDoMod.Size = New System.Drawing.Size(159, 23)
        Me.cmdDoMod.TabIndex = 0
        Me.cmdDoMod.Text = "Change Background"
        Me.cmdDoMod.UseVisualStyleBackColor = True
        '
        'tbFilePath
        '
        Me.tbFilePath.Location = New System.Drawing.Point(12, 368)
        Me.tbFilePath.Name = "tbFilePath"
        Me.tbFilePath.Size = New System.Drawing.Size(393, 20)
        Me.tbFilePath.TabIndex = 1
        Me.tbFilePath.Text = "The path to the image will appear here."
        '
        'OFD
        '
        Me.OFD.Filter = "JPG Files|*.jpg|BMP Files|*.bmp|PNG Files|*.png|TIFF Files|*.tiff|GIF Files (No A" & _
    "nimation)|*.gif|All Files|*.*"
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Location = New System.Drawing.Point(44, 394)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(169, 23)
        Me.cmdBrowse.TabIndex = 2
        Me.cmdBrowse.Text = "Browse for an Image"
        Me.cmdBrowse.UseVisualStyleBackColor = True
        '
        'PBPreview
        '
        Me.PBPreview.Location = New System.Drawing.Point(0, 0)
        Me.PBPreview.Name = "PBPreview"
        Me.PBPreview.Size = New System.Drawing.Size(579, 360)
        Me.PBPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PBPreview.TabIndex = 3
        Me.PBPreview.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = Global.Windows_10_Login_Background_Changer.My.Resources.Resources.login
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(579, 360)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'cmdSolidColor
        '
        Me.cmdSolidColor.Location = New System.Drawing.Point(260, 394)
        Me.cmdSolidColor.Name = "cmdSolidColor"
        Me.cmdSolidColor.Size = New System.Drawing.Size(107, 23)
        Me.cmdSolidColor.TabIndex = 5
        Me.cmdSolidColor.Text = "Choose a Color"
        Me.cmdSolidColor.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(223, 399)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(18, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Or"
        '
        'cmdCredits
        '
        Me.cmdCredits.Location = New System.Drawing.Point(411, 366)
        Me.cmdCredits.Name = "cmdCredits"
        Me.cmdCredits.Size = New System.Drawing.Size(159, 23)
        Me.cmdCredits.TabIndex = 7
        Me.cmdCredits.Text = "Credits"
        Me.cmdCredits.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 399)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "First"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(373, 399)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Then"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(15, 428)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(112, 17)
        Me.CheckBox1.TabIndex = 10
        Me.CheckBox1.Text = "Re-Encode Image"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'chkUserIcon
        '
        Me.chkUserIcon.AutoSize = True
        Me.chkUserIcon.Checked = True
        Me.chkUserIcon.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUserIcon.Location = New System.Drawing.Point(133, 428)
        Me.chkUserIcon.Name = "chkUserIcon"
        Me.chkUserIcon.Size = New System.Drawing.Size(108, 17)
        Me.chkUserIcon.TabIndex = 11
        Me.chkUserIcon.Text = "Show User Icon?"
        Me.chkUserIcon.UseVisualStyleBackColor = True
        '
        'chkNoBackground
        '
        Me.chkNoBackground.AutoSize = True
        Me.chkNoBackground.Location = New System.Drawing.Point(250, 430)
        Me.chkNoBackground.Name = "chkNoBackground"
        Me.chkNoBackground.Size = New System.Drawing.Size(123, 17)
        Me.chkNoBackground.TabIndex = 12
        Me.chkNoBackground.Text = "Accent Theme Color"
        Me.chkNoBackground.UseVisualStyleBackColor = True
        '
        'cmdRevert
        '
        Me.cmdRevert.Location = New System.Drawing.Point(411, 422)
        Me.cmdRevert.Name = "cmdRevert"
        Me.cmdRevert.Size = New System.Drawing.Size(159, 23)
        Me.cmdRevert.TabIndex = 13
        Me.cmdRevert.Text = "Revert to Original Background"
        Me.cmdRevert.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(578, 457)
        Me.Controls.Add(Me.cmdRevert)
        Me.Controls.Add(Me.chkNoBackground)
        Me.Controls.Add(Me.chkUserIcon)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmdCredits)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdSolidColor)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.PBPreview)
        Me.Controls.Add(Me.cmdBrowse)
        Me.Controls.Add(Me.tbFilePath)
        Me.Controls.Add(Me.cmdDoMod)
        Me.Name = "Form1"
        Me.Text = "Windows 10 Login Background Changer"
        CType(Me.PBPreview, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdDoMod As System.Windows.Forms.Button
    Friend WithEvents tbFilePath As System.Windows.Forms.TextBox
    Friend WithEvents OFD As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cmdBrowse As System.Windows.Forms.Button
    Friend WithEvents PBPreview As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ColorD As System.Windows.Forms.ColorDialog
    Friend WithEvents cmdSolidColor As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdCredits As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkUserIcon As System.Windows.Forms.CheckBox
    Friend WithEvents chkNoBackground As System.Windows.Forms.CheckBox
    Friend WithEvents cmdRevert As System.Windows.Forms.Button

End Class
