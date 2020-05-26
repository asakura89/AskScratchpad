


DECLARE
    @input NVARCHAR(MAX)

SELECT @input = N'tHanKS TO mY maGNifiCENt sElf!!'
--SELECT @input = UPPER(N'ÄÄ ÖÖ ÜÜ ÉÉ ØØ ĈĈ ÆÆ')

BEGIN
    DECLARE @idx INT
    DECLARE @currentChar CHAR(1)
    DECLARE @output VARCHAR(255)
    
    SELECT
        @output = LOWER(@input),
        @idx = 2,
        @output = STUFF(@output, 1, 1, UPPER(SUBSTRING(@input, 1, 1)))
    
    WHILE @idx <= LEN(@input)
    BEGIN
        SET @currentChar = SUBSTRING(@input, @idx, 1)
        IF @currentChar IN (' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&','''','(')
        IF @idx + 1 <= LEN(@input)
        BEGIN
            IF @currentChar != '''' OR UPPER(SUBSTRING(@input, @idx + 1, 1)) != 'S'
            SET @output = STUFF(@output, @idx + 1, 1,UPPER(SUBSTRING(@input, @idx + 1, 1)))
        END
        SET @idx = @idx + 1
    END
    
    SELECT ISNULL(@output, '')
END



/*

CREATE FUNCTION [dbo].[TitleCase] (@input NVARCHAR(MAX))
    RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @idx INT
    DECLARE @currentChar CHAR(1)
    DECLARE @output VARCHAR(255)
    
    SELECT
        @output = LOWER(@input),
        @idx = 2,
        @output = STUFF(@output, 1, 1, UPPER(SUBSTRING(@input, 1, 1)))
    
    WHILE @idx <= LEN(@input)
    BEGIN
        SET @currentChar = SUBSTRING(@input, @idx, 1)
        IF @currentChar IN (' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&','''','(')
        IF @idx + 1 <= LEN(@input)
        BEGIN
            IF @currentChar != '''' OR UPPER(SUBSTRING(@input, @idx + 1, 1)) != 'S'
            SET @output = STUFF(@output, @idx + 1, 1,UPPER(SUBSTRING(@input, @idx + 1, 1)))
        END
        SET @idx = @idx + 1
    END
    
    RETURN ISNULL(@output, '')
END

*/

