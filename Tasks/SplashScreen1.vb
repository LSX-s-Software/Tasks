Public NotInheritable Class SplashScreen1
    Private Sub SplashScreen1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Command() = "-h") AndAlso My.Settings.HideWhenAutoRun Then
            Hide()
        Else
            Show()
            Refresh()
        End If
        ProgressBar1.Value = 0
        Label1.Text = "加载主题"
        Region = New Region(FX.RoundedRectPath(ClientRectangle, 20)) '圆角
        FX.SetClassLong(Handle, FX.GCL_STYLE, FX.GetClassLong(Handle, FX.GCL_STYLE) Or FX.CS_DROPSHADOW) '阴影
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
        Version.Text = String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)
        'Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)
        '版权信息
        Copyright.Text = My.Application.Info.Copyright
        ProgressBar1.Value = 15
        Refresh()
        '----------加载存档-------
        Label1.Text = "加载存档"
        Form1.Hide()
        If Dir(Application.StartupPath & "\Saves.tasks") <> "" Then
            Dim rawfile, file() As String
            rawfile = FileIO.FileSystem.ReadAllText(Application.StartupPath & "\Saves.tasks")
            file = rawfile.Split("|")
            Dim i = 0
            With Form1.ListView1
                .BeginUpdate()
                Do While file(i) <> "END OF LIST1"
                    .Items.Add(file(i))
                    With .Items(.Items.Count - 1).SubItems
                        .Add(file(i + 1))
                        .Add(file(i + 2))
                        .Add(file(i + 3))
                    End With
                    .Items(.Items.Count - 1).ImageIndex = 0
                    i = i + 4
                Loop
                .EndUpdate()
            End With
            i = i + 1
            With Form1.ListView2
                .BeginUpdate()
                Do While file(i) <> "END OF FILE"
                    .Items.Add(file(i))
                    With .Items(.Items.Count - 1).SubItems
                        .Add(file(i + 1))
                        .Add(file(i + 2))
                        .Add(file(i + 3))
                        .Add(file(i + 4))
                        .Add(file(i + 5))
                    End With
                    i = i + 6
                Loop
                .EndUpdate()
            End With
        End If
        If My.Settings.FirstRun Then
            Label1.Text = "正在为第一次启动做准备..."
            Form1.PicList = {My.Resources.教程1, My.Resources.教程2, My.Resources.教程3, My.Resources.教程4, My.Resources.教程5, My.Resources.教程6, My.Resources.教程7}
        End If
        ProgressBar1.Value = 80
        Refresh()
        '----------加载窗体-------
        Label1.Text = "加载窗体"
        Dialog1.Hide()
        ProgressBar1.Value = 85
        Dialog2.Hide()
        ProgressBar1.Value = 90
        Settings.Hide()
        '----------加载完毕-------
        Label1.Text = "加载完毕"
        Refresh()
        ProgressBar1.Value = 100
        'Threading.Thread.Sleep(1000)
        'Animate()
        If Not (Command() = "-h" AndAlso My.Settings.HideWhenAutoRun) Then
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
