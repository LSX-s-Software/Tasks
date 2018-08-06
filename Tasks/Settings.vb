Public Class Settings
    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim path As Drawing2D.GraphicsPath = RoundedRectPath(ClientRectangle, 30)
        Region = New Region(path)
        BackColor = My.Settings.ThemeColor
        ForeColor = My.Settings.TForeColor
        Label.ForeColor = My.Settings.TForeColor
        Save_Button.BackColor = My.Settings.ThemeColor
        Save_Button.ForeColor = My.Settings.TForeColor
        Cancel_Button.BackColor = My.Settings.ThemeColor
        Cancel_Button.ForeColor = My.Settings.TForeColor
        Button1.BackColor = My.Settings.ThemeColor
        Button1.ForeColor = My.Settings.TForeColor
        PictureBox2.BackColor = My.Settings.ThemeColor
        PictureBox3.BackColor = My.Settings.TForeColor
        TrackBar1.Value = My.Settings.NoticeLevel
        TrackBar2.Value = My.Settings.WarningLevel
        ComboBox1.SelectedItem = My.Settings.RemindInterval

        DrawProgressBar()
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

    Private Sub DrawProgressBar()
        Dim bmp As Bitmap
        Dim gra As Graphics
        Dim ThemeColor As Color = My.Settings.ThemeColor
        Dim b As SolidBrush = New SolidBrush(ThemeColor) '普通的颜色
        Dim b1 As SolidBrush = New SolidBrush(Color.Orange) '注意的颜色
        Dim b2 As SolidBrush = New SolidBrush(Color.FromArgb(255, 0, 0)) '警告的颜色
        Dim NoticeLevel As Integer = My.Settings.NoticeLevel
        Dim WarningLevel As Integer = My.Settings.WarningLevel
        bmp = New Bitmap(PictureBox1.Width, PictureBox1.Height)
        gra = Graphics.FromImage(bmp)
        gra.FillRectangle(b, 0, 0, PictureBox1.Width, PictureBox1.Height)
        Dim NoticeStart As Integer = PictureBox1.Width * TrackBar1.Value / 100
        gra.FillRectangle(b1, NoticeStart, 0, PictureBox1.Width - NoticeStart, PictureBox1.Height)
        Dim WarningStart As Integer = PictureBox1.Width * TrackBar2.Value / 100
        gra.FillRectangle(b2, WarningStart, 0, PictureBox1.Width - WarningStart, PictureBox1.Height)
        PictureBox1.Image = bmp
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Dispose()
    End Sub

    Private Sub Save_Button_Click(sender As Object, e As EventArgs) Handles Save_Button.Click
        My.Settings.NoticeLevel = TrackBar1.Value
        My.Settings.WarningLevel = TrackBar2.Value
        If PictureBox2.BackColor <> PictureBox3.BackColor Then
            My.Settings.ThemeColor = PictureBox2.BackColor
            My.Settings.TForeColor = PictureBox3.BackColor
        Else
            MsgBox("主题色不能与字体颜色相同，请重新选择", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        My.Settings.RemindInterval = ComboBox1.Text
        My.Settings.Save()
        Close()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        DrawProgressBar()
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        DrawProgressBar()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Settings.Reset()
        Close()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Dim r As DialogResult
        r = ColorDialog1.ShowDialog()
        If r = DialogResult.OK Then
            PictureBox2.BackColor = ColorDialog1.Color
        End If
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Dim r As DialogResult
        r = ColorDialog1.ShowDialog()
        If r = DialogResult.OK Then
            PictureBox3.BackColor = ColorDialog1.Color
        End If
    End Sub
End Class