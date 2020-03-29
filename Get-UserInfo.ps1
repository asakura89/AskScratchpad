Clear-Host

@{
    User = $Env:Username
    UserProfile = $Env:UserProfile
    AppDataRoaming = $Env:AppData
    AppDataLocal = $Env:LocalAppData
} | 
Format-List
