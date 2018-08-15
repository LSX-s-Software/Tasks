Public Class Form2
    Public Declare Auto Function PlaySound Lib "winmm.dll" (ByVal lpszSoundName As String, ByVal hModule As Integer, ByVal dwFlags As Integer) As Integer
    Dim speed As Byte
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load
        Region = New Region(FX.RoundedRectPath(ClientRectangle, 20)) '圆角
        FX.SetClassLong(Handle, FX.GCL_STYLE, FX.GetClassLong(Handle, FX.GCL_STYLE) Or FX.CS_DROPSHADOW) '阴影
        Dim loc As Point
        loc.X = Screen.PrimaryScreen.WorkingArea.Width - Width - 10
        loc.Y = Screen.PrimaryScreen.WorkingArea.Height - Height - 40
        Location = loc
        BackColor = My.Settings.ThemeColor
        Label1.ForeColor = My.Settings.TForeColor
        Label2.ForeColor = My.Settings.TForeColor
        Button1.BackColor = My.Settings.ThemeColor
        PlaySound(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\media\Windows Notify Calendar.wav", &H10000, &H1)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Enabled = True
        Form1.Timer1.Enabled = True
        Form1.Timer3.Enabled = True
        AnimateOut()
        If My.Settings.FirstRun Then
            If Label1.Text.Contains("任务") Then
                Form1.ListView1.BackgroundImage = Form1.PicList(4)
                Form1.ListView2.BackgroundImage = Form1.PicList(5)
            Else
                Form1.ListView1.BackgroundImage = Nothing
                Form1.ListView2.BackgroundImage = Form1.PicList(6)
                Form1.Button1.Visible = True
                Form1.Button1.BringToFront()
            End If
        End If
        Dispose()
    End Sub

    Public Sub AnimateOut()
        Do Until Left > Screen.PrimaryScreen.WorkingArea.Width
            Select Case Screen.PrimaryScreen.WorkingArea.Width - Left
                Case < 50
                    speed = 1
                Case > 100
                    speed = 5
                Case > 150
                    speed = 10
                Case > 170
                    speed = 1
            End Select
            Left = Left + speed
            Threading.Thread.Sleep(2)
        Loop
    End Sub
End Class