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