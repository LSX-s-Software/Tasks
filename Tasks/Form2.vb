Public Class Form2
    Dim speed As Byte
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim path As Drawing2D.GraphicsPath = Dialog1.RoundedRectPath(ClientRectangle, 20)
        Region = New Region(path)
        Dim loc As Point
        loc.X = Screen.PrimaryScreen.WorkingArea.Width - Width - 10
        loc.Y = Screen.PrimaryScreen.WorkingArea.Height - Height - 40
        Location = loc
        BackColor = My.Settings.ThemeColor
        Label1.ForeColor = My.Settings.TForeColor
        Label2.ForeColor = My.Settings.TForeColor
        Button1.BackColor = My.Settings.ThemeColor
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Enabled = True
        Form1.Timer1.Enabled = True
        Form1.Timer3.Enabled = True
        AnimateOut()
        Dispose()
    End Sub

    Public Sub AnimateOut()
        Do Until Left > Screen.PrimaryScreen.WorkingArea.Width
            Select Case Screen.PrimaryScreen.WorkingArea.Width - Left
                Case < 50
                    speed = 1
                Case > 100
                    speed = 3
                Case > 150
                    speed = 5
                Case > 170
                    speed = 1
            End Select
            Left = Left + speed
            Threading.Thread.Sleep(1)
        Loop
    End Sub
End Class