Public Class Dialog2

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles OK_Button.Click
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub Dialog2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = My.Settings.ThemeColor
        ForeColor = My.Settings.TForeColor
        Show()
        With Form1
            If Form1.SelectedView Is .ListView1 Then
                Label1.Text = "更改您的任务内容"
                TextBox1.Text = .ListView1.SelectedItems(0).Text
                DateTimePicker1.Value = .ListView1.SelectedItems(0).SubItems(1).Text
                DateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss"
                TextBox2.Text = .ListView1.SelectedItems(0).SubItems(2).Text
                ComboBox1.Enabled = False
            Else
                Label1.Text = "更改您的提醒事项"
                TextBox1.Text = .ListView2.SelectedItems(0).SubItems(1).Text
                DateTimePicker1.Value = .ListView2.SelectedItems(0).Text
                DateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss"
                TextBox2.Text = .ListView2.SelectedItems(0).SubItems(2).Text
                ComboBox1.Enabled = True
                ComboBox1.SelectedItem = .ListView2.SelectedItems(0).SubItems(4).Text
            End If
        End With
    End Sub
End Class
