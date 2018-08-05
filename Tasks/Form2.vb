Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim path As Drawing2D.GraphicsPath = RoundedRectPath(ClientRectangle, 30)
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

    Private Function RoundedRectPath(ByVal Rectangle As Rectangle, ByVal r As Integer) As Drawing2D.GraphicsPath
        Rectangle.Offset(-1, -1)
        Dim RoundRect As New Rectangle(Rectangle.Location, New Size(r - 1, r - 1))
        Dim path As New Drawing2D.GraphicsPath
        path.AddArc(RoundRect, 180, 90)
        RoundRect.X = Rectangle.Right - r
        path.AddArc(RoundRect, 270, 90)
        RoundRect.Y = Rectangle.Bottom - r
        path.AddArc(RoundRect, 0, 90)
        RoundRect.X = Rectangle.Left
        path.AddArc(RoundRect, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Enabled = True
        AnimateOut()
    End Sub

    Public Sub AnimateOut()
        Do Until Left >= Screen.PrimaryScreen.WorkingArea.Width
            Left = Left + 2
            Threading.Thread.Sleep(1)
        Loop
        Dispose()
    End Sub
End Class