
/*

-- Create this table first

CREATE TABLE CommonKey
(
    DataName NVARCHAR(50),
    Seq INT,
    KeyType NVARCHAR(2),
    [Value] NVARCHAR(50),
    [Length] INT,
    BackSeparator NVARCHAR(3),
    CONSTRAINT pk_common_key PRIMARY KEY (DataName, Seq)
)
GO

INSERT INTO CommonKey VALUES
    ('TEST', 1, 'ST', 'DOC', 3, ''),
    ('TEST', 2, 'ST', 'UMENTATION', 5, '/'),
    ('TEST', 3, 'SY', '', 0, '/'),
    ('TEST', 4, 'LY', '', 0, '/'),
    ('TEST', 5, 'SM', '', 0, '.'),
    ('TEST', 6, 'LM', '', 0, '.'),
    ('TEST', 7, 'NM', '', 0, '/'),
    ('TEST', 8, 'DT', '', 0, '/'),
    ('TEST', 9, 'SD', '', 0, '.'),
    ('TEST', 10, 'LD', '', 0, '.'),
    ('TEST', 11, 'ND', '', 0, '/'),
    ('TEST', 12, 'CT', '123', 5, '')
GO

*/


DECLARE
    @dataName NVARCHAR(50),
    @customMonthName NVARCHAR(MAX),
    @customDayName NVARCHAR(MAX)

SELECT
    @dataName = N'TEST',
    @customMonthName = N'JANUARI,FEBRUARI,MARET,APRIL,MEI,JUNI,JULI,AGUSTUS,SEPTEMBER,OKTOBER,NOVEMBER,DESEMBER',
    @customDayName = N'MINGGU,SENIN,SELASA,RABU,KAMIS,JUMAT,SABTU'

BEGIN
    DECLARE @keyPart TABLE ( Seq INT, KeyType VARCHAR(2), [Value] VARCHAR(50), [Length] INT, BackSeparator VARCHAR(3) )
    DECLARE @splittedMonthName TABLE ( No INT, Split VARCHAR(100) )
    DECLARE @splittedDayName TABLE ( No INT, Split VARCHAR(100) )
    DECLARE @generatedKey NVARCHAR(MAX) = '', @paddedChar VARCHAR(50) = '0000000000' -- NOTE: hardcoded 10 chars length
    
    IF (LTRIM(RTRIM(ISNULL(@customMonthName, ''))) = '')
    BEGIN
        SET @customMonthName = N'JANUARY,FEBRUARY,MARCH,APRIL,MAY,JUNI,JULY,AUGUST,SEPTEMBER,OCTOBER,NOVEMBER,DECEMBER'
    END
    
    IF (LTRIM(RTRIM(ISNULL(@customDayName, ''))) = '')
    BEGIN
        SET @customDayName = N'SUNDAY,MONDAY,TUESDAY,WEDNESDAY,THURSDAY,FRIDAY,SATURDAY'
    END
    
    INSERT INTO @splittedMonthName SELECT Idx, Splitted FROM dbo.SplitString(@customMonthName, ',')
    INSERT INTO @splittedDayName SELECT Idx, Splitted FROM dbo.SplitString(@customDayName, ',')
    INSERT INTO @keyPart
    SELECT Seq, KeyType, [Value], [Length], BackSeparator
    FROM CommonKey WHERE DataName = @dataName
    ORDER BY Seq ASC
    
    DECLARE @keyPartCount INT = (SELECT COUNT(0) FROM @keyPart), @counter INT = 0
    WHILE @counter < @keyPartCount
    BEGIN
        DECLARE @currentDate DATETIME = (SELECT GETDATE())
        SELECT @generatedKey = @generatedKey +
        CASE
            WHEN pr.KeyType = 'ST' THEN -- String
                LEFT(pr.Value, pr.LENGTH)
            WHEN pr.KeyType = 'SY' THEN -- Short Year
                RIGHT(YEAR(@currentDate), 2)
            WHEN pr.KeyType = 'LY' THEN -- Long Year
                CAST(YEAR(@currentDate) AS VARCHAR)
            WHEN pr.KeyType = 'SM' THEN -- Short Month
                LEFT((SELECT Split FROM @splittedMonthName WHERE No = MONTH(@currentDate)), 3)
            WHEN pr.KeyType = 'LM' THEN -- Long Month
                (SELECT Split FROM @splittedMonthName WHERE No = MONTH(@currentDate))
            WHEN pr.KeyType = 'NM' THEN -- Numeric Month
                CAST(MONTH(@currentDate) AS VARCHAR)
            WHEN pr.KeyType = 'DT' THEN -- Date
                CAST(DAY(@currentDate) AS VARCHAR)
            WHEN pr.KeyType = 'SD' THEN -- Short Day
                LEFT((SELECT Split FROM @splittedDayName WHERE No = DATEPART(WEEKDAY, @currentDate)), 3)
            WHEN pr.KeyType = 'LD' THEN -- Long Day
                (SELECT Split FROM @splittedDayName WHERE No = DATEPART(WEEKDAY, @currentDate))
            WHEN pr.KeyType = 'ND' THEN -- Numeric Day
                CAST(DATEPART(WEEKDAY, @currentDate) AS VARCHAR)
            WHEN pr.KeyType = 'CT' THEN -- Counter
            RIGHT(CAST(
                STUFF(@paddedChar, LEN(@paddedChar)-LEN(CAST(pr.VALUE AS INT))+1, LEN(@paddedChar), CAST(pr.VALUE AS INT)+1)
            AS VARCHAR), pr.LENGTH)
        END + pr.BackSeparator
        FROM @keyPart pr WHERE pr.Seq = (SELECT @counter + 1)
        
        SET @counter = @counter + 1
    END

    SELECT @generatedKey
END

/*

CREATE FUNCTION dbo.GenerateKey
(
    @dataName NVARCHAR(50),
    @customMonthName NVARCHAR(MAX),
    @customDayName NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @keyPart TABLE ( Seq INT, KeyType VARCHAR(2), [Value] VARCHAR(50), [Length] INT, BackSeparator VARCHAR(3) )
    DECLARE @splittedMonthName TABLE ( No INT, Split VARCHAR(100) )
    DECLARE @splittedDayName TABLE ( No INT, Split VARCHAR(100) )
    DECLARE @generatedKey NVARCHAR(MAX) = '', @paddedChar VARCHAR(50) = '0000000000' -- NOTE: hardcoded 10 chars length
    
    IF (LTRIM(RTRIM(ISNULL(@customMonthName, ''))) = '')
    BEGIN
        SET @customMonthName = N'JANUARY,FEBRUARY,MARCH,APRIL,MAY,JUNI,JULY,AUGUST,SEPTEMBER,OCTOBER,NOVEMBER,DECEMBER'
    END
    
    IF (LTRIM(RTRIM(ISNULL(@customDayName, ''))) = '')
    BEGIN
        SET @customDayName = N'SUNDAY,MONDAY,TUESDAY,WEDNESDAY,THURSDAY,FRIDAY,SATURDAY'
    END
    
    INSERT INTO @splittedMonthName SELECT Idx, Splitted FROM dbo.SplitString(@customMonthName, ',')
    INSERT INTO @splittedDayName SELECT Idx, Splitted FROM dbo.SplitString(@customDayName, ',')
    INSERT INTO @keyPart
    SELECT Seq, KeyType, [Value], [Length], BackSeparator
    FROM CommonKey WHERE DataName = @dataName
    ORDER BY Seq ASC
    
    DECLARE @keyPartCount INT = (SELECT COUNT(0) FROM @keyPart), @counter INT = 0
    WHILE @counter < @keyPartCount
    BEGIN
        DECLARE @currentDate DATETIME = (SELECT GETDATE())
        SELECT @generatedKey = @generatedKey +
        CASE
            WHEN pr.KeyType = 'ST' THEN -- String
                LEFT(pr.Value, pr.LENGTH)
            WHEN pr.KeyType = 'SY' THEN -- Short Year
                RIGHT(YEAR(@currentDate), 2)
            WHEN pr.KeyType = 'LY' THEN -- Long Year
                CAST(YEAR(@currentDate) AS VARCHAR)
            WHEN pr.KeyType = 'SM' THEN -- Short Month
                LEFT((SELECT Split FROM @splittedMonthName WHERE No = MONTH(@currentDate)), 3)
            WHEN pr.KeyType = 'LM' THEN -- Long Month
                (SELECT Split FROM @splittedMonthName WHERE No = MONTH(@currentDate))
            WHEN pr.KeyType = 'NM' THEN -- Numeric Month
                CAST(MONTH(@currentDate) AS VARCHAR)
            WHEN pr.KeyType = 'DT' THEN -- Date
                CAST(DAY(@currentDate) AS VARCHAR)
            WHEN pr.KeyType = 'SD' THEN -- Short Day
                LEFT((SELECT Split FROM @splittedDayName WHERE No = DATEPART(WEEKDAY, @currentDate)), 3)
            WHEN pr.KeyType = 'LD' THEN -- Long Day
                (SELECT Split FROM @splittedDayName WHERE No = DATEPART(WEEKDAY, @currentDate))
            WHEN pr.KeyType = 'ND' THEN -- Numeric Day
                CAST(DATEPART(WEEKDAY, @currentDate) AS VARCHAR)
            WHEN pr.KeyType = 'CT' THEN -- Counter
            RIGHT(CAST(
                STUFF(@paddedChar, LEN(@paddedChar)-LEN(CAST(pr.VALUE AS INT))+1, LEN(@paddedChar), CAST(pr.VALUE AS INT)+1)
            AS VARCHAR), pr.LENGTH)
        END + pr.BackSeparator
        FROM @keyPart pr WHERE pr.Seq = (SELECT @counter + 1)
        
        SET @counter = @counter + 1
    END

    RETURN @generatedKey
END

*/
