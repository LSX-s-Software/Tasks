Public Class Form3
    Dim originXY As Point

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Region = New Region(FX.RoundedRectPath(ClientRectangle, 20)) '圆角
        FX.SetClassLong(Handle, FX.GCL_STYLE, FX.GetClassLong(Handle, FX.GCL_STYLE) Or FX.CS_DROPSHADOW) '阴影
        Dim loc As Point
        loc.X = Screen.PrimaryScreen.WorkingArea.Width - Width - 10
        loc.Y = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2
        Location = loc
        Label1.ForeColor = My.Settings.TForeColor
        Label2.ForeColor = My.Settings.TForeColor
        Button1.BackColor = BackColor
        Form2.PlaySound(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\media\Windows Notify Calendar.wav", &H10000, &H1)
    End Sub

    Private Sub Dialog1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If e.Button = MouseButtons.Left Then
            Location = PointToScreen(e.Location) - originXY
        End If
    End Sub

    Private Sub Dialog1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        originXY = e.Location
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FX.AnimateOut(Me)
        Close()
    End Sub
End Class