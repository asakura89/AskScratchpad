
/*
-- Uncomment this to see the error
IF ISNULL(NULLIF('a', ''), 0) <> 0
BEGIN
    SELECT 'b'
END
*/

SELECT NULLIF('a', '')

SELECT ISNULL(NULLIF('a', ''), 0)

IF ISNULL(NULLIF('', ''), 0) <> 0
BEGIN
    SELECT 'b'
END

IF ISNULL(NULLIF('', ''), 0) = 0
BEGIN
    SELECT 'b'
END


