Public Class Form2
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
        Do Until Left >= Screen.PrimaryScreen.WorkingArea.Width
            Left = Left + 2
            Threading.Thread.Sleep(1)
        Loop
    End Sub
End Class