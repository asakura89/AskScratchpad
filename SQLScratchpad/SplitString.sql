
DECLARE
    @dataString NVARCHAR(MAX),
    @separator CHAR(1) = '|'

DECLARE @SplittedString TABLE 
(
    Idx INT IDENTITY(1,1),
    Splitted NVARCHAR(MAX)
)


SELECT
    @dataString = N'18th Century Green,1975 Earth Red,1989 Miami Hotline,20000 Leagues Under the Sea,3AM in Shibuya',
    @separator= ','


BEGIN
    DECLARE @counter INT = 1
    WHILE(CHARINDEX(@separator, @dataString) > 0)
    BEGIN
        INSERT INTO @SplittedString VALUES (LTRIM(RTRIM(SUBSTRING(@dataString, 1, CHARINDEX(@separator, @dataString) - 1))))
        SELECT
            @dataString = SUBSTRING(@dataString, CHARINDEX(@separator, @dataString) + 1, LEN(@dataString)),
            @counter = @counter + 1
    END
    INSERT INTO @SplittedString VALUES (LTRIM(RTRIM(@dataString)))
    
    SELECT * FROM @SplittedString
END


/*
CREATE FUNCTION dbo.SplitString
(
    @dataString NVARCHAR(MAX),
    @separator CHAR(1) = '|'
)
RETURNS @SplittedString TABLE 
(
    Idx INT IDENTITY(1,1),
    Splitted NVARCHAR(MAX)
)
AS
BEGIN
    DECLARE @counter INT = 1
    WHILE(CHARINDEX(@separator, @dataString) > 0)
    BEGIN
        INSERT INTO @SplittedString VALUES (LTRIM(RTRIM(SUBSTRING(@dataString, 1, CHARINDEX(@separator, @dataString) - 1))))
        SELECT
            @dataString = SUBSTRING(@dataString, CHARINDEX(@separator, @dataString) + 1, LEN(@dataString)),
            @counter = @counter + 1
    END
    INSERT INTO @SplittedString VALUES (LTRIM(RTRIM(@dataString)))
    RETURN
END
*/