Clear-Host

@{
    User = $Env:Username
    UserProfile = $Env:UserProfile
    AppDataRoaming = $Env:AppData
    AppDataRoaming2 = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::ApplicationData)
    AppDataRoaming3 = [System.Environment]::GetFolderPath("ApplicationData")
    AppDataLocal = $Env:LocalAppData
    AppDataLocal2 = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::LocalApplicationData)
    AppDataLocal3 = [System.Environment]::GetFolderPath("LocalApplicationData")
} | 
Format-List
