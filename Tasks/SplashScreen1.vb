Public NotInheritable Class SplashScreen1
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

    Private Sub SplashScreen1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Command() = "-h" Then
            Hide()
        Else
            Show()
        End If
        ProgressBar1.Value = 0
        Label1.Text = "加载主题"
        Dim path As Drawing2D.GraphicsPath = RoundedRectPath(ClientRectangle, 30)
        Region = New Region(path)
        BackColor = My.Settings.ThemeColor
        ForeColor = My.Settings.TForeColor
        Label1.BackColor = Color.Transparent
        ProgressBar1.Value = 5
        Label1.Text = "加载应用程序信息"
        '根据应用程序的程序集信息在运行时设置对话框文本。  
        If My.Application.Info.Title <> "" Then
            ApplicationTitle.Text = My.Application.Info.Title
        Else
            '若应用程序标题丢失，则使用不带扩展名的应用程序名
            ApplicationTitle.Text = IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        '使用在设计时作为格式字符串设置到 Version 控件中的文本格式化版本信息。
        '  以便根据需要进行有效的本地化。
        '  使用以下代码，将 Version 控件的设计时文本 
        '  更改为“Version {0}.{1:00}.{2}.{3}”或类似格式，将内部版本和修订信息包括在内。
        '  有关更多信息，请参阅帮助中的 String.Format()。
        '
        '    Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)
        Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)
        '版权信息
        Copyright.Text = My.Application.Info.Copyright
        ProgressBar1.Value = 15
        '----------加载存档-------
        Label1.Text = "加载存档"
        Form1.Hide()
        If Dir(Application.StartupPath & "\Saves.tasks") <> "" Then
            Dim rawfile, file() As String
            rawfile = FileIO.FileSystem.ReadAllText(Application.StartupPath & "\Saves.tasks")
            file = rawfile.Split("|")
            Dim i = 0
            With Form1.ListView1
                Do While file(i) <> "END OF LIST1"
                    .Items.Add(file(i))
                    .Items(.Items.Count - 1).SubItems.Add(file(i + 1))
                    .Items(.Items.Count - 1).SubItems.Add(file(i + 2))
                    .Items(.Items.Count - 1).SubItems.Add(file(i + 3))
                    i = i + 4
                Loop
            End With
            i = i + 1
            With Form1.ListView2
                Do While file(i) <> "END OF FILE"
                    .Items.Add(file(i))
                    .Items(.Items.Count - 1).SubItems.Add(file(i + 1))
                    .Items(.Items.Count - 1).SubItems.Add(file(i + 2))
                    .Items(.Items.Count - 1).SubItems.Add(file(i + 3))
                    .Items(.Items.Count - 1).SubItems.Add(file(i + 4))
                    .Items(.Items.Count - 1).SubItems.Add(file(i + 5))
                    i = i + 6
                Loop
            End With
        End If
        ProgressBar1.Value = 80
        '----------加载窗体-------
        Label1.Text = "加载窗体"
        Dialog1.Hide()
        ProgressBar1.Value = 85
        Dialog2.Hide()
        ProgressBar1.Value = 90
        Settings.Hide()
        '----------加载完毕-------
        ProgressBar1.Value = 100
        'Animate()
        If Command() <> "-h" Then
            Form1.Show()
        End If
        Close()
    End Sub

    Private Sub Animate()
        Dim h = Height
        Do Until Height <= h - ProgressBar1.Height
            Height = Height - 1
            Threading.Thread.Sleep(5)
        Loop
    End Sub
End Class
