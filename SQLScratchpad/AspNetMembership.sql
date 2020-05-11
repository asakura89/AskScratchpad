--<< Get User Account >>-
SELECT
us.[UserName],
mem.[Email],
mem.[Password],
mem.[PasswordSalt]
FROM [dbo].[aspnet_Users] us
JOIN [dbo].[aspnet_Membership] mem
ON us.UserId = mem.UserId



--<< User Role >>--
;
WITH AspUser AS (
    SELECT
    us.ApplicationId AppId,
    app.ApplicationName AppName,
    us.UserId,
    us.UserName,
    mem.Email
    FROM dbo.aspnet_Users us
    JOIN dbo.aspnet_Applications app
    ON us.ApplicationId = app.ApplicationId
    JOIN dbo.aspnet_Membership mem
    ON us.UserId = mem.UserId
),
AspRole AS (
    SELECT
    usr.UserId,
    r.RoleId,
    r.RoleName
    FROM dbo.aspnet_UsersInRoles usr
    JOIN dbo.aspnet_Roles r
    ON usr.RoleId = r.RoleId
)
SELECT
au.AppName,
au.UserName,
au.Email,
ar.RoleName
FROM AspUser au
LEFT JOIN AspRole ar
ON au.UserId = ar.UserId



--<< Expired Password Reset >>--
BEGIN
    SET NOCOUNT ON
    BEGIN TRAN ResetPwd
    BEGIN TRY
        DECLARE @message VARCHAR(MAX)

        UPDATE dbo.aspnet_Membership SET
        IsApproved = '1',
        IsLockedOut = '0',
        LastLoginDate = DATEADD(DAY, -2, GETDATE()),
        LastPasswordChangedDate = DATEADD(DAY, -2, GETDATE())
        WHERE UserId IN (
            SELECT UserId
            FROM dbo.aspnet_Users
            WHERE UserName IN (
                'user_1',
                'user_2',
                'user_3',
                'user_4'
            )
        )
        COMMIT TRAN ResetPwd
        SET @message = 'S|Finish'
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN ResetPwd
        SET @message = 'E|' + CAST(ERROR_LINE() AS VARCHAR) + ': ' + ERROR_MESSAGE()
    END CATCH
                
    SET NOCOUNT OFF
    SELECT @message [Message]
END



--<< Search User with Id contains part of guid >>--
SELECT *
FROM dbo.aspnet_Users
WHERE LOWER(CONVERT(CHAR(36), UserId)) LIKE '4e18856f%'



--<< Check non-existent user from list >>--
;
WITH SuspectedUserList AS (
    SELECT * FROM (
        VALUES 
            ('MembershipUserTest01'),
            ('MembershipUserTest02'),
            ('MembershipUserTest03'),
            ('MembershipUserTest04'),
            ('MembershipUserTest05')
    ) UsernameList(Username)
),
UserList AS (
    SELECT
    lu.Username,
    u.ApplicationId U_AppId,
    u.[UserId] U_Id,
    u.[UserName] U_name
    FROM SuspectedUserList lu
    LEFT JOIN [dbo].[aspnet_Users] u
    ON UPPER(u.UserName) = UPPER(lu.Username)
)
SELECT * FROM UserList
WHERE U_Id IS NULL
ORDER BY Username


--<< Read User Profile >>--


DECLARE
    @Prop1 VARCHAR(50) = 'Location',
    @Prop2 VARCHAR(50) = 'Timezone',
    @Prop3 VARCHAR(50) = 'EnableHistory'

;
WITH UserInfo AS (
    SELECT
    m.ApplicationId, m.UserId,
    u.UserName, m.Email
    FROM dbo.aspnet_Membership m
    JOIN dbo.aspnet_Users u
    ON m.ApplicationId = u.ApplicationId
    AND m.UserId = u.UserId
),
UserRole AS (
    SELECT u.UserId, r.RoleId, r.RoleName
    FROM UserInfo u
    JOIN dbo.aspnet_UsersInRoles uir ON u.UserId = uir.UserId
    JOIN dbo.aspnet_Roles r ON uir.RoleId = r.RoleId
),
UserProfile AS (
    SELECT
    u.*, ur.RoleId, ur.RoleName,
    p.PropertyNames, p.PropertyValuesString,
    p.PropertyValuesBinary
    FROM dbo.aspnet_Profile p
    JOIN UserInfo u ON p.UserId = u.UserId
    JOIN UserRole ur ON p.UserId = ur.UserId
),
NameValue AS (
    SELECT
    UserId,
    ':' + CAST(PropertyNames AS VARCHAR(8000)) Names,
    PropertyValuesString [Values]
    FROM UserProfile
),
PropPos AS (
    SELECT
    UserId,
    PATINDEX('%:'+ @Prop1 +':S%', Names) Prop1Pos,
    CASE WHEN (PATINDEX('%:'+ @Prop1 +':S%', Names)) > 0
        THEN 1
        ELSE 0
    END Prop1Exist,
    PATINDEX('%:'+ @Prop2 +':S%', Names) Prop2Pos,
    CASE WHEN (PATINDEX('%:'+ @Prop2 +':S%', Names)) > 0
        THEN 1
        ELSE 0
    END Prop2Exist,
    PATINDEX('%:'+ @Prop3 +':S%', Names) Prop3Pos,
    CASE WHEN (PATINDEX('%:'+ @Prop3 +':S%', Names)) > 0
        THEN 1
        ELSE 0
    END Prop3Exist
    FROM NameValue
),
PropName AS (
    SELECT
    nv.UserId,
    CASE WHEN pp.Prop1Exist =  0
        THEN ''
        ELSE
            TRIM(':' FROM SUBSTRING(nv.Names, (SELECT pp.Prop1Pos + LEN(@Prop1) + 3), 1)) +
            SUBSTRING(nv.Names, (SELECT pp.Prop1Pos + LEN(@Prop1) + 4), LEN(nv.Names))
    END Prop1Name,
    CASE WHEN pp.Prop2Exist =  0
        THEN ''
        ELSE
            TRIM(':' FROM SUBSTRING(nv.Names, (SELECT pp.Prop2Pos + LEN(@Prop2) + 3), 1)) +
            SUBSTRING(nv.Names, (SELECT pp.Prop2Pos + LEN(@Prop2) + 4), LEN(nv.Names))
    END Prop2Name,
    CASE WHEN pp.Prop3Exist =  0
        THEN ''
        ELSE
            TRIM(':' FROM SUBSTRING(nv.Names, (SELECT pp.Prop3Pos + LEN(@Prop3) + 3), 1)) +
            SUBSTRING(nv.Names, (SELECT pp.Prop3Pos + LEN(@Prop3) + 4), LEN(nv.Names))
    END Prop3Name
    FROM NameValue nv
    JOIN PropPos pp ON nv.UserId = pp.UserId
),
PropValuePos AS (
    SELECT
    UserId,

    CASE WHEN RTRIM(LTRIM(Prop1Name)) = ''
        THEN 0
        ELSE CAST(
            SUBSTRING(Prop1Name, 1, CHARINDEX(':', Prop1Name) - 1)
        AS TINYINT) + 1
    END Prop1Start,

    CASE WHEN RTRIM(LTRIM(Prop1Name)) = ''
        THEN 0
        ELSE CAST(
            SUBSTRING(Prop1Name, CHARINDEX(':', Prop1Name) + 1, 
                ((CHARINDEX(':', Prop1Name, CHARINDEX(':', Prop1Name) + 1)) - (CHARINDEX(':', Prop1Name) + 1)))
        AS TINYINT)
    END Prop1Length,

    CASE WHEN RTRIM(LTRIM(Prop2Name)) = ''
        THEN 0
        ELSE CAST(
            SUBSTRING(Prop2Name, 1, CHARINDEX(':', Prop2Name) - 1)
        AS TINYINT) + 1
    END Prop2Start,

    CASE WHEN RTRIM(LTRIM(Prop2Name)) = ''
        THEN 0
        ELSE CAST(
            SUBSTRING(Prop2Name, CHARINDEX(':', Prop2Name) + 1, 
                ((CHARINDEX(':', Prop2Name, CHARINDEX(':', Prop2Name) + 1)) - (CHARINDEX(':', Prop2Name) + 1)))
        AS TINYINT)
    END Prop2Length,

    CASE WHEN RTRIM(LTRIM(Prop3Name)) = ''
        THEN 0
        ELSE CAST(
            SUBSTRING(Prop3Name, 1, CHARINDEX(':', Prop3Name) - 1)
        AS TINYINT) + 1
    END Prop3Start,

    CASE WHEN RTRIM(LTRIM(Prop3Name)) = ''
        THEN 0
        ELSE CAST(
            SUBSTRING(Prop3Name, CHARINDEX(':', Prop3Name) + 1, 
                ((CHARINDEX(':', Prop3Name, CHARINDEX(':', Prop3Name) + 1)) - (CHARINDEX(':', Prop3Name) + 1)))
        AS TINYINT)
    END Prop3Length
    FROM PropName
),
PropValue As (
    SELECT
    nv.UserId,
    SUBSTRING(nv.[Values], pvp.Prop1Start, pvp.Prop1Length) Prop1Value,
    SUBSTRING(nv.[Values], pvp.Prop2Start, pvp.Prop2Length) Prop2Value,
    SUBSTRING(nv.[Values], pvp.Prop3Start, pvp.Prop3Length) Prop3Value
    FROM NameValue nv
    JOIN PropValuePos pvp ON nv.UserId = pvp.UserId
),
ProfileValue AS (
    SELECT
    pv.UserId,
    CASE WHEN LEN(pv.Prop1Value) = 0
        THEN '-'
        ELSE pv.Prop1Value
    END [Location],
    CASE WHEN LEN(pv.Prop2Value) = 0
        THEN '-'
        ELSE pv.Prop2Value
    END Timezone,
    CASE WHEN LEN(pv.Prop3Value) = 0
        THEN '-'
        ELSE pv.Prop3Value
    END EnableHistory
    FROM PropValue pv
)
SELECT
u.UserName,
u.Email,
u.RoleName,
pv.[Location],
pv.Timezone,
pv.EnableHistory
FROM UserProfile u
JOIN ProfileValue pv ON u.UserId = pv.UserId

