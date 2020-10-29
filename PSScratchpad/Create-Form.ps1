Clear-Host

function CreateMainForm($title) {
    $form = New-Object System.Windows.Forms.Form
    $form.Text = $title
    $form.Width = 600
    $form.Height = 400
    $form.AutoSize = $true
    $form.StartPosition = [System.Windows.Forms.FormStartPosition]::CenterScreen

    Return $form
}

function CreateLabel($mainForm, $text, $prevLabel) {
    $label = New-Object System.Windows.Forms.Label
    $label.Font = New-Object System.Drawing.Font("Verdana", [single]10, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Point, [byte]0);
    $label.Text = $text
    $label.AutoSize = $true
    $x = 5
    $y = 12
    If ($prevLabel -Ne $null) {
        $y = $prevLabel.Location.Y + 30
    }

    $label.Location = New-Object System.Drawing.Point($x, $y)

    Return $label
}

function CreateTextBox($mainForm, $prevTextBox) {
    $textbox = New-Object System.Windows.Forms.TextBox
    $textbox.Font = New-Object System.Drawing.Font("Verdana", [single]12, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Point, [byte]0);
    $textbox.Size = New-Object System.Drawing.Size(450, 24);
    $textbox.AutoSize = $true
    $x = 90
    $y = 10
    If ($prevTextBox -Ne $null) {
        $y = $prevTextBox.Location.Y + 30
    }

    $textbox.Location = New-Object System.Drawing.Point($x, $y)

    Return $textbox
}

function CreateButton($mainForm, $text, $action, $prevTextBox) {
    $button = New-Object System.Windows.Forms.Button
    $button.Font = New-Object System.Drawing.Font("Verdana", [single]10, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Point, [byte]0);
    $button.Text = $text
    $button.Size = New-Object System.Drawing.Size(150, 24);
    $button.AutoSize = $true
    $x = 90
    $y = 10
    If ($prevTextBox -Ne $null) {
        $y = $prevTextBox.Location.Y + 30
    }

    $button.Location = New-Object System.Drawing.Point($x, $y)
    $button.Add_Click($action)

    Return $button
}

function CreateSecondButton($mainForm, $text, $action, $prevButton) {
    $button = New-Object System.Windows.Forms.Button
    $button.Font = New-Object System.Drawing.Font("Verdana", [single]10, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Point, [byte]0);
    $button.Text = $text
    $button.Size = New-Object System.Drawing.Size(150, 24);
    $button.AutoSize = $true
    $x = 90
    $y = 10
    If ($prevButton -Ne $null) {
        $x = $prevButton.Location.X + $prevButton.Width + 5
        $y = $prevButton.Location.Y
    }

    $button.Location = New-Object System.Drawing.Point($x, $y)
    $button.Add_Click($action)

    Return $button
}

function IntializeComponent() {
    $Script:mainF = CreateMainForm "App"

    $Script:lblName = CreateLabel $mainF "Name"
    $Script:txtName = CreateTextBox $mainF

    $Script:lblIdNo = CreateLabel $mainF "Id No." $lblName
    $Script:txtIdNo = CreateTextBox $mainF $txtName

    $Script:btnAlert = CreateButton $mainF "Alert" ${function:Alert} $txtIdNo
    $Script:btnNextAlert = CreateSecondButton $mainF "Next Alert" ${function:NextAlert} $btnAlert

    $Script:mainF.Controls.Add($lblName)
    $Script:mainF.Controls.Add($txtName)

    $Script:mainF.Controls.Add($lblIdNo)
    $Script:mainF.Controls.Add($txtIdNo)

    $Script:mainF.Controls.Add($btnAlert)
    $Script:mainF.Controls.Add($btnNextAlert)
}

function Alert($source, $args) {
    [System.Windows.Forms.MessageBox]::Show("Yuuhuueo")
}

function NextAlert($source, $args) {
    [System.Windows.Forms.MessageBox]::Show("Heyy Hoo")
}

Try {
    Add-Type -Assembly System.Drawing
    Add-Type -Assembly System.Windows.Forms

    IntializeComponent

    $mainF.ShowDialog()
}
Catch {
    Log $("Exception thrown: `n" + `
        $_.Exception.ItemName + " `n" + `
        $_.Exception.Message + " `n" + `
        $_.Exception.StackTrace)
}
